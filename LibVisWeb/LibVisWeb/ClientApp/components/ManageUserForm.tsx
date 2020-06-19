import * as React from 'react';
import { reduxForm, Field, InjectedFormProps } from 'redux-form';
import AvatarEditor from 'react-avatar-editor';
import ReactDropzone, { DropzoneRenderArgs } from 'react-dropzone';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect, Dispatch } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as AccountModel from '../models/Account';
import { routerActions } from 'react-router-redux';
import * as Toastr from 'react-redux-toastr'

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type ManageUserBaseProps =
    AccountStore.AccountState
    & Toastr.ToastrState    
    & { id: string }
    & typeof AccountStore.actionCreators
    & typeof Toastr.actions
    & typeof routerActions;

interface ManageUserFormProps {
    saveData: (data: AccountModel.ManagerChangePasswordModel) => void;
}

type InjectedManageUserFormProps =
    ManageUserBaseProps &
    ManageUserFormProps &
    InjectedFormProps<AccountModel.ManagerChangePasswordModel>
    & { id: string };

class ManageUserForm extends React.Component<InjectedManageUserFormProps, {}> {

    contentReady: boolean;

    constructor(props: InjectedManageUserFormProps, context: any) {

        super(props, context);

        this.contentReady = false;

        this.selRole = this.selRole.bind(this);
        this.confirm = this.confirm.bind(this);

    }

    componentDidMount() {

        if (!this.props.validToken || this.props.user == null) {

            // Não está autenticado

            this.props.push('/login');

        } else {

            this.props.requestUserForManager(this.props.id, this.props.lang);

        }

    }

    componentWillReceiveProps(props: InjectedManageUserFormProps) {

        // Inicializa

        if (!this.props.hasManageUser && props.hasManageUser && props.manageUser != null) {

            this.contentReady = true;

        }

        if (props.toastrs.length > 0) {

            this.contentReady = true;

        }

    }

    handleSubmitForm = (values: Partial<AccountModel.ManagerChangePasswordModel>, dispatch: Dispatch<any>, props: {}) => {

        if (this.contentReady) {

            this.contentReady = false;
            this.forceUpdate();

            var formData: AccountModel.ManagerChangePasswordModel = { ...values, Id: this.props.id, Lang: this.props.lang };

            //dispatch(this.props.changeUserManagerPassword(formData));

        }

    }

    selRole(role: number, lang: number) {

        if (this.props.manageUser != null) {



        }

    }

    confirm() {


    }

    public render() {

        if (this.props.manageUser != null) {

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
                                            <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.ManageUser)} '{this.props.manageUser.Name}'</span>
                                        </h4>
                                    </div>
                                </div>

                                <div className="row">

                                    <div className="col-lg-6 col-md-6 col-sm-12 col-xs-12">

                                        <div className="form-group">
                                            <span>{RacMsg.Get(RacMsg.Id.Name)}</span>
                                            {this.props.manageUser.Name}
                                        </div>

                                        <div className="form-group">
                                            <span>{RacMsg.Get(RacMsg.Id.Email)}</span>
                                            {this.props.manageUser.Email}
                                        </div>

                                        <div className="form-group">
                                            <span>{RacMsg.Get(RacMsg.Id.Observation)}</span>
                                            <div className="tda-module-meta-info">

                                                {this.props.manageUser.Blocked == 1 &&

                                                    <div className="user-info user-alert">
                                                        {RacMsg.Get(RacMsg.Id.Blocked)}
                                                    </div>

                                                }

                                                {this.props.manageUser.NotConfirmed == 1 &&

                                                    <div className="user-info user-alert">
                                                        {RacMsg.Get(RacMsg.Id.NotConfirmed)}
                                                    </div>

                                                }

                                                <a onClick={() => { this.confirm() }}>{RacMsg.Get(RacMsg.Id.Confirm)}</a>

                                            </div>
                                        </div>

                                        <div className="form-group">
                                            <span>{RacMsg.Get(RacMsg.Id.Roles)}</span>
                                            <div className="tda-module-meta-info">
                                                <ul className="tda-category">

