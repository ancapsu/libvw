import * as React from 'react';
import { Link, RouteComponentProps, NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type AdminProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class Admin extends React.Component<AdminProps, {}> {

    componentWillMount() {

        if (!this.props.validToken || this.props.user == null || (this.props.user.Account.Profile < 7 && this.props.user.Account.Staff != 1)) {

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
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="wpb_wrapper">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.Administration)}</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="tdc-video-row">
                    <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                        <div className="post type-post status-publish format-standard has-post-thumbnail category-entrepreneurs">

                            <div className="td-post-header">

                                <NavLink to={"/manage-users"} className="btn-publish">{RacMsg.Get(RacMsg.Id.ManageUserPageTitle)}</NavLink>
                                <NavLink to={"/approve-request"} className="btn-publish">{RacMsg.Get(RacMsg.Id.ApproveRequests)}</NavLink>

                            </div>

                        </div>
                    </div>
                </div>

                <div className="container open-line"></div>
                <div className="container open-line"></div>

                <ErrorMsg />

                <Footer />

            </div>

        );

    }

}

export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, ...routerActions }
)(Admin as any) as typeof Admin;
