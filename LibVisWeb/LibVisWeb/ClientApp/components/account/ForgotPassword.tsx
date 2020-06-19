import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import ForgotPasswordForm from './ForgotPasswordForm';
import ErrorMsg from '../../message/errormsg';
import Footer from '../Footer/Footer';
import Header from '../header/Header';

type ForgotPasswordProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & RouteComponentProps<{}>;

class ForgotPassword extends React.Component<ForgotPasswordProps, {}> {

    public render() {

        

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">

                    <ForgotPasswordForm />

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
)(ForgotPassword as any) as typeof ForgotPassword;
