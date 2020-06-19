import * as ReduxForm from 'redux-form';
import * as Toastr from 'react-redux-toastr'
import * as Account from './Account';
import * as HomePage from './HomePage';
import * as MainPage from './MainPage';
import * as UserPage from './UserPage';
import * as Article from './Article';
import * as Target from './Target';
import * as Video from './Video';

// The top-level state object
export interface ApplicationState {
    account: Account.AccountState;
    article: Article.ArticleState;
    target: Target.TargetState;
    video: Video.VideoState;
    homePage: HomePage.HomePageState;
    mainPage: MainPage.MainPageState;
    userPage: UserPage.UserPageState;
    form: ReduxForm.FormState;
    toastr: Toastr.ToastrState;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    account: Account.reducer,
    article: Article.reducer,
    target: Target.reducer,
    video: Video.reducer,
    homePage: HomePage.reducer,
    mainPage: MainPage.reducer,
    userPage: UserPage.reducer,
    form: ReduxForm.reducer,
    toastr: Toastr.reducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
