import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { push, routerActions, LocationChangeAction } from 'react-router-redux'
import { LoginResultModel } from '../models/Account';
import { TargetModel, TargetActionGantModel, TargetListModel, TargetCategoryModel, EditTargetBaseModel, RegisterVote, LinkModel, LinkResultModel, NewTargetModel, ChangeTargetModel, SearchTargetModel, SearchTargetActionModel } from '../models/Target';
import { NewsCategory, NewsAward, RegisterGrant, ChangeLanguage } from '../models/News';
import { toastr } from 'react-redux-toastr';
import RacMsg from '../message/racmsg';
import { AppThunkAction } from '.';
import { GenericIdModel, GenericStatusModel } from '../models/GenericStatus';
import { IncludeObservaton } from 'ClientApp/models/Article';

// -----------------
// Informações sobre a pauta ou as pautas

export interface TargetState {

    hasTarget: boolean;
    hasCategory: boolean;
    hasList: boolean;
    hasEditBase: boolean;
    hasLinkInfo: boolean;
    hasSearchList: boolean;

    searchParameters: SearchTargetModel | null;

    target: TargetModel | null;

    title: string;
    description: string;

    total: number;
    targets: TargetModel[];

    categories: NewsCategory[];
    awards: NewsAward[];
    link: LinkResultModel | null;

}

// -----------------
// Ações sobre a pauta


interface ReceiveTargetAction {
    type: 'RECEIVE_TARGET';
    payload: TargetModel;
}

interface ReceiveLinkAction {
    type: 'RECEIVE_LINK';
    payload: LinkResultModel;
}

interface VoteTargetAction {
    type: 'VOTE_TARGET';
    payload: RegisterVote;
}

interface RegisterGrantAction {
    type: 'REGISTER_GRANT';
    payload: RegisterGrant;
}

interface ReceiveTargetListAction {
    type: 'RECEIVE_TARGET_LIST';
    payload: TargetListModel;
}

interface ReceiveTargetSearchAction {
    type: 'RECEIVE_TARGET_SEARCH';
    payload: SearchTargetActionModel;
}

interface ReceiveTargetCategoryAction {
    type: 'RECEIVE_TARGET_CATEGORY';
    payload: TargetCategoryModel;
}

interface ReceiveBaseEditAction {
    type: 'RECEIVE_EDIT_TARGET_BASE';
    payload: EditTargetBaseModel;
}

interface ClearTargetEditBase {
    type: 'CLEAR_TARGET_EDIT_BASE';
}

interface ClearTarget {
    type: 'CLEAR_TARGET';    
}

interface ClearTargetList {
    type: 'CLEAR_TARGET_LIST';
}

interface ChangeTargetLanguage {
    type: 'CHANGE_TARGET_LANGUAGE';
    payload: string;
}

// ----------------
// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the declared type strings (and not any other arbitrary string).

type KnownAction = ReceiveTargetAction | ReceiveTargetListAction | VoteTargetAction | RegisterGrantAction | ReceiveTargetCategoryAction | ReceiveTargetSearchAction | ReceiveBaseEditAction | ReceiveLinkAction | ClearTargetEditBase | ClearTarget | ClearTargetList | ChangeTargetLanguage;

// ----------------
// Ações em cima das pautas

