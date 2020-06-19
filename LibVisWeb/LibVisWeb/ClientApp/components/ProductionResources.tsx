import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import ErrorMsg from '../message/errormsg';
import Footer from './Footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type ProductionResourcesProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class ProductionResources extends React.Component<ProductionResourcesProps, {}> {

    public render() {

        

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
                                        <span className="news-page-headline">Recursos para produção</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="tdc-video-row">
                    <p>
                        Para produzir o vídeo, você pode precisar de alguns recursos, você pode baixá-los aqui:
                    </p>
                    <br />
                    <b>Audios</b>
                    <table className="resource-table">
                        <tr>
                            <td className="resource-table-first-column">
                                <a href="/dist/resource/visao.mp3"><img src="/dist/resource/audio.png" className="resource-thumb" /></a>
                            </td>
                            <td>
                                <a href="/dist/resource/visao.mp3">Música de fundo do visão libertária</a>
                            </td>
                        </tr>
                        <tr>
                            <td className="resource-table-first-column">
                                <a href="/dist/resource/cronica.mp3"><img src="/dist/resource/audio.png" className="resource-thumb" /></a>
                            </td>
                            <td>
                                <a href="/dist/resource/cronica.mp3">Música de fundo da crônica libertária</a>
                            </td>
                        </tr>
                        <tr>
                            <td className="resource-table-first-column">
                                <a href="/dist/resource/tapa_fundo.mp3"><img src="/dist/resource/audio.png" className="resource-thumb" /></a>
                            </td>
                            <td>
                                <a href="/dist/resource/tapa_fundo.mp3">Música de fundo do tapa libertário</a>
                            </td>
                        </tr>
                        <tr>
                            <td className="resource-table-first-column">
                                <a href="/dist/resource/tapa_tapa.mp3"><img src="/dist/resource/audio.png" className="resource-thumb" /></a>
                            </td>
                            <td>
                                <a href="/dist/resource/tapa_tapa.mp3">Efeito de tapa do tapa libertário</a>
                            </td>
                        </tr>
                    </table>                    
                    <br />
                    <b>Imagens</b>
                    <p>
                        <table className="resource-table">
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/visao1.png"><img src="/dist/resource/visao1.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/visao1.png">Sticker visão libertária inferior esquerda</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/visao2.png"><img src="/dist/resource/visao2.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/visao2.png">Sticker visão libertária inferior direita</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/visao3.png"><img src="/dist/resource/visao3.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/visao3.png">Sticker visão libertária superior esquerda</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/visao4.png"><img src="/dist/resource/visao4.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/visao4.png">Sticker visão libertária superior direita</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/cronica1.png"><img src="/dist/resource/cronica1.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/cronica1.png">Sticker crônica libertária inferior esquerda</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/cronica2.png"><img src="/dist/resource/cronica2.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/cronica2.png">Sticker crônica libertária inferior direita</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/cronica3.png"><img src="/dist/resource/cronica3.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/cronica3.png">Sticker crônica libertária superior esquerda</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/tapa1.png"><img src="/dist/resource/tapa1.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/tapa1.png">Sticker crônica libertária inferior esquerda</a>
                                </td>
                            </tr>
                            <tr>
                                <td className="resource-table-first-column">
                                    <a href="/dist/resource/tapa2.png"><img src="/dist/resource/tapa2.png" className="resource-thumb" /></a>
                                </td>
                                <td>
                                    <a href="/dist/resource/tapa2.png">Sticker crônica libertária inferior direita</a>
                                </td>
                            </tr>
                        </table>
                    </p>
                </div>

                <ErrorMsg />

                <Footer />

            </div>

        );

    }

}

export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, ...routerActions }
)(ProductionResources as any) as typeof ProductionResources;
