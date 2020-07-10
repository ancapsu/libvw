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

type RecoverPasswordBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions;

interface RecoverPasswordFormProps {
    saveData: (data: AccountModel.RecoverPasswordModel) => void;
}

type InjectedRecoverPasswordFormProps =
    RecoverPasswordBaseProps
    & RecoverPasswordFormProps
    & InjectedFormProps<AccountModel.RecoverPasswordModel>
    & { code: string, email: string };

class RecoverPasswordForm extends React.Component<InjectedRecoverPasswordFormProps, {}> {

    contentReady: boolean;

    constructor(props: InjectedRecoverPasswordFormProps, context: any) {

        super(props, context);

        this.contentReady = true;

    }

    componentWillUpdate() {

        if (this.props.toastrs.length > 0)
            this.contentReady = true;

    }

    handleSubmitForm = (values: Partial<AccountModel.RecoverPasswordModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {

            var err: boolean = false;

            if (values.ChangeToken == null || values.ChangeToken.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.TokenSentInEmailIsMandatory)); 
                err = true;
            }

            if (values.ChangeToken != null && values.ChangeToken.length > 250) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.TokenTooLong)); 
                err = true;
            }

            if (values.NewPassword == null || values.NewPassword.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.NewPasswordIsRequired)); 
                err = true;
            }

            if (values.NewPassword != null && values.NewPassword.length > 20) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.NewPasswordTooBig));   
                err = true;
            }

            if (values.ConfirmNewPassword == null || values.ConfirmNewPassword.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordConfDoesNotMatch));   
                err = true;
            }

            if (values.ConfirmNewPassword != null && values.ConfirmNewPassword.length > 20) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordConfDoesNotMatch));   
                err = true;
            }

            if (values.ConfirmNewPassword != null && values.NewPassword == null && values.NewPassword != values.ConfirmNewPassword) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.PasswordConfDoesNotMatch));   
                err = true;
            }

            if (!err) {

                this.contentReady = false;
                this.forceUpdate();

                values = { ...values, Lang: this.props.lang };

                dispatch(AccountStore.actionCreators.recoverPassword(values));

            }

        }

    };

    componentWillMount() {

        var code: string = "";
        var email: string = "";

        if (this.props.code != null && Verify.isValidSessionCode(this.props.code))
            code = this.props.code;

        if (this.props.email != null && this.props.email.length < 250 && Verify.isValidEmail(this.props.email))
            email = this.props.email;

        this.props.initialize({ ChangeToken: code, Email: email });

    }

    public render() {

        

        return (

            <div className="col-xs-12 col-sm-6 col-md-4 col-lg-4 col-sm-offset-3 col-md-offset-4 col-lg-offset-4">

                <div className="other-actions">
                    <span>                        
                        <small>{RacMsg.Get(RacMsg.Id.ChooseNewPasswordBelow)}</small><br /><br />
                    </span>
                </div>

                <form role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>
                    
                    <div className="form-group">
                        <Field name="Email" component='input' type="email" className="form-control" id="Email" placeholder={RacMsg.Get(RacMsg.Id.EnterYourEmail)} />                        
                    </div>
                    <div className="form-group">
                        <Field name="ChangeToken" component='input' type="text" className="form-control" id="ChangeToken" placeholder={RacMsg.Get(RacMsg.Id.AuthorizationToken)} />
                    </div>
                    <div className="form-group">
                        <Field name="NewPassword" component='input' type="password" className="form-control" id="NewPassword" placeholder={RacMsg.Get(RacMsg.Id.NewPass)} />
                    </div>
                    <div className="form-group">
                        <Field name="ConfirmNewPassword" component='input' type="password" className="form-control" id="ConfirmNewPassword" placeholder={RacMsg.Get(RacMsg.Id.NewPassConfirmation)}  />
                    </div>
                    
                    <div className="actions">
                        <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Change)} </button><br />
                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedRecoverPasswordForm = reduxForm<AccountModel.RecoverPasswordModel>({ form: "recoverPasswordForm" })(RecoverPasswordForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(DecoratedRecoverPasswordForm) as any;

