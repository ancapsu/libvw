import * as React from 'react';
import { Link, RouteComponentProps, NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';

const logo_vl_pt_Img: string = require('../../theme/newspaper/img/logowname.png');
const logo_vl_en_Img: string = require('../../theme/newspaper/img/logowname_en.png');
const logo_vl_es_Img: string = require('../../theme/newspaper/img/logowname_es.png');
const logo_as_pt_Img: string = require('../../theme/newspaper/img/logoaswname.png');

type HeaderLogoProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators;    

class HeaderLogo extends React.Component<HeaderLogoProps, {}> {

    public render() {

        return (

            <div className="td-container tda-header-logo tda-header-logo-size">

                <div className="col-lg-4 col-md-4 col-sm-4 col-sx-4">

                    {this.renderLogo()}
                    
                </div>

                {this.renderLoggedUser()}

            </div>

        );

    }

    private renderLogo() {

        if (window.location.href.indexOf("ancap.su") > 0) {

            return <a href="/">
                <img src={logo_as_pt_Img} alt="ancap.su" className="tda-header-logo-img-size"></img>
                <span className="td-visual-hidden">ancap.su</span>
            </a>;

        } else {

            if (this.props.lang == 3) {

                return <a href="/">
                    <img src={logo_vl_pt_Img} alt="visão libertária" className="tda-header-logo-img-size"></img>
                    <span className="td-visual-hidden">{RacMsg.Get(RacMsg.Id.LibertarianViewpoint)}</span>
                </a>;

            } else if (this.props.lang == 4) {

                return <a href="/">
                    <img src={logo_vl_es_Img} alt="visão libertária" className="tda-header-logo-img-size"></img>
                    <span className="td-visual-hidden">{RacMsg.Get(RacMsg.Id.LibertarianViewpoint)}</span>
                </a>;

            } else {

                return <a href="/">
                    <img src={logo_vl_en_Img} alt="visão libertária" className="tda-header-logo-img-size"></img>
                    <span className="td-visual-hidden">{RacMsg.Get(RacMsg.Id.LibertarianViewpoint)}</span>
                </a>;

            }

        }
        
    }

    private renderLoggedUser() {

        if (this.props.validToken && this.props.user != null && this.props.user.Account != undefined) {

            return (<div className="col-lg-8 col-md-8 col-sm-8 col-sx-8 header-user-box-logged">

                    <div className="col-lg-2 col-md-2 col-sm-2 col-sx-2 header-logoff-box header-user-image">
                        <NavLink to={'/edit-profile'}>
                            <img src={'/api/Avatar/' + this.props.user.Account.Id} className="" ></img>
                        </NavLink>
                    </div>

                    <div className="col-lg-10 col-md-10 col-sm-10 col-sx-10 header-logoff-box">

                        <span className="header-username">
                            <NavLink to={'/edit-profile'}>
                                {this.props.user.Account.Name} ({this.props.user.Account.Email})
                            </NavLink>    
                            <a className="btn-header-logoff" onClick={() => { this.props.logoff(); }}>
                                {RacMsg.Get(RacMsg.Id.Logoff)}
                            </a>
                        </span>

                        <span className="header-qualifications">
                            {this.props.user.Account.GeneralQualification}
                        </span>

                        <span className="header-medals-box">
                            {

                                this.props.user.Account.Medals.map(m => {

                                    return (

                                        <div className="tda-social">
                                            <img src={'/dist/img/b/' + m.Id + '.png'} title={m.Name}></img>
                                        </div>

                                    );

                                })

                            }
                        </span>

                    </div>

                </div>);

        } else {

            return (<div className="col-lg-8 col-md-8 col-sm-8 col-sx-8 header-user-box">

                <NavLink className="btn-header-login" to={'\login'}>
                    {RacMsg.Get(RacMsg.Id.Login)}
                </NavLink>
                <NavLink className="btn-header-login" to={'\create-account'}>
                    {RacMsg.Get(RacMsg.Id.CreateAccount)}
                </NavLink>

            </div>);

        }

    }

}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(HeaderLogo as any);
