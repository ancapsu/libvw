import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as HomePageStore from '../store/HomePage';
import ErrorMsg from '../message/errormsg';
import Footer from './Footer/Footer';
import Header from './header/Header';
import { ArticleModel } from '../models/Article';
import { AccountModel, AccountChangeModel } from '../models/Account';
import { toastr } from 'react-redux-toastr';
import * as Verify from '../message/verify';

type HomeProps =
    AccountStore.AccountState
    & HomePageStore.HomePageState
    & typeof HomePageStore.actionCreators
    & typeof AccountStore.actionCreators
    & RouteComponentProps<{ startIndex: string }>;

class Home extends React.Component<HomeProps, {}> {

    newsLetterInput: string;
    contentReady: boolean;

    //
    //  Construtor inicializa
    //
    constructor(props: HomeProps, context: any) {

        super(props, context);

        this.newsLetterInput = "";
        this.contentReady = false;

        this.newsLetterInputChange = this.newsLetterInputChange.bind(this);
        this.newsLetterClick = this.newsLetterClick.bind(this);

    }

    componentDidMount() {

        this.props.requestHomePage(this.props.lang);

    }

    componentWillReceiveProps(props: HomeProps) {

        if (props.lang != this.props.lang) {

            this.props.requestHomePage(props.lang);
            this.contentReady = true;

        }

    }

    newsLetterInputChange(e: any) {

        this.newsLetterInput = e.target.value;

    }

