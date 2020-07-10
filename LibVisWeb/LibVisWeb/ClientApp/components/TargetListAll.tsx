﻿import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { routerActions } from 'react-router-redux';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as TargetStore from '../store/Target';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import ShowTargetRef from './ShowTargetRef';
import WaitPanel from './common/WaitPanel';

type TargetListAllProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class TargetListAll extends React.Component<TargetListAllProps, {}> {

    contentReady: boolean;

    constructor(props: TargetListAllProps, context: any) {

        super(props, context);

        this.LoadMore = this.LoadMore.bind(this);
        this.contentReady = false;

    }

    componentDidMount() {

        this.props.requestTargetListAll(this.props.lang, 0, 10);

    }

    componentWillReceiveProps(nextProps: TargetListAllProps) {

        if (nextProps.hasList) {

            this.contentReady = true;

        }

    }

    LoadMore() {

        if (this.props.hasList) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestTargetListAll(this.props.lang, this.props.targets.length, 10);

        }

    }

    public render() {

        return (

            <div>

                <Header />

                <WaitPanel isContentReady={this.contentReady} />

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="wpb_wrapper">
                                <div className="td_block_wrap">
                                    <div className="td-block-title-wrap">
                                        <h4 className="td-block-title">
                                            <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.CompleteListOfAgendas)}</span>
                                        </h4>
                                    </div>

                                    <div className="row">

                                        <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                            {RacMsg.Get(RacMsg.Id.ListOfAllRegisteredAgendas)}
                                        </div>

                                        <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                            <NavLink to={'/new-target'} className="btn-create-new float-right">{RacMsg.Get(RacMsg.Id.SuggestNewAgenda)}</NavLink>
                                        </div>

                                    </div>

                                    <div className="container open-line"></div>

                                    <div className="container open-line"></div>

                                    <div className="td_block_inner td-column-2">

                                        {

                                            this.props.targets.map((n, idx) => {

                                                return (

                                                    <ShowTargetRef index={idx} />

                                                );

                                            })

                                        }

                                    </div>

                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>

                                    <div className="td_block_inner td-column-2">

                                        {this.props.targets.length < this.props.total &&

                                            <a onClick={this.LoadMore} className="btn-carregar-mais">{RacMsg.Get(RacMsg.Id.LoadMore)}</a>

                                        }

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <Footer />

                <ErrorMsg />

            </div>

        );

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.target } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators, ...routerActions }
)(TargetListAll as any) as typeof TargetListAll;
