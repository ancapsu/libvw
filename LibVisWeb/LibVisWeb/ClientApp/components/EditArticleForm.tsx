import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import AvatarEditor from 'react-avatar-editor';
import ReactDropzone, { DropzoneRenderArgs } from 'react-dropzone';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as AccountModel from '../models/Account';
import * as ArticleModel from '../models/Article';
import * as ArticleStore from '../store/Article';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type EditArticleBaseProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & Toastr.ToastrState    
    & { id: string }
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface EditArticleFormProps {
    saveData: (data: ArticleModel.ChangeArticleModel) => void;
}

type InjectedEditArticleFormProps =
    EditArticleBaseProps &
    EditArticleFormProps &
    InjectedFormProps<ArticleModel.ChangeArticleModel>;

class EditArticleForm extends React.Component<InjectedEditArticleFormProps, {}> {

    image: File | string;
    avatareditor: AvatarEditor | null;
    dropzone: ReactDropzone | null;

    text: string;
    wordCount: number;
    textType: string;

    type: number;
    sizeerror: boolean;

    contentReady: boolean;

    categ: boolean[];

    constructor(props: InjectedEditArticleFormProps, context: any) {

        super(props, context);

        this.avatareditor = null;
        this.dropzone = null;
        this.image = "";

        this.categ = [];
        this.text = "";
        this.wordCount = 0;
        this.textType = RacMsg.Get(RacMsg.Id.TooShort);

        this.contentReady = false;

        this.type = 3;
        this.sizeerror = false;

        this.handleDrop = this.handleDrop.bind(this);
        this.onLoadFailure = this.onLoadFailure.bind(this);
        this.onLoadSuccess = this.onLoadSuccess.bind(this);
        this.handleSubmitForm = this.handleSubmitForm.bind(this);
        this.textChanged = this.textChanged.bind(this);
        this.CountWords = this.CountWords.bind(this);
        this.selType = this.selType.bind(this);
        this.selCateg = this.selCateg.bind(this);

    }

    componentDidMount() {

        if (!this.props.validToken || this.props.user == null) {

            // Não está autenticado

            this.props.push('/login');

        }
        else {

            // Carrega as informações do target anterior

            this.props.requestEditBase(this.props.lang, "", this.props.id);
            this.props.requestArticle(this.props.id, this.props.lang);


        }

    }

    componentWillReceiveProps(props: InjectedEditArticleFormProps) {

        // Inicializa

        if (!this.props.hasEditBase && props.hasEditBase) {

            // Tenho que inicializar alguma coisa aqui? acho que não

        }

        if (!this.props.hasArticle && props.hasArticle && props.article != null) {

            this.props.initialize({ Title: props.article.Title, Text: props.article.Text, TargetId: props.article.Target.Id })
            this.image = "/api/Article/Image?id=" + props.article.Id;
            this.type = props.article.Type;

            this.text = props.article.Text || "";
            this.CountWords(this.text);

        }
        
        if ((!this.props.hasEditBase || !this.props.hasArticle) && props.hasEditBase && props.hasArticle) {

            if (props.article != null) {

                for (var i = 0; i < props.categories.length; i++) {

                    var tem: boolean = false;

                    for (var j = 0; j < props.article.Categories.Categories.length; j++) {

                        if (props.categories[i].Label == props.article.Categories.Categories[j].Label)
                            tem = true;

                    }

                    this.categ[i] = tem;

                }

            }

            this.contentReady = true;

        }

        if (props.toastrs.length > 0) {

            this.contentReady = true;

        }

    }

    handleDrop = (files: File[]) => {

        if (files.length > 0) {

            this.image = files[0];
            this.forceUpdate();

        }

    }

    onLoadSuccess(imgInfo: any) {

    }

    onLoadFailure(event: any) {

    }

