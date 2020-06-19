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
import { NewVideoModel } from '../models/Video';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type NewVideoBaseProps =
    AccountStore.AccountState
    & VideoStore.VideoState
    & Toastr.ToastrState    
    & typeof AccountStore.actionCreators
    & typeof VideoStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface NewVideoFormProps {
    saveData: (data: NewVideoModel) => void;
}

type InjectedNewVideoFormProps =
    NewVideoBaseProps &
    NewVideoFormProps &
    InjectedFormProps<NewVideoModel>
    & { id: string };

class NewVideoForm extends React.Component<InjectedNewVideoFormProps, {}> {

    image: File | string;
    avatareditor: AvatarEditor | null;
    dropzone: ReactDropzone | null;
    linkInput: Field | null;
    
    contentReady: boolean;

    url: string;
    
    linkOk: boolean;

    categ: boolean[];

    constructor(props: InjectedNewVideoFormProps, context: any) {

        super(props, context);

        this.avatareditor = null;
        this.dropzone = null;
        this.linkInput = null;
        this.image = "";

        this.contentReady = true;

        this.linkOk = false;
        this.url = "";

        this.categ = [];

        this.handleDrop = this.handleDrop.bind(this);
        this.onLoadFailure = this.onLoadFailure.bind(this);
        this.onLoadSuccess = this.onLoadSuccess.bind(this);
        this.handleSubmitForm = this.handleSubmitForm.bind(this);
        this.linkChanged = this.linkChanged.bind(this);
        this.researchLink = this.researchLink.bind(this);
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
        
    }

    componentWillReceiveProps(props: InjectedNewVideoFormProps) {

        // Volta ao zero

        if (this.props.hasYoutubeInfo && !props.hasYoutubeInfo) {

            this.linkOk = false;
            this.contentReady = true;

        }

        // Recebe o link

        if (!this.props.hasYoutubeInfo && props.hasYoutubeInfo) {

            if (props.youtube != null) {

                this.props.initialize({ Title: props.youtube.Title, Tags: props.youtube.Tags, Description: props.youtube.Description, YoutubeLink: props.youtube.YoutubeLink, BitchuteLink: '' })
                this.image = "data:image/jpeg;base64," + props.youtube.Image;

                this.linkOk = true;

            }
            
            this.contentReady = true;
            
        }

        // Erro

        if (props.toastrs.length > 0) {

            this.contentReady = true;

        }

    }

    onLoadSuccess(imgInfo: any) {

    }

    onLoadFailure(event: any) {

    }

    handleSubmitForm = (values: Partial<NewVideoModel>, dispatch: Dispatch<any>, props: {}) => {
        
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

            var formData: NewVideoModel = { ...values, Categ: str, Lang: this.props.lang };

            if (this.avatareditor != null) {

                var res: string = this.avatareditor.getImageScaledToCanvas().toDataURL('image/jpeg', 3);
                formData = { ...formData, Image: res };

            }

            dispatch(this.props.registerNewVideo(formData));

        }
        
    }

    setEditorRef = (editor: any) => { if (editor) { this.avatareditor = editor; } }
    setDropzoneRef = (drop: any) => { if (drop) { this.dropzone = drop; } }
    setLinkInput = (link: any) => { if (link) { this.linkInput = link; } }

    linkChanged(e: any) {

        this.url = e.target.value;

    }

    researchLink() {

        if (this.contentReady) {

            if (this.url != "") {

                this.contentReady = false;
                this.forceUpdate();

                this.props.requestEditBase(this.props.lang);
                this.props.requestYoutubeInfo(this.url, this.props.lang);
                
            } else {

                Toastr.toastr.error("Erro", "Link para pesquisa invalido");

            }

        }

    }
    
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
                                        <span className="news-page-headline">Novo Vídeo</span>
                                    </h4>
                                </div>
                            </div>
                            
                            <div className={(!this.linkOk ? "td_block_wrap" : "hidden")}>

                                <div className="td_block_wrap">

                                    <div className="form-group">
                                        <span>Link do vídeo no youtube</span>
                                        <Field name="YoutubeLink" ref={this.setLinkInput} component='input' type="text" className="form-control" id="YoutubeLink" onChange={this.linkChanged} placeholder="Cole o link do vídeo no youtube" />
                                    </div>

                                </div>

                                <div className="actions">
                                    <a className={this.contentReady ? "btn-login" : "btn-login-disabled"} onClick={this.researchLink}>Pesquisar</a><br />
                                </div>
                                
                            </div>

                            <div className={(this.linkOk ? "td_block_wrap" : "hidden")}>
                                
                                <div className="col-lg-6 col-md-6 col-sm-12 col-xs-12">

                                    <div className="form-group">
                                        <span>Título do vídeo</span>
                                        <Field name="Title" component='input' type="text" className="form-control" id="Title" placeholder="Título do vídeo" />
                                    </div>

                                    <div className="form-group">
                                        <span>Link do youtube</span>
                                        <Field name="YoutubeLink" component='input' type="text" className="form-control" id="YoutubeLink" placeholder="Link para o Youtube" />
                                    </div>

                                    <div className="form-group">
                                        <span>Link do bitchute</span>
                                        <Field name="BitchuteLink" component='input' type="text" className="form-control" id="BitchuteLink" placeholder="Link para o Bitchute" />
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

                                <div className="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                    <div className="form-group">
                                        <span>Categoria</span>
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
                                        <span>Descrição do vídeo</span>
                                        <Field name="Description" component='textarea' type="multiline" className="form-control" id="Description" style={{ height: 80 }} placeholder="Description do vídeo" />
                                    </div>

                                    <div className="form-group">
                                        <span>Tags do vídeo</span>
                                        <Field name="Tags" component='input' type="text" className="form-control" id="Tags" style={{ height: 40 }}  placeholder="Tags do vídeo" />
                                    </div>

                                    <div className="form-group">
                                        <span>Script do vídeo</span>
                                        <Field name="Script" component='textarea' type="multiline" className="form-control" id="Script" style={{ height: 180 }} placeholder="Script do vídeo" />
                                    </div>

                                    <div className="actions">
                                        <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>Registrar</button><br />
                                    </div>

                                </div>

                            </div>

                        </div>

                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedNewVideoForm = reduxForm<NewVideoModel>({ form: "newVideoForm" })(NewVideoForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.video, ...state.toastr } },
    { ...AccountStore.actionCreators, ...VideoStore.actionCreators, ...routerActions }
)(DecoratedNewVideoForm);


