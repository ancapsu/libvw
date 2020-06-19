import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import { ArticleActionAudioModel } from 'ClientApp/models/Article';
import { Field } from 'redux-form';

const suggestedImg: string = require('../theme/newspaper/img/suggested.png');
const revisedImg: string = require('../theme/newspaper/img/revised.png');
const createdImg: string = require('../theme/newspaper/img/created.png');
const approvedImg: string = require('../theme/newspaper/img/approved.png');
const publishedImg: string = require('../theme/newspaper/img/published.png');
const removedImg: string = require('../theme/newspaper/img/removed.png');

type ShowArticleActionProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & { index: number }
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators;

class ShowArticleAction extends React.Component<ShowArticleActionProps, {}> {
        
    contentReady: boolean;
    observ: string;

    constructor(props: ShowArticleActionProps, context: any) {

        super(props, context);

        this.contentReady = true;
        this.observ = "";

        this.setAward = this.setAward.bind(this);
        this.obsChanged = this.obsChanged.bind(this);
        this.keyDown = this.keyDown.bind(this);
        this.eraseAction = this.eraseAction.bind(this);
    }

    setAward(actionId: string, awardId: string, t: number) {
        
        if (this.props.validToken && this.props.user != null && this.props.user.Account.Profile >= 7) {

            this.props.registerGrant(actionId, this.props.lang, awardId, t);

        }

    }

    obsChanged(e: any) {

        this.observ = e.target.value;

    }

    eraseAction(id: string) {

        if (this.props.validToken && this.props.user != null && this.props.user.Account.Profile >= 7) {

            this.props.eraseAction(id);

        }

    }

    keyDown(e: any) {

        if (e.key === 'Enter') {

            if (this.props.article != null) {
                this.props.registerNewObservation(this.props.article.Actions[this.props.index].Id, this.observ);
                this.observ = "";
            }

        }

    }

