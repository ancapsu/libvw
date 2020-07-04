import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import AvatarEditor from 'react-avatar-editor';
import ReactDropzone, { DropzoneRenderArgs } from 'react-dropzone';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import { SearchVideoModel } from '../models/Video';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'
import { SearchArticleModel } from 'ClientApp/models/Article';
import ShowArticleRef from './ShowArticleRef';
import WaitPanel from './common/WaitPanel';

type SearchArticleBaseProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & Toastr.ToastrState
    & { search?: string } 
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface SearchArticleFormProps {
    saveData: (data: SearchArticleModel) => void;
}

type InjectedSearchArticleFormProps =
    SearchArticleBaseProps &
    SearchArticleFormProps &
    InjectedFormProps<SearchVideoModel>
    & { id: string };

class SearchArticleForm extends React.Component<InjectedSearchArticleFormProps, {}> {

    contentReady: boolean;
    searchString: string;

    constructor(props: InjectedSearchArticleFormProps, context: any) {

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

            var data: SearchArticleModel = { SearchString: this.props.search || "", Lang: this.props.lang };
            this.props.requestArticleSearch(0, 10, data);

        }
        else
        {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: InjectedSearchArticleFormProps) {

        if ((props.hasSearchList && props.articles.length != this.props.articles.length) || (!this.props.hasSearchList && props.hasSearchList))
        {

            this.contentReady = true;

        }

    }

    LoadMore() {

        if (this.props.hasSearchList) {

            this.contentReady = false;
            this.forceUpdate();

            var data: SearchArticleModel = { SearchString: this.props.search || "", Lang: this.props.lang };
            this.props.requestArticleSearch(this.props.articles.length, 10, data);

        }

    }

    handleSubmitForm = (values: Partial<SearchVideoModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {

            if (this.searchString != "" && this.searchString.length > 3) {

                this.contentReady = false;
                this.forceUpdate();

                var data: SearchArticleModel = { SearchString: this.searchString, Lang: this.props.lang };
                this.props.requestArticleSearch(0, 10, data);
                
            } else {

                Toastr.toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.SearchStringIsInvalidOrTooShor));

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

                var data: SearchArticleModel = { SearchString: this.searchString, Lang: this.props.lang  };
                this.props.requestArticleSearch(0, 10, data);

            } else {

                Toastr.toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.SearchStringIsInvalidOrTooShor));

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
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.SearchArticle)}</span>
                                    </h4>
                                </div>
                            </div>

                            <div className="row">

                                <div className="td_block_inner td-column-2 col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                    {RacMsg.Get(RacMsg.Id.SearchArticleByTheTitleOrText)}
                                </div>

                            </div>

                            <div className="container open-line"></div>

                            <div className="container open-line"></div>

                            <div className="td_block_wrap">

                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.SearchString)}</span>
                                    <Field name="Search" component='input' type="text" className="form-control" id="Link" onKeyPress={this.keyPress} onChange={this.searchStringChanged} placeholder={RacMsg.Get(RacMsg.Id.SearchString)} />
                                </div>

                            </div>
                            <div className="actions">
                                <a className={this.contentReady ? "btn-login" : "btn-login-disabled"} onClick={this.search}>{RacMsg.Get(RacMsg.Id.Search)}</a><br />
                            </div>

                            <div className="td_block_inner td-column-2">

                                {

                                    this.props.articles.map((n, idx) => {

                                        return (

                                            <ShowArticleRef index={idx} />

                                        );

                                    })

                                }

                            </div>

                            <div className="container open-line"></div>
                            <div className="container open-line"></div>

                            <div className="td_block_inner td-column-2">

                                {this.props.articles.length < this.props.total &&

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

const DecoratedSearchArticleForm = reduxForm<SearchArticleModel>({ form: "searchArticleForm" })(SearchArticleForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.article, ...state.toastr } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators, ...routerActions }
)(DecoratedSearchArticleForm) as any;


