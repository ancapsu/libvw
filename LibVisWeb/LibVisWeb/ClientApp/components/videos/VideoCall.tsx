import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';

type VideoCallProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators;

class VideoCall extends React.Component<VideoCallProps, {}> {

    public render() {
        return (

            <div id="td_uid_1_5d29086a93e6a" className="tdc-row">
                <div className="vc_row td_uid_13_5d29086a93e6f_rand  wpb_row td-pb-row">
                    <div className="vc_column td_uid_14_5d29086a93f5e_rand  wpb_column vc_column_container tdc-column td-pb-span12">
                        <div className="wpb_wrapper">
                            <div className="td_block_wrap td_block_big_grid_9 td_uid_15_5d29086a94032_rand td-grid-style-1 td-hover-1 td-big-grids td-pb-border-top td_block_template_11" data-td-block-uid="td_uid_15_5d29086a94032">
                                <div id="td_uid_15_5d29086a94032" className="td_block_inner"><div className="td-big-grid-wrapper">
                                    <div className="td_module_mx15 td-animation-stack td-meta-info-hide td-big-grid-post-0 td-big-grid-post td-medium-thumb">
                                        <div className="td-module-thumb">
                                            <a href="https://visao.ancap.su/destaque/o-que-podemos-fazer-para-te-atrapalhar-hoje/" rel="bookmark" className="td-image-wrap" title="O que podemos fazer para te atrapalhar hoje?">
                                                <img width="356" height="364" className="entry-thumb td-animation-stack-type0-2" src="https://visao.ancap.su/wp-content/uploads/2019/07/Capturar-356x364.png" alt="" title="O que podemos fazer para te atrapalhar hoje?" ></img>
                                            </a>
                                        </div>
                                        <div className="td-meta-info-container">
                                            <div className="td-meta-align">
                                                <div className="td-big-grid-meta">
                                                    <a href="https://visao.ancap.su/category/destaque/" className="td-post-category">Destaque</a>
                                                    <h3 className="entry-title td-module-title"><a href="https://visao.ancap.su/destaque/o-que-podemos-fazer-para-te-atrapalhar-hoje/" rel="bookmark" title="O que podemos fazer para te atrapalhar hoje?">O que podemos fazer para te atrapalhar hoje?</a></h3>
                                                </div>
                                                <div className="td-module-meta-info">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="td_module_mx15 td-animation-stack td-meta-info-hide td-big-grid-post-1 td-big-grid-post td-medium-thumb">
                                        <div className="td-module-thumb">
                                            <a href="https://visao.ancap.su/destaque/mais-valia-existe/" rel="bookmark" className="td-image-wrap" title="Mais valia existe ?">
                                                <img width="356" height="364" className="entry-thumb td-animation-stack-type0-2" src="https://visao.ancap.su/wp-content/uploads/2019/07/2-356x364.jpg" alt="" title="Mais valia existe ?"></img>
                                            </a>
                                        </div>
                                        <div className="td-meta-info-container">
                                            <div className="td-meta-align">
                                                <div className="td-big-grid-meta">
                                                    <a href="https://visao.ancap.su/category/destaque/" className="td-post-category">Destaque</a>
                                                    <h3 className="entry-title td-module-title"><a href="https://visao.ancap.su/destaque/mais-valia-existe/" rel="bookmark" title="Mais valia existe ?">Mais valia existe ?</a></h3>
                                                </div>
                                                <div className="td-module-meta-info">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="td_module_mx15 td-animation-stack td-meta-info-hide td-big-grid-post-2 td-big-grid-post td-medium-thumb">
                                        <div className="td-module-thumb">
                                            <a href="https://visao.ancap.su/destaque/asteroide-recebeu-o-nome-de-cirao-da-massa/" rel="bookmark" className="td-image-wrap" title="Asteróide recebeu o nome de Cirão da Massa">
                                                <img width="356" height="364" className="entry-thumb td-animation-stack-type0-2" src="https://visao.ancap.su/wp-content/uploads/2019/07/3-356x364.png" alt="" title="Asteróide recebeu o nome de Cirão da Massa"></img>
                                            </a>
                                        </div>
                                        <div className="td-meta-info-container">
                                            <div className="td-meta-align">
                                                <div className="td-big-grid-meta">
                                                    <a href="https://visao.ancap.su/category/destaque/" className="td-post-category">Destaque</a>
                                                    <h3 className="entry-title td-module-title"><a href="https://visao.ancap.su/destaque/asteroide-recebeu-o-nome-de-cirao-da-massa/" rel="bookmark" title="Asteróide recebeu o nome de Cirão da Massa">Asteróide recebeu o nome de Cirão da Massa</a></h3>
                                                </div>
                                                <div className="td-module-meta-info">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="td_module_mx15 td-animation-stack td-meta-info-hide td-big-grid-post-3 td-big-grid-post td-medium-thumb">
                                        <div className="td-module-thumb">
                                            <a href="https://visao.ancap.su/destaque/capangas-da-mafia-querem-extorquir-mais/" rel="bookmark" className="td-image-wrap" title="Capangas da máfia querem extorquir mais">
                                                <img width="356" height="364" className="entry-thumb td-animation-stack-type0-2" src="https://visao.ancap.su/wp-content/uploads/2019/07/4-356x364.png" alt="" title="Capangas da máfia querem extorquir mais"></img>
                                            </a>
                                        </div>
                                        <div className="td-meta-info-container">
                                            <div className="td-meta-align">
                                                <div className="td-big-grid-meta">
                                                    <a href="https://visao.ancap.su/category/destaque/" className="td-post-category">Destaque</a>
                                                    <h3 className="entry-title td-module-title">
                                                        <a href="https://visao.ancap.su/destaque/capangas-da-mafia-querem-extorquir-mais/" rel="bookmark" title="Capangas da máfia querem extorquir mais">Capangas da máfia querem extorquir mais</a>
                                                    </h3>
                                                </div>
                                                <div className="td-module-meta-info">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                    <div className="clearfix">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        );

    }

}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(VideoCall as any);