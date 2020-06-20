import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import LoginForm from './LoginForm';
import ErrorMsg from '../../message/errormsg';
import Footer from '../footer/Footer';
import Header from '../header/Header';

type LoginProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & RouteComponentProps<{}>;

class Login extends React.Component<LoginProps, {}> {

    public render() {

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">

                    <LoginForm />

                    <ErrorMsg />

                </div>

                <Footer />

            </div>

        );

    }

}

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(Login as any) as typeof Login;
