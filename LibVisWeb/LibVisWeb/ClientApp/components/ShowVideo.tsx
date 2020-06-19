import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import ErrorMsg from '../message/errormsg';
import Footer from './Footer/Footer';
import Header from './header/Header';
import { LoginResultModel } from '../models/Account';
import { VideoModel } from 'ClientApp/models/Video';
import ShowCategories from './ShowCategories';
import ShowAuthors from './ShowAuthors';

type ShowVideoProps =
    AccountStore.AccountState
    & { video: VideoModel }
    & typeof AccountStore.actionCreators;

class ShowVideo extends React.Component<ShowVideoProps, {}> {
        
    public render() {
        
        

        var canEdit: boolean = false;

        if (this.props.validToken && this.props.user != null) {

            if (this.props.video.Authors.Authored.Id == this.props.user.Account.Id) {
                canEdit = true;
            }

            if (this.props.user.Account.Profile >= 7) {
                canEdit = true;
            }

        }

        return (

            <div className="news-category-main">

                <div className="news-category-image-box col-lg-3 col-md-4 col-sm-6 col-xs-12">
                    <NavLink to={'/video/' + this.props.video.Id} className="image-news-category" >
                        <img src={"/api/Video/Image?id=" + this.props.video.Id} ></img>
                    </NavLink>
                </div>
                <div className="news-category-details col-lg-9 col-md-8 col-sm-6 col-xs-12">
                    <h3 className="news-title-in-newscategory">
                        <NavLink to={'/video/' + this.props.video.Id} className="td-image-wrap" >
                            {this.props.video.Title}
                        </NavLink>
                    </h3>
                    <div className="td-module-meta-info">

                        <ShowAuthors authors={this.props.video.Authors} />

                        <div className="td-post-author-name">
                            <a href={this.props.video.YoutubeLink} className="gerar-artigo-agora">
                                &nbsp; Ver no youtube &nbsp; <i className="fa fa-chevron-right"></i>
                            </a>
                        </div>

                        <div className="td-post-author-name">
                            <a href={this.props.video.BitchuteLink} className="gerar-artigo-agora">
                                &nbsp; Ver no bitchute &nbsp; <i className="fa fa-chevron-right"></i>
                            </a>
                        </div>

                        {canEdit &&
                            <div className="td-post-author-name">
                                <NavLink to={"/edit-video/" + this.props.video.Id} className="gerar-artigo-agora">
                                    &nbsp; Editar video &nbsp; <i className="fa fa-chevron-right"></i>
                                </NavLink>
                            </div>
                        }

                    </div>
                    <div className="td-module-meta-info">
                        {this.props.video.StartingDescription}
                    </div>

                    <ShowCategories categ={this.props.video.Categories} link="video-category" />
                    
                </div>

            </div>

        );

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, }
)(ShowVideo as any) as any;
