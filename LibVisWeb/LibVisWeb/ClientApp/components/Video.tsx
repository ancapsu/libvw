import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store'; 
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as VideoStore from '../store/Video';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import ShowAuthors from './ShowAuthors';
import ShowCategories from './ShowCategories';
import WaitPanel from './common/WaitPanel';

type VideoProps =
    AccountStore.AccountState
    & VideoStore.VideoState
    & typeof AccountStore.actionCreators
    & typeof VideoStore.actionCreators
    & RouteComponentProps<{ id: string }>;

class Video extends React.Component<VideoProps, {}> {

    contentReady: boolean;

    constructor(props: VideoProps, context: any) {

        super(props, context);

        this.contentReady = false;

    }

    componentDidMount() {

        this.props.requestEditBase(this.props.lang);
        this.props.requestVideo(this.props.match.params.id, this.props.lang);

    }

    componentWillReceiveProps(props: VideoProps) {

        if (!props.hasVideo) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestVideo(this.props.match.params.id, this.props.lang);

        }

        if (props.hasVideo && props.hasEditBase) {

            this.contentReady = true;

        }

    }

    public render() {

        

        if (this.props.hasVideo && this.props.video != null) {

            var primeiro: string = this.props.video.ScriptPars[0].trim();
            var capital: string = primeiro.substring(0, 1);
            primeiro = primeiro.substring(1);
            var jafoiprim: boolean = false;

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

                <div>

                    <Header />

                    <WaitPanel isContentReady={this.contentReady} />

                    <div className="container open-line"></div>

                    <div className="tdc-video-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="post type-post status-publish format-standard has-post-thumbnail category-entrepreneurs">

                                <div className="td-post-header">

                                    <ShowCategories categ={this.props.video.Categories} link="video-category" />

                                    <header className="td-post-title">
                                        <h1 className="entry-title">{this.props.video.Title}</h1>
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
                                                        &nbsp; Editar vídeo &nbsp; <i className="fa fa-chevron-right"></i>
                                                    </NavLink>
                                                </div>
                                            }

                                        </div>
                                    </header>
                                </div>

                                <div className="container open-line"></div>

                                <div className="container open-line"></div>

                                <div className="td-post-content news-post-content">

                                    <div className="td-post-featured-image">
                                        <img className="news-featured-image" src={"/api/Video/Image?id=" + this.props.video.Id} ></img>
                                    </div>

                                    <h2 className="entry-title">Descrição</h2>

                                    {

                                        this.props.video.DescriptionPars.map(par => {

                                                return <p>{par}</p>

                                        })

                                    }

                                    <h2 className="entry-title">Links</h2>
                                    <p>{this.props.video.YoutubeLink}</p>
                                    <p>{this.props.video.BitchuteLink}</p>
     
                                </div>

                                {this.props.video.Tags != null && this.props.video.Tags.length > 0 &&

                                    <div className="td-post-content news-post-content">
                                        
                                        <h2 className="entry-title">Tags</h2>
                                        <p>{this.props.video.Tags}</p>

                                    </div>

                                }

                                {this.props.video.Script != null && this.props.video.Script.length > 0 &&

                                    <div className="td-post-content news-post-content">

                                        <h2 className="entry-title">Script</h2>

                                        <p>
                                            <span className="dropcap dropcap3">{capital}</span>
                                            {primeiro}
                                        </p>

                                        {

                                            this.props.video.ScriptPars.map(par => {

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
    (state: ApplicationState) => { return { ...state.account, ...state.video } },
    { ...AccountStore.actionCreators, ...VideoStore.actionCreators }
)(Video as any) as typeof Video;
