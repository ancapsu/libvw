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

type HelpProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class Help extends React.Component<HelpProps, {}> {

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
                                        <span className="news-page-headline">Como o visão libertária funciona?</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="tdc-video-row">
                    <p>
                        O site do visão libertária tem por objetivo produzir vídeos para o site visão libertária. Existem diversas etapas na produção que podem ser feitos por quem deseja contribuir. E, além de ganhar pontos e medalhas no site, você pode ganhar também um trocado em satoshis.
                    </p>
                    <img src="/dist/img/processo.png" />
                    <p>
                        
                    </p>
                    <br />
                    <b>Links para explicação</b>
                    <p>
                        Veja mais detalhes nos links abaixo.
                    </p>
                    <p>
                        <NavLink to='/help-points' className='plain-link'>Sistema de pontos</NavLink> - Como ganhar pontos e medalhas no site.<br />
                        <NavLink to='/help-values' className='plain-link'>Prêmios e valores em satoshis</NavLink> - Como ganhar satoshis fazendo as atividades.<br />
                        <NavLink to='/style-manual' className='plain-link'>Manual de estilo</NavLink> - Para quem quiser escrever ou revisar artigos para o site.<br />
                    </p>
                    <br />
                    <b>Vídeo explicativo</b>
                    <p>
                        O vídeo que explica como o site funciona, em timestamps definidos para cada coisa. Se quiser ver o vídeo todo, só clicar no primeiro link e deixar rolar.
                    </p>
                    <p>
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY" className='plain-link'>Introdução</a> - No início do vídeo explico a motivação do site.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=2m10s" className='plain-link'>Funções públicas</a> - O que você pode fazer sem estar logado.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=3m05s" className='plain-link'>Newsletter</a> - Como se cadastrar e para que serve o newsletter.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=4m00s" className='plain-link'>Criar conta</a> - Como criar um usuário no site.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=7m25s" className='plain-link'>Configuração da conta</a> - Como complementar configuração do usuário.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=10m10s" className='plain-link'>Sugerir notícia</a> - Como sugerir uma notícia para o canal.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=12m03s" className='plain-link'>Votação de pauta</a> - Como votar nas pautas que você quer ver representadas.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=13m40s" className='plain-link'>Criar artigo</a> - Como criar um artigo a partir de uma pauta.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=18m37s" className='plain-link'>Aprovar artigo</a> - Como aprovar um artigo já escrito.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=19m42s" className='plain-link'>Revisão do artigo</a> - Como revisar um artigo já aprovado.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=22m58s" className='plain-link'>Encaminhar para narração</a> - Como escolher um narrador e encaminhar para narração.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=25m33s" className='plain-link'>Narrar um vídeo</a> - Como narrar o vídeo.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=28m07s" className='plain-link'>Produzir um vídeo</a> - Como produzir um vídeo.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=29m26s" className='plain-link'>Encaminhar para produção</a> - Como o vídeo sai da narração e vai para produção.<br />
                        <a href="https://www.youtube.com/watch?v=5RxJj3Cb2WY&t=30m30s" className='plain-link'>Conclusão</a> - Considerações finais.<br />
                    </p>
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
)(Help as any) as typeof Help;
