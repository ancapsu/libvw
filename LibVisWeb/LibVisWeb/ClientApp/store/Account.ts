import { Action, Reducer } from 'redux';
import { History, Location, Path, LocationState, LocationDescriptor } from 'history';
import { push, routerActions, LocationChangeAction } from 'react-router-redux'
import { fetch, addTask } from 'domain-task';
import { Dispatch } from 'react-redux';
import * as AccountModel from '../models/Account';
import RacMsg from '../message/racmsg';
import { CreateAccountModel, FileDataModel, SendAgainModel } from '../models/Account';
import { GenericStatusModel } from '../models/GenericStatus';
import { AppThunkAction, ApplicationState } from './';
import { toastr } from 'react-redux-toastr';
import storage from '../utils/storage';
import { Route } from 'react-router';
import { UserIdModel } from 'ClientApp/models/News';

// -----------------
// USER IN LOCAL STORAGE (Cookies)

const userKey = 'racusrtkn';
const versionKey = 'tknvers';
const userVersion = '5';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AccountState {

    validToken: boolean;
    keepLogged: boolean;
    checkedCookie: boolean;
    lang: number;
    user: AccountModel.LoginResultModel | null;

    hasList: boolean;
    hasManageUser: boolean;

    ini: number;
    total: number;
    users: AccountModel.AccountModel[];
    manageUser: AccountModel.AccountModel | null;

    searchParameters: AccountModel.SearchUserModel | null;

}

// -----------------
// Inicialização do estado do usuário

var startingState: AccountState = { validToken: false, keepLogged: false, checkedCookie: false, lang: 3, user: null, hasList: false, hasManageUser: false, ini: 0, total: 0, users: [], searchParameters: null, manageUser: null };

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

