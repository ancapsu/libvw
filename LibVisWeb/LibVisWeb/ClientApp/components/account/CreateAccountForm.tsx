import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../../store';
import * as Toastr from 'react-redux-toastr'
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import * as AccountModel from '../../models/Account';
import { toastr } from 'react-redux-toastr';
import * as Verify from '../../message/verify';
import WaitPanel from '../common/WaitPanel';

type CreateAccountBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState    
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions;

interface CreateAccountFormProps {
    saveData: (data: AccountModel.CreateAccountModel) => void;
}

type InjectedCreateAccountFormProps =
    CreateAccountBaseProps &
    CreateAccountFormProps &
    InjectedFormProps<AccountModel.CreateAccountModel>;

class CreateAccountForm extends React.Component<InjectedCreateAccountFormProps, {}> {
    
    contentReady: boolean;

    constructor(props: InjectedCreateAccountFormProps, context: any) {

        super(props, context);

        this.verifyCallback = this.verifyCallback.bind(this);
        this.displayRecaptcha = this.displayRecaptcha.bind(this);

        this.contentReady = true;

    }

    componentWillUpdate() {
                
        if (this.props.toastrs.length > 0)
            this.contentReady = true;

    }

    handleSubmitForm = (values: Partial<AccountModel.CreateAccountModel>, dispatch: Dispatch<any>, props: {}) => {

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

            if (values.Password == null || values.Password.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordIsRequired));
                err = true;
            }

            if (values.Password != null && values.Password.length > 20) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordTooBig20CharsMax));
                err = true;
            }

            if (values.ConfirmPassword == null || values.ConfirmPassword.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordConfDoesNotMatch));
                err = true;
            }

            if (values.ConfirmPassword != null && values.ConfirmPassword.length > 20) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordConfDoesNotMatch));
                err = true;
            }

            if (values.ConfirmPassword != null && values.Password == null && values.Password != values.ConfirmPassword) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordConfDoesNotMatch));
                err = true;
            }

            if (!err) {

                this.contentReady = false;
                this.forceUpdate();

                values = { ...values, Lang: this.props.lang };

                dispatch(AccountStore.actionCreators.createAccount(values));

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

        this.displayRecaptcha();

        return (

            <div className="col-xs-12 col-sm-6 col-md-4 col-lg-4 col-sm-offset-3 col-md-offset-4 col-lg-offset-4">

                <div className="other-actions">
                    <span>
                        <small>{RacMsg.Get(RacMsg.Id.PleaseProvideYouDataBelow)}</small><br /><br />
                    </span>
                </div>

                <WaitPanel isContentReady={this.contentReady} />

                <form id="captcha-form" role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>
                                                
                    <div className="form-group">
                        <Field name="Name" component='input' type="text" className="form-control" id="Name" placeholder={RacMsg.Get(RacMsg.Id.ChooseAName)} />                        
                    </div>
                    <div className="form-group">
                        <Field name="Email" component='input' type="email" className="form-control" id="Email" placeholder={RacMsg.Get(RacMsg.Id.EnterYourEmail)} />                        
                    </div>
                    <div className="form-group">
                        <Field name="Password" component='input' type="password" className="form-control" id="Password" placeholder={RacMsg.Get(RacMsg.Id.ChooseAPassword)} />
                    </div>
                    <div className="form-group">
                        <Field name="ConfirmPassword" component='input' type="password" className="form-control" id="ConfirmPassword" placeholder={RacMsg.Get(RacMsg.Id.ConfirmThePassword)}  />
                    </div>
                    <div className="form-group">
                        <small>&nbsp; {RacMsg.Get(RacMsg.Id.PasswordsCriteria)}</small>
                    </div>
                    <div className="form-group">
                        <div className="g-recaptcha" data-sitekey="6LeIKgsTAAAAAEfqxGo9eRukmXQXeUpyExJTfCwZ" data-callback={this.verifyCallback} ></div>
                    </div>

                    
                    <div className="actions">
                        <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Register)}</button><br />
                    </div>

                </form>

            </div>


        );

    }

}

const DecoratedCreateAccountForm = reduxForm<AccountModel.CreateAccountModel>({ form: "createAccountForm" })(CreateAccountForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(DecoratedCreateAccountForm);

