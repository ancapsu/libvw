import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import AvatarEditor from 'react-avatar-editor';
import ReactDropzone, { DropzoneRenderArgs } from 'react-dropzone';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as TargetStore from '../store/Target';
import { NewTargetModel, SearchTargetModel } from '../models/Target';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'
import ShowTargetRef from './ShowTargetRef';

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type SearchTargetBaseProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & Toastr.ToastrState
    & { search?: string } 
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface SearchTargetFormProps {
    saveData: (data: SearchTargetModel) => void;
}

type InjectedSearchTargetFormProps =
    SearchTargetBaseProps &
    SearchTargetFormProps &
    InjectedFormProps<SearchTargetModel>
    & { id: string };

class SearchTargetForm extends React.Component<InjectedSearchTargetFormProps, {}> {

    contentReady: boolean;
    searchString: string;

    constructor(props: InjectedSearchTargetFormProps, context: any) {

        super(props, context);

        this.contentReady = true;
        this.searchString = "";

        this.handleSubmitForm = this.handleSubmitForm.bind(this);
        this.searchStringChanged = this.searchStringChanged.bind(this);
        this.search = this.search.bind(this);
        this.LoadMore = this.LoadMore.bind(this);
        this.keyPress = this.keyPress.bind(this);

        this.contentReady = false;

    }

    componentDidMount() {

        // Carrega as informações do target anterior

        if (this.props.search != "" && this.props.search != undefined && (!this.props.hasSearchList || (this.props.searchParameters != undefined && this.props.searchParameters.SearchString != this.props.search)))
        {

            var data: SearchTargetModel = { SearchString: this.props.search || "", Lang: this.props.lang};
            this.props.requestTargetSearch(0, 10, data);

        }
        else
        {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: InjectedSearchTargetFormProps) {

        if ((props.hasSearchList && props.targets.length != this.props.targets.length) || (!this.props.hasSearchList && props.hasSearchList))
        {

            this.contentReady = true;

        }

    }

    LoadMore() {

        if (this.props.hasSearchList) {

            this.contentReady = false;
            this.forceUpdate();

            var data: SearchTargetModel = { SearchString: this.props.search || "", Lang: this.props.lang  };
            this.props.requestTargetSearch(this.props.targets.length, 10, data);

        }

    }

    handleSubmitForm = (values: Partial<SearchTargetModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {

            if (this.searchString != "" && this.searchString.length > 3) {

                this.contentReady = false;
                this.forceUpdate();

                var data: SearchTargetModel = { SearchString: this.searchString };
                this.props.requestTargetSearch(0, 10, data);
                
            } else {

                Toastr.toastr.error("Erro", "String para pesquisa invalido ou muito pequeno. Use pelo menos 3 letras.");

            }
            
        }

    }

    searchStringChanged(e: any) {

        this.searchString = e.target.value;

    }

    keyPress(e: any) {

        if (e.charCode == 13) {
            e.preventDefault();
            this.search();
        }

    }

    search() {

        if (this.contentReady) {

            if (this.searchString != "" && this.searchString.length > 3) {

                this.contentReady = false;
                this.forceUpdate();

                var data: SearchTargetModel = { SearchString: this.searchString };
                this.props.requestTargetSearch(0, 10, data);

            } else {

                Toastr.toastr.error("Erro", "String para pesquisa invalido ou muito pequeno. Use pelo menos 3 letras.");

            }

        }

    }

    public render() {

        

        return (

            <div>

                <form role="form">

                    <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">

                        <div className={(!this.contentReady ? "wait-panel" : "wait-panel-disabled")}>
                            <img src={waitGif} ></img>
                        </div>

                        <div className="wpb_wrapper">
                            <div className="td-pb-border-top">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">buscar pautas</span>
                                    </h4>
                                </div>
                            </div>

                            <div className="row">

                                <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                    Buscar pautas que contenham o texto abaixo
                                </div>

                            </div>

                            <div className="container open-line"></div>

                            <div className="container open-line"></div>

                            <div className="td_block_wrap">

                                <div className="form-group">
                                    <span>Texto para busca</span>
                                    <Field name="Search" component='input' type="text" className="form-control" id="Link" onKeyPress={this.keyPress} onChange={this.searchStringChanged} placeholder="Cole o link da notícia" />
                                </div>

                            </div>
                            <div className="actions">
                                <a className={this.contentReady ? "btn-login" : "btn-login-disabled"} onClick={this.search}>Procurar</a><br />
                            </div>

                            <div className="td_block_inner td-column-2">

                                {

                                    this.props.targets.map((n, idx) => {

                                        return (

                                            <ShowTargetRef index={idx} />

                                        );

                                    })

                                }

                            </div>

                            <div className="container open-line"></div>
                            <div className="container open-line"></div>

                            <div className="td_block_inner td-column-2">

                                {this.props.targets.length < this.props.total &&

                                    <a onClick={this.LoadMore} className="btn-carregar-mais">{RacMsg.Get(RacMsg.Id.LoadMore)}</a>

                                }

                            </div>

                            <div className="container open-line"></div>
                            <div className="container open-line"></div>

                        </div>

                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedSearchTargetForm = reduxForm<SearchTargetModel>({ form: "searchTargetForm" })(SearchTargetForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.target, ...state.toastr } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators, ...routerActions }
)(DecoratedSearchTargetForm) as any;


