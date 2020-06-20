import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import EditArticleForm from './EditArticleForm';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';
import ManageUserForm from './ManageUserForm';

type ManageUserProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{ id: string }>;

class ManageUser extends React.Component<ManageUserProps, {}> {

    componentWillMount() {

        if (!this.props.validToken || this.props.user == null) {

            this.props.push('/login');

        }

    }

    public render() {
                
        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">

                        <ManageUserForm id={this.props.match.params.id} />

                        <ErrorMsg />

                    </div>
                </div>

                <Footer />

            </div>

        );

    }

}

export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, ...routerActions }
)(ManageUser as any) as typeof ManageUser;
