import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import ShowAuthors from './ShowAuthors';
import ShowCategories from './ShowCategories';

type ShowUserRefProps =
    AccountStore.AccountState
    & { index: number }
    & typeof AccountStore.actionCreators;

class ShowUserRef extends React.Component<ShowUserRefProps, {}> {
        
    contentReady: boolean;

    constructor(props: ShowUserRefProps, context: any) {

        super(props, context);

        this.contentReady = true;

    }
    
    public render() {

        var canEdit: boolean = false;

        if (this.props.hasList && this.props.users.length > this.props.index) {

            return (

                <div className="news-category-main">

                    <div className="news-category-image-box col-lg-1 col-md-2 col-sm-3 col-xs-4">
                        <NavLink to={'/manage-user/' + this.props.users[this.props.index].Id}>
                            <img src={"/api/Avatar/" + this.props.users[this.props.index].Id} className="search-user-img" ></img>
                        </NavLink>
                    </div>
                    <div className="news-category-details col-lg-11 col-md-10 col-sm-9 col-xs-8">

                        <h3 className="user-search-name">
                            <NavLink to={'/manage-user/' + this.props.users[this.props.index].Id} className="td-image-wrap" >
                                {this.props.users[this.props.index].Name}
                            </NavLink>
                        </h3>

                        <div className="td-module-meta-info">

                            {this.props.users[this.props.index].Blocked == 1 &&

                                <div className="user-info user-alert">
                                    {RacMsg.Get(RacMsg.Id.Blocked)}
                                </div>
                            
                            }

                            {this.props.users[this.props.index].NotConfirmed == 1 &&

                                <div className="user-info user-alert">
                                    {RacMsg.Get(RacMsg.Id.NotConfirmed)}
                                </div>

                            }

                            {this.props.users[this.props.index].Admin == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Administrator)}
                                </div>

                            }

                            {this.props.users[this.props.index].Staff == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Staff)}
                                </div>

                            }

                            {this.props.users[this.props.index].Sponsor == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Sponsor)}
                                </div>

                            }
                            
                            {this.props.users[this.props.index].RoleRevisorEn == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Revisor)} {RacMsg.Get(RacMsg.Id.Engilsh)}
                                </div>

                            }

                            {this.props.users[this.props.index].RoleRevisorEn == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Narrator)} {RacMsg.Get(RacMsg.Id.Engilsh)}
                                </div>

                            }

                            {this.props.users[this.props.index].RoleProducerEn == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Producer)} {RacMsg.Get(RacMsg.Id.Engilsh)}
                                </div>

                            }


                            {this.props.users[this.props.index].RoleRevisorPt == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Revisor)} {RacMsg.Get(RacMsg.Id.Portugues)}
                                </div>

                            }

                            {this.props.users[this.props.index].RoleRevisorPt == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Narrator)} {RacMsg.Get(RacMsg.Id.Portugues)}
                                </div>

                            }

                            {this.props.users[this.props.index].RoleProducerPt == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Producer)} {RacMsg.Get(RacMsg.Id.Portugues)}
                                </div>

                            }


                            {this.props.users[this.props.index].RoleRevisorEs == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Revisor)} {RacMsg.Get(RacMsg.Id.Espanol)}
                                </div>

                            }

                            {this.props.users[this.props.index].RoleRevisorEs == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Narrator)} {RacMsg.Get(RacMsg.Id.Espanol)}
                                </div>

                            }

                            {this.props.users[this.props.index].RoleProducerEs == 1 &&

                                <div className="user-info">
                                    {RacMsg.Get(RacMsg.Id.Producer)} {RacMsg.Get(RacMsg.Id.Espanol)}
                                </div>

                            }

                        </div>

                    </div>

                </div>

            );
            
        } else {

            return (

                <div className="news-category-main">Carregando informações...</div>

            );

        }

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators }
)(ShowUserRef as any) as any;
