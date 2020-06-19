import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { HomePageModel, NewsLetterRegister } from '../models/News';
import { AppThunkAction, ApplicationState } from '.';
import { GenericStatusModel } from 'ClientApp/models/GenericStatus';
import { toastr } from 'react-redux-toastr';
import RacMsg from '../message/racmsg';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface HomePageState {
    homePage: HomePageModel;
    seqHomePage: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface ReceiveHomePageAction {
    type: 'RECEIVE_HOME_PAGE';    
    payload: HomePageModel;
}

// Sequencia
var seq_homepage: number = 0;

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = ReceiveHomePageAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {

    requestHomePage: (lang: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

        seq_homepage = seq_homepage + 1;

        let fetchTask = fetch(`api/News/HomePage?lang=${lang}&seq=${seq_homepage}`)
            .then(response => response.json() as Promise<HomePageModel>)
            .then(data => {

                dispatch({ type: 'RECEIVE_HOME_PAGE', payload: data });

            });

        addTask(fetchTask); 
        
    },

    registerNewsletter: (email: string): AppThunkAction<KnownAction> => () => {

        var data: NewsLetterRegister = new NewsLetterRegister();

        data.Type = 1;  // 1 = Email
        data.Data = email;
        
        let fetchTask = fetch(`api/News/NewsLetterRegister`, {
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
                    
                    toastr.info('Informação', 'Email registrado na newsletter. Para remover, clique no link enviado no próprio email');

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

};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: HomePageState = { seqHomePage: 0, homePage: { Videos: [], Statistics: [], Articles: [], Warnings: [], NumTargets: 0, NumApproval: 0, NumRevision: 0, NumNarration: 0, NumProduction: 0, Seq: 0 } };

export const reducer: Reducer<HomePageState> = (state: HomePageState, incomingAction: Action) => {

    const action = incomingAction as KnownAction;

    switch (action.type) {        

        case 'RECEIVE_HOME_PAGE': {

            if (state.seqHomePage < action.payload.Seq)
                return { ...state, seqHomePage: action.payload.Seq, homePage: action.payload };
            else
                return state;

        }

//        default:            
//            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;

};