    handleSubmitForm = (values: Partial<ArticleModel.ChangeArticleModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.sizeerror) {

            Toastr.toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.TheTextIsNotTheExpectedSize));

        } else {

            if (this.contentReady) {

                this.contentReady = false;
                this.forceUpdate();

                var str: string = "";

                for (var i = 0; i < this.props.categories.length; i++) {

                    if (this.categ[i])
                        str += "1";
                    else
                        str += "0";

                }

                var formData: ArticleModel.ChangeArticleModel = { ...values, Categ: str, Type: this.type, Id: this.props.id, Lang: this.props.lang };

                if (this.avatareditor != null) {

                    var res: string = this.avatareditor.getImageScaledToCanvas().toDataURL('image/jpeg', 3);
                    formData = { ...formData, Image: res, Categ: str };

                }

                dispatch(this.props.changeArticle(formData));

            }

        }

    }

    setEditorRef = (editor: any) => { if (editor) { this.avatareditor = editor; } }
    setDropzoneRef = (drop: any) => { if (drop) { this.dropzone = drop; } }
    
    selCateg(i: number) {

        this.categ[i] = !this.categ[i];
        this.forceUpdate();

    }

    selType(i: number) {

        this.type = i;
        this.CountWords(this.text);
        this.forceUpdate();

    }

    textChanged(e: any) {

        this.text = e.target.value;
        this.CountWords(this.text);

    }

    CountWords(text: string): number {

        var c: number = text.split(/[ \r\n\t]+/).length;

        this.wordCount = c;

        if (this.type == 3) {

            if (c < 600) {
                this.textType = RacMsg.Get(RacMsg.Id.TooShortForAnArticle);
                this.sizeerror = true;
            }
            else if (c < 3000) {
                this.textType = RacMsg.Get(RacMsg.Id.GreatForAnArticle);
                this.sizeerror = false;
            }
            else {
                this.textType = RacMsg.Get(RacMsg.Id.TooLongForAnArticle);
                this.sizeerror = false;
            }

        } else if (this.type == 4) {

            if (c < 800) {
                this.textType = RacMsg.Get(RacMsg.Id.TooShortForAChronicle);
                this.sizeerror = true;
            }
            else if (c < 4000) {
                this.textType = RacMsg.Get(RacMsg.Id.GreatForAChronicle);
                this.sizeerror = false;
            }
            else {
                this.textType = RacMsg.Get(RacMsg.Id.TooLongForAChronicle);
                this.sizeerror = false;
            }

        }

        this.forceUpdate();

        return c;

    }

    public render() {

        return (

            <div>

                <form role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>

                    <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">

                        <div className={(!this.contentReady ? "wait-panel" : "wait-panel-disabled")}>
                            <img src={waitGif} ></img>
                        </div>

                        <div className="wpb_wrapper">
                            <div className="td-pb-border-top">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.EditArticle)}</span>
                                    </h4>
                                </div>
                            </div>

                            <div className="row">

                                <div className="col-lg-6 col-md-6 col-sm-12 col-xs-12">

                                    <div className="form-group">
                                        <span>{RacMsg.Get(RacMsg.Id.BaseAgenda)}</span>
                                        <Field name="TargetId" component='select' type="text" className="form-control" id="TargetId" placeholder="Pauta base">
                                            <option value="0">{RacMsg.Get(RacMsg.Id.NoAgenda)}</option>
                                            {this.props.targets.map(o => {

                                                return (

                                                    <option value={o.Id}>{o.Title}</option>

                                                );

                                            })
                                            }
                                        </Field>
                                    </div>

                                    <div className="form-group">
                                        <span>{RacMsg.Get(RacMsg.Id.Title)}</span>
                                        <Field name="Title" component='input' type="text" className="form-control" id="Title" placeholder={RacMsg.Get(RacMsg.Id.Title)} />
                                    </div>

                                    <div className="form-group">
                                        <span>{RacMsg.Get(RacMsg.Id.Type)}</span>
                                        <div className="tda-module-meta-info">
                                            <ul className="tda-category">                                                
                                                <li className={(this.type == 3 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                    <a onClick={() => { this.selType(3) }}>{RacMsg.Get(RacMsg.Id.Article)}</a>
                                                </li>
                                                <li className={(this.type == 4 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                    <a onClick={() => { this.selType(4) }}>{RacMsg.Get(RacMsg.Id.Chronicle)}</a>
                                                </li>                                                                                            
                                            </ul>
                                        </div>
                                    </div>

                                    <div className="form-group">
                                        <span>{RacMsg.Get(RacMsg.Id.Categories)}</span>
                                        <div className="tda-module-meta-info">
                                            <ul className="tda-category">
                                                {this.props.categories.map((m, i) => {
                                                    return (
                                                        <li className={(this.categ[i] ? "tda-category-selected" : "tda-category-not-selected")}>
                                                            <a onClick={() => { this.selCateg(i) }}>{m.Category}</a>
                                                        </li>)
                                                })
                                                }
                                            </ul>
                                        </div>
                                    </div>

                                    <div className="form-group">
                                        <span>{RacMsg.Get(RacMsg.Id.TextSize)}</span>&nbsp;
                                        <p className={(this.sizeerror ? "size-error" : "no-size-error")}>{this.textType}</p>
                                    </div>

                                </div>

                                <div className="col-lg-6 col-md-6 col-sm-12 col-xs-12">

                                    <ReactDropzone onDrop={this.handleDrop} ref={this.setDropzoneRef} preventDropOnDocument={true} accept="image/jpeg, image/png, image/gif" disableClick={true} >
                                        {(x: DropzoneRenderArgs) => {

                                            return (
                                                <section className="target-image-article-container">

                                                    <div {...x.getRootProps({ className: 'dropzone-article' })}>
                                                        <input {...x.getInputProps()} />
                                                        <div className="target-article-image-overlay" onClick={() => { if (this.dropzone != null) this.dropzone.open() }}>
                                                            {RacMsg.Get(RacMsg.Id.DragAnImageOrClickToSelectFile)}
                                                        </div>
                                                        <AvatarEditor
                                                            ref={this.setEditorRef}
                                                            image={this.image}
                                                            style={{ width: '420px', height: '236px' }}
                                                            width={1280}
                                                            height={720}
                                                            onLoadSuccess={this.onLoadSuccess}
                                                            onLoadFailure={this.onLoadFailure}
                                                            className="target-editor"
                                                        />
                                                    </div>

                                                </section>

                                            );
                                        }}
                                    </ReactDropzone>

                                </div>

                            </div>

                            <div className="td_block_wrap">

                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.ArticleText)} ({this.wordCount} {RacMsg.Get(RacMsg.Id.words)})</span>
                                    <Field name="Text" component='textarea' type="multiline" onChange={this.textChanged} className="form-control" id="Text" style={{ height: 280 }} placeholder={RacMsg.Get(RacMsg.Id.ArticleText)} />
                                </div>

                                <div className="actions">
                                    <button disabled={!this.contentReady || this.sizeerror} type="submit" className={(this.contentReady && !this.sizeerror) ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Register)}</button><br />
                                </div>

                            </div>
                        </div>

                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedNewArticleForm = reduxForm<ArticleModel.NewArticleModel>({ form: "editArticleForm" })(EditArticleForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.article, ...state.toastr } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators, ...routerActions }
)(DecoratedNewArticleForm) as any;


