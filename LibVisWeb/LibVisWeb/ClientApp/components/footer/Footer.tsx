import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';

const youtubeImg: string = require('../../theme/newspaper/img/youtube16w.png');
const bitchuteImg: string = require('../../theme/newspaper/img/bitchute16w.png');
const facebookImg: string = require('../../theme/newspaper/img/facebook16w.png');
const mindsImg: string = require('../../theme/newspaper/img/minds16w.png');
const twitterImg: string = require('../../theme/newspaper/img/twitter16w.png');
const gabImg: string = require('../../theme/newspaper/img/gab16w.png');

const logo01Img: string = require('../../theme/newspaper/img/logownamey.png');
const logo02Img: string = require('../../theme/newspaper/img/logownamey_en.png');
const logo03Img: string = require('../../theme/newspaper/img/logownamey_es.png');

const logo1Img: string = require('../../theme/newspaper/img/logoaswnamey.png');

const apoiaseImg: string = require('../../theme/newspaper/img/apoiase70.png');
const padrimImg: string = require('../../theme/newspaper/img/padrim70.png');
const patreonImg: string = require('../../theme/newspaper/img/patreon70.png');

const canecaImg: string = require('../../theme/newspaper/img/caneca1.png');
const tshirtImg: string = require('../../theme/newspaper/img/tshirt1.png');

type FooterProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators;

class Footer extends React.Component<FooterProps, {}> {

