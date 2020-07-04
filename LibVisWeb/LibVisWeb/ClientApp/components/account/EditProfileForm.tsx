import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import AvatarEditor from 'react-avatar-editor';
import ReactDropzone, { DropzoneRenderArgs } from 'react-dropzone';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../../store';
import * as Toastr from 'react-redux-toastr'
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import * as AccountModel from '../../models/Account';
import { routerActions } from 'react-router-redux';
import * as Verify from '../../message/verify';
import { toastr } from 'react-redux-toastr';
import WaitPanel from '../common/WaitPanel';

const defaultProfile: string = require('../../theme/newspaper/img/defaultprofile.png');

type EditProfileBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface EditProfileFormProps {
    saveData: (data: AccountModel.AccountChangeModel) => void;
}

type InjectedEditProfileFormProps =
    EditProfileBaseProps &
    EditProfileFormProps &
    InjectedFormProps<AccountModel.AccountChangeModel>;

class EditProfileForm extends React.Component<InjectedEditProfileFormProps, {}> {

    contentReady: boolean;

    public image: File | string;
    public avatareditor: AvatarEditor;
    public dropzone: ReactDropzone;

    constructor(props: InjectedEditProfileFormProps, context: any) {

        super(props, context);

        this.avatareditor = new AvatarEditor;
        this.dropzone = new ReactDropzone;
        this.image = defaultProfile;

        this.handleDrop = this.handleDrop.bind(this);
        this.onLoadFailure = this.onLoadFailure.bind(this);
        this.onLoadSuccess = this.onLoadSuccess.bind(this);
        this.handleSubmitForm = this.handleSubmitForm.bind(this);

        this.contentReady = true;

    }

    handleDrop = (files: File[]) => {

        if (files.length > 0) {

            this.image = files[0];
            this.forceUpdate();

        }

    }

    componentWillMount() {

        if (!this.props.validToken || this.props.user == null) {

            // Não está autenticado

            this.props.push('/login');

        }
        else
        {

            // Inicializa

            this.props.initialize({ Name: this.props.user.Account.Name, Email: this.props.user.Account.Email, Bitcoin: this.props.user.Account.Bitcoin, NewsLetter: this.props.user.Account.NewsLetter });
            this.image = '/api/Avatar/' + this.props.user.Account.Id;

        }

    }

    onLoadSuccess(imgInfo: any) {

    }

    onLoadFailure(event: any) {

        toastr.error(RacMsg.Get(RacMsg.Id.InternalError), JSON.stringify(event));
        
    }

    componentWillUpdate() {

        if (this.props.toastrs.length > 0)
            this.contentReady = true;

    }

    handleSubmitForm = (values: Partial<AccountModel.AccountChangeModel>, dispatch: Dispatch<any>, props: {}) => {

        var res: string = this.avatareditor.getImageScaledToCanvas().toDataURL('image/jpeg', 3);
        var formData: AccountModel.AccountChangeModel = { ...values, Avatar: res };
        
        if (this.contentReady) {

            var err: boolean = false;

            if (values.Name == null || values.Name.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.NameIsRequired));
                err = true;
            }

