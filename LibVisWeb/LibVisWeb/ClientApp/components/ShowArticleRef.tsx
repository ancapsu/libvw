import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import ShowAuthors from './ShowAuthors';
import ShowCategories from './ShowCategories';

type ShowArticleRefProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & { index: number }
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators;

class ShowArticleRef extends React.Component<ShowArticleRefProps, {}> {
        
    contentReady: boolean;

    constructor(props: ShowArticleRefProps, context: any) {

        super(props, context);

        this.voteArticle = this.voteArticle.bind(this);
        this.setPriority = this.setPriority.bind(this);
        this.contentReady = true;

    }
    
    voteArticle(id: string, v: number) {

        this.props.registerArticleVote(id, v);
        
    }

    setPriority(id: string, prio: number, def: number) {

        this.props.registerPriority(id, prio, def);

    }

    public render() {

        var canEdit: boolean = false;

        if ((this.props.hasList || this.props.hasCategory || this.props.hasListApproval || this.props.hasListNarration || this.props.hasListProduction || this.props.hasListPublish || this.props.hasListRevision || this.props.hasSearchList) && this.props.articles.length > this.props.index) {

            if (this.props.validToken && this.props.user != null) {

                if (this.props.articles[this.props.index].Authors.Authored.Id == this.props.user.Account.Id) {
                    canEdit = true;
                }

                if (this.props.user.Account.Profile >= 7) {
                    canEdit = true;
                }

            }

            return (

                <div className="news-category-main">

                    <div className="news-category-image-box col-lg-3 col-md-4 col-sm-6 col-xs-12">
                        <NavLink to={'/article/' + this.props.articles[this.props.index].Id} className="image-news-category" >
                            <img src={"/api/Article/Image?id=" + this.props.articles[this.props.index].Id} ></img>
                        </NavLink>
                    </div>
                    <div className="news-category-details col-lg-9 col-md-8 col-sm-6 col-xs-12">
                        <h3 className="news-title-in-newscategory">
                            <NavLink to={'/article/' + this.props.articles[this.props.index].Id} className="td-image-wrap" >
                                {this.props.articles[this.props.index].Title}
                            </NavLink>
                        </h3>
                        <div className="td-module-meta-info">

                            <ShowAuthors authors={this.props.articles[this.props.index].Authors} />

                            {canEdit &&
                                <div className="td-post-author-name">
                                <NavLink to={"/edit-article/" + this.props.articles[this.props.index].Id} className="gerar-artigo-agora">
                                        &nbsp; Editar artigo &nbsp; <i className="fa fa-chevron-right"></i>
                                    </NavLink>
                                </div>
                            }

                        </div>
                        <div className="td-module-meta-info">
                            {this.props.articles[this.props.index].StartingText}
                        </div>

                        <ShowCategories categ={this.props.articles[this.props.index].Categories} link="article-category" />


                        {this.props.articles[this.props.index].Status == 1 &&

                            <div className="td-module-meta-info">
                                <ul className="vote-target-list">

                                    <li className={((this.props.articles[this.props.index].UserVote == 1) ? "voto-atual-good" : "vote-good")} onClick={() => { this.voteArticle(this.props.articles[this.props.index].Id, 1); }}>
                                        {(this.props.articles[this.props.index].VoteApprove > 0) ? this.props.articles[this.props.index].VoteApprove : ""}
                                        &nbsp;<i className="fa fa-heart"></i>
                                        &nbsp;Aprovo
                                    </li>
                                    <li className={((this.props.articles[this.props.index].UserVote == 2) ? "voto-atual-bad" : "vote-bad")} onClick={() => { this.voteArticle(this.props.articles[this.props.index].Id, 2); }}>
                                        {(this.props.articles[this.props.index].VoteNot > 0) ? this.props.articles[this.props.index].VoteNot : ""}
                                        &nbsp;<i className="fa fa-thumbs-down"></i>
                                        &nbsp;Não aprovo
                                    </li>

                                </ul>
                            </div>

                        }

                        {this.props.articles[this.props.index].Status == 2 &&

                            <div className="td-module-meta-info">
                                <div className="vote-target-list">

                                    {this.props.user != null && this.props.articles[this.props.index].beingRevised == this.props.user.Account.Id && this.props.articles[this.props.index].beingRevised != "" && this.props.articles[this.props.index].beingRevised != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-pencil"></i>
                                            &nbsp;Está sendo revisado por você
                                        </div>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].beingRevised != this.props.user.Account.Id && this.props.articles[this.props.index].beingRevised != "" && this.props.articles[this.props.index].beingRevised != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-pencil"></i>
                                            &nbsp;Está sendo revisado por {this.props.articles[this.props.index].beingRevisedName}
                                        </div>

                                    }

                                    {(this.props.articles[this.props.index].beingRevised == "" || this.props.articles[this.props.index].beingRevised == null) &&

                                        <a className="will-do" onClick={() => { this.setPriority(this.props.articles[this.props.index].Id, 2, 1); }}>
                                            &nbsp;<i className="fa fa-pencil"></i>
                                            &nbsp;Vou revisar esse artigo
                                        </a>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].beingRevised == this.props.user.Account.Id && this.props.articles[this.props.index].beingRevised != "" && this.props.articles[this.props.index].beingRevised != null &&

                                        <a className="will-do" onClick={() => { this.setPriority(this.props.articles[this.props.index].Id, 2, 0); }}>
                                            &nbsp;<i className="fa fa-arrow-left"></i>
                                            &nbsp;Desisti de revisar
                                        </a>

                                    }

                                </div>
                            </div>

                        }

                        {this.props.articles[this.props.index].Status == 3 &&

                            <div className="td-module-meta-info">
                                <div className="vote-target-list">

                                    {this.props.user != null && this.props.articles[this.props.index].beingNarrated == this.props.user.Account.Id && this.props.articles[this.props.index].beingNarrated != "" && this.props.articles[this.props.index].beingNarrated != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-microphone"></i>
                                            &nbsp;Está sendo narrado por você
                                        </div>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].beingNarrated != this.props.user.Account.Id && this.props.articles[this.props.index].beingNarrated != "" && this.props.articles[this.props.index].beingNarrated != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-microphone"></i>
                                            &nbsp;Está sendo narrado por {this.props.articles[this.props.index].beingNarratedName}
                                        </div>

                                    }

                                    {(this.props.articles[this.props.index].beingNarrated == "" || this.props.articles[this.props.index].beingNarrated == null) &&

                                        <a className="will-do" onClick={() => { this.setPriority(this.props.articles[this.props.index].Id, 3, 1); }}>
                                            &nbsp;<i className="fa fa-microphone"></i>
                                            &nbsp;Vou narrar esse artigo
                                        </a>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].beingNarrated == this.props.user.Account.Id && this.props.articles[this.props.index].beingNarrated != "" && this.props.articles[this.props.index].beingNarrated != null &&

                                        <a className="will-do" onClick={() => { this.setPriority(this.props.articles[this.props.index].Id, 3, 0); }}>
                                            &nbsp;<i className="fa fa-arrow-left"></i>
                                            &nbsp;Desisti de narrar
                                        </a>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].preferredNarrator == this.props.user.Account.Id &&

