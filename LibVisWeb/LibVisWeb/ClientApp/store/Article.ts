import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { push, routerActions, LocationChangeAction } from 'react-router-redux'
import { TargetModel, RegisterVote } from '../models/Target';
import { UserIdModel, AuthorInfo, CategInfo, NewsCategory, NewsAward, RegisterGrant } from "../models/News";
import { ArticleModel, ArticleListModel, ArticleCategoryModel, EditArticleBaseModel, NewArticleModel, ChangeArticleModel, IncludeActionWithFile, SearchArticleActionModel, IncludeObservaton, ArticleActionObservationModel, ArticleCommentModel, RegisterPriority, RegisterPriorityPayload, ArticleActionModel, SearchArticleModel, Publish, MonthValueDiscrimination } from '../models/Article';
import { GenericIdModel, GenericStatusModel } from '../models/GenericStatus';
import { toastr } from 'react-redux-toastr';
import RacMsg from '../message/racmsg';
import { AppThunkAction } from '.';
import { LoginResultModel } from '../models/Account';

// -----------------
// Informações sobre o artigo ou artigos

export interface ArticleState {

    hasArticle: boolean;
    hasCategory: boolean;
    hasList: boolean;
    hasListApproval: boolean;
    hasListRevision: boolean;
    hasListNarration: boolean;
    hasListProduction: boolean;
    hasListPublish: boolean;
    hasListTranslation: boolean;
    hasEditBase: boolean;
    hasSearchList: boolean;
    hasValueSheet: boolean;

    monthValue: MonthValueDiscrimination | null;

    searchParameters: SearchArticleModel | null;

    article: ArticleModel | null;     

    title: string;
    description: string;

    ini: number;
    total: number;
    articles: ArticleModel[];

    targets: TargetModel[];
    categories: NewsCategory[];
    awards: NewsAward[];
    revisors: UserIdModel[];
    narrators: UserIdModel[];
    producers: UserIdModel[];

}

// -----------------
// Ações sobre o artigo

interface ReceiveArticleAction {
    type: 'RECEIVE_ARTICLE';
    payload: ArticleModel;
}

interface ReceiveArticleListAction {
    type: 'RECEIVE_ARTICLE_LIST';
    payload: ArticleListModel;
}

interface ReceiveArticleTranslationListAction {
    type: 'RECEIVE_ARTICLE_TRANSLATION_LIST';
    payload: ArticleListModel;
}

interface ReceiveArticleCategoryAction {
    type: 'RECEIVE_ARTICLE_CATEGORY';
    payload: ArticleCategoryModel;
}

interface ReceiveArticleSearchAction {
    type: 'RECEIVE_ARTICLE_SEARCH';
    payload: SearchArticleActionModel;
}

interface VoteArticleAction {
    type: 'VOTE_ARTICLE';
    payload: RegisterVote;
}

interface RegisterGrantAction {
    type: 'REGISTER_GRANT';
    payload: RegisterGrant;
}

interface ReceiveBaseEditAction {
    type: 'RECEIVE_EDIT_ARTICLE_BASE';
    payload: EditArticleBaseModel;
}

interface ArticleNoAction {
    type: 'ARTICLE_NO_ACTION';
}

interface ClearArticleEditBase {
    type: 'CLEAR_ARTICLE_EDIT_BASE';
}

interface ClearArticle {
    type: 'CLEAR_ARTICLE';
}

interface ClearArticleList {
    type: 'CLEAR_ARTICLE_LIST';
}

interface NewObservationAction {
    type: 'NEW_OBSERVATION_ACTION';
    payload: IncludeObservaton;
}

interface NewCommentAction {
    type: 'NEW_COMMENT_ACTION';
    payload: IncludeObservaton;
}

interface RemoveCommentAction {
    type: 'REMOVE_COMMENT_ACTION';
    payload: string;
}

interface ChangeCommentAction {
    type: 'CHANGE_COMMENT_ACTION';
    payload: IncludeObservaton;
}

interface RegisterPriorityAction {
    type: 'REGISTER_PRIORITY_ARTICLE';
    payload: RegisterPriorityPayload;
}