            if (values.Name != null && values.Name.length > 40) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.NameTooLong));
                err = true;
            }

            if (values.Email == null || values.Email.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailIsRequired));
                err = true;
            }

            if (values.Email != null && values.Email.length > 200) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailTooBig200CharsMax));
                err = true;
            }

            if (values.Email != null && !Verify.isValidEmail(values.Email)) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.InvalidCharactersInTheEmail));
                err = true;
            }
            
            if (values.Bitcoin != null && values.Bitcoin.length > 64) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.BitcoinAddressTooLong));
                err = true;
            }

            if (!err) {

                this.contentReady = false;
                this.forceUpdate();

                values = { ...values, Lang: this.props.lang };

                dispatch(AccountStore.actionCreators.changeAccount(formData));

            }

        }

    };

    setEditorRef = (editor: any) => { if (editor) { this.avatareditor = editor; } };
    setDropzoneRef = (drop: any) => { if (drop) { this.dropzone = drop; } };

    public render() {

        

        return (

            <div>

                <form role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>

                    <WaitPanel isContentReady={this.contentReady} />

                    <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span4">

                        <div className="wpb_wrapper">
                            <div className="td-pb-border-top">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.ProfileAvatar)}</span>
                                    </h4>
                                </div>
                            </div>
                            <div className="td_block_wrap">

                                <span className="change-image-note">({RacMsg.Get(RacMsg.Id.publicInformation)} *)</span><br /><br /><br />

                                <ReactDropzone onDrop={this.handleDrop} ref={this.setDropzoneRef} preventDropOnDocument={true} accept="image/jpeg, image/png, image/gif" disableClick={true} >
                                    {(x: DropzoneRenderArgs) => {

                                        return (
                                            <section className="change-image-container">
                                                
                                                <div {...x.getRootProps({ className: 'dropzone' })}>
                                                    <input {...x.getInputProps()} />
                                                    <div className="change-image-overlay" onClick={() => { this.dropzone.open() }}>
                                                        {RacMsg.Get(RacMsg.Id.DragAnImageOrClickToSelectFile)}
                                                    </div>
                                                    <AvatarEditor
                                                        ref={this.setEditorRef}
                                                        image={this.image}
                                                        style={{ width: '200px', height: '200px' }}                                                                              
                                                        onLoadSuccess={this.onLoadSuccess}
                                                        onLoadFailure={this.onLoadFailure}
                                                        className="avatar-editor"
                                                    />
                                                </div>

                                            </section>

                                        );
                                    }}
                                </ReactDropzone>

                            </div>
                        </div>

                    </div>

                    <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span8">

                        <div className="wpb_wrapper">
                            <div className="td-pb-border-top">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.OtherInformation)}</span>
                                    </h4>
                                </div>
                            </div>
                            
                            <div className="td_block_wrap">

                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.Name)} ({RacMsg.Get(RacMsg.Id.publicInformation)} *)</span>
                                    <Field name="Name" component='input' type="text" className="form-control" id="Name" placeholder={RacMsg.Get(RacMsg.Id.ChooseAName)} />
                                </div>
                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.Email)} ({RacMsg.Get(RacMsg.Id.privateInformation)} **, {RacMsg.Get(RacMsg.Id.ifChangedConfirmationRequired)})</span>
                                    <Field name="Email" component='input' type="email" className="form-control" id="Email" placeholder={RacMsg.Get(RacMsg.Id.EnterYourEmail)} />
                                </div>
                                <div className="form-group">
                                    <span>{RacMsg.Get(RacMsg.Id.BitcoinAddress)} ({RacMsg.Get(RacMsg.Id.privateInformation)} **)</span>
                                    <Field name="Bitcoin" component='input' type="text" className="form-control" id="Bitcoin" placeholder={RacMsg.Get(RacMsg.Id.AddressForPaymentForYourPersonalBitcoinWallet)} />
                                </div>
                                <div className="actions">
                                    <span>{RacMsg.Get(RacMsg.Id.NewsletterForNewVideosAndChannelNews)} ({RacMsg.Get(RacMsg.Id.privateInformation)} **)</span><br />
                                    <Field name="NewsLetter" component='input' type="checkbox" className="form-check-input" id="NewsLetter" />
                                    <label className="form-check-label">&nbsp; {RacMsg.Get(RacMsg.Id.IncludeMyEmailInTheNewsletter)}</label>
                                </div>

                            </div>

                            <div className="td_block_wrap">

                                * {RacMsg.Get(RacMsg.Id.PublicInformationMeans)} <br /><br />
                                ** {RacMsg.Get(RacMsg.Id.PrivateInformationMeans)}

                            </div>

                        </div>

                        <div className="actions">
                            <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Change)}</button><br />
                        </div>

                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedEditProfileForm = reduxForm<AccountModel.ChangePasswordModel>({ form: "editProfileForm" })(EditProfileForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(DecoratedEditProfileForm);

