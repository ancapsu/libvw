import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import ChangePasswordForm from './ChangePasswordForm';
import ErrorMsg from '../../message/errormsg';
import Footer from '../footer/Footer';
import Header from '../header/Header';

type ChangePasswordProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & RouteComponentProps<{}>;

class ChangePassword extends React.Component<ChangePasswordProps, {}> {

    public render() {
                
        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">

                    <ChangePasswordForm />

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
)(ChangePassword as any) as typeof ChangePassword;
