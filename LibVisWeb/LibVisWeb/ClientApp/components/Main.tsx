import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as MainPageStore from '../store/MainPage';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';
import ShowArticle from './ShowArticle';
import ShowVideo from './ShowVideo';
import ShowTarget from './ShowTarget';

type MainProps =
    AccountStore.AccountState
    & MainPageStore.MainPageState
    & typeof AccountStore.actionCreators
    & typeof MainPageStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class Main extends React.Component<MainProps, {}> {

    componentDidMount() {

        if (!this.props.validToken || this.props.user == null) {

            this.props.push('/login');

        } else {

            this.props.requestMainPage();

        }

    }

    public render() {
        
        if (!this.props.isLoading && this.props.mainPage != null && this.props.user != null) {

            return (

                <div>

                    <Header />

                    <div className="container open-line"></div>

                    <div className="tdc-row">
                        <div className="td-business-home-row wpb_row td-pb-row">
                            <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span4">
                                <div className="wpb_wrapper">

                                    <div className="td-pb-border-top">
                                        <div className="td-block-title-wrap">
                                            <h4 className="td-block-title">
                                                <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.Profile)}</span>
                                            </h4>
                                        </div>
                                    </div>

                                    <div className="td_block_wrap">

                                        <div className="user-profile">

                                            <img src={'/api/Avatar/' + this.props.user.Account.Id} ></img> <br /> <br />
                                            <p>
                                                {this.props.user.Account.Name}<br />
                                                {this.props.user.Account.ProfileName}
                                            </p>                                                
                                            <p>
                                                {

                                                    this.props.user.Account.Medals.map(m => {

                                                        return (

                                                            <div className="tda-social">
                                                                <img src={'/dist/img/b/' + m.Id + '.png'} title={m.Name}></img>
                                                            </div>

                                                        );

                                                    })

                                                }

                                            </p>
                                            <p className="points">
                                                {this.props.user.Account.Xp} <small>{RacMsg.Get(RacMsg.Id.ExperiencePoints)} ({this.props.user.Account.NextXp} {RacMsg.Get(RacMsg.Id.nextMedals)})</small>
                                            </p>
                                            <p className="points">
                                                {this.props.user.Account.Ap} <small>{RacMsg.Get(RacMsg.Id.AgendaPoints)} ({this.props.user.Account.NextAp} {RacMsg.Get(RacMsg.Id.nextMedals)})</small>
                                            </p>
                                            <p className="points">
                                                {this.props.user.Account.Rp} <small>{RacMsg.Get(RacMsg.Id.WriterPoints)} ({this.props.user.Account.NextRp} {RacMsg.Get(RacMsg.Id.nextMedals)})</small>
                                            </p>                                                
                                            <br /><br />
                                            <p>
                                                <NavLink to={'/edit-profile'} className="btn-edit-profile">{RacMsg.Get(RacMsg.Id.EditProfile)}</NavLink><br />
                                                <NavLink to={'/help-points'} className="btn-edit-profile">{RacMsg.Get(RacMsg.Id.KnowThePointSystem)}</NavLink>
                                            </p>

                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span8">
                                <div className="wpb_wrapper">
                                    <div className="td-pb-border-top">

                                        <div className="td-block-title-wrap">
                                            <h4 className="td-block-title">
                                                <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.YourPayments)}</span>
                                            </h4>
                                        </div>

                                        <div className="td_block_inner td-column-2">

                                            <div className="td-block-span12">

                                                <table className="pay-table">
                                                    <tr className="pay-table-header">
                                                        <td>{RacMsg.Get(RacMsg.Id.Ref)}</td>
                                                        <td>{RacMsg.Get(RacMsg.Id.Value)}</td>
                                                        <td>{RacMsg.Get(RacMsg.Id.Observation)}</td>
                                                        <td>{RacMsg.Get(RacMsg.Id.Address)}</td>
                                                    </tr>

                                                    {

                                                        this.props.user.Account.Payments.map(n => {

                                                            return (

                                                                <tr>
                                                                    <td>{n.MonthYearRef}</td>
                                                                    <td>{n.Value}</td>
                                                                    <td>{n.Observation}</td>
                                                                    <td><a href={'https://www.blockchain.com/pt/btc/tx/' + n.TransactionId}>{n.Address}</a></td>
                                                                </tr>

                                                            );

                                                        })

                                                    }

                                                </table>                                                

                                            </div>

                                        </div>

                                        {this.props.user.Account.Videos != undefined &&

                                            <div>

                                                <div className="tdc-video-row">
                                                    <br /><br />
                                                {RacMsg.Get(RacMsg.Id.EstimatedReceiptAmountThisMonth)}: {this.props.user.Account.Value.toFixed(6)} BTC < br /> <br />

                                                </div>

                                                <div className="tdc-video-row">
                                                    
                                                    <table className="pay-table">
                                                        <tr className="pay-table-header">
                                                            <td>{RacMsg.Get(RacMsg.Id.Date)}</td>
                                                            <td>{RacMsg.Get(RacMsg.Id.Article)}</td>
                                                            <td>{RacMsg.Get(RacMsg.Id.Role)}</td>
                                                            <td>{RacMsg.Get(RacMsg.Id.Words)}</td>
                                                            <td>{RacMsg.Get(RacMsg.Id.Total)}</td>
                                                        </tr>

                                                        {

                                                            this.props.user.Account.Videos.map(n => {

                                                                return (

                                                                    <tr>
                                                                        <td>{n.Date}</td>
                                                                        <td><NavLink to={'/article/' + n.Id}>{n.Title}</NavLink></td>
                                                                        <td>{n.Role}</td>
                                                                        <td>{n.Word.toFixed(0)}</td>
                                                                        <td>{n.Total.toFixed(6)}</td>
                                                                    </tr>

                                                                );

                                                            })


                                                        }

                                                    </table>
                                                <br /><br />
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

                                    {this.props.mainPage.Videos.length > 0 && 

                                        <div className="td-pb-border-top">
                                            <div className="td-block-title-wrap">
                                                <h4 className="td-block-title">
                                                    <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.YourLastVideos)}</span>
                                                </h4>
                                            </div>
                                            <div className="td_block_inner td-column-2">

                                                <div className="td-block-span12">

                                                    {

                                                        this.props.mainPage.Videos.map(n => {

                                                            return (

                                                                <ShowVideo video={n} />

                                                            );

                                                        })

                                                    }

                                                </div>

                                            </div>

                                        </div>

                                    }

                                    {this.props.mainPage.Articles.length > 0 &&

                                        <div className="td-pb-border-top">
                                            <div className="td-block-title-wrap">
                                                <h4 className="td-block-title">
                                                    <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.YourLastArticles)}</span>
                                                </h4>
                                            </div>
                                            <div className="td_block_inner td-column-2">

                                                <div className="td-block-span12">

                                                    {

                                                        this.props.mainPage.Articles.map(n => {

                                                            return (

                                                                <ShowArticle article={n} />

                                                            );

                                                        })

                                                    }

                                                </div>

                                            </div>

                                        </div>

                                    }

                                    {this.props.mainPage.Targets.length > 0 &&

                                        <div className="td-pb-border-top">
                                            <div className="td-block-title-wrap">
                                                <h4 className="td-block-title">
                                                    <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.YourLastAgendas)}</span>
                                                </h4>
                                            </div>
                                            <div className="td_block_inner td-column-2">

                                                <div className="td-block-span12">

                                                    {

                                                        this.props.mainPage.Targets.map(n => {

                                                            return (

                                                                <ShowTarget target={n} />

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

        }
        else
        {

            return (

                <div>

                    <Header />

                    <div className="container open-line"></div>

                    <div className="tdc-row">
                        <div className="td-business-home-row wpb_row td-pb-row">
                            <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                                <div className="wpb_wrapper">
                                    <div className="td_block_wrap">

                                        {RacMsg.Get(RacMsg.Id.LoadindInformation)}...

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
    (state: ApplicationState) => { return { ...state.account, ...state.mainPage } },
    { ...AccountStore.actionCreators, ...MainPageStore.actionCreators, ...routerActions }
)(Main as any) as typeof Main;