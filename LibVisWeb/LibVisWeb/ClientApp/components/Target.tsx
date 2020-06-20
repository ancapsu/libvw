import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as TargetStore from '../store/Target';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import ShowAuthors from './ShowAuthors';
import ShowCategories from './ShowCategories';
import ShowTargetAction from './ShowTargetAction';

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type TargetProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators
    & RouteComponentProps<{ id: string }>;

class Target extends React.Component<TargetProps, {}> {

    contentReady: boolean;

    constructor(props: TargetProps, context: any) {

        super(props, context);

        this.contentReady = false;

    }

    componentDidMount() {

        this.props.requestEditBase(this.props.lang);
        this.props.requestTarget(this.props.match.params.id, this.props.lang);
        
    }

    componentWillReceiveProps(props: TargetProps) {

        if (!props.hasTarget) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestTarget(this.props.match.params.id, this.props.lang);
            
        }

        if (props.hasTarget && props.hasEditBase) {

            this.contentReady = true;

        }

    }

    public render() {

        

        if (this.props.hasTarget && this.props.target != null && this.props.target.Paragraphs.length > 0) {

            var primeiro: string = this.props.target.Paragraphs[0].trim();
            var capital: string = primeiro.substring(0, 1);
            primeiro = primeiro.substring(1);
            var jafoiprim: boolean = false;

            var canEdit: boolean = false;

            if (this.props.validToken && this.props.user != null) {

                if (this.props.target.Authors.Authored.Id == this.props.user.Account.Id) {
                    canEdit = true;
                }

                if (this.props.user.Account.Profile >= 7) {
                    canEdit = true;
                }

            }

            return (

                <div>

                    <Header />

                    <div className={(!this.contentReady ? "wait-panel" : "wait-panel-disabled")}>
                        <img src={waitGif} ></img>
                    </div>

                    <div className="container open-line"></div>

                    <div className="tdc-video-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">

                            <div className="post type-post status-publish format-standard has-post-thumbnail category-entrepreneurs">

                                <div className="td-post-header">

                                    <ShowCategories categ={this.props.target.Categories} link="target-category" />

                                    <header className="td-post-title">
                                        <h1 className="entry-title">{this.props.target.Title}</h1>
                                        <div className="td-module-meta-info">

                                            <ShowAuthors authors={this.props.target.Authors} />

                                            <div className="td-post-author-name">
                                                <NavLink to={"/new-article/" + this.props.target.Id} className="gerar-artigo-agora">
                                                    &nbsp; Gerar notícia agora &nbsp; <i className="fa fa-chevron-right"></i>
                                                </NavLink>
                                            </div>

                                            {canEdit &&
                                                <div className="td-post-author-name">
                                                    <NavLink to={"/edit-target/" + this.props.target.Id} className="gerar-artigo-agora">
                                                        &nbsp; Editar pauta &nbsp; <i className="fa fa-chevron-right"></i>
                                                    </NavLink>
                                                </div>
                                            }

                                        </div>
                                    </header>
                                </div>
                                
                                <div className="td-post-content news-post-content">

                                    <a href={this.props.target.Link}>{this.props.target.Link}</a>

                                </div>
                                                                
                                <div className="container open-line"></div>
                                <div className="container open-line"></div>

                                <div className="td-post-content news-post-content">
                                    <div className="td-post-featured-image">
                                        <img className="news-featured-image" src={"/api/Target/Image?id=" + this.props.target.Id} ></img>
                                    </div>
                                    <p>
                                        <span className="dropcap dropcap3">{capital}</span>
                                        {primeiro}
                                    </p>

                                    {

                                        this.props.target.Paragraphs.map(par => {

                                            if (jafoiprim) {

                                                return <p>{par}</p>

                                            }
                                            else {

                                                jafoiprim = true;
                                                return <span></span>

                                            }

                                        })

                                    }
                                                                        
                                </div>
                            </div>

                            <div className="td_block_inner td-column-2">

                                {

                                    this.props.target.Actions.map((n, idx) => {

                                        return (

                                            <ShowTargetAction index={idx} />

                                        );

                                    })

                                }

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

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.target } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators }
)(Target as any) as typeof Target;
