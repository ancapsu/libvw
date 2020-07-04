import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../../store';
import * as Toastr from 'react-redux-toastr'
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import * as AccountModel from '../../models/Account';
import * as Verify from '../../message/verify';
import { toastr } from 'react-redux-toastr';

type LoginBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions;

interface LoginFormProps {
    onSubmit: (data: AccountModel.LoginRequestModel) => void;
}

type InjectedLoginFormProps =
    LoginBaseProps &
    LoginFormProps &
    InjectedFormProps<AccountModel.LoginRequestModel>;

class LoginForm extends React.Component<InjectedLoginFormProps, {}> {

    contentReady: boolean;

    constructor(props: InjectedLoginFormProps, context: any) {

        super(props, context);

        this.contentReady = true;

    }

    componentWillUpdate() {

        if (this.props.toastrs.length > 0)
            this.contentReady = true;

    }

    handleSubmitForm = (values: Partial<AccountModel.LoginRequestModel>, dispatch: Dispatch<any>, props: {}) => {        

        if (this.contentReady) {

            var err: boolean = false;

            if (values.Login == null || values.Login.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailIsRequired));
                err = true;
            }

            if (values.Login != null && values.Login.length > 200) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailTooBig200CharsMax));
                err = true;
            }

            if (values.Login != null && !Verify.isValidEmail(values.Login)) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.InvalidEmailFormat));
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
            
            if (!err) {

                this.contentReady = false;
                this.forceUpdate();

                values = { ...values, Lang: this.props.lang };

                dispatch(AccountStore.actionCreators.login(values));
            }

        }

    };

    public render() {

        return (

            <div className="col-xs-12 col-sm-6 col-md-4 col-lg-4 col-sm-offset-3 col-md-offset-4 col-lg-offset-4">
                                
                <form role="form" onSubmit={this.props.handleSubmit(this.handleSubmitForm)}>
                                                
                    <div className="form-group">
                        <Field name="Login" component='input' type="email" className="form-control" id="Login" placeholder={RacMsg.Get(RacMsg.Id.UserEmail)} />
                    </div>
                    <div className="form-group">                        
                        <Field name="Password" component='input' type="password" className="form-control" id="Password" placeholder={RacMsg.Get(RacMsg.Id.Password)} />                        
                    </div>

                    <div className="actions">                        
                        <Field name="KeepLogged" component='input' type="checkbox" className="form-check-input" id="KeepLogged" />
                        <label className="form-check-label">&nbsp;{RacMsg.Get(RacMsg.Id.RememberMe)}</label>
                        <button disabled={!this.contentReady} type="submit" className={this.contentReady ? "btn-login float-right" : "btn-login-disabled float-right"}>{RacMsg.Get(RacMsg.Id.Login)}</button>
                    </div>

                    <div className="actions">                        
                        <br /><br />
                        <NavLink to={"create-account"} className="btn-other">{RacMsg.Get(RacMsg.Id.CreateAccount)}</NavLink><br />
                        <NavLink to={"forgot-password"} className="btn-other">{RacMsg.Get(RacMsg.Id.ForgotPass)}</NavLink>  <br />     
                    </div>

                </form>

            </div>


        );

    }

}

const DecoratedLoginForm = reduxForm<AccountModel.LoginRequestModel>({ form: "loginForm" })(LoginForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(DecoratedLoginForm);