                                                    <li className={(this.props.manageUser.RoleRevisorEn == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(3, 2) }}>{RacMsg.Get(RacMsg.Id.Revisor)} {RacMsg.Get(RacMsg.Id.Engilsh)}</a>
                                                    </li>
                                                    <li className={(this.props.manageUser.RoleNarratorEn == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(4, 2) }}>{RacMsg.Get(RacMsg.Id.Narrator)} {RacMsg.Get(RacMsg.Id.Engilsh)}</a>
                                                    </li>
                                                    <li className={(this.props.manageUser.RoleProducerEn == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(5, 2) }}>{RacMsg.Get(RacMsg.Id.Producer)} {RacMsg.Get(RacMsg.Id.Engilsh)}</a>
                                                    </li>

                                                </ul>
                                                <ul className="tda-category">

                                                    <li className={(this.props.manageUser.RoleRevisorPt == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(3, 3) }}>{RacMsg.Get(RacMsg.Id.Revisor)} {RacMsg.Get(RacMsg.Id.Portugues)}</a>
                                                    </li>
                                                    <li className={(this.props.manageUser.RoleNarratorPt == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(4, 3) }}>{RacMsg.Get(RacMsg.Id.Narrator)} {RacMsg.Get(RacMsg.Id.Portugues)}</a>
                                                    </li>
                                                    <li className={(this.props.manageUser.RoleProducerPt == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(5, 3) }}>{RacMsg.Get(RacMsg.Id.Producer)} {RacMsg.Get(RacMsg.Id.Portugues)}</a>
                                                    </li>

                                                </ul>
                                                <ul className="tda-category">

                                                    <li className={(this.props.manageUser.RoleRevisorEs == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(3, 4) }}>{RacMsg.Get(RacMsg.Id.Revisor)} {RacMsg.Get(RacMsg.Id.Espanol)}</a>
                                                    </li>
                                                    <li className={(this.props.manageUser.RoleNarratorEs == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(4, 4) }}>{RacMsg.Get(RacMsg.Id.Narrator)} {RacMsg.Get(RacMsg.Id.Espanol)}</a>
                                                    </li>
                                                    <li className={(this.props.manageUser.RoleProducerEs == 1 ? "tda-category-selected" : "tda-category-not-selected")}>
                                                        <a onClick={() => { this.selRole(5, 4) }}>{RacMsg.Get(RacMsg.Id.Producer)} {RacMsg.Get(RacMsg.Id.Espanol)}</a>
                                                    </li>

                                                </ul>
                                            </div>
                                        </div>

                                        <div className="form-group">
                                            <span>{RacMsg.Get(RacMsg.Id.ChangePassword)}</span>
                                            <div className="tda-module-meta-info">

                                                <div className="form-group">
                                                    <Field name="NewPassword" component='input' type="password" className="form-control" id="NewPassword" placeholder={RacMsg.Get(RacMsg.Id.NewPass)} />
                                                </div>
                                                <div className="form-group">
                                                    <Field name="ConfirmNewPassword" component='input' type="password" className="form-control" id="ConfirmNewPassword" placeholder={RacMsg.Get(RacMsg.Id.NewPassConfirmation)} />
                                                </div>

                                            </div>
                                        </div>

                                    </div>

                                    <div className="col-lg-6 col-md-6 col-sm-12 col-xs-12">


                                    </div>

                                </div>

                                <div className="td_block_wrap">

                                    <div className="actions">
                                        <button disabled={!this.contentReady} type="submit" className={(this.contentReady) ? "btn-login" : "btn-login-disabled"}>{RacMsg.Get(RacMsg.Id.Change)}</button><br />
                                    </div>

                                </div>
                            </div>

                        </div>

                    </form>

                </div>

            );

        } else {

            return (

                <div>

                    <div className="container open-line"></div>

                    <div className="tdc-row">
                        <div className="td-business-home-row wpb_row td-pb-row">
                            <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                                <div className="wpb_wrapper">
                                    <div className="td_block_wrap">

                                        Carregando informações...

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                                        
                </div>

            );

        }

    }

}

const DecoratedManageUserForm = reduxForm<AccountModel.ManagerChangePasswordModel>({ form: "manageUserForm" })(ManageUserForm as any) as any;

export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators, ...routerActions }
)(DecoratedManageUserForm) as any;