                                        <div className="prefered-to-do">
                                            &nbsp;<i className="fa fa-heart"></i>
                                            &nbsp;Você é narrador preferencial para esse artigo
                                        </div>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].preferredNarrator != this.props.user.Account.Id && this.props.articles[this.props.index].preferredNarrator != "" && this.props.articles[this.props.index].preferredNarrator != null &&

                                        <div className="prefered-to-do">
                                            &nbsp;<i className="fa fa-heart"></i>
                                            &nbsp;{this.props.articles[this.props.index].preferredNarratorName} é narrador preferencial desse artigo
                                        </div>

                                    }

                                </div>
                            </div>

                        }

                        {this.props.articles[this.props.index].Status == 4 &&

                            <div className="td-module-meta-info">
                                <div className="vote-target-list">

                                    {this.props.user != null && this.props.articles[this.props.index].beingProduced == this.props.user.Account.Id && this.props.articles[this.props.index].beingRevised != "" && this.props.articles[this.props.index].beingRevised != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-film"></i>
                                            &nbsp;Está sendo produzido por você
                                        </div>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].beingProduced != this.props.user.Account.Id && this.props.articles[this.props.index].beingProduced != "" && this.props.articles[this.props.index].beingProduced != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-film"></i>
                                            &nbsp;Está sendo produzido por {this.props.articles[this.props.index].beingProducedName}
                                        </div>

                                    }

                                    {(this.props.articles[this.props.index].beingProduced == "" || this.props.articles[this.props.index].beingProduced == null) &&

                                        <a className="will-do" onClick={() => { this.setPriority(this.props.articles[this.props.index].Id, 4, 1); }}>
                                            &nbsp;<i className="fa fa-film"></i>
                                            &nbsp;Vou produzir esse artigo
                                        </a>

                                    }

                                    {this.props.user != null && this.props.articles[this.props.index].beingProduced == this.props.user.Account.Id && this.props.articles[this.props.index].beingProduced != "" && this.props.articles[this.props.index].beingProduced != null &&

                                        <a className="will-do" onClick={() => { this.setPriority(this.props.articles[this.props.index].Id, 4, 0); }}>
                                            &nbsp;<i className="fa fa-arrow-left"></i>
                                            &nbsp;Desisti de produzir
                                        </a>

                                    }

                                </div>
                            </div>

                        }

                    </div>

                </div>

            );
            
        } else {

            return (

                <div className="news-category-main">Carregando informações...</div>

            );

        }

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.article } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators }
)(ShowArticleRef as any) as any;
