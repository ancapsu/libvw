import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as TargetStore from '../store/Target';
import ShowAuthors from './ShowAuthors';
import ShowCategories from './ShowCategories';

type ShowTargetRefProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & { index: number }
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators;

class ShowTargetRef extends React.Component<ShowTargetRefProps, {}> {
        
    contentReady: boolean;

    constructor(props: ShowTargetRefProps, context: any) {

        super(props, context);

        this.voteTarget = this.voteTarget.bind(this);
        this.changeLanguage = this.changeLanguage.bind(this);
        this.contentReady = true;

    }
    
    voteTarget(id: string, v: number) {
                
        this.props.registerTargetVote(id, this.props.lang, v);
        
    }

    changeLanguage(id: string, l: number) {

        this.props.changeTargetLanguage(id, this.props.lang, l);

    }

    public render() {

        var canEdit: boolean = false;

        if ((this.props.hasList || this.props.hasCategory || this.props.hasSearchList) && this.props.targets.length > this.props.index) {

            if (this.props.validToken && this.props.user != null) {

                if (this.props.targets[this.props.index].Authors.Authored.Id == this.props.user.Account.Id) {
                    canEdit = true;
                }

                if (this.props.user.Account.Profile >= 7) {
                    canEdit = true;
                }

            }

            return (

                <div className="news-category-main">

                    <div className="news-category-image-box col-lg-3 col-md-4 col-sm-6 col-xs-12">
                        <NavLink to={'/target/' + this.props.targets[this.props.index].Id} className="image-news-category" >
                            <img src={"/api/Target/Image?id=" + this.props.targets[this.props.index].Id} ></img>
                        </NavLink>
                    </div>
                    <div className="news-category-details col-lg-9 col-md-8 col-sm-6 col-xs-12">
                        <h3 className="news-title-in-newscategory">
                            <NavLink to={'/target/' + this.props.targets[this.props.index].Id} className="td-image-wrap" >
                                {this.props.targets[this.props.index].Title}
                            </NavLink>
                        </h3>
                        <div className="td-module-meta-info">

                            <ShowAuthors authors={this.props.targets[this.props.index].Authors} />

                            <div className="td-post-author-name">
                                <a href={this.props.targets[this.props.index].Link} className="gerar-artigo-agora">
                                    &nbsp; {RacMsg.Get(RacMsg.Id.ViewOriginalNews)} &nbsp; <i className="fa fa-chevron-right"></i>
                                </a>
                            </div>
                            <div className="td-post-author-name">
                                <NavLink to={"/new-article/" + this.props.targets[this.props.index].Id} className="gerar-artigo-agora">
                                    &nbsp; {RacMsg.Get(RacMsg.Id.GenerateArticle)} &nbsp; <i className="fa fa-chevron-right"></i>
                                </NavLink>
                            </div>
                            {canEdit &&
                                <div className="td-post-author-name">
                                    <NavLink to={"/edit-target/" + this.props.targets[this.props.index].Id} className="gerar-artigo-agora">
                                        &nbsp; {RacMsg.Get(RacMsg.Id.EditAgenda)} &nbsp; <i className="fa fa-chevron-right"></i>
                                    </NavLink>
                                </div>
                            }
                        </div>
                        <div className="td-module-meta-info">
                            {this.props.targets[this.props.index].StartingText}
                        </div>

                        <ShowCategories categ={this.props.targets[this.props.index].Categories} link="target-category" />

                        <div className="td-module-meta-info">
                            <ul className="vote-target-list">

                                <li className={((this.props.targets[this.props.index].UserVote == 1) ? "voto-atual-write" : "vote-write")} onClick={() => { this.voteTarget(this.props.targets[this.props.index].Id, 1); }}>
                                    {(this.props.targets[this.props.index].VoteWrite > 0) ? this.props.targets[this.props.index].VoteWrite : ""}
                                    &nbsp;<i className="fa fa-file-text"></i>
                                    &nbsp;{RacMsg.Get(RacMsg.Id.WillWriteArticle)}
                                </li>
                                <li className={((this.props.targets[this.props.index].UserVote == 2) ? "voto-atual-good" : "vote-good")} onClick={() => { this.voteTarget(this.props.targets[this.props.index].Id, 2); }}>
                                    {(this.props.targets[this.props.index].VoteVery > 0) ? this.props.targets[this.props.index].VoteVery : ""}
                                    &nbsp;<i className="fa fa-heart"></i>
                                    &nbsp;{RacMsg.Get(RacMsg.Id.LikeVeryMuch)}
                                </li>
                                <li className={((this.props.targets[this.props.index].UserVote == 3) ? "voto-atual-good" : "vote-good")} onClick={() => { this.voteTarget(this.props.targets[this.props.index].Id, 3); }}>
                                    {(this.props.targets[this.props.index].VoteGood > 0) ? this.props.targets[this.props.index].VoteGood : ""}
                                    &nbsp;<i className="fa fa-thumbs-up"></i>
                                    &nbsp;{RacMsg.Get(RacMsg.Id.Like)}
                                </li>
                                <li className={((this.props.targets[this.props.index].UserVote == 4) ? "voto-atual-bad" : "vote-bad")} onClick={() => { this.voteTarget(this.props.targets[this.props.index].Id, 4); }}>
                                    {(this.props.targets[this.props.index].VoteNot > 0) ? this.props.targets[this.props.index].VoteNot : ""}
                                    &nbsp;<i className="fa fa-thumbs-down"></i>
                                    &nbsp;{RacMsg.Get(RacMsg.Id.NotRelevant)}
                                </li>
                                <li className={((this.props.targets[this.props.index].UserVote == 5) ? "voto-atual-bad" : "vote-bad")} onClick={() => { this.voteTarget(this.props.targets[this.props.index].Id, 5); }}>
                                    {(this.props.targets[this.props.index].VoteOld > 0) ? this.props.targets[this.props.index].VoteOld : ""}
                                    &nbsp;<i className="fa fa-calendar"></i>
                                    &nbsp;{RacMsg.Get(RacMsg.Id.OldNews)}
                                </li>
                                <li className={((this.props.targets[this.props.index].UserVote == 6) ? "voto-atual-bad" : "vote-bad")} onClick={() => { this.voteTarget(this.props.targets[this.props.index].Id, 6); }}>
                                    {(this.props.targets[this.props.index].VoteFake > 0) ? this.props.targets[this.props.index].VoteFake : ""}
                                    &nbsp;<i className="fa fa-facebook"></i>
                                    &nbsp;{RacMsg.Get(RacMsg.Id.FakeNews)}
                                </li>

                                {this.props.lang != 2 &&

                                    <li className="vote-bad" onClick={() => { this.changeLanguage(this.props.targets[this.props.index].Id, 2); }}>
                                        &nbsp;EN
                                        &nbsp;{RacMsg.Get(RacMsg.Id.SendToEnglish)}
                                    </li>

                                }

                                {this.props.lang != 3 &&

                                    <li className="vote-bad" onClick={() => { this.changeLanguage(this.props.targets[this.props.index].Id, 3); }}>
                                    &nbsp;PT
                                        &nbsp;{RacMsg.Get(RacMsg.Id.SendToPortuguese)}
                                    </li>

                                }

                                {this.props.lang != 4 &&

                                    <li className="vote-bad" onClick={() => { this.changeLanguage(this.props.targets[this.props.index].Id, 4); }}>
                                        &nbsp;ES
                                        &nbsp;{RacMsg.Get(RacMsg.Id.SendToSpanish)}
                                    </li>

                                }

                            </ul>
                        </div>

                        {this.props.targets[this.props.index].AssociatedArticleId != "" &&

                            <div className="td-module-meta-info">
                            {RacMsg.Get(RacMsg.Id.ThereIsAlreadyAnArticleFromThisAgenda)} &nbsp; 
                                <NavLink to={"/article/" + this.props.targets[this.props.index].AssociatedArticleId} className="gerar-artigo-agora">
                                    "{this.props.targets[this.props.index].AssociatedArticleTitle}"
                                </NavLink>
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
    (state: ApplicationState) => { return { ...state.account, ...state.target } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators }
)(ShowTargetRef as any) as any;
