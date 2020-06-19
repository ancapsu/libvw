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
import * as AccountModel from '../models/Account';
import * as NewsModel from '../models/News';
import { ChangeTargetModel } from '../models/Target';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type EditTargetBaseProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & Toastr.ToastrState
    & { id: string }
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface EditTargetFormProps {
    saveData: (data: ChangeTargetModel) => void;
}

type InjectedEditTargetFormProps =
    EditTargetBaseProps &
    EditTargetFormProps &
    InjectedFormProps<ChangeTargetModel>;

class EditTargetForm extends React.Component<InjectedEditTargetFormProps, {}> {

    image: File | string;
    avatareditor: AvatarEditor | null;
    dropzone: ReactDropzone | null;
    
    contentReady: boolean;

    categ: boolean[];
    
    constructor(props: InjectedEditTargetFormProps, context: any) {

        super(props, context);

        this.avatareditor = null;
        this.dropzone = null;
        this.image = "";
        this.contentReady = false;

        this.categ = [];

        this.handleDrop = this.handleDrop.bind(this);
        this.onLoadFailure = this.onLoadFailure.bind(this);
        this.onLoadSuccess = this.onLoadSuccess.bind(this);
        this.handleSubmitForm = this.handleSubmitForm.bind(this);
        this.selCateg = this.selCateg.bind(this);

    }

    handleDrop = (files: File[]) => {

        if (files.length > 0) {

            this.image = files[0];
            this.forceUpdate();

        }

    }

    componentDidMount() {

        if (!this.props.validToken || this.props.user == null) {

            // Não está autenticado

            this.props.push('/login');

        }
        else
        {

            this.props.requestEditBase(this.props.lang);
            this.props.requestTarget(this.props.id, this.props.lang);

        }

    }

    componentWillReceiveProps(props: InjectedEditTargetFormProps) {

        // Inicializa

        if (!this.props.hasEditBase && props.hasEditBase) {

            // Tenho que inicializar alguma coisa aqui? acho que não

        }

        if (!this.props.hasTarget && props.hasTarget && props.target != null) {

            this.props.initialize({ Title: props.target.Title, Text: props.target.Text, Link: props.target.Link })
            this.image = "/api/Target/Image?id=" + props.target.Id;

        }

        if ((!this.props.hasEditBase || !this.props.hasTarget) && props.hasEditBase && props.hasTarget) {

            if (props.target != null) {

                for (var i = 0; i < props.categories.length; i++) {

                    var tem: boolean = false;

                    for (var j = 0; j < props.target.Categories.Categories.length; j++) {

                        if (props.categories[i].Label == props.target.Categories.Categories[j].Label)
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

    onLoadSuccess(imgInfo: any) {

    }

    onLoadFailure(event: any) {

    }

    handleSubmitForm = (values: Partial<ChangeTargetModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady && this.props.target != null) {

            this.contentReady = false;
            this.forceUpdate();

            var str: string = "";

            for (var i = 0; i < this.props.categories.length; i++) {

                if (this.categ[i])
                    str += "1";
                else
                    str += "0";

            }
            
            var formData: ChangeTargetModel = { ...values, Categ: str, Id: this.props.target.Id };

            if (this.avatareditor != null) {

                var res: string = this.avatareditor.getImageScaledToCanvas().toDataURL('image/jpeg', 3);
                formData = { ...formData, Image: res };

            }

            dispatch(this.props.changeTarget(formData));

        }

    }

    setEditorRef = (editor: any) => { if (editor) { this.avatareditor = editor; } }
    setDropzoneRef = (drop: any) => { if (drop) { this.dropzone = drop; } }
    
    selCateg(i: number) {

        this.categ[i] = !this.categ[i];
        this.forceUpdate();

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
                                        <span className="news-page-headline">Alterar pauta</span>
                                    </h4>
                                </div>
                            </div>

                            <div className="row">

                                <div className="col-lg-6 col-md-6 col-sm-12 col-xs-12">

                                    <div className="form-group">
                                        <span>Link para a notícia</span>
                                        <Field name="Link" component='input' type="text" className="form-control" id="Link" placeholder="Cole o link da notícia" />
                                    </div>

                                    <div className="form-group">
                                        <span>Título da pauta</span>
                                        <Field name="Title" component='input' type="text" className="form-control" id="Title" placeholder="T&iacute;tulo da not&iacute;cia" />
                                    </div>

                                    <div className="form-group">
                                        <span>Categoria</span>
                                        <div className="tda-module-meta-info">
                                            <ul className="tda-category">
                                                {this.props.target != null && this.props.categories.map((m, i) => {
                                                    return (
                                                        <li className={(this.categ[i] ? "tda-category-selected" : "tda-category-not-selected")}>
                                                            <a onClick={() => { this.selCateg(i) }}>{m.Category}</a>
                                                        </li>)
                                                })
                                                }
                                            </ul>
                                        </div>
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
                                                            Arraste uma imagem ou clique para carregar
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
                                    <span>Texto da pauta</span>
                                    <Field name="Text" component='textarea' type="multiline" className="form-control" id="Text" style={{ height: 80 }} placeholder="Texto da not&iacute;cia" />
                                </div>

                                <div className="actions">
                                    <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>Alterar</button><br />
                                </div>

                            </div>
                        </div>

                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedEditTargetForm = reduxForm<ChangeTargetModel>({ form: "editTargetForm" })(EditTargetForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.target, ...state.toastr } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators, ...routerActions }
)(DecoratedEditTargetForm) as any;


