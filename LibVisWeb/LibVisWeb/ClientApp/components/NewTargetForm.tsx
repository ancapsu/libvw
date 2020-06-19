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
import { NewTargetModel } from '../models/Target';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type NewTargetBaseProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & Toastr.ToastrState    
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface NewTargetFormProps {
    saveData: (data: NewTargetModel) => void;
}

type InjectedNewTargetFormProps =
    NewTargetBaseProps &
    NewTargetFormProps &
    InjectedFormProps<NewTargetModel>
    & { id: string };

class NewTargetForm extends React.Component<InjectedNewTargetFormProps, {}> {

    image: File | string;
    avatareditor: AvatarEditor | null;
    dropzone: ReactDropzone | null;
    linkInput: Field | null;
    
    contentReady: boolean;

    url: string;
    
    linkOk: boolean;
    selectedImage: number;

    categ: boolean[];

    constructor(props: InjectedNewTargetFormProps, context: any) {

        super(props, context);

        this.avatareditor = null;
        this.dropzone = null;
        this.linkInput = null;
        this.image = "";

        this.contentReady = true;

        this.linkOk = false;
        this.selectedImage = 0;
        this.url = "";

        this.categ = [];

        this.handleDrop = this.handleDrop.bind(this);
        this.onLoadFailure = this.onLoadFailure.bind(this);
        this.onLoadSuccess = this.onLoadSuccess.bind(this);
        this.handleSubmitForm = this.handleSubmitForm.bind(this);
        this.linkChanged = this.linkChanged.bind(this);
        this.imageClicked = this.imageClicked.bind(this);
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

    componentWillReceiveProps(props: InjectedNewTargetFormProps) {

        // Volta ao zero

        if (this.props.hasLinkInfo && !props.hasLinkInfo) {

            this.linkOk = false;
            this.contentReady = true;

        }

        // Inicializa

        if (!this.props.hasLinkInfo && props.hasLinkInfo) {

            if (props.link != null) {

                this.props.initialize({ Link: props.link.Link, Title: props.link.Title, Lang: props.lang, Text: props.link.Text })
                this.linkOk = true;

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

    handleSubmitForm = (values: Partial<NewTargetModel>, dispatch: Dispatch<any>, props: {}) => {

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

            var formData: NewTargetModel = { ...values, ImageType: 0, Categ: str, Lang: this.props.lang };

            if (this.selectedImage == 4) {

                if (this.avatareditor != null) {

                    var res: string = this.avatareditor.getImageScaledToCanvas().toDataURL('image/jpeg', 3);
                    formData = { ...formData, Image: res, ImageType: 2 };

                }

            } else {

                if (this.selectedImage == 0 && this.props.link != null && this.props.link.Image.length > 0)
                    formData = { ...formData, Image: this.props.link.Image[0], ImageType: 1 };

                if (this.selectedImage == 1 && this.props.link != null && this.props.link.Image.length > 1)
                    formData = { ...formData, Image: this.props.link.Image[1], ImageType: 1 };

                if (this.selectedImage == 2 && this.props.link != null && this.props.link.Image.length > 2)
                    formData = { ...formData, Image: this.props.link.Image[2], ImageType: 1 };

            }

            dispatch(this.props.registerNewTarget(formData));
            
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
                this.props.requestLinkInfo(this.url, this.props.lang);
                
            } else {

                Toastr.toastr.error("Erro", "Link para pesquisa invalido");

            }

        }

    }

    imageClicked(i: number) {

        this.selectedImage = i;        
        this.forceUpdate();

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
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.NewAgenda)}</span>
                                    </h4>
                                </div>
                            </div>
                            
                            <div className={(!this.linkOk ? "td_block_wrap" : "hidden")}>

                                <div className="td_block_wrap">

                                    <div className="form-group">
                                        <span>{RacMsg.Get(RacMsg.Id.LinkToTheNews)}</span>
                                        <Field name="Link" ref={this.setLinkInput} component='input' type="text" className="form-control" id="Link" onChange={this.linkChanged} placeholder={RacMsg.Get(RacMsg.Id.PasteLinkToTheNews)} />
                                    </div>

                                </div>
                                <div className="actions">
                                    <a className={this.contentReady ? "btn-login" : "btn-login-disabled"} onClick={this.researchLink}>{RacMsg.Get(RacMsg.Id.Research)}</a><br />
                                </div>
                                
                            </div>

                            <div className={(this.linkOk ? "td_block_wrap" : "hidden")}>
                                
                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.LinkToTheNews)}</span>
                                    <Field name="Link" component='input' type="text" className="form-control" id="Link" />
                                </div>

                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.ImageForTheAgenda)}</span>
                                    <br /><br />
                                    <div className="row">

                                        {this.props.link != null && this.props.link.Image.map((img, idx) => {

                                            if (this.selectedImage == idx) {

                                                return (
                                                    <div className="col-lg-3 col-md-3 col-sm-6 col-xs-6 sample-image-clip">
                                                        <input type="radio" radioGroup="image" name="image" checked value={idx} onClick={() => { this.imageClicked(idx) }}></input><br /><br />
                                                        <img className="sample-image" src={img}></img>
                                                    </div>
                                                );

                                            } else {

                                                return (
                                                    <div className="col-lg-3 col-md-3 col-sm-6 col-xs-6 sample-image-clip">
                                                        <input type="radio" radioGroup="image" name="image" value={idx} onClick={() => { this.imageClicked(idx) }}></input><br /><br />
                                                        <img className="sample-image" src={img}></img>
                                                    </div>
                                                );

                                            }

                                        })

                                        }

                                        <div className="col-lg-3 col-md-3 col-sm-6 col-xs-6 sample-image-clip">

                                            {this.selectedImage == 4 &&
                                                (<input type="radio" radioGroup="image" name="image" value="4" checked  onClick={() => { this.imageClicked(4) }}></input> )
                                            }
                                            {this.selectedImage != 4 &&
                                                (<input type="radio" radioGroup="image" name="image" value="4" onClick={() => { this.imageClicked(4) }}></input> )
                                            }

                                            <br /> <br />
                                            <ReactDropzone onDrop={this.handleDrop} ref={this.setDropzoneRef} preventDropOnDocument={true} accept="image/jpeg, image/png, image/gif" disableClick={true} >
                                                {(x: DropzoneRenderArgs) => {

                                                    return (
                                                        <section className="target-image-container">

                                                            <div {...x.getRootProps({ className: 'dropzone' })}>
                                                                <input {...x.getInputProps()} />
                                                                <div className="target-image-overlay" onClick={() => { if (this.dropzone != null) this.dropzone.open() }}>
                                                                    {RacMsg.Get(RacMsg.Id.DragAnImageOrClickToSelectFile)}
                                                                </div>
                                                                <AvatarEditor
                                                                    ref={this.setEditorRef}
                                                                    image={this.image}
                                                                    style={{ width: '200px', height: '112px' }}
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

                                </div>

                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.Title)}</span>
                                    <Field name="Title" component='input' type="text" className="form-control" id="Title" placeholder={RacMsg.Get(RacMsg.Id.ChooseATittleForTheAgenda)} />
                                </div>

                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.Text)}</span>
                                    <Field name="Text" component='textarea' type="multiline" className="form-control" id="Text" style={{ height: 80 }} placeholder={RacMsg.Get(RacMsg.Id.IncludeSomeLinesOfTextFromTheOriginalNews)} />
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

                                <div className="actions">
                                    <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Register)}</button><br />
                                </div>

                            </div>
                        </div>

                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedNewTargetForm = reduxForm<NewTargetModel>({ form: "newTargetForm" })(NewTargetForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.target, ...state.toastr } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators, ...routerActions }
)(DecoratedNewTargetForm);


