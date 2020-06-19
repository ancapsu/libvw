import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import RecoverPasswordForm from './RecoverPasswordForm';
import ErrorMsg from '../../message/errormsg';
import Footer from '../Footer/Footer';
import Header from '../header/Header';
import * as Verify from '../../message/verify';

type RecoverPasswordProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & RouteComponentProps<{ code?: string, email?: string }>; 

class RecoverPassword extends React.Component<RecoverPasswordProps, {}> {

    public render() {

        var code: string = "";
        var email: string = "";

        if (this.props.match.params.code != null && Verify.isValidSessionCode(this.props.match.params.code))
            code = this.props.match.params.code;

        if (this.props.match.params.email != null && this.props.match.params.email.length < 1500 && Verify.isValidEmail(atob(this.props.match.params.email)))
            email = atob(this.props.match.params.email);

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">

                    <RecoverPasswordForm code={code} email={email} />

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
)(RecoverPassword as any) as typeof RecoverPassword;
