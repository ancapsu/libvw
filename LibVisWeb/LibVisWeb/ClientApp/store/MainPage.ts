import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { MainPageModel } from '../models/News';
import { AppThunkAction } from '.';
import { LoginResultModel } from '../models/Account';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface MainPageState {
    isLoading: boolean;
    mainPage: MainPageModel | null;
} 

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestMainPageAction {
    type: 'REQUEST_MAIN_PAGE';
}

interface ReceiveMainPageAction {
    type: 'RECEIVE_MAIN_PAGE';
    payload: MainPageModel;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestMainPageAction | ReceiveMainPageAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestMainPage: (): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null) {

            if (getState().mainPage.isLoading) {

                let fetchTask = fetch(`api/News/MainPage?token=${user.Token}`)
                    .then(response => response.json() as Promise<MainPageModel>)
                    .then(data => {
                        dispatch({ type: 'RECEIVE_MAIN_PAGE', payload: data });
                    });

                addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
                dispatch({ type: 'REQUEST_MAIN_PAGE' });

            }

        }

    }

};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: MainPageState = { isLoading: true, mainPage: null };

export const reducer: Reducer<MainPageState> = (state: MainPageState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_MAIN_PAGE':
            return { ...state, isLoading: false };
        case 'RECEIVE_MAIN_PAGE':
            return { ...state, isLoading: false, mainPage: action.payload };
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;

};
