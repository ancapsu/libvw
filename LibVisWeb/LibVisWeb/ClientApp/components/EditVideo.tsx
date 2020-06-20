import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import EditVideoForm from './EditVideoForm';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type EditTargetProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{ id: string }>;

class EditVideo extends React.Component<EditTargetProps, {}> {

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

                        <EditVideoForm id={this.props.match.params.id} />

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
)(EditVideo as any) as typeof EditVideo;
