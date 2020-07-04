import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import AvatarEditor from 'react-avatar-editor';
import ReactDropzone, { DropzoneRenderArgs } from 'react-dropzone';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as VideoStore from '../store/Video';
import { SearchVideoModel } from '../models/Video';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'
import ShowVideo from './ShowVideo';
import WaitPanel from './common/WaitPanel';


type SearchVideoBaseProps =
    AccountStore.AccountState
    & VideoStore.VideoState
    & Toastr.ToastrState
    & { search?: string } 
    & typeof AccountStore.actionCreators
    & typeof VideoStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface SearchVideoFormProps {
    saveData: (data: SearchVideoModel) => void;
}

type InjectedSearchVideoFormProps =
    SearchVideoBaseProps &
    SearchVideoFormProps &
    InjectedFormProps<SearchVideoModel>
    & { id: string };

class SearchVideoForm extends React.Component<InjectedSearchVideoFormProps, {}> {

    contentReady: boolean;
    searchString: string;

    constructor(props: InjectedSearchVideoFormProps, context: any) {

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

            var data: SearchVideoModel = { SearchString: this.props.search || "", Lang: this.props.lang };
            this.props.requestVideoSearch(0, 10, data);

        }
        else
        {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: InjectedSearchVideoFormProps) {

        if ((props.hasSearchList && props.videos.length != this.props.videos.length) || (!this.props.hasSearchList && props.hasSearchList))
        {

            this.contentReady = true;

        }

    }

    LoadMore() {

        if (this.props.hasSearchList) {

            this.contentReady = false;
            this.forceUpdate();

            var data: SearchVideoModel = { SearchString: this.props.search || "", Lang: this.props.lang };
            this.props.requestVideoSearch(this.props.videos.length, 10, data);

        }

    }

    handleSubmitForm = (values: Partial<SearchVideoModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {

            if (this.searchString != "" && this.searchString.length > 3) {

                this.contentReady = false;
                this.forceUpdate();

                var data: SearchVideoModel = { SearchString: this.searchString };
                this.props.requestVideoSearch(0, 10, data);
                
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

                var data: SearchVideoModel = { SearchString: this.searchString };
                this.props.requestVideoSearch(0, 10, data);

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

                        <WaitPanel isContentReady={this.contentReady} />

                        <div className="wpb_wrapper">
                            <div className="td-pb-border-top">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">buscar vídeos</span>
                                    </h4>
                                </div>
                            </div>

                            <div className="row">

                                <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                    Buscar vídeos que contenham o texto abaixo
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

                                    this.props.videos.map((n, idx) => {

                                        return (

                                            <ShowVideo video={n} />

                                        );

                                    })

                                }

                            </div>

                            <div className="container open-line"></div>
                            <div className="container open-line"></div>

                            <div className="td_block_inner td-column-2">

                                {this.props.videos.length < this.props.total &&

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

const DecoratedSearchVideoForm = reduxForm<SearchVideoModel>({ form: "searchVideoForm" })(SearchVideoForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.video, ...state.toastr } },
    { ...AccountStore.actionCreators, ...VideoStore.actionCreators, ...routerActions }
)(DecoratedSearchVideoForm) as any;


