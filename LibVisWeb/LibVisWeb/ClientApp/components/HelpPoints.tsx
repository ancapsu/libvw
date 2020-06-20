import * as React from 'react';
import { Link, RouteComponentProps, NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type HelpPointsProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class HelpPoints extends React.Component<HelpPointsProps, {}> {

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
                                        <span className="news-page-headline">Sistema de pontos</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="tdc-video-row">
                    <p>
                        O site do visão libertária funciona com um sistema de pontos. Você ganha pontos em cada ação que faz no site e, com os pontos, ganha medalhas especiais que aparecem próximo ao seu nome de usuário. Lógico, isso além da remuneração que você pode ver em <NavLink to='/help-values' className='plain-link'>Prêmios e valores em satoshis</NavLink>. Veja abaixo como ganhar pontos.
                    </p>
                    <br />
                    <b>Sugerir uma pauta</b>
                    <p>
                        Qualquer um pode sugerir uma pauta. Os pontos que você ganha são:
                    </p>
                    <ul>
                        <li>Sugestão de pauta – 100 XP, 100 AP, máximo de 3 por dia</li>
                        <li>Pauta aprovada – 1.000 XP, 2.500 AP</li>
                        <li>Pauta usada em texto – 3.000 XP, 5.000 AP</li>
                    </ul>
                    <p>
                        Você pode ganhar prêmios extras que dão direito a medalhas específicas:
                    </p>
                    <ul>
                        <li>Pauta original, dada quando uma pauta não foi pensada e é muito rara no jornal, 5.000 XP, 7.500 AP, 100 OP, 100 EP</li>
                        <li>Pauta extraordinária, uma pauta que, na visão do aprovador, foi realmente muito legal, por qualquer motivo, 10.000 XP, 10.000 AP, 500 QP, 100 EP</li>
                        <li>Pauta internacional, quando é uma pauta de longe, sem referência local, ou seja, que só saiu em jornais lá fora e, portanto, para colocar é necessário ler em outra língua. Note que não basta ser em inglês, tem que ser algo que não há equivalente em português ainda, 7.500 XP 10.000 AP, 500 TP, 100 EP</li>
                        <li>Pauta urgente, referente a algo sensível que acabou de acontecer, mas é extremamente relevante, 15.000 XP, 10.000 AP, 100 UP, 100 SP, 100 EP</li>
                    </ul>
                    <p>
                        Também pode sofrer penalizações
                    </p>
                    <ul>
                        <li>Apontar pautas já existentes ou muito antigas – menos 50 XP, 50 AP</li>
                        <li>Flood com pautas inválidas – menos 500 XP, 500 AP, zera EP, bloqueio 5 dias</li>
                    </ul>
                    <p>
                        A carreira como pautador segue esse caminho:
                    </p>
                    <ul>
                        <li>2.500 AP – Pitaqueiro</li>
                        <li>20.000 AP – Pautador</li>
                        <li>250.000 AP – Chefe de pauta</li>
                        <li>5.000.000 AP – Gerente de reportagem</li>
                        <li>25.000.000 AP – Diretor de jornal</li>
                        <li>500.000.000 AP – Executivo de pauta midiática</li>
                    </ul>
                    <br />
                    <b>Escrever um artigo ou crônica</b>
                    <p>
                        Qualquer um pode escrever um artigo, crônia, tapa ou nota libertária. 
                    </p>
                    <ul>
                        <li>Tapa enviado, 100 XP, 100 RP</li>
                        <li>Nota enviado, 500 XP, 500 RP</li>
                        <li>Artigo enviado, 1.000 XP, 1.000 RP</li>
                        <li>Tapa aprovado, 500 XP, 500 RP</li>
                        <li>Nota aprovado, 2.500 XP, 2.500 RP</li>
                        <li>Artigo aprovado, 5.000 XP, 5.000 RP, 100 LP</li>
                        <li>Tapa publicado, 1.500 XP, 1.000 RP</li>
                        <li>Nota publicado, 7.500 XP, 2.000 RP, 100 LP</li>
                        <li>Reportagem publicado, 15.000 XP, 10.000 RP, 200 LP</li>
                    </ul>
                    <p>
                        Você pode ganhar prêmios extras que dão direito a medalhas específicas:
                    </p>
                    <ul>
                        <li>Reportagem original, reportagem que contém vídeo gravado pela própria pessoa e áudio grava¬do pela própria pessoa ou é sobre uma pauta original, 10.000 XP, 10.000 RP, 500 OP, 100 EP</li>
                        <li>Reportagem em local original, reportagem que contém vídeo gravado pela própria pessoa e áudio gravado pela própria pessoa em um lugar distante, pouco acessível e raramente reportado, 50.000 XP, 50.000 RP, 1.000 OP, 200 EP</li>
                        <li>Reportagem extraordinária, uma reportagem que, na visão do aprovador, foi realmente muito legal, por qualquer motivo, 10.000 XP, 10.000 RP, 2.500 QP, 200 EP</li>
                        <li>Reportagem do exterior, originalmente em outra língua, sem referência aqui em BR, 10.000 XP, 10.000 RP, 500 TP, 100 EP</li>
                        <li>Reportagem gramaticalmente impecável, uma reportagem que não contém erros de português, erros de estrutura de texto, 10.000 XP, 10.000 RP, 1.500 SP, 100 EP</li>
                        <li>Reportagem urgente, referente a algo sensível que acabou de acontecer, mas é extremamente relevante, 15.000 XP, 10.000 RP, 500 QP, 100 UP, 100 SP, 100 EP</li>
                        <li>Furo de reportagem, algo temporalmente relevante, e tão novo que sequer foi noticiado em outras mídias ainda, 50.000 XP, 25.000 RP, 2.500 QP, 500 UP, 100 SP, 200 EP</li>
                        <li>Super-entrevista, entrevista com alguém de destaque, pessoa muito conhecida mesmo fora do meio libertário, 25.000 XP, 25.000 RP, 2.500 QP, 100 SP, 200 EP</li>
                        <li>Campeã de audiência, matérias que saiam em visões com performance 1 no youtube (melhores que os 10 últimos vídeos), 15.000 XP, 10.000 RP, 100 AP, 100 EP</li>
                        <li>Usa os termos do canal sempre que possível, mas sem ficar repetitivo demais. 10.000 XP, 10.000 RP, 100 QP, 100 SP, 100 EP</li>
                    </ul>
                    <p>
                        Também pode sofrer penalizações
                    </p>
                    <ul>
                        <li>Textos com muitos erros de redação – menos 100 XP, 100 RP</li>
                        <li>Vídeos fora do padrão ou que requeiram ajustes – menos 500 XP, 500 RP</li>
                        <li>Áudios fora do padrão ou que requeiram ajustes – menos 500 XP, 500 RP</li>
                        <li>Flood com textos inválidos – menos 1.000 XP, 1.000 RP, zera EP, bloqueio 5 dias</li>
                    </ul>
                    <p>
                        A carreira como pautador segue esse caminho:
                    </p>
                    <ul>
                        <li>2.500 AP – Foca</li>
                        <li>20.000 AP – Repórter</li>
                        <li>250.000 AP – Correspondente</li>
                        <li>5.000.000 AP – Jornalista</li>
                        <li>25.000.000 AP – Chefe de reportagem</li>
                        <li>500.000.000 AP – Âncora</li>
                    </ul>
                    <br /><br />
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
)(HelpPoints as any) as typeof HelpPoints;
