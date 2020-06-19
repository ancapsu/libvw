import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';

type HeaderMenuProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators;    

class HeaderMenu extends React.Component<HeaderMenuProps , {}> {

    mobile: boolean;
    selected: number;
    
    constructor(props: HeaderMenuProps, context: any) {

        super(props, context);

        this.mobile = false;
        this.selected = -1;
        this.changeMobile = this.changeMobile.bind(this);
        this.setSelected = this.setSelected.bind(this);

    }

    changeMobile() {

        this.mobile = !this.mobile;
        this.forceUpdate();

    }

    setSelected(n: number) {

        this.selected = n;
        this.forceUpdate();

    }

    public render() {

        return (

            <div className="td-container td-header-menu">

                <div className="tda-menu-container">
                    <ul className="sf-menu">
                        <li className="td-menu-item">
                            <NavLink to={"/"}><i className="fa fa-home"></i></NavLink>
                        </li>
                        <li className="td-menu-item">
                            <NavLink to={"/video-list"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Videos)}</NavLink>
                            <ul>
                                <li>
                                    <NavLink to={"/video-category/theory"} className="tda-menu">{RacMsg.Get(RacMsg.Id.LibertarianTheory)}</NavLink>
                                </li>
                                <li>
                                    <NavLink to={"/video-category/news"} className="tda-menu">{RacMsg.Get(RacMsg.Id.LibertarianViewpoint)}</NavLink>
                                </li>
                                <li>
                                    <NavLink to={"/video-category/comic"} className="tda-menu">{RacMsg.Get(RacMsg.Id.ComicVideos)}</NavLink>
                                </li>
                                <li>
                                    <NavLink to={"/search-video"} className="tda-menu">{RacMsg.Get(RacMsg.Id.SearchVideo)}</NavLink>
                                </li>
                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && this.props.user.Account.Profile >= 7 &&
                                    <li>
                                        <NavLink to={"/new-video"} className="tda-menu">{RacMsg.Get(RacMsg.Id.NewVideo)}</NavLink>
                                    </li>
                                }
                            </ul>
                        </li>
                        <li className="td-menu-item">
                            <NavLink to={"/article-list"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Articles)}</NavLink>
                            <ul>                                                                
                                {(!this.props.validToken || this.props.user == null) &&
                                    <li>
                                    <NavLink to={"/article-category/article"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Articles)}</NavLink>
                                    </li>
                                }                                
                                {(!this.props.validToken || this.props.user == null) &&
                                    <li>
                                    <NavLink to={"/article-category/chronicle"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Chronicles)}</NavLink>
                                    </li>
                                }                                
                                <li>
                                    <NavLink to={"/search-article"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Search)}</NavLink>
                                </li>
                                {this.props.validToken && this.props.user != null &&
                                    <li>
                                        <NavLink to={"/new-article"} className="tda-menu">{RacMsg.Get(RacMsg.Id.NewArticle)}</NavLink>
                                    </li>
                                }
                                {this.props.validToken && this.props.user != null &&
                                    <li>
                                        <NavLink to={"/article-list-translation"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Translate)}</NavLink>
                                    </li>
                                }
                                {this.props.validToken && this.props.user != null && 
                                    <li>
                                        <NavLink to={"/article-list-approval"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Approve)}</NavLink>
                                    </li>
                                }
                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Revisor == 1) &&
                                    <li>
                                        <NavLink to={"/article-list-revision"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Revise)}</NavLink>
                                    </li>
                                }
                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Narrator == 1) &&                                
                                    <li>
                                        <NavLink to={"/article-list-narration"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Narrate)}</NavLink>
                                    </li>
                                }
                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Producer == 1) &&
                                    <li>
                                        <NavLink to={"/article-list-production"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Produce)}</NavLink>
                                    </li>
                                }    
                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7) &&
                                    <li>
                                        <NavLink to={"/article-list-publish"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Publish)}</NavLink>
                                    </li>
                                }    
                            </ul>
                        </li>
                        <li className="td-menu-item">
                            <NavLink to={"/target-list"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Targets)}</NavLink>
                            <ul>
                                {this.props.validToken && this.props.user != null &&
                                    <li>
                                        <NavLink to={"/new-target"} className="tda-menu">{RacMsg.Get(RacMsg.Id.SuggestTarget)}</NavLink>
                                    </li>
                                }
                                <li>
                                    <NavLink to={"/target-list-all"} className="tda-menu">{RacMsg.Get(RacMsg.Id.AllAgendas)}</NavLink>
                                </li>
                                <li>
                                    <NavLink to={"/search-target"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Search)}</NavLink>
                                </li>
                            </ul>
                        </li>
                        <li className="td-menu-item">
                            <NavLink to={"/main"} className="tda-menu">{RacMsg.Get(RacMsg.Id.MyAccount)}</NavLink>
                            {this.props.validToken && this.props.user != null &&
                                <ul>
                                    <li>
                                        <NavLink to={"/main"} className="tda-menu">{RacMsg.Get(RacMsg.Id.MyProfile)}</NavLink>
                                    </li>
                                    <li>
                                        <NavLink to={"/change-password"} className="tda-menu">{RacMsg.Get(RacMsg.Id.ChangePassword)}</NavLink>
                                </li>
                                    {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Staff == 1) &&
                                        <li>
                                            <NavLink to={"/admin"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Administration)}</NavLink>
                                        </li>
                                    }
                                    {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Staff == 1 || this.props.user.Account.Sponsor == 1) &&
                                        <li>
                                            <NavLink to={"/accountability"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Accountability)}</NavLink>
                                        </li>
                                    }
                                    <li>
                                    <a className="tda-menu">{RacMsg.Get(RacMsg.Id.Language)}</a>
                                        <ul className="sec-level">
                                            <li>
                                                <a href="#" onClick={() => { this.props.setLanguage(2); }} className="tda-menu">English</a>
                                            </li>
                                            <li>
                                                <a href="#" onClick={() => { this.props.setLanguage(3); }} className="tda-menu">Português</a>
                                            </li>
                                            <li>
                                                <a href="#" onClick={() => { this.props.setLanguage(4); }} className="tda-menu">Español</a>
                                            </li>
                                        </ul>
                                    </li>
                                    <li>
                                        <NavLink to={"/help"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Help)}</NavLink>
                                    </li>
                                    <li>
                                        <a onClick={() => { this.props.logoff(); }}>{RacMsg.Get(RacMsg.Id.Logoff)}</a>
                                    </li>
                                </ul>
                            }
                            {(!this.props.validToken || this.props.user == null) &&
                                <ul>
                                    <li>
                                        <NavLink to={"/login"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Login)}</NavLink>
                                    </li>
                                    <li>
                                        <NavLink to={"/create-account"} className="tda-menu">{RacMsg.Get(RacMsg.Id.CreateAccount)}</NavLink>
                                    </li>
                                    <li>
                                        <NavLink to={"/forgot-password"} className="tda-menu">{RacMsg.Get(RacMsg.Id.ForgotPass)}</NavLink>
                                    </li>
                                    <li>
                                        <NavLink to={"/confirm-email"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Confirmation)}</NavLink>
                                    </li>
                                    <li>
                                        <a className="tda-menu">{RacMsg.Get(RacMsg.Id.Language)}</a>
                                        <ul className="sec-level">
                                            <li>
                                                <a href="#" onClick={() => { this.props.setLanguage(2); }} className="tda-menu">English</a>
                                            </li>
                                            <li>
                                                <a href="#" onClick={() => { this.props.setLanguage(3); }} className="tda-menu">Português</a>
                                            </li>
                                            <li>
                                                <a href="#" onClick={() => { this.props.setLanguage(4); }} className="tda-menu">Español</a>
                                            </li>
                                        </ul>
                                    </li>
                                    <li>
                                        <NavLink to={"/help"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Help)}</NavLink>
                                    </li>
                                </ul>
                            }

                        </li>
                    </ul>
                </div>

                {!this.mobile &&

                    <div className="visible-only-mobile mobile-menu-on" onClick={() => { this.changeMobile() }}>

                        <i className="fa fa-bars"></i>

                    </div>

                }

                {this.mobile &&

                    <div className="visible-only-mobile mobile-menu" onClick={() => { this.changeMobile() }}>

                        <i className="fa fa-bars"></i>

                    </div>

                }

                {this.mobile &&

                    <div className="visible-only-mobile mobile-menu-body">

                        <div className="mobile-menu-item-top">
                            <NavLink to={"/"}><i className="fa fa-home"></i></NavLink>
                        </div>

                        {this.selected == 1 &&

                            <div>

                                <div className="mobile-menu-item-top-selected" onClick={() => { this.setSelected(-1); }}>
                                    Vídeos
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/video-list"} className="tda-menu">Todos os vídeos</NavLink>
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/video-category/theory"} className="tda-menu">Teoria libertária</NavLink>
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/video-category/news"} className="tda-menu">Visão libertária</NavLink>
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/video-category/comic"} className="tda-menu">Cômicos</NavLink>
                                </div>

                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && this.props.user.Account.Profile >= 7 &&

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/new-video"} className="tda-menu">Novo vídeo</NavLink>
                                    </div>

                                }

                            </div>

                        }

                        {this.selected != 1 &&

                            <div className="mobile-menu-item-top" onClick={() => { this.setSelected(1); }}>
                                Vídeos
                            </div>

                        }

                        {this.selected == 2 &&

                            <div>

                                <div className="mobile-menu-item-top-selected" onClick={() => { this.setSelected(-1); }}>
                                    Matérias
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/article-list"} className="tda-menu">Todas as matérias</NavLink>
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/article-category/article"} className="tda-menu">Artigos</NavLink>
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/article-category/chronicle"} className="tda-menu">Crônica libertária</NavLink>
                                </div>

                                {this.props.validToken && this.props.user != null &&

                                    <li>
                                        <NavLink to={"/new-article"} className="tda-menu">{RacMsg.Get(RacMsg.Id.NewArticle)}</NavLink>
                                    </li>

                                }

                                {this.props.validToken && this.props.user != null &&

                                    <li>
                                        <NavLink to={"/article-list-translation"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Translate)}</NavLink>
                                    </li>

                                }

                                {this.props.validToken && this.props.user != null && 

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/article-list-approval"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Approve)}</NavLink>
                                    </div>

                                }

                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Revisor == 1) &&

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/article-list-revision"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Revise)}</NavLink>
                                    </div>

                                }

                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Narrator == 1) &&

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/article-list-narration"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Narrate)}</NavLink>
                                    </div>

                                }

                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Producer == 1) &&

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/article-list-produce"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Produce)}</NavLink>
                                    </div>

                                }

                                {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && this.props.user.Account.Profile >= 7 &&

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/article-list-publish"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Publish)}</NavLink>
                                    </div>

                                }

                            </div>

                        }

                        {this.selected != 2 &&

                            <div className="mobile-menu-item-top" onClick={() => { this.setSelected(2); }}>
                                Matérias
                            </div>

                        }

                        {this.selected == 3 &&

                            <div>

                                <div className="mobile-menu-item-top-selected" onClick={() => { this.setSelected(-1); }}>
                                    Pautas
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/target-list"} className="tda-menu">{RacMsg.Get(RacMsg.Id.LastAgendas)}</NavLink>
                                </div>

                                <div className="mobile-menu-item">
                                    <NavLink to={"/target-list-all"} className="tda-menu">{RacMsg.Get(RacMsg.Id.AllAgendas)}</NavLink>
                                </div>

                                {this.props.validToken && this.props.user != null &&

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/new-target"} className="tda-menu">{RacMsg.Get(RacMsg.Id.SuggestNewAgenda)}</NavLink>
                                    </div>
                                }

                            </div>

                        }

                        {this.selected != 3 &&

                            <div className="mobile-menu-item-top" onClick={() => { this.setSelected(3); }}>
                                Pautas
                            </div>

                        }

                        {this.selected == 4 &&

                            <div>

                                <div className="mobile-menu-item-top-selected" onClick={() => { this.setSelected(-1); }}>
                                    Linguagem
                                </div>
                        
                                <div className="mobile-menu-item">
                                    <a href="#" onClick={() => { this.props.setLanguage(2); }} className="tda-menu">English</a>
                                </div>

                                <div className="mobile-menu-item">
                                    <a href="#" onClick={() => { this.props.setLanguage(3); }} className="tda-menu">Português</a>
                                </div>

                                <div className="mobile-menu-item">
                                    <a href="#" onClick={() => { this.props.setLanguage(4); }} className="tda-menu">Español</a>
                                </div>
                            
                            </div>

                        }

                        {this.selected != 4 &&

                            <div className="mobile-menu-item-top" onClick={() => { this.setSelected(4); }}>
                                Linguagem
                            </div>

                        }

                        {this.selected == 5 &&

                            <div>

                                <div className="mobile-menu-item-top-selected" onClick={() => { this.setSelected(-1); }}>
                                    Minha conta
                                </div>

                            {this.props.validToken && this.props.user != null &&

                                <div>

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/main"} className="tda-menu">{RacMsg.Get(RacMsg.Id.YourAccount)}</NavLink>
                                    </div>

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/change-password"} className="tda-menu">{RacMsg.Get(RacMsg.Id.ChangePassword)}</NavLink>
                                    </div>

                                    <div className="mobile-menu-item">
                                        <a onClick={() => { this.props.logoff(); }}>{RacMsg.Get(RacMsg.Id.Logoff)}</a>
                                    </div>

                                    {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Staff == 1) &&

                                        <div className="mobile-menu-item">
                                            <NavLink to={"/admin"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Administration)}</NavLink>
                                        </div>

                                    }

                                    {this.props.validToken && this.props.user != null && this.props.user.Account != undefined && (this.props.user.Account.Profile >= 7 || this.props.user.Account.Staff == 1 || this.props.user.Account.Sponsor == 1) &&

                                        <div className="mobile-menu-item">
                                            <NavLink to={"/accountability"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Accountability)}</NavLink>
                                        </div>

                                    }

                                </div>
                            
                            }

                            {(!this.props.validToken || this.props.user == null) &&

                                <div>

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/login"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Login)}</NavLink>
                                    </div>

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/create-account"} className="tda-menu">{RacMsg.Get(RacMsg.Id.CreateAccount)}</NavLink>
                                    </div>

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/forgot-password"} className="tda-menu">{RacMsg.Get(RacMsg.Id.ForgotPass)}</NavLink>
                                    </div>

                                    <div className="mobile-menu-item">
                                        <NavLink to={"/confirm-email"} className="tda-menu">{RacMsg.Get(RacMsg.Id.Confirmation)}</NavLink>
                                    </div>

                                </div>

                            }
                
                            </div>
                            
                        }

                        {this.selected != 5 &&

                            <div className="mobile-menu-item-top" onClick={() => { this.setSelected(5); }}>
                                Minha conta
                            </div>

                        }

                    </div>

                }

            </div>

        );

    }

}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(HeaderMenu as any);