    newsLetterClick(e: any) {

        if (this.newsLetterInput != null) {

            var err: boolean = false;

            if (this.newsLetterInput == null || this.newsLetterInput.length <= 0) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailIsRequired));
                err = true;
            }

            if (this.newsLetterInput != null && this.newsLetterInput.length > 200) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.EmailTooBig200CharsMax));
                err = true;
            }

            if (this.newsLetterInput != null && !Verify.isValidEmail(this.newsLetterInput)) {
                toastr.error(RacMsg.Get(RacMsg.Id.Error), RacMsg.Get(RacMsg.Id.InvalidEmailFormat));
                err = true;
            }
            
            if (!err) {

                // Tem que acertar se for o próprio cara

                if (this.props.user != null && this.props.user.Account.Email == this.newsLetterInput) {

                    var cad: AccountChangeModel = new AccountChangeModel();

                    cad.Name = this.props.user.Account.Name;
                    cad.Email = this.props.user.Account.Email;
                    cad.Bitcoin = this.props.user.Account.Bitcoin;
                    cad.NewsLetter = true;

                    this.props.changeAccount(cad);

                } else {

                    this.props.registerNewsletter(this.newsLetterInput);

                }

            }

        }

    }

    public render() {

        return (

            <div>

                <Header />

                {this.props.homePage != null && this.props.homePage.Warnings.length > 0 &&

                    <div className="container open-line"></div>

                }

                {this.props.homePage != null && this.props.homePage.Warnings.map(w => {

                    if (w.Type == 1) {

                        return (

                            <div className="tdc-row">
                                <div className="td-business-home-row wpb_row td-pb-row">
                                    <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                                        <div className="wpb_wrapper">
                                            <div className="td-block-title-wrap">
                                                <h4 className="red-warning-block">
                                                    <span className="red-warning">{w.Title}</span>
                                                </h4>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        );

                    } else if (w.Type == 2) {

                        var str: string = w.Text;

                        var parts: string[] = str.split('\r');

                        return (

                            <div className="tdc-row">
                                <div className="td-business-home-row wpb_row td-pb-row">
                                    <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                                        <div className="wpb_wrapper">
                                            <div className="td-block-title-wrap">
                                                <h4 className="td-block-title">
                                                    <span className="news-page-headline">{w.Title}</span>
                                                </h4>
                                            </div>
                                            {parts.map(s => {

                                                return (<div className="td_block_inner td-column-2 home-container-button"> {s} </div>);

                                            })
                                            }
                                                
                                        </div>
                                    </div>
                                </div>
                            </div>

                        );

                    } 

                })}

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">

                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span6">

                            <div className="wpb_wrapper">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.ChannelNewsletter)}</span>
                                    </h4>
                                </div>
                            </div>

                            <form>

                                <div className="row">

                                    <div className="col-lg-12 col-md-12 col-sm-12 col-xs-12 form-group">
                                        {RacMsg.Get(RacMsg.Id.RegisterYourEmailInTheNewsletter)}
                                    </div>

                                </div>

                                <div className="row">

                                    <div className="col-lg-8 col-md-8 col-sm-6 col-xs-6 form-group">                                            
                                        <input type="text" onChange={this.newsLetterInputChange} name="newsletter" id='newsletter' className="form-control" placeholder={RacMsg.Get(RacMsg.Id.YourArrobaEmailDotCom)} />
                                    </div>

                                    <div className="col-lg-4 col-md-4 col-sm-6 col-xs-6 form-group">
                                        <a className='btn-newsletter' onClick={this.newsLetterClick}>{RacMsg.Get(RacMsg.Id.Register)}</a><br />
                                    </div>

                                </div>

                            </form>

                        </div>

                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span6">

                            <div className="wpb_wrapper">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.ProductionFunnel)}</span>
                                    </h4>
                                </div>
                            </div>

                            <div className="row">

                                <NavLink to={"/target-list"} className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-target funil-link">
                                    <b>{this.props.homePage.NumTargets}</b><br />
                                    {RacMsg.Get(RacMsg.Id.Targets)}
                                </NavLink>
                                <NavLink to={"/article-list-approval"} className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-approve funil-link">
                                    <b>{this.props.homePage.NumApproval}</b><br />
                                    {RacMsg.Get(RacMsg.Id.Approval)}
                                </NavLink>

                                {(this.props.user == null || this.props.user.Account.Revisor == 0) &&
                                    <div className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-revise funil-no-link">
                                        <b>{this.props.homePage.NumRevision}</b><br />
                                        {RacMsg.Get(RacMsg.Id.Review)}
                                    </div>
                                }
                                {this.props.user != null && this.props.user.Account.Revisor != 0 &&
                                    <NavLink to={"/article-list-revision"} className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-revise funil-link">
                                        <b>{this.props.homePage.NumRevision}</b><br />
                                        {RacMsg.Get(RacMsg.Id.Review)} 
                                    </NavLink>
                                }
                                    
                                {(this.props.user == null || this.props.user.Account.Narrator == 0) &&
                                    <div className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-narrate funil-no-link">
                                        <b>{this.props.homePage.NumNarration}</b><br />
                                        {RacMsg.Get(RacMsg.Id.Narration)} 
                                    </div>
                                }
                                {this.props.user != null && this.props.user.Account.Narrator != 0 &&
                                    <NavLink to={"/article-list-narration"} className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-narrate funil-link">
                                        <b>{this.props.homePage.NumNarration}</b><br />
                                        {RacMsg.Get(RacMsg.Id.Narration)} 
                                    </NavLink>
                                }

                                {(this.props.user == null || this.props.user.Account.Producer == 0) &&
                                    <div className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-produce funil-no-link">
                                        <b>{this.props.homePage.NumProduction}</b><br />
                                        {RacMsg.Get(RacMsg.Id.Production)}
                                    </div>
                                }
                                {this.props.user != null && this.props.user.Account.Revisor != 0 &&
                                    <NavLink to={"/article-list-production"} className="col-lg-2 col-md-2 col-sm-4 col-xs-6 funil-item funil-item-produce funil-link">
                                        <b>{this.props.homePage.NumProduction}</b><br />
                                        {RacMsg.Get(RacMsg.Id.Production)}
                                    </NavLink>
                                }

                            </div>

                        </div>

                    </div>
                </div>
                    

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="wpb_wrapper">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.LastVideosOnTheChannel)}</span>
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="tdc-video-row">

                    {this.props.homePage.Videos.map(video => {

                        return (

                            <div className="col-lg-3 col-md-6 col-sm-12 col-xs-12 news-box" id={video.YoutubeLink}>

                                <div className="td_module_mx15 td-animation-stack td-meta-info-hide td-big-grid-post-0 td-big-grid-post td-medium-thumb">
                                    <div className="td-module-thumb">
                                        <NavLink to={'/video/' + video.Id} className="td-image-wrap video-title" title="{call.Title}">
                                            <img className="header-thumb" src={"/api/Video/Image?id=" + video.Id} alt="" title="{call.Title}"></img>
                                        </NavLink>
                                    </div>
                                    <div className="news-box-overlay"></div>
                                    <div className="td-meta-info-container">
                                        <div className="td-meta-align">
                                            <div className="td-big-grid-meta">
                                                <NavLink to={'/video-category/' + video.Categories.MainCategory.Label} className="td-post-category">
                                                    {video.Categories.MainCategory.Category}
                                                </NavLink>
                                                <h3 className="entry-title td-module-title">
                                                    <NavLink to={'/video/' + video.Id} className="video-title" title={video.Title}>{video.Title}</NavLink>
                                                </h3>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>

                        );

                    }

                    )}

                </div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span4">
                            <div className="wpb_wrapper">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.WantToHelp)}</span>
                                    </h4>
                                </div>
                                <div className="td_block_inner td-column-2 home-container-button">

                                    <div className="col-lg-4 col-md-4 col-sm-4 col-xs-4 home-container-button">
                                        <NavLink to={'/new-target'} className="home-help-button home-help-button-1">{RacMsg.Get(RacMsg.Id.SuggestTarget)}</NavLink>
                                    </div>
                                    <div className="col-lg-4 col-md-4 col-sm-4 col-xs-4 home-container-button">
                                        <NavLink to={'/new-article'} className="home-help-button home-help-button-2">{RacMsg.Get(RacMsg.Id.WriteAnArticle)}</NavLink>
                                    </div>
                                    <div className="col-lg-4 col-md-4 col-sm-4 col-xs-4 home-container-button">
                                        <NavLink to={'/article-list-translation'} className="home-help-button home-help-button-3">{RacMsg.Get(RacMsg.Id.TranslateAnArticle)}</NavLink>
                                    </div>

                                </div>
                            </div>
                            <div className="td_block_inner td-column-2 home-container-button">&nbsp;</div>
                            <div className="wpb_wrapper">
                                <div className="td-block-title-wrap">
                                    <h4 className="td-block-title">
                                        <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.Numbers)}</span>
                                    </h4>
                                </div>
                                <div className="td_block_wrap td_block_social_counter td-social-style2 td-social-font-icons td-pb-border-top td_block_template_11">
                                    <div className="td-block-title-wrap"></div>
                                    <div className="td-social-list">

                                        {

                                            this.props.homePage.Statistics.map(sts => {

                                                return (

                                                    <div className="td_social_type td-pb-margin-side">
                                                        <div className="td-social-box">
                                                            <div className="tda-social">
                                                                <img src={sts.ImageLink}></img>
                                                            </div>
                                                            <span className="td_social_info">{sts.Value}</span>
                                                            <span className="td_social_info">{sts.Parameter}</span>
                                                        </div>
                                                    </div>

                                                );

                                            }

                                            )}

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span8">
                            <div className="wpb_wrapper">
                                <div className="td_block_wrap td_block_25 td-pb-border-top td-column-2">
                                    <div className="td-block-title-wrap">
                                        <h4 className="td-block-title">
                                            <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.LastPublishedArticles)}</span>
                                        </h4>
                                    </div>
                                    <div className="td_block_inner td-column-2">

                                        {

                                            this.props.homePage.Articles.map(row => {

                                                return (

                                                    <div className="td-block-row">

                                                        {this.renderNewsColumn(row.Articles[0])}
                                                        {this.renderNewsColumn(row.Articles[1])}
                                                        {this.renderNewsColumn(row.Articles[2])}

                                                    </div>

                                                );

                                            })

                                        }

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <Footer />

                <ErrorMsg />

            </div>

        );

    }
    
    private renderNewsColumn(nm: ArticleModel) {

        if (nm != null) {

            return (

                <div className="td-block-span4">
                    <div className="td_module_mx4 td_module_wrap td-animation-stack td-meta-info-hide">
                        <div className="td-module-image">
                            <div className="td-module-thumb">
                                <NavLink to={'/article/' + nm.Id} className="td-image-wrap" title={nm.Title}>
                                    <img className="entry-thumb td-animation-stack-type0-2" src={"/api/Article/Image?id=" + nm.Id}></img>
                                </NavLink>
                            </div>
                            <NavLink to={'/article-category/' + nm.Categories.MainCategory.Label} className="td-post-category">{nm.Categories.MainCategory.Category}</NavLink>
                        </div>
                        <h3 className="entry-title td-module-title">
                            <NavLink to={'/article/' + nm.Id} className="td-image-wrap" title={nm.Title}>
                                {nm.Title}
                            </NavLink>
                        </h3>
                    </div>
                </div>

            );

        } else {
            
            return (

                <div className="td-block-span4">
            
                </div>

            );

        }

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.homePage } },
    { ...AccountStore.actionCreators, ...HomePageStore.actionCreators }
)(Home as any) as typeof Home;
