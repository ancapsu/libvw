import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import NewVideoForm from './NewVideoForm';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type NewVideoProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class NewVideo extends React.Component<NewVideoProps, {}> {


    componentWillMount() {

        if (!this.props.validToken || this.props.user == null) {

            this.props.push('/login');

        }

        if (!this.props.validToken && this.props.user != null && this.props.user.Account.Profile < 7) {

            this.props.push('/home');

        }

    }


    public render() {

        

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">

                        <NewVideoForm />

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
)(NewVideo as any) as typeof NewVideo;
