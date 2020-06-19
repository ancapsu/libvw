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

type HelpValuesProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{}>;

class HelpValues extends React.Component<HelpValuesProps, {}> {

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
                                        <span className="news-page-headline">Sistema de prêmios</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="tdc-video-row">
                    <p>
                        A prêmio para cada tipo de ação no site segue conforme a seguir. Note que apenas vídeos que são efetivamente publicados no canal recebem premiação. Portanto, tenha em mente que, sempre que colaborar com o canal, existe a possibilidade de não ser remunerado por isso.
                    </p>
                    <p>
                        A premiação é feita sempre no último dia do mês. Os satoshis são enviados para o endereço bitcoin cadastrado no seu perfil de usuário. Apenas fazemos pagamentos em bitcoins, devido a ser a moeda mais facilmente aceita. Se seu endereço for inválido ou não estiver preenchido, você não receberá pelo método automático. Quando preencher a carteira, nos avise via email que programaremos seu pagamento.
                    </p>
                    <br />
                    <b>Sugerir uma pauta</b>
                    <p>
                        Qualquer um pode sugerir uma pauta. Para isso você precisa estra registrado como usuário, ou seja, precisa criar uma conta, confirmar seu email e se autenticar. Se sua pauta acabar em um vídeo, você terá seu nome citado como pauta sugerida por... mas não irá receber nenhum prêmio em satoshis.
                    </p>
                    <br />
                    <b>Escrever um artigo ou crônica</b>
                    <p>
                        Qualquer um pode escrever um artigo, crônia, tapa ou nota libertária. Se seu artigo for usado em um vídeo, você terá seu nome citado como autor e receberá 80 satoshis por palavra do seu texto. Note que o texto final pode ser maior que o inicial, porém você só fará juz as palavras originais do seu texto. Também nos reservamos o direito de cortar parte do seu artigo que julgarmos repetitivas ou sem propósito e seu prêmio será proporcional apenas a parte aproveitada do seu texto.
                        Note que escrever um artigo com cerca de 1000 palavras é o que tem mais chance de ser produzido. Tapas, notas e crônicas temos em muita quantidade e acabamos descartando a maioria simplesmente por falta de tempo.
                    </p>
                    <br />
                    <b>Revisar um artigo ou crônica</b>
                    <p>
                        Apenas revisores selecionados podem revisar o texto. Se quiser revisar, me avise. Revisar o texto consiste em checar se o texto está adequado, se usa as expressões do canal, enfim, se está de acordo o padrão e ideologia do canal. Após revisar, você pode escolher quem vai narrar preferencialmente o texto, dentre os narradores cadastrados. Se a pessoa não narrar em 1 dia, o vídeo abre para outros narradores. Você recebera 50 satoshis por cada palavra do artigo original revisado, mais 80 satoshis por palavra nova que incluir na revisão. A maior parte dos artigos é curta demais, então sempre vale a pena complementar. Artigos com cerca de 1000 palavras é o que desejamos sempre.
                    </p>
                    <br />
                    <b>Narrar um artigo ou crônica</b>
                    <p>
                        Qualquer um pode narrar um vídeo. Mas existem narradores preferenciais. O primeiro dia, o vídeo é designado para um narrador preferencial específico. Se ele não narrar nesse dia, abre para todos. No segundo dia, se um dos outros narradores preferenciais narrar, o vídeo sai da lista de vídeos para narrar. Note que mais de uma pessoa pode ter narrado. Vai caber ao produtor escolher a narração a ser usada. Após o segundo dia, qualquer pessoa que narrar vai tirar o vídeo da lista de vídeos para narração, indo para produção. O narrador recebe 50 satoshis por palavra do artigo narrado.
                    </p>
                    <br />
                    <b>Produzir o vídeo</b>
                    <p>
                        Apenas produtores selecionados podem produzir o vídeo. Se quiser produzir, me avise. Produzir o vídeo significa capturar os vídeos, juntar ao audio e criar o vídeo em si. Após a produção vai para a publicação que é simplesmente a colocação no canal. O produtor recebe 80 satoshis por palavra do vídeo produzido.
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
)(HelpValues as any) as typeof HelpValues;
