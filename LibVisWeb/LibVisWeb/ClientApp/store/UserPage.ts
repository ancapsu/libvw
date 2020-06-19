import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { UserPageModel } from '../models/News';
import { toastr } from 'react-redux-toastr';
import RacMsg from '../message/racmsg';
import { AppThunkAction } from '.';
import { LoginResultModel } from '../models/Account';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UserPageState {
    isLoading: boolean;
    userPage: UserPageModel | null;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestUserPageAction {
    type: 'REQUEST_USER_PAGE';
}

interface ReceiveUserPageAction {
    type: 'RECEIVE_USER_PAGE';
    payload: UserPageModel;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestUserPageAction | ReceiveUserPageAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestUserPage: (id: string): AppThunkAction<KnownAction> => (dispatch, getState) => {

        var token: string = "";
        var user: LoginResultModel | null = getState().account.user;

        if (getState().account.validToken && user != null)
            token = user.Token;

        let fetchTask = fetch(`api/News/UserPage?token=${token}&id=${id}`)
            .then(response => response.json() as Promise<UserPageModel>)
            .then(data => {

                dispatch({ type: 'RECEIVE_USER_PAGE', payload: data });

            }).catch(e => {

                toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(e));

                for (let er in e.response.data.errors) {

                    toastr.error(RacMsg.Get(RacMsg.Id.Error), er);

                }

                dispatch({ type: 'REQUEST_USER_PAGE' });

            });

        addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
        dispatch({ type: 'REQUEST_USER_PAGE' });

    }

};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: UserPageState = { isLoading: true, userPage: null };

export const reducer: Reducer<UserPageState> = (state: UserPageState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_USER_PAGE':
            return { ...state, isLoading: false };
        case 'RECEIVE_USER_PAGE':
            return { ...state, isLoading: false, userPage: action.payload };
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;

};
