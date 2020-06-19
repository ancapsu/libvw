import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import { LoginResultModel } from '../models/Account';
import { ArticleModel } from 'ClientApp/models/Article';
import ShowAuthors from './ShowAuthors';
import ShowCategories from './ShowCategories';

type ShowArticleProps =
    AccountStore.AccountState
    & { article: ArticleModel }
    & typeof AccountStore.actionCreators;

class ShowArticle extends React.Component<ShowArticleProps, {}> {
    
    public render() {
        
        

        var canEdit: boolean = false;

        if (this.props.article != null) {

            if (this.props.validToken && this.props.user != null) {

                if ((this.props.article.Authors.Authored.Id == this.props.user.Account.Id) && (this.props.article.Status <= 2)) {
                    canEdit = true;
                }

                if (this.props.user.Account.Profile >= 7) {
                    canEdit = true;
                }

            }

            return (

                <div className="news-category-main">

                    <div className="news-category-image-box col-lg-3 col-md-4 col-sm-6 col-xs-12">
                        <NavLink to={'/article/' + this.props.article.Id} className="image-news-category" >
                            <img src={"/api/Article/Image?id=" + this.props.article.Id} ></img>
                        </NavLink>
                    </div>
                    <div className="news-category-details col-lg-9 col-md-8 col-sm-6 col-xs-12">
                        <h3 className="news-title-in-newscategory">
                            <NavLink to={'/article/' + this.props.article.Id} className="td-image-wrap" >
                                {this.props.article.Title}
                            </NavLink>
                        </h3>
                        <div className="td-module-meta-info">

                            <ShowAuthors authors={this.props.article.Authors} />

                            {canEdit &&
                                <div className="td-post-author-name">
                                <NavLink to={"/edit-article/" + this.props.article.Id} className="gerar-artigo-agora">
                                        &nbsp; Editar artigo &nbsp; <i className="fa fa-chevron-right"></i>
                                    </NavLink>
                                </div>
                            }

                        </div>
                        <div className="td-module-meta-info">
                            {this.props.article.StartingText}
                        </div>

                        <ShowCategories categ={this.props.article.Categories} link="article-category" />

                        {this.props.article.Status == 2 &&

                            <div className="td-module-meta-info">
                                <div className="vote-target-list">

                                    {this.props.user != null && this.props.article.beingRevised == this.props.user.Account.Id && this.props.article.beingRevised != "" && this.props.article.beingRevised != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-pencil"></i>
                                            &nbsp;Está sendo revisado por você
                                        </div>

                                    }

                                    {this.props.user != null && this.props.article.beingRevised != this.props.user.Account.Id && this.props.article.beingRevised != "" && this.props.article.beingRevised != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-pencil"></i>
                                            &nbsp;Está sendo revisado por {this.props.article.beingRevisedName}
                                        </div>

                                    }

                                </div>
                            </div>

                        }

                        {this.props.article.Status == 3 && 

                            <div className="td-module-meta-info">
                                <div className="vote-target-list">

                                {this.props.user != null && this.props.article.beingNarrated == this.props.user.Account.Id && this.props.article.beingNarrated != "" && this.props.article.beingNarrated != null && 

                                    <div className="being-done">
                                        &nbsp;<i className="fa fa-microphone"></i>
                                        &nbsp;Está sendo narrado por você
                                    </div>

                                }

                                {this.props.user != null && this.props.article.beingNarrated != this.props.user.Account.Id && this.props.article.beingNarrated != "" && this.props.article.beingNarrated != null &&

                                    <div className="being-done">
                                        &nbsp;<i className="fa fa-microphone"></i>
                                        &nbsp;Está sendo narrado por {this.props.article.beingNarratedName}
                                    </div>

                                }
                                
                                {this.props.user != null && this.props.article.preferredNarrator == this.props.user.Account.Id &&
                                    
                                    <div className="prefered-to-do">                                        
                                        &nbsp;<i className="fa fa-heart"></i>
                                        &nbsp;Você é narrador preferencial para esse artigo
                                    </div>

                                }

                                {this.props.user != null && this.props.article.preferredNarrator != this.props.user.Account.Id && this.props.article.preferredNarrator != "" && this.props.article.preferredNarrator != null &&

                                    <div className="prefered-to-do">
                                        &nbsp;<i className="fa fa-heart"></i>
                                        &nbsp;{this.props.article.preferredNarratorName} é narrador preferencial desse artigo
                                    </div>

                                }

                                </div>
                            </div>

                        }
                        
                        {this.props.article.Status == 4 &&

                            <div className="td-module-meta-info">
                                <div className="vote-target-list">

                                    {this.props.user != null && this.props.article.beingProduced == this.props.user.Account.Id && this.props.article.beingProduced != "" && this.props.article.beingProduced != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-film"></i>
                                            &nbsp;Está sendo produzido por você
                                        </div>

                                    }

                                    {this.props.user != null && this.props.article.beingProduced != this.props.user.Account.Id && this.props.article.beingProduced != "" && this.props.article.beingProduced != null &&

                                        <div className="being-done">
                                            &nbsp;<i className="fa fa-film"></i>
                                            &nbsp;Está sendo produzido por {this.props.article.beingProducedName}
                                        </div>

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
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators }
)(ShowArticle as any) as any;
