import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { push, routerActions, LocationChangeAction } from 'react-router-redux'
import { LoginResultModel } from '../models/Account';
import { YoutubeModel, YoutubeResultModel, VideoModel, VideoListModel, VideoCategoryModel, NewVideoModel, EditVideoBaseModel, ChangeVideoModel, SearchVideoModel, SearchVideoActionModel } from '../models/Video';
import { NewsCategory } from '../models/News';
import { toastr } from 'react-redux-toastr';
import RacMsg from '../message/racmsg';
import { AppThunkAction } from '.';
import { GenericIdModel } from '../models/GenericStatus';

// -----------------
// Informações sobre a pauta ou as pautas

export interface VideoState {

    hasVideo: boolean;
    hasCategory: boolean;
    hasList: boolean;
    hasEditBase: boolean;
    hasYoutubeInfo: boolean;
    hasSearchList: boolean;

    searchParameters: SearchVideoModel | null;

    video: VideoModel | null;

    title: string;
    description: string;

    ini: number;
    total: number;
    videos: VideoModel[];

    categories: NewsCategory[];
    youtube: YoutubeResultModel | null;

}

// -----------------
// Ações sobre a pauta


interface ReceiveVideoAction {
    type: 'RECEIVE_VIDEO';
    payload: VideoModel;
}

interface ReceiveVideoListAction {
    type: 'RECEIVE_VIDEO_LIST';
    payload: VideoListModel;
}

interface ReceiveYoutubeInfoAction {
    type: 'RECEIVE_YOUTUBE';
    payload: YoutubeResultModel;
}

interface ReceiveVideoCategoryAction {
    type: 'RECEIVE_VIDEO_CATEGORY';
    payload: VideoCategoryModel;
}

interface ReceiveVideoSearchAction {
    type: 'RECEIVE_VIDEO_SEARCH';
    payload: SearchVideoActionModel;
}

interface ReceiveBaseEditAction {
    type: 'RECEIVE_EDIT_VIDEO_BASE';
    payload: EditVideoBaseModel;
}

interface VideoNoAction {
    type: 'VIDEO_NO_ACTION';
}

interface ClearVideoEditBase {
    type: 'CLEAR_VIDEO_EDIT_BASE';
}

interface ClearVideo {
    type: 'CLEAR_VIDEO';
}

interface ClearVideoList {
    type: 'CLEAR_VIDEO_LIST';
}


// ----------------
// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the declared type strings (and not any other arbitrary string).

type KnownAction = ReceiveVideoAction | ReceiveVideoListAction | ReceiveYoutubeInfoAction | ReceiveVideoCategoryAction | ReceiveBaseEditAction | ReceiveVideoSearchAction | ReceiveBaseEditAction | VideoNoAction | ClearVideoEditBase | ClearVideo | ClearVideoList;

// ----------------
// Ações em cima das pautas

