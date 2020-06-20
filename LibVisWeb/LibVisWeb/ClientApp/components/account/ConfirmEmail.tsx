import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import ConfirmEmailForm from './ConfirmEmailForm';
import ErrorMsg from '../../message/errormsg';
import Footer from '../footer/Footer';
import Header from '../header/Header';
import * as Verify from '../../message/verify';

type ConfirmEmailFormProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & RouteComponentProps<{ code?: string, email?: string }>; 

class ConfirmEmail extends React.Component<ConfirmEmailFormProps, {}> {

    public render() {

        var code: string = "";
        var email: string = "";

        if (this.props.match.params.code != null && Verify.isValidCode(this.props.match.params.code)) 
            code = this.props.match.params.code;

        if (this.props.match.params.email != null && this.props.match.params.email.length < 1500 && Verify.isValidEmail(atob(this.props.match.params.email))) 
            email = atob(this.props.match.params.email);

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">

                    <ConfirmEmailForm code={code} email={email} />

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
)(ConfirmEmail as any) as typeof ConfirmEmail;
