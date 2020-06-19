import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { routerActions } from 'react-router-redux';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import ErrorMsg from '../message/errormsg';
import Footer from './Footer/Footer';
import Header from './header/Header';
import ShowArticleRef from './ShowArticleRef';

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type ArticleListApprovalProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class ArticleListApproval extends React.Component<ArticleListApprovalProps, {}> {

    contentReady: boolean;

    constructor(props: ArticleListApprovalProps, context: any) {

        super(props, context);

        this.LoadMore = this.LoadMore.bind(this);
        this.contentReady = false;

    }

    componentWillMount() {

        if (!this.props.validToken || this.props.user == null) {

            this.props.push('/login');

        }

    }

    componentDidMount() {

        if (!this.props.hasListApproval) {

            this.props.requestArticleList(this.props.lang, 0, 10, 1);

        } else {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: ArticleListApprovalProps) {

        if ((props.hasListApproval && props.articles.length != this.props.articles.length) || (!this.props.hasListApproval && props.hasListApproval)) {

            this.contentReady = true;

        }

    }

    LoadMore() {

        if (this.props.hasListApproval) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestArticleList(this.props.lang, this.props.articles.length, 10, 1);

        }

    }

    public render() {

        return (

            <div>

                <Header />

                <div className={(!this.contentReady ? "wait-panel" : "wait-panel-disabled")}>
                    <img src={waitGif} ></img>
                </div>

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="wpb_wrapper">
                                <div className="td_block_wrap">
                                    <div className="td-block-title-wrap">
                                        <h4 className="td-block-title">
                                            <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.ArticlesForApproval)}</span>
                                        </h4>
                                    </div>

                                    <div className="td_block_inner td-column-2 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        {RacMsg.Get(RacMsg.Id.ListOfArticlesForApproval)}
                                    </div>

                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>

                                    <span className="news-page-sub-headline">{RacMsg.Get(RacMsg.Id.ImportantInformationForApproval)}</span>

                                    <div className="news-page-text">
                                        {RacMsg.Get(RacMsg.Id.ImportantInformationForApprovalItem1)}
                                    </div>

                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>
                                    <div className="td_block_inner td-column-2">

                                        {

                                            this.props.articles.map((n, idx) => {

                                                return (

                                                    <ShowArticleRef index={idx} />

                                                );

                                            })

                                        }

                                    </div>

                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>

                                    <div className="td_block_inner td-column-2">

                                        {this.props.articles.length < this.props.total &&

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
    (state: ApplicationState) => { return { ...state.account, ...state.article } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators, ...routerActions }
)(ArticleListApproval as any) as typeof ArticleListApproval;
