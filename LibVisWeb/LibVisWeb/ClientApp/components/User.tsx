import * as React from 'react';
import { Link, RouteComponentProps, NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as UserPageStore from '../store/UserPage';
import ErrorMsg from '../message/errormsg';
import Footer from './Footer/Footer';
import Header from './header/Header';
import ShowArticle from './ShowArticle';
import ShowVideo from './ShowVideo';
import ShowTarget from './ShowTarget';

type UserProps =
    AccountStore.AccountState
    & UserPageStore.UserPageState
    & typeof AccountStore.actionCreators
    & typeof UserPageStore.actionCreators
    & RouteComponentProps<{ id: string }>;

class User extends React.Component<UserProps, {}> {

    userid: string;
    
    constructor(props: UserProps, context: any) {

        super(props, context);

        this.userid = "";

    }

    componentDidMount() {

        if (this.userid != this.props.match.params.id) {
            this.props.requestUserPage(this.props.match.params.id);
            this.userid = this.props.match.params.id;
        }

    }

    componentWillReceiveProps() {

        if (this.userid != this.props.match.params.id) {
            this.props.requestUserPage(this.props.match.params.id);
            this.userid = this.props.match.params.id;
        }

    }

    public render() {

        

        if (!this.props.isLoading && this.props.userPage != null) {

            return (

                <div>

                    <Header />

                    <div className="container open-line"></div>

                    <div className="tdc-row">
                        <div className="td-business-home-row wpb_row td-pb-row">
                            <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                                <div className="wpb_wrapper">
                                    <div className="td-pb-border-top">
                                        <div className="td-block-title-wrap">
                                            <h4 className="td-block-title">
                                                <span className="news-page-headline">Usu&aacute;rio</span>
                                            </h4>
                                        </div>
                                    </div>
                                    <div className="td_block_wrap">

                                        {this.props.user != null &&

                                            <div>

                                                <div className="col-lg-3 col-md-4 col-sm-6 col-xs-6">

                                                <img src={'/api/Avatar/' + this.props.userPage.User.Id} className="user-page-image-thumb" ></img>

                                                </div>
                                                <div className="col-lg-9 col-md-8 col-sm-6 col-xs-6">

                                                    <p className="user-page-name">
                                                        {this.props.userPage.User.Name}
                                                    </p>
                                                    <p>
                                                    </p>
                                                    <p>
                                                        {

                                                            this.props.userPage.User.Medals.map(m => {

                                                                return (

                                                                    <div className="tda-social">
                                                                        <img src={'/dist/img/b/' + m.Id + '.png'} title={m.Name}></img>
                                                                    </div>

                                                                );

                                                            })

                                                        }
                                                    </p>

                                                </div>

                                            </div>

                                        }

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="tdc-row">
                        <div className="td-business-home-row wpb_row td-pb-row">
                            <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                                <div className="wpb_wrapper">

                                    {this.props.userPage.Targets.length > 0 &&

                                        <div className="td-pb-border-top">

                                            <div className="container open-line"></div>
                                            <div className="container open-line"></div>

                                            <div className="td-block-title-wrap">
                                                <h4 className="td-block-title">
                                                    <span className="news-page-headline">Pautas do usu&aacute;rio</span>
                                                </h4>
                                            </div>
                                            <div className="td_block_inner td-column-2">

                                                <div className="td-block-span12">

                                                    {

                                                        this.props.userPage.Targets.map(n => {

                                                            return (
                                                                
                                                                <ShowTarget target={n} />
                                                                
                                                            );

                                                        })

                                                    }

                                                </div>

                                            </div>

                                        </div>

                                    }

                                    {this.props.userPage.Articles.length > 0 &&

                                        <div className="td-pb-border-top">

                                            <div className="container open-line"></div>
                                            <div className="container open-line"></div>

                                            <div className="td-block-title-wrap">
                                                <h4 className="td-block-title">
                                                    <span className="news-page-headline">Not&iacute;cias do usu&aacute;rio</span>
                                                </h4>
                                            </div>
                                            <div className="td_block_inner td-column-2">

                                                <div className="td-block-span12">

                                                    {

                                                    this.props.userPage.Articles.map(n => {

                                                            return (

                                                                <ShowArticle article={n} />

                                                            );

                                                        })

                                                    }

                                                </div>

                                            </div>

                                        </div>

                                    }

                                    {this.props.userPage.Videos.length > 0 &&

                                        <div className="td-pb-border-top">

                                            <div className="container open-line"></div>
                                            <div className="container open-line"></div>

                                            <div className="td-block-title-wrap">
                                                <h4 className="td-block-title">
                                                    <span className="news-page-headline">V&iacute;deos do usu&aacute;rio</span>
                                                </h4>
                                            </div>
                                            <div className="td_block_inner td-column-2">

                                                <div className="td-block-span12">

                                                    {

                                                        this.props.userPage.Videos.map(n => {

                                                            return (
                                                                
                                                                <ShowVideo video={n} />

                                                            );

                                                        })

                                                    }

                                                </div>

                                            </div>

                                        </div>

                                    }

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

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.userPage } },
    { ...AccountStore.actionCreators, ...UserPageStore.actionCreators }
)(User as any) as typeof User;