    public render() {

        return (

            <div className="td-footer-wrapper td-container-wrap td-footer-template-3 ">
                <div className="td-container">
                    <div className="td-pb-row">
                        <div className="td-pb-span12">
                            
                        </div>
                    </div>
                    <div className="td-pb-row">
                        <div className="td-pb-span4">
                            <div className="td-footer-info">
                                <div className="footer-logo-wrap">
                                    {this.renderLogo()}    
                                </div>

                                {this.renderMessage()}

                                <div className="footer-social-wrap td-social-style-2">
                                    <span className="tda-social-icon-wrap">
                                        <a target="_blank" href="https://youtube.com/ancapsu" title="Youtube">
                                            <img src={youtubeImg} ></img>
                                        </a>
                                    </span>
                                    <span className="tda-social-icon-wrap">
                                        <a target="_blank" href="https://bitchute.com/ancapsu" title="Bitchute">
                                            <img src={bitchuteImg} ></img>
                                        </a>
                                    </span>

                                    <span className="tda-social-icon-wrap">
                                        <a target="_blank" href="https://www.facebook.com/pageancapsu/" title="Facebook">
                                            <img src={facebookImg} ></img>
                                        </a>
                                    </span>
                                    <span className="tda-social-icon-wrap">
                                        <a target="_blank" href="https://minds.com/ancapsu" title="Minds">
                                            <img src={mindsImg} ></img>
                                        </a>
                                    </span>

                                    <span className="tda-social-icon-wrap">
                                        <a target="_blank" href="https://twitter.com/ancapsu" title="Twitter">
                                            <img src={twitterImg} ></img>
                                        </a>
                                    </span>
                                    <span className="tda-social-icon-wrap">
                                        <a target="_blank" href="https://gab.ai/ancapsu" title="Gab">
                                            <img src={gabImg} ></img>
                                        </a>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div className="td-pb-span4">
                            <div className="td_block_wrap td_block_7 td_block_widget td_uid_55_5d33621557e17_rand td-pb-border-top td_block_template_11 td-column-1 td_block_padding" data-td-block-uid="td_uid_55_5d33621557e17">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="td-pulldown-size">{RacMsg.Get(RacMsg.Id.HelpOurProject)}</span>
                                    </h4>
                                </div>

                                {this.props.lang == 3 &&

                                    <div className="td_block_inner">
                                        <div className="td-block-span12">
                                            <div className="td_module_6 td_module_wrap td-animation-stack td-meta-info-hide">
                                                <div className="td-module-thumb">
                                                    <a href="https://apoia.se/ancapsu" title="apoia.se">
                                                        <img className="thumb-support-site" src={apoiaseImg} alt="apoia.se ancapsu" title="apoia.se ancapsu"></img>
                                                    </a>
                                                </div>
                                                <div className="item-details">
                                                    <h3 className="entry-title td-module-title">
                                                        <a href="https://apoia.se/ancapsu" title="apoia.se">
                                                            <b>Apoia.se</b> &nbsp;
                                                        <small>Oferece todos os modos de pagamento tradicionais no Brasil, inclusive boleto.</small>
                                                        </a>
                                                    </h3>
                                                    <div className="td-module-meta-info">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="td-block-span12">
                                            <div className="td_module_6 td_module_wrap td-animation-stack td-meta-info-hide">
                                                <div className="td-module-thumb">
                                                    <a href="https://padrim.com.br/ancapsu" title="padrim">
                                                        <img className="thumb-support-site" src={padrimImg} alt="padrim ancapsu" title="padrim ancapsu"></img>
                                                    </a>
                                                </div>
                                                <div className="item-details">
                                                    <h3 className="entry-title td-module-title">
                                                        <a href="https://padrim.com.br/ancapsu" title="padrim">
                                                            <b>Padrim</b> &nbsp;
                                                        <small>Oferece todos os modos de pagamento tradicionais no Brasil, inclusive boleto.</small>
                                                        </a>
                                                    </h3>
                                                    <div className="td-module-meta-info">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="td-block-span12">
                                            <div className="td_module_6 td_module_wrap td-animation-stack td-meta-info-hide">
                                                <div className="td-module-thumb">
                                                    <a href="https://patreon.com/ancapsu" title="patreon">
                                                        <img className="thumb-support-site" src={patreonImg} alt="patreon ancapsu" title="patreon ancapsu"></img>
                                                    </a>
                                                </div>
                                                <div className="item-details">
                                                    <h3 className="entry-title td-module-title">
                                                        <a href="https://patreon.com/ancapsu" title="patreon ">
                                                            <b>Patreon</b> &nbsp;
                                                        <small>{RacMsg.Get(RacMsg.Id.PatreonDescription)}.</small>
                                                        </a>
                                                    </h3>
                                                    <div className="td-module-meta-info">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                }

                                {this.props.lang != 3 &&

                                    <div className="td_block_inner">                                        
                                        <div className="td-block-span12">
                                            <div className="td_module_6 td_module_wrap td-animation-stack td-meta-info-hide">
                                                <div className="td-module-thumb">
                                                    <a href="https://patreon.com/ancapsu" title="patreon">
                                                        <img className="thumb-support-site" src={patreonImg} alt="patreon ancapsu" title="patreon ancapsu"></img>
                                                    </a>
                                                </div>
                                                <div className="item-details">
                                                    <h3 className="entry-title td-module-title">
                                                        <a href="https://patreon.com/ancapsu" title="patreon ">
                                                            <b>Patreon</b> &nbsp;
                                                        <small>{RacMsg.Get(RacMsg.Id.PatreonDescription)}.</small>
                                                        </a>
                                                    </h3>
                                                    <div className="td-module-meta-info">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                }


                            </div>
                        </div>

                        <div className="td-pb-span4">
                            <div className="td_block_wrap td_block_10 td_block_widget td_uid_56_5d33621558ec7_rand td-pb-border-top td_block_template_11 td-column-1 td_block_padding td_block_bot_line" data-td-block-uid="td_uid_56_5d33621558ec7">

                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="td-pulldown-size">{RacMsg.Get(RacMsg.Id.BuyInOurStore)}</span>
                                    </h4>
                                </div>

                                <div className="td_block_inner">

                                    <div className="td-block-span12">
                                        <div className="td_module_6 td_module_wrap td-animation-stack td-meta-info-hide">
                                            <div className="td-module-thumb">
                                                <a href="https://apoio.ancap.su/caneca-ancapsu" title="caneca">
                                                    <img className="thumb-support-site" src={canecaImg} alt={RacMsg.Get(RacMsg.Id.AncapSuMug)} title={RacMsg.Get(RacMsg.Id.AncapSuMug)}></img>
                                                </a>
                                            </div>
                                            <div className="item-details">
                                                <h3 className="entry-title td-module-title">
                                                    <a href="https://apoio.ancap.su/caneca-ancapsu" title={RacMsg.Get(RacMsg.Id.AncapSuMug)}>
                                                        <b>{RacMsg.Get(RacMsg.Id.AncapSuMug)}</b> &nbsp;
                                                        <small>{RacMsg.Get(RacMsg.Id.AncapSuMugDescription)}</small>
                                                    </a>
                                                </h3>
                                                <div className="td-module-meta-info">
                                                </div>
                                            </div>
                                        </div>
                                    </div>       

                                    <div className="td-block-span12">
                                        <div className="td_module_6 td_module_wrap td-animation-stack td-meta-info-hide">
                                            <div className="td-module-thumb">
                                                <a href="https://apoio.ancap.su/camiseta-ancapsu" title="camisa">
                                                    <img className="thumb-support-site" src={tshirtImg} alt={RacMsg.Get(RacMsg.Id.AncapSuShirt)} title={RacMsg.Get(RacMsg.Id.AncapSuShirt)}></img>
                                                </a>
                                            </div>
                                            <div className="item-details">
                                                <h3 className="entry-title td-module-title">
                                                    <a href="https://apoio.ancap.su/camiseta-ancapsu" title={RacMsg.Get(RacMsg.Id.AncapSuShirt)}>
                                                        <b>{RacMsg.Get(RacMsg.Id.AncapSuShirt)}</b> &nbsp;
                                                        <small>{RacMsg.Get(RacMsg.Id.AncapSuShirtDescription)}</small>
                                                    </a>
                                                </h3>
                                                <div className="td-module-meta-info">
                                                </div>
                                            </div>
                                        </div>
                                    </div>   
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        );

    }

