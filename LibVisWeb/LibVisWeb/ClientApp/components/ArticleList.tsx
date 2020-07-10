import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import ShowArticle from './ShowArticle';
import WaitPanel from './common/WaitPanel';

type ArticleListProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators
    & RouteComponentProps<{}>;

class ArticleList extends React.Component<ArticleListProps, {}> {

    contentReady: boolean;

    constructor(props: ArticleListProps, context: any) {

        super(props, context);

        this.LoadMore = this.LoadMore.bind(this);
        this.contentReady = false;

    }

    componentDidMount() {

        if (!this.props.hasList) {

            this.props.requestArticleList(this.props.lang, 0, 10, 0);

        } else {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: ArticleListProps) {

        if ((props.hasList && props.articles.length != this.props.articles.length) || (!this.props.hasList && props.hasList)) {

            this.contentReady = true;

        }

    }

    LoadMore() {

        if (this.props.hasList) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestArticleList(this.props.lang, this.props.articles.length, 10, 0);

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
                                            <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.Articles)}</span>
                                        </h4>
                                    </div>

                                    <div className="row">

                                        <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                            {RacMsg.Get(RacMsg.Id.ListOfArticles)}
                                        </div>

                                        <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                            <NavLink to={'/new-article'} className="btn-create-new float-right">{RacMsg.Get(RacMsg.Id.CreateNewArticle)}</NavLink>
                                        </div>

                                    </div>

                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>
                                    <div className="td_block_inner td-column-2">

                                        {

                                            this.props.articles.map(n => {

                                                return (

                                                    <ShowArticle article={n} />

                                                );

                                            })

                                        }

                                    </div>

                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>

                                    <div className="td_block_inner td-column-2">

                                        {this.props.articles.length < this.props.total &&

                                            <a onClick={this.LoadMore} className="btn-carregar-mais">{RacMsg.Get(RacMsg.Id.LoadMore)}s</a>

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
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators }
)(ArticleList as any) as typeof ArticleList;
