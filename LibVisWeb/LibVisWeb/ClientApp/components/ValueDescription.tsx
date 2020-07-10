import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';
import WaitPanel from './common/WaitPanel';

type ValueDescriptionProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{ month: string }>;

class ValueDescription extends React.Component<ValueDescriptionProps, {}> {

    contentReady: boolean;
    year: number;
    month: number;

    constructor(props: ValueDescriptionProps, context: any) {

        super(props, context);

        this.year = 2020;
        this.month = 1;

        var syear: string = this.props.match.params.month.substring(0, 4);
        var smonth: string = this.props.match.params.month.substring(4, 6);

        this.year = parseInt(syear);
        this.month = parseInt(smonth);

        this.contentReady = false;

    }

    componentDidMount() {

        if (!this.props.validToken || this.props.user == null || this.props.user.Account.Profile < 7) {

            this.props.push('/login');

        }

        if (this.props.match.params.month != undefined && this.props.match.params.month != null) {

            this.props.requestValueDescription(this.year, this.month);
            
        }
        
    }

    componentWillReceiveProps(props: ValueDescriptionProps) {

        if (this.props.match.params.month != undefined && this.props.match.params.month != null) {

            if (props.hasValueSheet && props.monthValue != null && props.monthValue.Month == this.props.match.params.month) {

                this.contentReady = true;
                this.forceUpdate();

            }

        }

    }

    public render() {

        

        if (this.props.hasValueSheet && this.props.monthValue != null && this.props.monthValue.Month == this.props.match.params.month) {

            return (

                <div>

                    <Header />

                    <WaitPanel isContentReady={this.contentReady} />

                    <div className="container open-line"></div>

                    <div className="tdc-video-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">

                            <div className="post type-post status-publish format-standard has-post-thumbnail category-entrepreneurs">

                                <div className="td-post-header">

                                    <header className="td-post-title">
                                        <h1 className="entry-title">Descrição de valores para o mês {this.month} / {this.year}</h1>                                    
                                    </header>
                                </div>

                                <div className="container open-line"></div>
                                <div className="container open-line"></div>

                            </div>

                        </div>
                    </div>

                    <div className="tdc-video-row">

                        Valor total: {this.props.monthValue.Total.toFixed(6)} BTC < br /> <br />

                    </div>

                    <div className="tdc-video-row">
                        Por vídeo:<br /><br />

                        <table className="pay-table">
                            <tr className="pay-table-header">
                                <td rowSpan={2}>Artigo</td>
                                <td rowSpan={2}>Data</td>
                                <td colSpan={2}>Autor</td>
                                <td colSpan={2}>Revisor</td>
                                <td colSpan={2}>Narrador</td>
                                <td colSpan={2}>Produtor</td>
                                <td rowSpan={2}>Total</td>
                            </tr>
                            <tr className="pay-table-header">
                                <td>Usuário</td>
                                <td>Valor</td>
                                <td>Usuário</td>
                                <td>Valor</td>
                                <td>Usuário</td>
                                <td>Valor</td>
                                <td>Usuário</td>
                                <td>Valor</td>
                            </tr>

                        {
                            
                            this.props.monthValue.Videos.map(n => {

                                return (

                                    <tr>
                                        <td><NavLink to={'/article/' + n.Id}>{n.Title}</NavLink></td>
                                        <td>{n.Date}</td>
                                        <td>{n.Author.Name}</td>
                                        <td>{n.Author.Value.toFixed(6)}</td>
                                        <td>{n.Revisor.Name}</td>
                                        <td>{n.Revisor.Value.toFixed(6)}</td>
                                        <td>{n.Narrator.Name}</td>
                                        <td>{n.Narrator.Value.toFixed(6)}</td>
                                        <td>{n.Producer.Name}</td>
                                        <td>{n.Producer.Value.toFixed(6)}</td>
                                        <td>{n.Total.toFixed(6)}</td>
                                    </tr>                                    

                                );

                            })


                        }

                        </table>
                    </div>

                    <div className="container open-line"></div>
                    <div className="container open-line"></div>

                    <div className="tdc-video-row">
                        Por usuário:<br /><br />
                        <table className="pay-table">
                            <tr className="pay-table-header">
                                <td>Id</td>
                                <td>Usuário</td>
                                <td>Descrição</td>
                                <td>Bitcoin</td>
                                <td>Número</td>
                                <td>Total</td>
                            </tr>
                            {

                                this.props.monthValue.Users.map(n => {

                                    return (

                                        <tr>
                                            <td>{n.Id}</td>
                                            <td><NavLink to={'/user/' + n.Id}>{n.Name}</NavLink></td>
                                            <td>{n.Description}</td>
                                            <td>{n.Bicoin}</td>
                                            <td>{n.Values.length}</td>
                                            <td>{n.Total.toFixed(6)}</td>
                                        </tr>

                                    );

                                })

                            }
                        </table>
                    </div>

                    <div className="container open-line"></div>
                    <div className="container open-line"></div>

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
    (state: ApplicationState) => { return { ...state.account, ...state.article } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators }
)(ValueDescription as any) as typeof ValueDescription;