    public render() {

        

        var canEdit: boolean = false;

        if (this.props.validToken && this.props.user != null && this.props.user.Account.Profile >= 7) {

            canEdit = true;

        }

        if (this.props.article != null) {

            return (

                <div className="news-category-main">

                    <div className="news-category-image-box col-lg-5 col-md-5 col-sm-6 col-xs-12">
                        {this.props.article.Actions[this.props.index].Type == 1 &&
                            <img src={suggestedImg} className="action-itens"></img>
                        }
                        {this.props.article.Actions[this.props.index].Type == 2 &&
                            <img src={createdImg} className="action-itens"></img>
                        }
                        {this.props.article.Actions[this.props.index].Type == 3 &&
                            <img src={revisedImg} className="action-itens"></img>
                        }
                        {this.props.article.Actions[this.props.index].Type == 4 &&
                            <img src={approvedImg} className="action-itens"></img>
                        }
                        {this.props.article.Actions[this.props.index].Type == 5 &&
                            <img src={publishedImg} className="action-itens"></img>
                        }
                        {this.props.article.Actions[this.props.index].Type == 6 &&
                            <img src={removedImg} className="action-itens"></img>
                        }
                        {this.props.article.Actions[this.props.index].TypeName} por {this.props.article.Actions[this.props.index].UserName} em {this.props.article.Actions[this.props.index].Date}

                        {this.props.article.Actions[this.props.index].BillableWords > 0 &&
                            <span> ({this.props.article.Actions[this.props.index].BillableWords} palavras)</span>
                        }

                        {this.props.article.Actions[this.props.index].Audios.map((n: ArticleActionAudioModel, idx:number) => {

                            return (

                                <div>                                    
                                    {this.props.article != null &&
                                        <audio controls>
                                            <source src={'/api/Audio/' + this.props.article.Actions[this.props.index].Id + '/' + idx} type={n.MimeType} />
                                            Seu navegador não suporta audio. Arquivo {n.FileName}
                                        </audio>
                                    }
                                </div>

                            );

                            })
                            
                        }
                    </div>
                    {canEdit &&
                        <div className="news-category-image-box col-lg-7 col-md-7 col-sm-6 col-xs-12">
    
                            <a className="will-do" onClick={() => { if (this.props.article != null) this.eraseAction(this.props.article.Actions[this.props.index].Id); }}>
                                &nbsp;<i className="fa fa-trash"></i>
                                &nbsp;Apagar ação
                            </a>
                            
                            {this.props.awards.map((n, idx) => {

                                var tem: boolean = false;

                                if (this.props.article != null) {

                                    for (var i = 0; i < this.props.article.Actions[this.props.index].Grants.length; i++) {

                                        if (this.props.article.Actions[this.props.index].Grants[i].AwardId == n.Id)
                                            tem = true;

                                    }

                                }

                                if (tem) {

                                    return (

                                        <a onClick={() => { if (this.props.article != null) { this.setAward(this.props.article.Actions[this.props.index].Id, n.Id, 0); }}}>
                                            <img src={'/dist/img/a/' + n.Id + '.png'} title={n.Name} className="awards-itens"></img>
                                        </a>

                                    );

                                } else {

                                    return (

                                        <a onClick={() => { if (this.props.article != null) { this.setAward(this.props.article.Actions[this.props.index].Id, n.Id, 1) } }}>
                                            <img src={'/dist/img/a/' + n.Id + '_off.png'} title={n.Name} className="awards-itens"></img>
                                        </a>

                                    );

                                }


                            })

                            }
                        </div>
                    }
                    {!canEdit &&
                        <div className="news-category-image-box col-lg-7 col-md-7 col-sm-6 col-xs-12">
                            {this.props.awards.map((n, idx) => {

                                var tem: boolean = false;

                                if (this.props.article != null) {

                                    for (var i = 0; i < this.props.article.Actions[this.props.index].Grants.length; i++) {

                                        if (this.props.article.Actions[this.props.index].Grants[i].AwardId == n.Id)
                                            tem = true;

                                    }

                                }

                                if (tem) {

                                    return (

                                        <img src={'/dist/img/a/' + n.Id + '.png'} title={n.Name} className="awards-itens"></img>

                                    );

                                } else {

                                    return (

                                        <p></p>

                                    );

                                }


                            })

                            }
                        </div>
                    }

                    {this.props.article.Actions[this.props.index].Observations.map(n => {

                        return (

                            <div className="news-category-image-box col-lg-12 col-md-12 col-sm-12 col-xs-12 action-obs">

                                <div className="news-category-image-box col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                    
                                    <img src={'/api/Avatar/' + n.IncludedById} className="comment-user-img" ></img>

                                    <span className="date-for-comment">
                                        {n.Included}
                                    </span>

                                    <span className="user-name-for-comment">
                                        {n.IncludedBy}
                                    </span>

                                </div>

                                <div className="news-category-image-box col-lg-10 col-md-10 col-sm-10 col-xs-10">

                                    <span className="comment-comment">
                                        {n.Observation}
                                    </span>

                                </div>

                            </div>

                        );

                    })}

                    {this.props.user != null && this.props.user.Account.Profile >= 7 &&

                        <div className="news-category-image-box col-lg-12 col-md-12 col-sm-12 col-xs-12 action-obs">

                            <div className="news-category-image-box col-lg-2 col-md-2 col-sm-2 col-xs-2">
                            
                                <img src={'/api/Avatar/' + this.props.user.Account.Id} className="comment-user-img" ></img>

                                <span className="date-for-comment">
                                    Agora
                                </span>

                                <span className="user-name-for-comment">
                                    {this.props.user.Account.Name}
                                </span>
                            
                            </div>

                            <div className="news-category-image-box col-lg-10 col-md-10 col-sm-10 col-xs-10">

                                <div className="form-group">

                                    <input type="text" className="form-control" name="obs" id="obs" placeholder="Observation" onChange={this.obsChanged} onKeyDown={this.keyDown} />                                    

                                </div>

                            </div>
                            
                        </div>

                    }

                </div>

            );

        } else {

            return (

                <div className="news-category-main">

                </div>

            );

        }

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.article } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators }
)(ShowArticleAction as any) as any;
