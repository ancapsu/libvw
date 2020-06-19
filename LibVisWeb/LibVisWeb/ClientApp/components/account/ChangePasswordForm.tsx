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

const waitGif: string = require('../../theme/newspaper/img/wait.gif');

type ChangePasswordBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions;

interface ChangePasswordFormProps {
    saveData: (data: AccountModel.ChangePasswordModel) => void;
}

type InjectedChangePasswordFormProps =
    ChangePasswordBaseProps &
    ChangePasswordFormProps &
    InjectedFormProps<AccountModel.ChangePasswordModel>;

class ChangePasswordForm extends React.Component<InjectedChangePasswordFormProps, {}> {

    contentReady: boolean;

    constructor(props: InjectedChangePasswordFormProps, context: any) {

        super(props, context);

        this.contentReady = true;

    }

    componentWillUpdate() {

        if (this.props.toastrs.length > 0)
            this.contentReady = true;

    }

    handleSubmitForm = (values: Partial<AccountModel.ChangePasswordModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {

            var err: boolean = false;

            if (values.OldPassword == null || values.OldPassword.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.OldPasswordIsRequired));  
                err = true;
            }

            if (values.OldPassword != null && values.OldPassword.length > 20) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.OldPasswordTooBig));                     
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

                dispatch(AccountStore.actionCreators.changePassword(values));

            }

        }

    };

    public render() {

        return (

            <div className="col-xs-12 col-sm-6 col-md-4 col-lg-4 col-sm-offset-3 col-md-offset-4 col-lg-offset-4">

                <div className="other-actions">
                    <span>
                        <small>{RacMsg.Get(RacMsg.Id.ChooseAPasswordForYou)}. &nbsp; {RacMsg.Get(RacMsg.Id.PasswordsCriteria)}</small><br /><br />
                    </span>
                </div>

                <div className={(!this.contentReady ? "wait-panel" : "wait-panel-disabled")}>
                    <img src={waitGif} ></img>
                </div>

                <form role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>
                    
                    <div className="form-group">
                        <Field name="OldPassword" component='input' type="password" className="form-control" id="OldPassword" placeholder={RacMsg.Get(RacMsg.Id.OldPass)} />
                    </div>
                    <div className="form-group">
                        <Field name="NewPassword" component='input' type="password" className="form-control" id="NewPassword" placeholder={RacMsg.Get(RacMsg.Id.NewPass)} />
                    </div>
                    <div className="form-group">
                        <Field name="ConfirmNewPassword" component='input' type="password" className="form-control" id="ConfirmNewPassword" placeholder={RacMsg.Get(RacMsg.Id.NewPassConfirmation)} />
                    </div>

                    <div className="actions">
                        <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Change)}</button><br />
                    </div>

                </form>

            </div>

        );

    }

}

const DecoratedChangePasswordForm = reduxForm<AccountModel.ChangePasswordModel>({ form: "changePasswordForm" })(ChangePasswordForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(DecoratedChangePasswordForm);