    private renderLogo() {

        if (window.location.href.indexOf("ancap.su") > 0) {

            return <a href="/">
                <img className="td-retina-data" src={logo1Img}></img>
            </a>;

        } else {

            if (this.props.lang == 3) {

                return <a href="/">
                    <img className="td-retina-data" src={logo01Img}></img>
                </a>;

            } else if (this.props.lang == 4) {

                return <a href="/">
                    <img className="td-retina-data" src={logo03Img}></img>
                </a>;

            } else {

                return <a href="/">
                    <img className="td-retina-data" src={logo02Img}></img>
                </a>;

            }

        }

    }

    private renderMessage() {

        if (window.location.href.indexOf("ancap.su") > 0) {

            return <div className="footer-text-wrap">
                {RacMsg.Get(RacMsg.Id.FooterAncapSuMessage1)} &nbsp;
                <a className="yellow-link" href={RacMsg.Get(RacMsg.Id.LibertarianViewSiteUrl)}>{RacMsg.Get(RacMsg.Id.LibertarianViewSiteName)}</a>
                <div className="footer-email-wrap">{RacMsg.Get(RacMsg.Id.ContactUs)}:&nbsp;
                    <a href="mailto:ancapsu@gmail.com" className="yellow-link">ancapsu@gmail.com</a>
                </div>
            </div>;

        } else {

            return <div className="footer-text-wrap">
                {RacMsg.Get(RacMsg.Id.FooterLibertarianViewMessage1)} &nbsp;
                <a className="yellow-link" href={RacMsg.Get(RacMsg.Id.AncapSuSiteUrl)}>{RacMsg.Get(RacMsg.Id.AncapSuSiteName)}</a>
                &nbsp; {RacMsg.Get(RacMsg.Id.FooterLibertarianViewMessage2)}
                <div className="footer-email-wrap">{RacMsg.Get(RacMsg.Id.ContactUs)}:&nbsp;
                    <a href="mailto:ancapsu@gmail.com" className="yellow-link">ancapsu@gmail.com</a>
                </div>
            </div>;

        }

    }

}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }          
)(Footer as any);