export const actionCreators = {

    requestVideo: (id: string, lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_VIDEO' });

        let fetchTask = fetch(`api/Video/Get?token=${token}&lang=${lang}&id=${id}`, {
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
            .then(response => response.json() as Promise<VideoModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_VIDEO', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

                dispatch({ type: 'VIDEO_NO_ACTION' });

            });

        addTask(fetchTask); 
        
    },

    changeVideo: (data: ChangeVideoModel): AppThunkAction<any> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token;

            let fetchTask = fetch(`api/Video/Change?token=${token}`, {
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

                        dispatch({ type: 'CLEAR_VIDEO_LIST' });

                        dispatch(routerActions.push('/video/' + res.Id));

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
    
    requestVideoList: (lang: number, ini: number, max: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_VIDEO_LIST' });

        let fetchTask = fetch(`api/Video/List?token=${token}&lang=${lang}&ini=${ini}&max=${max}`)
            .then(response => response.json() as Promise<VideoListModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_VIDEO_LIST', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask); 

    },

    requestVideoCategory: (lang: number, category: string, ini: number, max: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_VIDEO_LIST' });

        let fetchTask = fetch(`api/Video/ByCategory?token=${token}&lang=${lang}&categ=${category}&ini=${ini}&max=${max}`)
            .then(response => response.json() as Promise<VideoCategoryModel>)
            .then(res => {

                dispatch({ type: 'RECEIVE_VIDEO_CATEGORY', payload: res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask); 

    },

    requestVideoSearch: (ini: number, max: number, data: SearchVideoModel): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        dispatch({ type: 'CLEAR_VIDEO_LIST' });

        let fetchTask = fetch(`api/Video/Search?token=${token}&ini=${ini}&max=${max}`, {
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
            .then(response => response.json() as Promise<VideoListModel>)
            .then(res => {

                var data_res: SearchVideoActionModel = { SearchData: data, List: res };

                dispatch({ type: 'RECEIVE_VIDEO_SEARCH', payload: data_res });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

            });

        addTask(fetchTask);

    },

    requestYoutubeInfo: (link: string, lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token;
            var data: YoutubeModel = { Link: link, Lang: lang };

            let fetchTask = fetch(`api/Video/Link?token=${token}`, {
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
                .then(response => response.json() as Promise<YoutubeResultModel>)
                .then(res => {

                    if (res.Result == 0) {

                        dispatch({ type: 'RECEIVE_YOUTUBE', payload: res });

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

    requestEditBase: (lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null) {

            token = user.Token;

            dispatch({ type: 'CLEAR_VIDEO_EDIT_BASE' });

            let fetchTask = fetch(`api/Video/EditBase?token=${token}&lang=${lang}`)
                .then(response => response.json() as Promise<EditVideoBaseModel>)
                .then(res => {

                    dispatch({ type: 'RECEIVE_EDIT_VIDEO_BASE', payload: res });

                }).catch(e => {

                    toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                    for (let er in e.response.data.errors) {

                        toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                    }

                    dispatch({ type: 'VIDEO_NO_ACTION' });

                });

            addTask(fetchTask);

        }

    },

    registerNewVideo: (data: NewVideoModel): AppThunkAction<any> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (user != null) {

            var token: string = user.Token;

            let fetchTask = fetch(`api/Video/Register?token=${token}`, {
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

                        dispatch({ type: 'CLEAR_VIDEO_LIST' });

                        dispatch({ type: 'CLEAR_VIDEO_EDIT_BASE' });

                        dispatch(routerActions.push('/video/' + res.Id));

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

const unloadedState: VideoState = { hasVideo: false, hasCategory: false, hasList: false, hasSearchList: false, hasEditBase: false, hasYoutubeInfo: false, video: null, searchParameters: null, title: "", description: "", ini: 0, total: 0, videos: [], categories: [], youtube: null };


export const reducer: Reducer<VideoState> = (state: VideoState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {

        case 'VIDEO_NO_ACTION':
            return { ...state };

        case 'RECEIVE_VIDEO': {

            return { ...state, hasVideo: true, video: action.payload };

        }

        case 'RECEIVE_VIDEO_LIST': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.Ini == 0)
                return { ...state, hasList: true, hasCategory: false, videos: action.payload.Videos, total: action.payload.Total };

            // Pega todos os targets atuais

            var joinVideos: VideoModel[] = [];
            for (var i = 0; i < state.videos.length; i++)
                joinVideos.push(state.videos[i]);

            // Agora cria vazios até o fim

            var totaljoin: number = action.payload.Ini + action.payload.Videos.length;
            for (var i = joinVideos.length; i < totaljoin; i++)
                joinVideos.push(new VideoModel());

            // Agora para cada item recebido altera a posição
            for (var i = action.payload.Ini; i < totaljoin; i++)
                joinVideos[i] = action.payload.Videos[i - action.payload.Ini];

            return { ...state, hasList: true, hasCategory: false, videos: joinVideos, total: action.payload.Total };

        }

        case 'RECEIVE_VIDEO_CATEGORY': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.Ini == 0)
                return { ...state, hasCategory: true, hasList: false, title: action.payload.Title, description: action.payload.Description, videos: action.payload.Videos, total: action.payload.Total };
            
            // Pega todos os targets atuais

            var joinVideos: VideoModel[] = [];
            for (var i = 0; i < state.videos.length; i++)
                joinVideos.push(state.videos[i]);

            // Agora cria vazios até o fim

            var totaljoin: number = action.payload.Ini + action.payload.Videos.length;
            for (var i = joinVideos.length; i < totaljoin; i++)
                joinVideos.push(new VideoModel());

            // Agora para cada item recebido altera a posição
            for (var i = action.payload.Ini; i < totaljoin; i++)
                joinVideos[i] = action.payload.Videos[i - action.payload.Ini];

            return { ...state, hasCategory: true, hasList: false, title: action.payload.Title, description: action.payload.Description, videos: joinVideos, total: action.payload.Total };

        }

        case 'RECEIVE_VIDEO_SEARCH': {

            // Se tá retornando do início, é sempre do zero, ignora o que tá atrás

            if (action.payload.List != undefined) {

                if (action.payload.List.Ini == 0)
                    return { ...state, hasList: false, hasCategory: false, hasSearchList: true, videos: action.payload.List.Videos, total: action.payload.List.Total };

                // Pega todos os targets atuais

                var joinVideos: VideoModel[] = [];
                for (var i = 0; i < state.videos.length; i++)
                    joinVideos.push(state.videos[i]);

                // Agora cria vazios até o fim

                var totaljoin: number = action.payload.List.Ini + action.payload.List.Videos.length;
                for (var i = joinVideos.length; i < totaljoin; i++)
                    joinVideos.push(new VideoModel());

                // Agora para cada item recebido altera a posição
                for (var i = action.payload.List.Ini; i < totaljoin; i++)
                    joinVideos[i] = action.payload.List.Videos[i - action.payload.List.Ini];

                return { ...state, hasCategory: false, hasList: false, hasSearchList: true, videos: joinVideos, total: action.payload.List.Total };

            } else {

                return { ...state };

            }

        }

        case 'RECEIVE_EDIT_VIDEO_BASE':
            return { ...state, hasEditBase: true, categories: action.payload.Categories };

        case 'RECEIVE_YOUTUBE':
            return { ...state, hasYoutubeInfo: true, youtube: action.payload };

        case 'CLEAR_VIDEO_EDIT_BASE':
            return { ...state, hasEditBase: false, hasYoutubeInfo: false };

        case 'CLEAR_VIDEO':
            return { ...state, hasVideo: false };

        case 'CLEAR_VIDEO_LIST':
            return { ...state, hasList: false, hasCategory: false };

        default:
            const exhaustiveCheck: never = action;

    }

    return state || unloadedState;

};
