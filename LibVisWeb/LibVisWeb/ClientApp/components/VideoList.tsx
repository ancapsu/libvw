import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as VideoStore from '../store/Video';
import ErrorMsg from '../message/errormsg';
import Footer from './Footer/Footer';
import Header from './header/Header';
import ShowVideo from './ShowVideo';

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type VideoListProps =
    AccountStore.AccountState
    & VideoStore.VideoState
    & typeof AccountStore.actionCreators
    & typeof VideoStore.actionCreators
    & RouteComponentProps<{}>;

class VideoList extends React.Component<VideoListProps, {}> {

    contentReady: boolean;

    constructor(props: VideoListProps, context: any) {

        super(props, context);

        this.LoadMore = this.LoadMore.bind(this);
        this.contentReady = false;

    }

    componentDidMount() {

        if (!this.props.hasList) {

            this.props.requestVideoList(this.props.lang, 0, 10);

        } else {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: VideoListProps) {

        if ((props.hasList && props.videos.length != this.props.videos.length) || (!this.props.hasList && props.hasList)) {

            this.contentReady = true;

        }

    }

    LoadMore() {

        if (this.props.hasList) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestVideoList(this.props.lang, this.props.videos.length, 10);

        }

    }

    public render() {

        

        if (this.props.hasList) {

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
                                                <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.Videos)}</span>
                                            </h4>
                                        </div>


                                        <div className="row">

                                            <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                                {RacMsg.Get(RacMsg.Id.ListOfChannelVideos)}
                                            </div>

                                            {this.props.user != null && this.props.user.Account.Profile >= 7 &&

                                                <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                                    <NavLink to={'/new-video'} className="btn-create-new float-right">{RacMsg.Get(RacMsg.Id.RegisterNewVideo)}</NavLink>
                                                </div>

                                            }

                                        </div>

                                        <div className="container open-line"></div>

                                        <div className="container open-line"></div>

                                        <div className="td_block_inner td-column-2">

                                            {

                                                this.props.videos.map(n => {

                                                    return (

                                                        <ShowVideo video={n} />

                                                    );

                                                })

                                            }

                                        </div>


                                        <div className="container open-line"></div>

                                        <div className="container open-line"></div>

                                        <div className="td_block_inner td-column-2">

                                            {this.props.videos.length < this.props.total &&

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
    (state: ApplicationState) => { return { ...state.account, ...state.video } },
    { ...AccountStore.actionCreators, ...VideoStore.actionCreators }
)(VideoList as any) as typeof VideoList;
