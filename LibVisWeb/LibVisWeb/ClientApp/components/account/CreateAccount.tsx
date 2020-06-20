import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import CreateAccountForm from './CreateAccountForm';
import ErrorMsg from '../../message/errormsg';
import Footer from '../footer/Footer';
import Header from '../header/Header';

type CreateAccountProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & RouteComponentProps<{}>;

class CreateAccount extends React.Component<CreateAccountProps, {}> {
    
    public render() {

        

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">

                    <CreateAccountForm />

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
)(CreateAccount as any) as typeof CreateAccount;
