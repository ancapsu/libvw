import * as React from 'react';
import { Link, RouteComponentProps, NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import ErrorMsg from '../message/errormsg';
import Footer from './Footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type AccountabilityProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class Accountability extends React.Component<AccountabilityProps, {}> {

    componentWillMount() {

        if (!this.props.validToken || this.props.user == null || (this.props.user.Account.Profile < 7 && this.props.user.Account.Staff != 1 && this.props.user.Account.Sponsor != 1)) {

            this.props.push('/login');

        }

    }

    public render() {

        var months: string[] = [];
        var curyear: number = new Date().getFullYear();

        for (var y = 2020; y <= curyear; y++) {

            var curmonth: number = 12;
            if (y == curyear)
                curmonth = new Date().getMonth() + 1;

            for (var m = 1; m <= curmonth; m++) {

                var ym: string = y.toString();

                if (m < 10)
                    ym += "0";

                ym += m.toString();

                months.push(ym);

            }

        }

        

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="wpb_wrapper">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.Accountability)}</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="tdc-video-row">
                    {RacMsg.Get(RacMsg.Id.DetailedPaymentInformationForMonth)}:<br /><br />

                    {

                        months.map(n => {

                            return (

                                <NavLink to={'/value-description/' + n} className="tda-menu">{n}<br/></NavLink>

                            );

                        })

                    }
                </div>

                <div className="container open-line"></div>
                <div className="container open-line"></div>

                <ErrorMsg />

                <Footer />

            </div>

        );

    }

}

export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, ...routerActions }
)(Accountability as any) as typeof Accountability;