interface ReceiveValueDescriptionAction {
    type: 'RECEIVE_VALUE_DESCRIPTION';
    payload: MonthValueDiscrimination;
}

interface EraseActionAction {
    type: 'ERASE_ACTION';
    payload: string;
}

// ----------------
// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the declared type strings (and not any other arbitrary string).

type KnownAction = ReceiveArticleAction | ReceiveArticleListAction | ReceiveArticleCategoryAction | ReceiveArticleTranslationListAction | VoteArticleAction | RegisterGrantAction | ReceiveArticleSearchAction | ReceiveBaseEditAction | ArticleNoAction | ClearArticleEditBase | ClearArticle | ClearArticleList | NewObservationAction | NewCommentAction | RemoveCommentAction | ChangeCommentAction | RegisterPriorityAction | ReceiveValueDescriptionAction | EraseActionAction;

// ----------------
// Ações em cima dos artigos

export const actionCreators = {

    requestArticle: (id: string, lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_ARTICLE' });

        let fetchTask = fetch(`api/Article/Get?token=${token}&lang=${lang}&id=${id}`)
            .then(response => response.json() as Promise<ArticleModel>)
            .then(data => {

                dispatch({ type: 'RECEIVE_ARTICLE', payload: data });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), e);

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

                dispatch({ type: 'ARTICLE_NO_ACTION' });

            });

        addTask(fetchTask); 
        
    },

    requestArticleList: (lang: number, ini: number, max: number, sts: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_ARTICLE_LIST' });

        let fetchTask = fetch(`api/Article/List?token=${token}&lang=${lang}&ini=${ini}&max=${max}&sts=${sts}`)
            .then(response => response.json() as Promise<ArticleListModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_ARTICLE_LIST', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask); 

    },

    requestArticleListForTranslation: (lang: number, ini: number, max: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_ARTICLE_LIST' });

        let fetchTask = fetch(`api/Article/ListForTranslation?token=${token}&lang=${lang}&ini=${ini}&max=${max}`)
            .then(response => response.json() as Promise<ArticleListModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_ARTICLE_TRANSLATION_LIST', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },
    
    requestArticleByCategory: (lang: number, category: string, ini: number, max: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_ARTICLE_LIST' });

        let fetchTask = fetch(`api/Article/ByCategory?token=${token}&lang=${lang}&categ=${category}&ini=${ini}&max=${max}`)
            .then(response => response.json() as Promise<ArticleCategoryModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_ARTICLE_CATEGORY', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

                dispatch({ type: 'ARTICLE_NO_ACTION' });

            });

        addTask(fetchTask); 

    },

    requestArticleSearch: (ini: number, max: number, data: SearchArticleModel): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_ARTICLE_LIST' });

        let fetchTask = fetch(`api/Article/Search?token=${token}&ini=${ini}&max=${max}`, {
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
            .then(response => response.json() as Promise<ArticleListModel>)
            .then(res => {

                var data_res: SearchArticleActionModel = { SearchData: data, List: res };

                dispatch({ type: 'RECEIVE_ARTICLE_SEARCH', payload: data_res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    requestEditBase: (lang: number, targetId: string, articleId: string): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;
        
        if (getState().account.validToken && user != null) {

            token = user.Token;

            dispatch({ type: 'CLEAR_ARTICLE_EDIT_BASE' });

            let fetchTask = fetch(`api/Article/EditBase?token=${token}&lang=${lang}&articleId=${articleId}&targetId=${targetId}`)
                .then(response => response.json() as Promise<EditArticleBaseModel>)
                .then(res => {

                    dispatch({ type: 'RECEIVE_EDIT_ARTICLE_BASE', payload: res });

                }).catch(e => {

                    toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                    for (let er in e.response.data.errors) {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                    }

                });

            addTask(fetchTask); 

        }

    },

    changeArticle: (data: ChangeArticleModel): AppThunkAction<any> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token;

            dispatch({ type: 'CLEAR_TARGET' });

            let fetchTask = fetch(`api/Article/Change?token=${token}`, {
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

                        dispatch({ type: 'CLEAR_ARTICLE_LIST' });

                        dispatch(routerActions.push('/article/' + res.Id));

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

    registerNewArticle: (data: NewArticleModel): AppThunkAction<any> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token;

            let fetchTask = fetch(`api/Article/Register?token=${token}`, {
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

                        dispatch({ type: 'CLEAR_ARTICLE_LIST' });

                        dispatch({ type: 'CLEAR_ARTICLE_EDIT_BASE' }); 
                        
                        dispatch(routerActions.push('/article-list'));

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
    
    changeStatus: (articleId: string, status: number, info: string): AppThunkAction<any> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null)
            token = user.Token;

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para alterar o status de artigos");

        } else {

            dispatch({ type: 'CLEAR_ARTICLE_LIST' });

            var data: Publish = { Id: articleId, Status: status, Info: info };

            let fetchTask = fetch(`api/Article/Publish?token=${token}`, {
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

                        if (status == 1)
                            dispatch(routerActions.push('/article-list-approval'));
                        else if (status == 2)
                            dispatch(routerActions.push('/article-list-revision'));
                        else if (status == 3)
                            dispatch(routerActions.push('/article-list-narration'));
                        else if (status == 4)
                            dispatch(routerActions.push('/article-list-production'));
                        else if (status == 5)
                            dispatch(routerActions.push('/article-list-publish'));
                        else
                            dispatch(routerActions.push('/article-list'));

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
    
    changeStatusWithFiles: (data: IncludeActionWithFile): AppThunkAction<any> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null)
            token = user.Token;

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para definir narração");

        } else {

            dispatch({ type: 'CLEAR_ARTICLE_LIST' });

            var s: string = JSON.stringify(data);
            var n: number = s.length;
            
            let fetchTask = fetch(`api/Article/PublishWithFiles?token=${token}`, {
                body: s, // must match 'Content-Type' header
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
                .then(response => {
                    
                    return response.json() as Promise<GenericIdModel>;

                })
                .then(res => {

                    if (res.Result == 0) {

                        if (data.Status == 1)
                            dispatch(routerActions.push('/article-list-approval'));
                        else if (data.Status == 2)
                            dispatch(routerActions.push('/article-list-revision'));
                        else if (data.Status == 3)
                            dispatch(routerActions.push('/article-list-narration'));
                        else if (data.Status == 4)
                            dispatch(routerActions.push('/article-list-production'));
                        else if (data.Status == 5)
                            dispatch(routerActions.push('/article-list-publish'));
                        else
                            dispatch(routerActions.push('/article-list'));

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

    registerArticleVote: (id: string, vote: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null)
            token = user.Token;

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para votar");

        } else {

            var data: RegisterVote = { Id: id, Vote: vote };

            let fetchTask = fetch(`api/Article/RegisterVote?token=${token}`, {
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

                        dispatch({ type: 'VOTE_ARTICLE', payload: data });

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

    registerGrant: (actionId: string, lang: number, awardId: string, add: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null)
            token = user.Token;

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para definir prêmios");

        } else {

            dispatch({ type: 'CLEAR_ARTICLE_LIST' });

            var data: RegisterGrant = { ActionId: actionId, AwardId: awardId, Add: add, Lang: lang };

            let fetchTask = fetch(`api/Article/RegisterGrant?token=${token}`, {
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
    
    registerNewObservation: (id: string, obs: string): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;
        var username: string = "";
        var userid: string = "";

        if (user != null) {
            token = user.Token;
            username = user.Account.Name;
            userid = user.Account.Id;
        }

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para votar");

        } else {

            var data: IncludeObservaton = { Id: id, CommentId: NewGuid(), Comment: obs, UserName: username, UserId: userid, Included: new Date().toDateString() };

            let fetchTask = fetch(`api/Article/IncludeObservation?token=${token}`, {
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

                        dispatch({ type: 'NEW_OBSERVATION_ACTION', payload: data });

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

    registerNewComment: (id: string, obs: string): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;
        var username: string = "";
        var userid: string = "";

        if (user != null) {
            token = user.Token;
            username = user.Account.Name;
            userid = user.Account.Id;
        }

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para votar");

        } else {

            var data: IncludeObservaton = { Id: id, CommentId: NewGuid(), Comment: obs, UserName: username, UserId: userid, Included: new Date().toDateString() };

            let fetchTask = fetch(`api/Article/IncludeComment?token=${token}`, {
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

                        dispatch({ type: 'NEW_COMMENT_ACTION', payload: data });

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

    changedComment: (id: string, obs: string): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;
        var username: string = "";
        var userid: string = "";

        if (user != null) {
            token = user.Token;
            username = user.Account.Name;
            userid = user.Account.Id;
        }

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para votar");

        } else {

            var data: IncludeObservaton = { Id: id, CommentId: id, Comment: obs, UserName: username, UserId: userid, Included: new Date().toDateString() };

            let fetchTask = fetch(`api/Article/ChangeComment?token=${token}`, {
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

                        dispatch({ type: 'CHANGE_COMMENT_ACTION', payload: data });

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
    
    removeComment: (id: string): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;
        
        if (user != null) 
            token = user.Token;
            
        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para votar");

        } else {

            let fetchTask = fetch(`api/Article/RemoveComment?token=${token}&id=${id}`, {
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
                .then(response => response.json() as Promise<GenericStatusModel>)
                .then(res => {

                    if (res.Result != 0) {

                        toastr.error(RacMsg.Get(res.Result), res.ResultComplement);

                    } else {

                        dispatch({ type: 'REMOVE_COMMENT_ACTION', payload: id });

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
    
    registerPriority: (id: string, prio: number, def: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var name: string = "";
        var idusr: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {
            token = user.Token;
            name = user.Account.Name;
            idusr = user.Account.Id;
        }

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para votar");

        } else {

            var data: RegisterPriority = { Priority: prio, Define: def, Id: id };
            var data_ret: RegisterPriorityPayload = { Priority: prio, Define: def, Id: id, UserId: idusr, UserName: name };

            let fetchTask = fetch(`api/Article/RegisterPriority?token=${token}`, {
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

                        dispatch({ type: 'REGISTER_PRIORITY_ARTICLE', payload: data_ret });

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

    eraseAction: (id: string): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (user != null) 
            token = user.Token;

        if (token == "") {

            toastr.error(RacMsg.Get(RacMsg.Id.Error), "Você precisa estar logado para apagar ação");

        } else {

            let fetchTask = fetch(`api/Article/RemoveAction?token=${token}&id=${id}`, {
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
                .then(response => response.json() as Promise<GenericStatusModel>)
                .then(res => {

                    if (res.Result != 0) {

                        toastr.error(RacMsg.Get(res.Result), res.ResultComplement);

                    } else {

                        dispatch({ type: 'ERASE_ACTION', payload: id });

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

    requestValueDescription: (year: number, month: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        let fetchTask = fetch(`api/Article/GetMonthValueDiscrimination?token=${token}&year=${year}&month=${month}`)
            .then(response => response.json() as Promise<MonthValueDiscrimination>)
            .then(data => {

                dispatch({ type: 'RECEIVE_VALUE_DESCRIPTION', payload: data });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), e);

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

};

function NewGuid() : string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

// ----------------
// Pega o inicial

const unloadedState: ArticleState = { hasArticle: false, hasCategory: false, hasList: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, hasSearchList: false, hasEditBase: false, hasValueSheet: false, monthValue: null, article: null, searchParameters: null, title: "", description: "", ini: 0, total: 0, articles: [], targets: [], categories: [], awards: [], revisors: [], narrators: [], producers: [] };

// ----------------
// Faz o redux

export const reducer: Reducer<ArticleState> = (state: ArticleState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {

        case 'ARTICLE_NO_ACTION':
            return { ...state };

        case 'RECEIVE_ARTICLE': {

            return { ...state, hasArticle: true, article: action.payload };

        }

        case 'RECEIVE_VALUE_DESCRIPTION': {

            return { ...state, hasValueSheet: true, monthValue: action.payload };

        }

        case 'VOTE_ARTICLE': {

            var joinArticle: ArticleModel[] = [];
            for (var i = 0; i < state.articles.length; i++) {

                var art: ArticleModel = state.articles[i];

                if (state.articles[i].Id == action.payload.Id) {

                    switch (art.UserVote) {

                        case 1:
                            art.VoteApprove--;
                            break;

                        case 2:
                            art.VoteNot--;
                            break;

                        case 3:
                            art.VoteAlready--;
                            break;

                        case 4:
                            art.VotePoorly--;
                            break;

                    }

                    if (action.payload.Vote == art.UserVote) {

                        art.UserVote = 0;

                    } else if (action.payload.Vote != undefined) {

                        art.UserVote = action.payload.Vote;

                        switch (art.UserVote) {

                            case 1:
                                art.VoteApprove++;
                                break;

                            case 2:
                                art.VoteNot++;
                                break;

                            case 3:
                                art.VoteAlready++;
                                break;

                            case 4:
                                art.VotePoorly++;
                                break;

                        }

                    }

                }

                if (art.VoteApprove - art.VoteNot < 5)
                    joinArticle.push(art);

            }

            return { ...state, hasList: true, articles: joinArticle };

        }

        case 'RECEIVE_ARTICLE_LIST': {
            
            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.Ini == 0) {

                if (action.payload.Sts == 1)
                    return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: true, hasListProduction: false, hasListPublish: false, hasListTranslation: false,  articles: action.payload.Articles, total: action.payload.Total };

                if (action.payload.Sts == 2)
                    return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: true, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, articles: action.payload.Articles, total: action.payload.Total };

                if (action.payload.Sts == 3)
                    return { ...state, hasList: false, hasCategory: false, hasListNarration: true, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, articles: action.payload.Articles, total: action.payload.Total };

                if (action.payload.Sts == 4)
                    return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: true, hasListPublish: false, hasListTranslation: false, articles: action.payload.Articles, total: action.payload.Total };

                if (action.payload.Sts == 5)
                    return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: true, hasListTranslation: false, articles: action.payload.Articles, total: action.payload.Total };

                return { ...state, hasList: true, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListPublish: false, hasListTranslation: false, articles: action.payload.Articles, total: action.payload.Total };

            }

            // Pega todos os targets atuais

            var joinArticles: ArticleModel[] = [];
            for (var i = 0; i < state.articles.length; i++)
                joinArticles.push(state.articles[i]);

            // Agora cria vazios até o fim

            var totaljoin: number = action.payload.Ini + action.payload.Articles.length;
            for (var i = joinArticles.length; i < totaljoin; i++)
                joinArticles.push(new ArticleModel());

            // Agora para cada item recebido altera a posição
            for (var i = action.payload.Ini; i < totaljoin; i++)
                joinArticles[i] = action.payload.Articles[i - action.payload.Ini];
            
            if (action.payload.Sts == 1)
                return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: true, hasListProduction: false, hasListPublish: false, hasListTranslation: false, articles: joinArticles, total: action.payload.Total };

            if (action.payload.Sts == 2)
                return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: true, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, articles: joinArticles, total: action.payload.Total };

            if (action.payload.Sts == 3)
                return { ...state, hasList: false, hasCategory: false, hasListNarration: true, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, articles: joinArticles, total: action.payload.Total };

            if (action.payload.Sts == 4)
                return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: true, hasListPublish: false, hasListTranslation: false, articles: joinArticles, total: action.payload.Total };

            if (action.payload.Sts == 5)
                return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: true, hasListTranslation: false, articles: joinArticles, total: action.payload.Total };

            return { ...state, hasList: true, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, articles: joinArticles, total: action.payload.Total };

        }

        case 'RECEIVE_ARTICLE_TRANSLATION_LIST': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.Ini == 0) {

                return { ...state, hasList: true, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListPublish: false, hasListTranslation: true, articles: action.payload.Articles, total: action.payload.Total };

            }

            // Pega todos os targets atuais

            var joinArticles: ArticleModel[] = [];
            for (var i = 0; i < state.articles.length; i++)
                joinArticles.push(state.articles[i]);

            // Agora cria vazios até o fim

            var totaljoin: number = action.payload.Ini + action.payload.Articles.length;
            for (var i = joinArticles.length; i < totaljoin; i++)
                joinArticles.push(new ArticleModel());

            // Agora para cada item recebido altera a posição
            for (var i = action.payload.Ini; i < totaljoin; i++)
                joinArticles[i] = action.payload.Articles[i - action.payload.Ini];

            return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: true, articles: joinArticles, total: action.payload.Total };

        }

        case 'RECEIVE_ARTICLE_CATEGORY': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.Ini == 0)
                return { ...state, hasCategory: true, hasList: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, title: action.payload.Title, description: action.payload.Description, articles: action.payload.Articles, total: action.payload.Total };

            // Pega todos os targets atuais

            var joinArticles: ArticleModel[] = [];
            for (var i = 0; i < state.articles.length; i++)
                joinArticles.push(state.articles[i]);

            // Agora cria vazios até o fim

            var totaljoin: number = action.payload.Ini + action.payload.Articles.length;
            for (var i = joinArticles.length; i < totaljoin; i++)
                joinArticles.push(new ArticleModel());

            // Agora para cada item recebido altera a posição
            for (var i = action.payload.Ini; i < totaljoin; i++)
                joinArticles[i] = action.payload.Articles[i - action.payload.Ini];

            return { ...state, hasCategory: true, hasList: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false, title: action.payload.Title, description: action.payload.Description, articles: joinArticles, total: action.payload.Total };

        }

        case 'RECEIVE_ARTICLE_SEARCH': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.List != undefined) {

                if (action.payload.List.Ini == 0)
                    return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasSearchList: true, hasListTranslation: false, articles: action.payload.List.Articles, total: action.payload.List.Total };

                // Pega todos os targets atuais

                var joinArticles: ArticleModel[] = [];
                for (var i = 0; i < state.articles.length; i++)
                    joinArticles.push(state.articles[i]);

                // Agora cria vazios até o fim

                var totaljoin: number = action.payload.List.Ini + action.payload.List.Articles.length;
                for (var i = joinArticles.length; i < totaljoin; i++)
                    joinArticles.push(new ArticleModel());

                // Agora para cada item recebido altera a posição
                for (var i = action.payload.List.Ini; i < totaljoin; i++)
                    joinArticles[i] = action.payload.List.Articles[i - action.payload.List.Ini];

                return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasSearchList: true, hasListTranslation: false, articles: joinArticles, total: action.payload.List.Total, searchParameters: action.payload.SearchData || null };

            } else {

                return { ...state };

            }

        }
            
        case 'RECEIVE_EDIT_ARTICLE_BASE': {

            return { ...state, hasEditBase: true, targets: action.payload.Targets, categories: action.payload.Categories, awards: action.payload.Awards, revisors: action.payload.Revisors, narrators: action.payload.Narrators, producers: action.payload.Producers };

        }

        case 'CLEAR_ARTICLE_EDIT_BASE':
            return { ...state, hasEditBase: false };

        case 'CLEAR_ARTICLE':
            return { ...state, hasArticle: false };

        case 'CLEAR_ARTICLE_LIST':
            return { ...state, hasList: false, hasCategory: false, hasListNarration: false, hasListRevision: false, hasListApproval: false, hasListProduction: false, hasListPublish: false, hasListTranslation: false};

        case 'REGISTER_GRANT': {

            return { ...state };

        }

        case 'NEW_OBSERVATION_ACTION': {

            // Pega todos os targets atuais

            if (state.article != null) {

                var newArticle: ArticleModel = { ...state.article }

                for (var i = 0; i < newArticle.Actions.length; i++) {

                    if (newArticle.Actions[i].Id == action.payload.Id) {

                        var obs: ArticleActionObservationModel = new ArticleActionObservationModel();

                        obs.Included = action.payload.Included || "";
                        obs.IncludedBy = action.payload.UserName || "";
                        obs.Observation = action.payload.Comment || "";
                        obs.IncludedById = action.payload.UserId || "";

                        newArticle.Actions[i].Observations.push(obs);
                    }

                }

                return { ...state, article: newArticle };

            } else {

                return { ...state };

            }

        }
            
        case 'NEW_COMMENT_ACTION': {

            // Pega todos os targets atuais

            if (state.article != null) {

                var newArticle: ArticleModel = { ...state.article }

                var cmt: ArticleCommentModel = new ArticleCommentModel();

                cmt.Id = action.payload.CommentId || "";
                cmt.Included = action.payload.Included || "";
                cmt.IncludedBy = action.payload.UserName || "";
                cmt.Comment = action.payload.Comment || "";
                cmt.IncludedById = action.payload.UserId || "";

                newArticle.Comments.push(cmt);

                return { ...state, article: newArticle };

            } else {

                return { ...state };

            }

        }

        case 'CHANGE_COMMENT_ACTION': {

            // Pega todos os targets atuais

            if (state.article != null) {

                var newArticle: ArticleModel = { ...state.article }

                newArticle.Comments = [];

                var cmt: ArticleCommentModel = new ArticleCommentModel();

                cmt.Id = action.payload.Id || "";
                cmt.Included = action.payload.Included || "";
                cmt.IncludedBy = action.payload.UserName || "";
                cmt.Comment = action.payload.Comment || "";
                cmt.IncludedById = action.payload.UserId || "";

                for (var i = 0; i < state.article.Comments.length; i++) {

                    if (state.article.Comments[i].Id != action.payload.CommentId)
                        newArticle.Comments.push(state.article.Comments[i]);
                    else
                        newArticle.Comments.push(cmt);

                }

                return { ...state, article: newArticle };

            } else {

                return { ...state };

            }

        }

        case 'REMOVE_COMMENT_ACTION': {

            // Pega todos os targets atuais

            if (state.article != null) {

                var newArticle: ArticleModel = { ...state.article }

                newArticle.Comments = [];

                for (var i = 0; i < state.article.Comments.length; i++) {

                    if (state.article.Comments[i].Id != action.payload)
                        newArticle.Comments.push(state.article.Comments[i]);

                }

                return { ...state, article: newArticle };

            } else {

                return { ...state };

            }

        }

        case 'REGISTER_PRIORITY_ARTICLE': {
            
            // Pega todos os artigos atuais
            
            var joinArticle: ArticleModel[] = [];
            for (var i = 0; i < state.articles.length; i++) {

                var art: ArticleModel = state.articles[i];

                if (state.articles[i].Id == action.payload.Id) {
                                        
                    if (action.payload.Define == 1) {

                        switch (action.payload.Priority) {

                            case 2:
                                art = { ...art, beingRevised: action.payload.UserId, beingRevisedName: action.payload.UserName };
                                break;

                            case 3:
                                art = { ...art, beingNarrated: action.payload.UserId, beingNarratedName: action.payload.UserName };
                                break;

                            case 4:
                                art = { ...art, beingProduced: action.payload.UserId, beingProducedName: action.payload.UserName };
                                break;

                        }

                    } else {

                        switch (action.payload.Priority) {

                            case 2:
                                art = { ...art, beingRevised: "", beingRevisedName: "" };
                                break;

                            case 3:
                                art = { ...art, beingNarrated: "", beingNarratedName: "" };
                                break;

                            case 4:
                                art = { ...art, beingProduced: "", beingProducedName: "" };
                                break;

                        }

                    }

                }

                joinArticle.push(art);

            }

            return { ...state, articles: joinArticle };

        }


        case 'ERASE_ACTION': {

            // Pega todos os artigos atuais

            var joinArticle: ArticleModel[] = [];
            for (var i = 0; i < state.articles.length; i++) {

                var art: ArticleModel = state.articles[i];
                var joinAction: ArticleActionModel [] = [];

                for (var j = 0; j < art.Actions.length; j++) {

                    var act: ArticleActionModel = art.Actions[j];

                    if (act.Id != action.payload)
                        joinAction.push(act);

                }

                art = { ...art, Actions: joinAction };

                joinArticle.push(art);

            }

            return { ...state, articles: joinArticle };

        }

        default:            
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;

};
