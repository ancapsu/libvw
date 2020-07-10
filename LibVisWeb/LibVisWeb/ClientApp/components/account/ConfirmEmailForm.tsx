import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../../store';
import * as Toastr from 'react-redux-toastr'
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import * as AccountModel from '../../models/Account';
import * as Verify from '../../message/verify';
import { toastr } from 'react-redux-toastr';
import WaitPanel from '../common/WaitPanel';

type ConfirmEmailBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions;

interface ConfirmEmailFormProps {
    saveData: (data: AccountModel.ConfirmEmailModel) => void;
}

type InjectedConfirmEmailFormProps =
    ConfirmEmailBaseProps
    & ConfirmEmailFormProps
    & InjectedFormProps<AccountModel.ConfirmEmailModel>
    & { code: string, email: string };

class ConfirmEmailForm extends React.Component<InjectedConfirmEmailFormProps, {}> {

    contentReady: boolean;
    email: string;

    constructor(props: InjectedConfirmEmailFormProps, context: any) {

        super(props, context);

        this.verifyCallback = this.verifyCallback.bind(this);
        this.displayRecaptcha = this.displayRecaptcha.bind(this);
        this.sendAgain = this.sendAgain.bind(this);
        this.sendAgainText = this.sendAgainText.bind(this);
        this.changeEmail = this.changeEmail.bind(this);

        this.contentReady = true;
        this.email = "";

    }

    changeEmail(event: any) {

        this.email = event.target.value;

    }

    sendAgain() {

        if (this.contentReady) {

            var err: boolean = false;

            if (this.email == null || this.email.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailIsRequired));
                err = true;
            }

            if (this.email != null && this.email.length > 200) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailTooBig200CharsMax));
                err = true;
            }

            if (this.email != null && !Verify.isValidEmail(this.email)) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.InvalidCharactersInTheEmail));
                err = true; 
            }
            
            if (!err) {
                
                this.contentReady = false;
                this.forceUpdate();

                this.props.sendAgain(this.email, 0, this.props.lang);

            }

        }

    }

    sendAgainText() {

        if (this.contentReady) {

            var err: boolean = false;

            if (this.email == null || this.email.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailIsRequired));
                err = true;
            }

            if (this.email != null && this.email.length > 200) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailTooBig200CharsMax));
                err = true;
            }

            if (this.email != null && !Verify.isValidEmail(this.email)) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.InvalidCharactersInTheEmail));
                err = true;
            }

            if (!err) {

                this.contentReady = false;
                this.forceUpdate();

                this.props.sendAgain(this.email, 1, this.props.lang);

            }

        }

    }

    componentWillUpdate() {

        if (this.props.toastrs.length > 0)
            this.contentReady = true;

    }

    handleSubmitForm = (values: Partial<AccountModel.ConfirmEmailModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {
            
            var err: boolean = false;

            if (this.email == null || this.email.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailIsRequired));
                err = true;
            }

            if (this.email != null && this.email.length > 200) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailTooBig200CharsMax));
                err = true;
            }

            if (this.email != null && !Verify.isValidEmail(this.email)) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.InvalidCharactersInTheEmail));
                err = true;
            }

            if (values.Code == null || values.Code.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.CodeIsRequired));             
                err = true;
            }

            if (values.Code != null && values.Code.length > 20) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.CodeTooBig));                              
                err = true;
            }

            if (!err) {

                this.contentReady = false;
                this.forceUpdate();

                dispatch(AccountStore.actionCreators.confirmEmail(values));

            }

        }

    };
    
    displayRecaptcha() {

        var doc: HTMLElement | null = document.getElementById('captcha-form');

        if (doc != null) {

            var script = document.createElement('script');
            script.innerHTML = '';
            script.src = 'https://www.google.com/recaptcha/api.js';
            script.async = true;
            script.defer = true;
            doc.appendChild(script);

        }

    }

    verifyCallback(response: string) {

        alert(response);

    }

    componentWillMount() {
        
        var code: string = "";
        var email: string = "";

        if (this.props.code != null && Verify.isValidCode(this.props.code))
            code = this.props.code;

        if (this.props.email != null && this.props.email.length < 200 && Verify.isValidEmail(this.props.email)) {
            email = this.props.email;
            this.email = this.props.email;
        }

        this.props.initialize({ Code: code, Email: email });

    }

    public render() {

        RacMsg.SetLanguage(3);

        this.displayRecaptcha();

        return (

            <div className="row">


                <form id="captcha-form" role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>


                    <div className="col-xs-12 col-sm-6 col-md-4 col-lg-4 col-md-offset-2 col-lg-offset-2">

                        <div className="other-actions">
                            <span>
                                <small>{RacMsg.Get(RacMsg.Id.WeHaveSentYouAnEmailWithTheConfirmation)}</small><br /><br />
                            </span>
                        </div>

                        <WaitPanel isContentReady={this.contentReady} />

                        <div className="form-group">
                            <Field name="Email" component='input' type="email" className="form-control" id="Email" onChange={this.changeEmail} placeholder={RacMsg.Get(RacMsg.Id.EnterYourEmail)} />
                        </div>
                        <div className="form-group">
                            <Field name="Code" component='input' type="text" className="form-control" id="Code" placeholder={RacMsg.Get(RacMsg.Id.EnterTheCodeSentInTheEmail)} />
                        </div>
                        <div className="form-group">
                            <div className="g-recaptcha" data-sitekey="6LeIKgsTAAAAAEfqxGo9eRukmXQXeUpyExJTfCwZ" data-callback={this.verifyCallback} ></div>
                        </div>

                        <div className="actions">
                            <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Confirm)}</button><br />
                        </div>

                    </div>

                    <div className="col-xs-12 col-sm-6 col-md-4 col-lg-4">

                        <div className="other-actions">                            
                            <span>
                                <small>{RacMsg.Get(RacMsg.Id.InCaseYouHaventReceivedTheConfirmationEmail)}</small><br /><br />
                            </span>
                        </div>

                        <div className="actions">
                            <button disabled={!this.contentReady} type="button" onClick={this.sendAgain} className={this.contentReady ? "btn-other" : "btn-other-disabled"}>{RacMsg.Get(RacMsg.Id.SendConfirmationEmailAgain)}</button><br />
                            <button disabled={!this.contentReady} type="button" onClick={this.sendAgainText} className={this.contentReady ? "btn-other" : "btn-other-disabled"}>{RacMsg.Get(RacMsg.Id.SendTextOnlyEmail)}</button><br />
                        </div>

                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedConfirmEmailForm = reduxForm<AccountModel.ConfirmEmailModel>({ form: "confirmEmailForm" })(ConfirmEmailForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(DecoratedConfirmEmailForm) as any;





