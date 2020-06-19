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

const waitGif: string = require('../../theme/newspaper/img/wait.gif');

type ForgotPasswordBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions;

interface ForgotPasswordFormProps {
    saveData: (data: AccountModel.ForgotPasswordModel) => void;
}

type InjectedForgotPasswordFormProps =
    ForgotPasswordBaseProps &
    ForgotPasswordFormProps &
    InjectedFormProps<AccountModel.ForgotPasswordModel>;

class ForgotPasswordForm extends React.Component<InjectedForgotPasswordFormProps, {}> {

    contentReady: boolean;

    constructor(props: InjectedForgotPasswordFormProps, context: any) {

        super(props, context);

        this.verifyCallback = this.verifyCallback.bind(this);
        this.displayRecaptcha = this.displayRecaptcha.bind(this);

        this.contentReady = true;

    }

    componentWillUpdate() {

        if (this.props.toastrs.length > 0)
            this.contentReady = true;

    }

    handleSubmitForm = (values: Partial<AccountModel.ForgotPasswordModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {

            var err: boolean = false;

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

            if (!err) {

                this.contentReady = false;
                this.forceUpdate();

                values = { ...values, Lang: this.props.lang };

                dispatch(AccountStore.actionCreators.forgotPassword(values));

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

    public render() {

        //

        this.displayRecaptcha();

        return (

            <div className="col-xs-12 col-sm-6 col-md-4 col-lg-4 col-sm-offset-3 col-md-offset-4 col-lg-offset-4">

                <div className="other-actions">
                    <span>                    
                        <small>{RacMsg.Get(RacMsg.Id.ProvideEmailAddressUsedInYourRegistration)}</small><br /><br />
                    </span>
                </div>

                <div className={(!this.contentReady ? "wait-panel" : "wait-panel-disabled")}>
                    <img src={waitGif} ></img>
                </div>

                <form id="captcha-form" role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>
                    
                    <div className="form-group">
                        <Field name="Email" component='input' type="email" className="form-control" id="Email" placeholder={RacMsg.Get(RacMsg.Id.EnterYourEmail)} />                        
                    </div>
                    <div className="form-group">
                        <div className="g-recaptcha" data-sitekey="6LeIKgsTAAAAAEfqxGo9eRukmXQXeUpyExJTfCwZ" data-callback={this.verifyCallback} ></div>
                    </div>

                    <div className="actions">
                        <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Send)}</button><br />
                    </div>

                </form>

            </div>


        );

    }

}

const DecoratedForgotPasswordForm = reduxForm<AccountModel.ForgotPasswordModel>({ form: "forgotPasswordForm" })(ForgotPasswordForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(DecoratedForgotPasswordForm);