interface LoginSuccessAction { type: 'LOGIN_SUCCESS', payload: AccountModel.LoginResultModel, keepLogged: boolean }
interface LoginFailureAction { type: 'LOGIN_FAILURE' }
interface LogoffAction { type: 'ACCOUNT_LOGOFF' }
interface ValidateTokenAction { type: 'VALIDATE_TOKEN' }
interface AccountChangeAction { type: 'CHANGE_USER', payload: AccountModel.AccountChangeModel }
interface SetLanguageAction { type: 'SET_LANGUAGE', payload: number }
interface ClearUserList { type: 'CLEAR_USER_LIST' }
interface ReceiveSearchUserAction { type: 'RECEIVE_USER_SEARCH', payload: AccountModel.SearchUserActionModel }
interface ReceiveUserForManagerAction { type: 'RECEIVE_USER_FOR_MANAGER', payload: AccountModel.AccountModel }

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = LoginSuccessAction | LoginFailureAction | LogoffAction | ValidateTokenAction | AccountChangeAction | SetLanguageAction | ClearUserList | ReceiveSearchUserAction | ReceiveUserForManagerAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {

    login: (data: AccountModel.LoginRequestModel): AppThunkAction<any> => (dispatch, getState) => {

        let fetchTask = fetch(`api/Login`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })

            .then(response => response.json() as Promise<AccountModel.LoginResultModel>)
            .then(res => {

                if (res.Result == 0) {

                    dispatch({ type: 'LOGIN_SUCCESS', payload: res, keepLogged: data.KeepLogged });

                    dispatch(routerActions.goBack());

                    // Remove as chaves do cookie de sessão

                    storage.clear();
                    //storage.remove(userKey);
                    //storage.remove(versionKey);

                    // Se mandou manter, coloca de novo

                    if (data.KeepLogged) {

                        storage.add(userKey, JSON.stringify(res));
                        storage.add(versionKey, userVersion);

                    }
                    
                } else {

                    if (res.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result) + ": " + res.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result));

                    }

                    dispatch({ type: 'LOGIN_FAILURE' });
                    
                }

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

                dispatch({ type: 'LOGIN_FAILURE' });

            });

        addTask(fetchTask);

    },

    logoff: (): AppThunkAction<any> => (dispatch, getState) => {

        var apps: ApplicationState = getState();

        if (apps != null && apps.account.validToken && apps.account.user != null) {

            let fetchTask = fetch(`api/Login?token=${apps.account.user.Token}&lang=${apps.account.lang}`, {
                cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
                credentials: 'same-origin', // include, same-origin, *omit
                headers: {
                    'user-agent': 'Mozilla/4.0 MDN Example',
                },
                method: 'DELETE', // *GET, POST, PUT, DELETE, etc.
                mode: 'cors', // no-cors, cors, *same-origin
                redirect: 'follow', // manual, *follow, error
                referrer: 'no-referrer', // *client, no-referrer
            })

                .then(data => {

                    // Remove as chaves do cookie de sessão

                    storage.clear();
                    //storage.remove(userKey);
                    //storage.remove(versionKey);

                    dispatch(routerActions.push('/'));

                    dispatch({ type: 'ACCOUNT_LOGOFF' });

                })
                .catch(e => {

                    toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                    for (let er in e.response.data.errors) {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                    }

                    dispatch({ type: 'ACCOUNT_LOGOFF' });

                });

            addTask(fetchTask);

        }

    },

    validateLoginToken: (): AppThunkAction<any> => (dispatch, getState) => {

        // Existe informação em cookie de usuário logado?

        var apps: ApplicationState = getState();

        if (!apps.account.checkedCookie) {

            if (apps == null || !apps.account.validToken || apps.account.user == null) {

                var c: any = storage.get(userKey);
                var v: any = storage.get(versionKey);

                // O usuário e a versão precisam ser válidos e estar na mesma versão (senão, melhor ignorar)
                if (c != null && c != undefined && v != null && v != undefined && v == userVersion) {

                    // Carrega essa informação

                    var cookieState: AccountState = JSON.parse(c);

                    dispatch({ type: 'LOGIN_SUCCESS', payload: cookieState, keepLogged: true });

                    // Checa se a informação ainda é válida? --- não por hora

                    /*
                    let fetchTask = fetch(`api/Account/Validate`, {
                        body: JSON.stringify(data), // must match 'Content-Type' header
                        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
                        credentials: 'same-origin', // include, same-origin, *omit
                        headers: {
                            'user-agent': 'Mozilla/4.0 MDN Example',
                            'content-type': 'application/json'
                        },
                        method: 'POST', // *GET, POST, PUT, DELETE, etc.
                        mode: 'cors', // no-cors, cors, *same-origin
                        redirect: 'follow', // manual, *follow, error
                        referrer: 'no-referrer', // *client, no-referrer
                    })
        
                        .then(response => response.json() as Promise<GenericStatusModel>)
                        .then(res => {
        
                            if (res.Result == 0) {
        
                                dispatch({ type: 'CREAT_ACCOUNT' });
        
                                if (data.Email != undefined)
                                    dispatch(routerActions.push('/confirm-email/0/' + btoa(data.Email)));
                                else
                                    dispatch(routerActions.push('/confirm-email/'));
        
                            } else {
        
                                if (res.ResultComplement != "") {
        
                                    toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result) + ": " + res.ResultComplement);
        
                                } else {
        
                                    toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result));
        
                                }
        
                                dispatch({ type: 'VALIDATE_TOKEN' });
        
                            }
        
                        }).catch(e => {
        
                            toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));
        
                            for (let er in e.response.data.errors) {
        
                                toastr.error(RacMsg.Get(RacMsg.Id.Error), er);
        
                            }
        
                            dispatch({ type: 'VALIDATE_TOKEN' });
        
                        });
        
                    addTask(fetchTask);
                    dispatch({ type: 'VALIDATE_TOKEN' });
        
                    */

                } else {

                    dispatch({ type: 'VALIDATE_TOKEN' });

                }
                
            }

        }

    },

    requestUserSearch: (ini: number, max: number, data: AccountModel.SearchUserModel): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: AccountModel.LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_USER_LIST' });

        let fetchTask = fetch(`api/Account/Search?token=${token}&ini=${ini}&max=${max}`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })
            .then(response => response.json() as Promise<AccountModel.UserListModel>)
            .then(res => {

                var data_res: AccountModel.SearchUserActionModel  = { SearchData: data, List: res };

                dispatch({ type: 'RECEIVE_USER_SEARCH', payload: data_res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    requestUserForManager: (id: string, lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: AccountModel.LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        let fetchTask = fetch(`api/Account/GetForManager?token=${token}&lang=${lang}&id=${id}`, {
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'GET', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })
            .then(response => response.json() as Promise<AccountModel.AccountForManagerModel>)
            .then(res => {

                if (res.Result == 0 && res.Account != null) {
                    
                    dispatch({ type: 'RECEIVE_USER_FOR_MANAGER', payload: res.Account });

                } else {

                    if (res.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result) + ": " + res.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result));

                    }

                }

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    createAccount: (data: AccountModel.CreateAccountModel): AppThunkAction<any> => (dispatch, getState) => {

        let fetchTask = fetch(`api/Account/CreateAccount`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })

            .then(response => response.json() as Promise<GenericStatusModel>)
            .then(res => {

                if (res.Result == 0) {

                    if (data.Email != undefined)
                        dispatch(routerActions.push('/confirm-email/0/' + btoa(data.Email)));
                    else
                        dispatch(routerActions.push('/confirm-email/'));

                } else {

                    if (res.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result) + ": " + res.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result));

                    }

                }

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    confirmEmail: (data: AccountModel.ConfirmEmailModel): AppThunkAction<any> => (dispatch, getState) => {

        let fetchTask = fetch(`api/Account/ConfirmEmail`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })

            .then(response => response.json() as Promise<GenericStatusModel>)
            .then(data => {

                if (data.Result == 0) {

                    dispatch(routerActions.push('/'));

                    toastr.info("Sucesso", "Seu email foi confirmado, por favor, se autentique para acessar a &aacute;rea interna");

                } else {

                    if (data.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(data.Result) + ": " + data.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(data.Result));

                    }

                }

            }).catch(e => {

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    sendAgain: (email: string, type: number, lang: number): AppThunkAction<any> => (dispatch, getState) => {

        var data: SendAgainModel = { Email: email, Type: type, Lang: lang };

        let fetchTask = fetch(`api/Account/SendAgain`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })

            .then(response => response.json() as Promise<GenericStatusModel>)
            .then(data => {

                if (data.Result == 0) {

                    toastr.info("Sucesso", "Enviamos novo email para o endereço indicado. Verifique se não caiu na caixa de spam");

                } else {

                    if (data.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(data.Result) + ": " + data.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(data.Result));

                    }

                }

            }).catch(e => {

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    changePassword: (data: AccountModel.ChangePasswordModel): AppThunkAction<any> => (dispatch, getState) => {

        var token: string = "";
        var user: AccountModel.LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        let fetchTask = fetch(`api/Account/ChangePassword?token=${token}`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })

            .then(response => response.json() as Promise<GenericStatusModel>)
            .then(data => {

                if (data.Result == 0) {

                    dispatch(routerActions.push('/main'));

                    toastr.info("Sucesso", "Sua senha foi alterada");

                } else {

                    if (data.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(data.Result) + ": " + data.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(data.Result));

                    }

                }

            }).catch(e => {

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    forgotPassword: (data: AccountModel.ForgotPasswordModel): AppThunkAction<any> => (dispatch, getState) => {

        let fetchTask = fetch(`api/Account/ForgotPassword`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })

            .then(response => response.json() as Promise<GenericStatusModel>)
            .then(res => {

                if (res.Result == 0) {

                    toastr.info("Informação","Foi enviado um link para seu email para troca de senha. Clique no link do email para prosseguir.");

                    dispatch(routerActions.push('/'));

                } else {

                    if (res.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result) + ": " + res.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result));

                    }

                }

            }).catch(e => {

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    recoverPassword: (data: AccountModel.RecoverPasswordModel): AppThunkAction<any> => (dispatch, getState) => {

        let fetchTask = fetch(`api/Account/RecoverPassword`, {
            body: JSON.stringify(data), // must match 'Content-Type' header
            cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
            credentials: 'same-origin', // include, same-origin, *omit
            headers: {
                'user-agent': 'Mozilla/4.0 MDN Example',
                'content-type': 'application/json'
            },
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            mode: 'cors', // no-cors, cors, *same-origin
            redirect: 'follow', // manual, *follow, error
            referrer: 'no-referrer', // *client, no-referrer
        })

            .then(response => response.json() as Promise<GenericStatusModel>)
            .then(res => {

                if (res.Result == 0) {

                    dispatch(routerActions.push('/'));

                    toastr.info("Sucesso", "Sua senha foi alterada com sucesso, por favor, se autentique novamente para acessar a área interna");

                } else {

                    if (res.ResultComplement != "") {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result) + ": " + res.ResultComplement);

                    } else {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result));

                    }

                }

            }).catch(e => {

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);
        
    },

    changeAccount: (data: AccountModel.AccountChangeModel): AppThunkAction<any> => (dispatch, getState) => {

        var user: AccountModel.LoginResultModel | null = getState().account.user;
                
        if (user != null) {

            var token: string = user.Token;

            let fetchTask = fetch(`api/Account/ChangeAccount?token=${token}`, {
                body: JSON.stringify(data), // must match 'Content-Type' header
                cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
                credentials: 'same-origin', // include, same-origin, *omit
                headers: {
                    'user-agent': 'Mozilla/4.0 MDN Example',
                    'content-type': 'application/json'
                },
                method: 'POST', // *GET, POST, PUT, DELETE, etc.
                mode: 'cors', // no-cors, cors, *same-origin
                redirect: 'follow', // manual, *follow, error
                referrer: 'no-referrer', // *client, no-referrer
            })

                .then(response => response.json() as Promise<GenericStatusModel>)
                .then(res => {

                    if (res.Result == 0) {
                        
                        dispatch({ type: 'CHANGE_USER', payload: data });

                        dispatch(routerActions.goBack());

                        // Remove as chaves do cookie de sessão

                        storage.remove(userKey);
                        storage.remove(versionKey);

                        // Se mandou manter, coloca de novo

                        if (getState().account.keepLogged) {

                            storage.add(userKey, JSON.stringify(res));
                            storage.add(versionKey, userVersion);

                        }

                        toastr.info("Sucesso", "Sua conta foi alterada com sucesso");

                    } else {

                        if (res.ResultComplement != "") {

                            toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result) + ": " + res.ResultComplement);

                        } else {

                            toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(res.Result));

                        }

                    }

                }).catch(e => {

                    toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                    for (let er in e.response.data.errors) {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                    }

                });

            addTask(fetchTask);

        }

    },

    setLanguage: (l: number): AppThunkAction<any> => (dispatch, getState) => {

        var apps: ApplicationState = getState();

        if (apps != null && apps.account.validToken && apps.account.user != null) {

            let fetchTask = fetch(`api/Account/ChangeAccountLanguage?token=${apps.account.user.Token}&lang=${l}`, {
                cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
                credentials: 'same-origin', // include, same-origin, *omit
                headers: {
                    'user-agent': 'Mozilla/4.0 MDN Example',
                },
                method: 'GET', // *GET, POST, PUT, DELETE, etc.
                mode: 'cors', // no-cors, cors, *same-origin
                redirect: 'follow', // manual, *follow, error
                referrer: 'no-referrer', // *client, no-referrer
            })

                .then(data => {

                    dispatch({ type: 'SET_LANGUAGE', payload: l });

                })
                .catch(e => {

                    toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                    for (let er in e.response.data.errors) {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                    }

                });

            addTask(fetchTask);

        }
        else
        {

            dispatch({ type: 'SET_LANGUAGE', payload: l });

        }

    }

};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<AccountState> = (state: AccountState, action: KnownAction) => {

    switch (action.type) {

        case 'ACCOUNT_LOGOFF':
            return { ...state, validToken: false, keepLogged: false, checkedCookie: true,user: null };

        case 'LOGIN_SUCCESS':
            RacMsg.SetLanguage(action.payload.Account.Lang);
            return { ...state, validToken: true, keepLogged: action.keepLogged, checkedCookie: true, lang: action.payload.Account.Lang, user: action.payload };

        case 'LOGIN_FAILURE':            
            return { ...state, validToken: false, keepLogged: false, checkedCookie: true, user: null };

        case 'VALIDATE_TOKEN':
            return { ...state, validToken: false, keepLogged: false, checkedCookie: true, user: null };

        case 'CHANGE_USER': {

            if (state.user == null) {

                return { ...state };

            } else {

                var newsletter: boolean = state.user.Account.NewsLetter;

                if (action.payload.NewsLetter != undefined)
                    newsletter = action.payload.NewsLetter;

                return { ...state, user: { ...state.user, Account: { ...state.user.Account, Name: action.payload.Name || state.user.Account.Name, Bitcoin: action.payload.Bitcoin || state.user.Account.Bitcoin, Email: action.payload.Email || state.user.Account.Email, NewsLetter: newsletter } } };

            }

        }

        case 'SET_LANGUAGE': {

            if (state.user == null || !state.validToken) {

                RacMsg.SetLanguage(action.payload);
                return { ...state, lang: action.payload };

            } else {
                
                // O usuário e a versão precisam ser válidos e estar na mesma versão (senão, melhor ignorar)
                if (state.keepLogged) {

                    // Carrega essa informação

                    var cookieState: AccountModel.LoginResultModel = { ...state.user, Account: { ...state.user.Account, Lang: action.payload } };

                    storage.clear();
                    //storage.remove(userKey);
                    //storage.remove(versionKey);

                    // Se mandou manter, coloca de novo

                    storage.add(userKey, JSON.stringify(cookieState));
                    storage.add(versionKey, userVersion);

                }

                RacMsg.SetLanguage(action.payload);
                return { ...state, lang: action.payload, user: { ...state.user, Account: { ...state.user.Account, Language: action.payload } } };

            }

        }

        case 'RECEIVE_USER_FOR_MANAGER': {

            return { ...state, hasManageUser: true, manageUser: action.payload };

        }

        case 'CLEAR_USER_LIST':
            return { ...state, hasList: false, ini: 0, total: 0, users: [] };

        case 'RECEIVE_USER_SEARCH': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.List != undefined) {

                if (action.payload.List.Ini == 0)
                    return { ...state, hasList: true, users: action.payload.List.List, total: action.payload.List.Total };

                // Pega todos os targets atuais

                var joinUsers: AccountModel.AccountModel[] = [];
                for (var i = 0; i < state.users.length; i++)
                    joinUsers.push(state.users[i]);

                // Agora cria vazios até o fim

                var totaljoin: number = action.payload.List.Ini + action.payload.List.List.length;
                for (var i = joinUsers.length; i < totaljoin; i++)
                    joinUsers.push(new AccountModel.AccountModel());

                // Agora para cada item recebido altera a posição
                for (var i = action.payload.List.Ini; i < totaljoin; i++)
                    joinUsers[i] = action.payload.List.List[i - action.payload.List.Ini];

                return { ...state, hasList: true, users: joinUsers, total: action.payload.List.Total, searchParameters: action.payload.SearchData || null };

            } else {

                return { ...state };

            }

        }

        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    // For unrecognized actions (or in cases where actions have no effect), must return the existing state
    //  (or default initial state if none was supplied)
    return state || startingState;

};
