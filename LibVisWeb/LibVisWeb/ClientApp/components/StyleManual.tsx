import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type StyleManualProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class StyleManual extends React.Component<StyleManualProps, {}> {

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
                                        <span className="news-page-headline">Manual de estilo Visão Libertária</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="tdc-video-row">
                    <p>
                        A revisão dos artigos deve ser feita segundo as diretrizes abaixo.
                    </p>
                    <br />
                    <b>Revisão ortográfia e gramatical</b>
                    <p>
                        Verificar se o artigo está bem escrito, de forma geral. Usar o word para checar ortografia. Checar concordância verbal.
                    </p>
                    <br />
                    <b>Revisão de estilo</b>
                    <p>
                        Não basta estar escrito corretamente, tem que estar escrito de forma que o narrador consiga ler.
                    </p>
                    <p>
                        Normalmente, a maior parte dos textos é muito curta. Então, se puder complementar, incluir mais alguma coisa, sempre dá vídeos melhores e mais completos. A falta de uma descrição do assunto é muito comum. Favor complementar.
                    </p>
                    <ul>
                        <li>Frases deve ter, no máximo absoluto 30 palavras. Preferencialmente 20. Se houver mais de 30 palavras entre pontos, não vírgulas, pontos, é preciso quebrar a frase.</li>
                        <li>O artigo precisa ter um começo, uma introdução que descreva o problema ou notícia em questão. Mesmo que para a maioria só o aspecto libertário importe, tem gente que não viu a notícia em si.</li>
                        <li>O artigo precisa ter referências libertárias, explicando o problema do ponto de vista liberário.</li>
                        <li>O artigo precisa ter uma conclusão. Um último parágrafo ou frase que resuma a conclusão libertária sobre o ponto tratado.</li>
                        <li>Usar as expressões comuns do canal é sempre recomendável, mas não é obrigatório.</li>
                        <li>Palavrões podem ser usados, mas recomendamos apenas do meio para o final do vídeo, de forma a preservar a monetização.</li>
                    </ul>
                    <br />
                    <b>Alinhamento com as ideias do canal</b>
                    <p>
                        Não basta estar escrito corretamente, e com estilo, tem que ser algo adequado. O canal é libertário anarcocapitalista. Somos abertos a quaisquer assuntos correlatos, mas assuntos totalmente fora de propósito devem ser descartados. Liberdade de expressão deve ser absoluta, mas esse é um canal privado e nos reservamos o direito de não publicar coisas com as quais não concordamos.
                    </p>
                    <ul>
                        <li>Racismo, antisemitismo, antiislamismo, teorias da conspiração, terraplanismo, antivax, é seu direito de liberdade de expressão, mas não vamos publicar aqui.</li>
                        <li>Ofender políticos e figuras públicas, ok. Merecem. Ofender pessoas desconhecidas, pessoas do twitter ou mídias sociais, funcionários públicos subalternos, não ok. Risco de processo.</li>
                        <li>Libertarianismo e anarcocapitalismo é o foco, mas assuntos correlatos são bem vindos. Política, redes sociais, bitcoins, aplicativos descentralizados, etc.</li>
                        <li>Propaganda de produto, site ou evento específico vamos conversar antes de aprovar. Sempre tem risco.</li>
                    </ul>
                    <br />
                    <b>Escolher o narrador</b>
                    <p>
                        O revisor pode escolher o narrador que acha mais conveniente para aquele artigo em questão. Se o narrador escolhido não narrar o artigo em 1 dia, o artigo vai para o público, qualquer um pode narrar. Vai caber ao produtor do vídeo escolher a melhor narração nesse caso. Escolha na caixa de opções acima do botão de concluir a revisão, que fica no final do artigo.
                    </p>
                    <br />
                    <b>Descartar o artigo</b>
                    <p>
                        Preferencialmente, o revisor deve consertar o artigo. Retirar trechos fora do padrão do canal e adaptá-lo ao desejado. Mas, algumas vezes, o texto pode estar totalmente inaproveitável. Nesse caso, o revisor pode descartá-lo. O valor da revisão ainda vai ser creditado, mas, lembre-se, isso deve ser feito em último caso.
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
)(StyleManual as any) as typeof StyleManual;
