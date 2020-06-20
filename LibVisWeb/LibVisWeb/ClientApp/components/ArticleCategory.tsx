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
import { ArticleModel } from 'ClientApp/models/Article';
import ShowArticle from './ShowArticle';

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type ArticleCategoryProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators
    & RouteComponentProps<{ categ: string }>;

class ArticleCategory extends React.Component<ArticleCategoryProps, {}> {

    contentReady: boolean;
    categ: string;
    
    constructor(props: ArticleCategoryProps, context: any) {

        super(props, context);

        this.contentReady = false;
        this.categ = props.match.params.categ;

        this.componentDidMount = this.componentDidMount.bind(this);
        this.componentWillReceiveProps = this.componentWillReceiveProps.bind(this);
        this.LoadMore = this.LoadMore.bind(this);

    }

    componentDidMount() {

        if (!this.props.hasCategory || this.categ != this.props.match.params.categ) {

            this.props.requestArticleByCategory(this.props.lang, this.props.match.params.categ, 0, 10);

        } else {

            this.contentReady = true;
            this.forceUpdate();

        }
        
    }

    componentWillReceiveProps(props: ArticleCategoryProps) {

        if (this.categ != props.match.params.categ) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestArticleByCategory(this.props.lang, this.props.match.params.categ, 0, 10);
            this.categ = this.props.match.params.categ;

        } else {

            if ((props.hasCategory && props.articles.length != this.props.articles.length) || (!this.props.hasCategory && props.hasCategory)) {

                this.contentReady = true;

            }

        }

    }

    LoadMore() {

        if (this.contentReady) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestArticleByCategory(this.props.lang, this.props.match.params.categ, this.props.articles.length, 10);
            
        }

    }

    public render() {

        

        if (this.props.hasCategory) {

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
                                                <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.ListOfArticlesWithCategory)} '{this.props.title}'</span>
                                            </h4>
                                        </div>
                                        <div className="td_block_inner td-column-2">
                                            {this.props.description}
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
            
        } else {

            return (

                <div>

                    <Header />

                    <div className="container open-line"></div>

                    <div className="tdc-row">
                        <div className="td-business-home-row wpb_row td-pb-row">
                            <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                                <div className="wpb_wrapper">
                                    <div className="td_block_wrap">

                                        Carregando informa&ccedil;&otilde;es...

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
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.article } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators }
)(ArticleCategory as any) as typeof ArticleCategory;