export const actionCreators = {

    requestTarget: (id: string, lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
                
        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_TARGET'});

        let fetchTask = fetch(`api/Target/Get?token=${token}&lang=${lang}&id=${id}`, {
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
            .then(response => response.json() as Promise<TargetModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_TARGET', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask); 
        
    },

    changeTarget: (data: ChangeTargetModel): AppThunkAction<any> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token; 

            let fetchTask = fetch(`api/Target/Change?token=${token}`, {
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
                .then(response => response.json() as Promise<GenericIdModel>)
                .then(res => {

                    if (res.Result == 0) {

                        dispatch({ type: 'CLEAR_TARGET_LIST' });

                        dispatch(routerActions.push('/target/' + res.Id));

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
    
    requestTargetList: (lang: number, ini: number, max: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_TARGET_LIST' });

        let fetchTask = fetch(`api/Target/List?token=${token}&lang=${lang}&ini=${ini}&max=${max}`)
            .then(response => response.json() as Promise<TargetListModel>)
            .then(res => {
                
                dispatch({ type: 'RECEIVE_TARGET_LIST', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask); 

    },

    requestTargetSearch: (ini: number, max: number, data: SearchTargetModel): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_TARGET_LIST' });

        let fetchTask = fetch(`api/Target/Search?token=${token}&ini=${ini}&max=${max}`, {
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
            .then(response => response.json() as Promise<TargetListModel>)
            .then(res => {

                var data_res: SearchTargetActionModel = { SearchData: data, List: res };

                dispatch({ type: 'RECEIVE_TARGET_SEARCH', payload: data_res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    requestTargetListAll: (lang: number, ini: number, max: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_TARGET_LIST' });

        let fetchTask = fetch(`api/Target/ListAll?token=${token}&lang=${lang}&ini=${ini}&max=${max}`)
            .then(response => response.json() as Promise<TargetListModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_TARGET_LIST', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    registerTargetVote: (id: string, lang: number, vote: number): AppThunkAction<KnownAction> => (dispatch: any, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null)
            token = user.Token;

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.YouNeedToBeLoggedInForThisAction));

        } else {

            var data: RegisterVote = { Id: id, Vote: vote, Lang: lang };

            let fetchTask = fetch(`api/Target/RegisterVote?token=${token}`, {
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

                    if (res.Result != 0) {

                        toastr.error(RacMsg.Get(res.Result), res.ResultComplement);

                    } else {
                        
                        dispatch({ type: 'VOTE_TARGET', payload: data });

                        if (vote == 1) {
                                                        
                            dispatch(routerActions.push('/new-article/' + id));
                            
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

    changeTargetLanguage: (id: string, lang: number, newlang: number): AppThunkAction<KnownAction> => (dispatch: any, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null)
            token = user.Token;

        if (newlang != lang && newlang >= 2 && newlang <= 4) {

            if (token == "") {

                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.YouNeedToBeLoggedInForThisAction));

            } else {

                var data: ChangeLanguage = { Id: id, NewLang: newlang, Lang: lang };

                let fetchTask = fetch(`api/Target/ChangeLanguage?token=${token}`, {
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

                        if (res.Result != 0) {

                            toastr.error(RacMsg.Get(res.Result), res.ResultComplement);

                        } else {

                            dispatch({ type: 'CHANGE_TARGET_LANGUAGE', payload: id });

                        }

                    }).catch(e => {

                        toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                        for (let er in e.response.data.errors) {

                            toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                        }

                    });

                addTask(fetchTask);

            }

        }

    },

    registerGrant: (actionId: string, lang: number, awardId: string, add: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null)
            token = user.Token;

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para definir prêmios");

        } else {

            dispatch({ type: 'CLEAR_TARGET_LIST' });

            var data: RegisterGrant = { ActionId: actionId, AwardId: awardId, Add: add, Lang: lang };

            let fetchTask = fetch(`api/Target/RegisterGrant?token=${token}`, {
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
                .then(response => response.json() as Promise<GenericIdModel>)
                .then(res => {

                    if (res.Result != 0) {

                        toastr.error(RacMsg.Get(res.Result), res.ResultComplement);

                    } else {

                        dispatch({ type: 'REGISTER_GRANT', payload: data });

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

    requestTargetByCategory: (category: string, ini: number, max: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_TARGET_LIST' });

        let fetchTask = fetch(`api/Target/ByCategory?token=${token}&categ=${category}&ini=${ini}&max=${max}`)
            .then(response => response.json() as Promise<TargetCategoryModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_TARGET_CATEGORY', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask); 

    },
    
    requestEditBase: (lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null) {

            token = user.Token;

            dispatch({ type: 'CLEAR_TARGET_EDIT_BASE' });

            let fetchTask = fetch(`api/Target/EditBase?token=${token}&lang=${lang}`)
                .then(response => response.json() as Promise<EditTargetBaseModel>)
                .then(res => {

                    dispatch({ type: 'RECEIVE_EDIT_TARGET_BASE', payload: res });

                }).catch(e => {

                    toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                    for (let er in e.response.data.errors) {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                    }

                });

            addTask(fetchTask);

        }

    },

    requestLinkInfo: (link: string, lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token;
            var data: LinkModel = { Link: link, Lang: lang };

            let fetchTask = fetch(`api/Target/Link?token=${token}`, {
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
                .then(response => response.json() as Promise<LinkResultModel>)
                .then(res => {

                    if (res.Result == 0) {

                        dispatch({ type: 'RECEIVE_LINK', payload: res });

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

            addTask(fetchTask); // Ensure server-side prerendering waits for this to complete

        }

    },

    registerNewTarget: (data: NewTargetModel): AppThunkAction<any> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token;

            let fetchTask = fetch(`api/Target/Register?token=${token}`, {
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
                .then(response => response.json() as Promise<GenericIdModel>)
                .then(res => {

                    if (res.Result == 0) {

                        dispatch({ type: 'CLEAR_TARGET_EDIT_BASE' });
                        dispatch({ type: 'CLEAR_TARGET_LIST' });

                        dispatch(routerActions.push('/target/' + res.Id));

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

    }

};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TargetState = { hasTarget: false, hasCategory: false, hasList: false, hasEditBase: false, hasLinkInfo: false, hasSearchList: false, target: null, searchParameters: null, title: "", description: "", total: 0, targets: [], categories: [], awards: [], link: null };

export const reducer: Reducer<TargetState> = (state: TargetState, incomingAction: Action) => {

    const action = incomingAction as KnownAction;

    switch (action.type) {

        case 'RECEIVE_TARGET': {

            return { ...state, hasTarget: true, target: action.payload };

        }

        case 'VOTE_TARGET': {

            var joinTargets: TargetModel[] = [];
            for (var i = 0; i < state.targets.length; i++) {

                var trg: TargetModel = state.targets[i];

                if (state.targets[i].Id == action.payload.Id) {

                    switch (trg.UserVote) {

                        case 1:
                            trg.VoteWrite--;
                            break;

                        case 2:
                            trg.VoteVery--;
                            break;

                        case 3:
                            trg.VoteGood--;
                            break;

                        case 4:
                            trg.VoteNot--;
                            break;

                        case 5:
                            trg.VoteOld--;
                            break;

                        case 6:
                            trg.VoteFake--;
                            break;

                    }

                    if (action.payload.Vote == trg.UserVote) {

                        trg.UserVote = 0;

                    } else if (action.payload.Vote != undefined) {

                        trg.UserVote = action.payload.Vote;

                        switch (trg.UserVote) {

                            case 1:
                                trg.VoteWrite++;
                                break;

                            case 2:
                                trg.VoteVery++;
                                break;

                            case 3:
                                trg.VoteGood++;
                                break;

                            case 4:
                                trg.VoteNot++;
                                break;

                            case 5:
                                trg.VoteOld++;
                                break;

                            case 6:
                                trg.VoteFake++;
                                break;

                        }

                    }

                }

                joinTargets.push(trg);

            }

            return { ...state, hasList: true, targets: joinTargets };

        }

        case 'RECEIVE_TARGET_LIST': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.Ini == 0)
                return { ...state, hasList: true, hasCategory: false, targets: action.payload.Targets, total: action.payload.Total };

            // Pega todos os targets atuais

            var joinTargets: TargetModel[] = [];
            for (var i = 0; i < state.targets.length; i++)
                joinTargets.push(state.targets[i]);

            // Agora cria vazios até o fim

            var totaljoin: number = action.payload.Ini + action.payload.Targets.length;
            for (var i = joinTargets.length; i < totaljoin; i++)
                joinTargets.push(new TargetModel());

            // Agora para cada item recebido altera a posição
            for (var i = action.payload.Ini; i < totaljoin; i++)
                joinTargets[i] = action.payload.Targets[i - action.payload.Ini];

            return { ...state, hasList: true, hasCategory: false, hasSearchList: false, targets: joinTargets, total: action.payload.Total };

        }

        case 'CHANGE_TARGET_LANGUAGE': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload == "")
                return { ...state };

            // Pega todos os targets atuais

            var joinTargets: TargetModel[] = [];
            for (var i = 0; i < state.targets.length; i++)
                if (state.targets[i].Id != action.payload)
                    joinTargets.push(state.targets[i]);

            return { ...state, targets: joinTargets,  };

        }

        case 'RECEIVE_TARGET_SEARCH': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.List != undefined) {

                if (action.payload.List.Ini == 0)
                    return { ...state, hasList: false, hasCategory: false, hasSearchList: true, targets: action.payload.List.Targets, total: action.payload.List.Total };

                // Pega todos os targets atuais

                var joinTargets: TargetModel[] = [];
                for (var i = 0; i < state.targets.length; i++)
                    joinTargets.push(state.targets[i]);

                // Agora cria vazios até o fim

                var totaljoin: number = action.payload.List.Ini + action.payload.List.Targets.length;
                for (var i = joinTargets.length; i < totaljoin; i++)
                    joinTargets.push(new TargetModel());

                // Agora para cada item recebido altera a posição
                for (var i = action.payload.List.Ini; i < totaljoin; i++)
                    joinTargets[i] = action.payload.List.Targets[i - action.payload.List.Ini];
                                
                return { ...state, hasList: true, hasCategory: false, targets: joinTargets, total: action.payload.List.Total, searchParameters: action.payload.SearchData || null };

            } else {

                return { ...state };

            }

        }

        case 'RECEIVE_TARGET_CATEGORY': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.Ini == 0)
                return { ...state, hasCategory: true, hasList: false, hasSearchList: false, title: action.payload.Title, description: action.payload.Description, targets: action.payload.Targets, total: action.payload.Total };
            
            // Pega todos os targets atuais

            var joinTargets: TargetModel[] = [];
            for (var i = 0; i < state.targets.length; i++)
                joinTargets.push(state.targets[i]);

            // Agora cria vazios até o fim

            var totaljoin: number = action.payload.Ini + action.payload.Targets.length;
            for (var i = joinTargets.length; i < totaljoin; i++)
                joinTargets.push(new TargetModel());

            // Agora para cada item recebido altera a posição
            for (var i = action.payload.Ini; i < totaljoin; i++)
                joinTargets[i] = action.payload.Targets[i - action.payload.Ini];

            return { ...state, hasCategory: true, hasList: false, title: action.payload.Title, description: action.payload.Description, targets: joinTargets, total: action.payload.Total };

        }

        case 'RECEIVE_EDIT_TARGET_BASE':
            return { ...state, hasEditBase: true, categories: action.payload.Categories, awards: action.payload.Awards };

        case 'RECEIVE_LINK':
            return { ...state, hasLinkInfo: true, link: action.payload };

        case 'CLEAR_TARGET_EDIT_BASE':
            return { ...state, hasEditBase: false, hasLinkInfo: false, hasSearchList: false };

        case 'CLEAR_TARGET':
            return { ...state, hasTarget: false };

        case 'CLEAR_TARGET_LIST':
            return { ...state, hasList: false, hasCategory: false, hasSearchList: false };

        case 'REGISTER_GRANT': {

            return { ...state };

        }

        default:
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;

};
