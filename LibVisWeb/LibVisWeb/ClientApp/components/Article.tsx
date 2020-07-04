import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import ReactDropzone, { DropzoneRenderArgs } from 'react-dropzone';
import * as AccountStore from '../store/Account';
import * as ArticleStore from '../store/Article';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import ShowAuthors from './ShowAuthors';
import ShowCategories from './ShowCategories';
import ShowArticleAction from './ShowArticleAction';
import { toastr } from 'react-redux-toastr';
import { IncludeActionWithFile, ActionFile } from '../models/Article';
import WaitPanel from './common/WaitPanel';

const editPng: string = require('../theme/newspaper/img/edit.png');
const deletePng: string = require('../theme/newspaper/img/delete.png');

type ArticleProps =
    AccountStore.AccountState
    & ArticleStore.ArticleState
    & typeof AccountStore.actionCreators
    & typeof ArticleStore.actionCreators
    & RouteComponentProps<{ id: string }>;

class Article extends React.Component<ArticleProps, {}> {

    files: File[];
    dropzone: ReactDropzone | null;
    contentReady: boolean;
    observ: string;
    editcomment: string;
    selnarrator: string;
    curid: string;

    constructor(props: ArticleProps, context: any) {

        super(props, context);

        this.contentReady = false;
        this.observ = "";
        this.editcomment = "";
        this.selnarrator = "";
        this.curid = "";

        this.files = [];
        this.dropzone = null;

        this.publish = this.publish.bind(this);
        this.handleDrop = this.handleDrop.bind(this);
        this.setDropzoneRef = this.setDropzoneRef.bind(this);
        this.obsChanged = this.obsChanged.bind(this);
        this.keyDown = this.keyDown.bind(this);
        this.deleteComment = this.deleteComment.bind(this);
        this.editComment = this.editComment .bind(this);
        this.changeSel = this.changeSel.bind(this);
        this.componentDidMount = this.componentDidMount.bind(this);

    }

    componentDidMount() {

        if (!this.props.hasArticle || !this.props.hasEditBase || this.curid != this.props.match.params.id) {

            this.props.requestEditBase(this.props.lang, "", this.props.match.params.id);
            this.props.requestArticle(this.props.match.params.id, this.props.lang);
            this.curid = this.props.match.params.id;

        } else {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: ArticleProps) {

        if (!props.hasArticle) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestArticle(this.props.match.params.id, this.props.lang);

        }

        if (props.hasArticle && props.hasEditBase) {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    publish(sts: number) {

        if (this.props.article != null) {

            if (sts == 3) {

                if (this.files.length > 0) {

                    this.contentReady = false;
                    this.forceUpdate();

                    var x: IncludeActionWithFile = new IncludeActionWithFile;

                    const toBase64 = (file: File) => new Promise((resolve, reject) => {
                        const reader = new FileReader();
                        reader.onload = () => resolve(reader.result);
                        reader.onerror = error => reject(error);
                        reader.readAsDataURL(file);
                    });

                    x.Id = this.props.article.Id;
                    x.Status = sts;
                    x.Files = [];

                    const fileProcessing = this.files.map(async fil => {

                        var af: ActionFile = new ActionFile;

                        af.FileName = fil.name;
                        af.Content = await toBase64(fil) as string;

                        x.Files.push(af);

                    });

                    Promise.all(fileProcessing).then(() => {

                        this.props.changeStatusWithFiles(x);

                    });
                    
                } else {

                    this.props.changeStatus(this.props.article.Id, sts, "");

                }

            } else if (sts == 5) {

                if (this.files.length > 0) {

                    this.contentReady = false;
                    this.forceUpdate();

                    var x: IncludeActionWithFile = new IncludeActionWithFile;

                    const toBase64 = (file: File) => new Promise((resolve, reject) => {
                        const reader = new FileReader();
                        reader.onload = () => resolve(reader.result);
                        reader.onerror = error => reject(error);
                        reader.readAsDataURL(file);
                    });

                    x.Id = this.props.article.Id;
                    x.Status = sts;
                    x.Info = this.selnarrator;
                    x.Files = [];

                    const fileProcessing = this.files.map(async fil => {

                        var af: ActionFile = new ActionFile;

                        af.FileName = fil.name;
                        af.Content = await toBase64(fil) as string;

                        x.Files.push(af);

                    });

                    Promise.all(fileProcessing).then(() => {

                        this.props.changeStatusWithFiles(x);

                    });

                } else {

                    this.props.changeStatus(this.props.article.Id, sts, this.selnarrator);

                }

            } else {

                this.props.changeStatus(this.props.article.Id, sts, "");

            }

        }

    }

    setDropzoneRef = (drop: any) => { if (drop) { this.dropzone = drop; } }

    handleDrop = (files: File[]) => {

        if (files.length > 0) {

            this.files = this.files.concat(files);
            this.forceUpdate();

        }

    }

    obsChanged(e: any) {

        this.observ = e.target.value;

    }

    keyDown(e: any) {

        if (e.key === 'Enter') {

            if (this.editcomment == "") {

                if (this.props.article != null)
                    this.props.registerNewComment(this.props.article.Id, this.observ);

                e.currentTarget.value = "";

                if (e.preventDefault)
                    e.preventDefault();

                return false;

            } else {

                
                if (this.props.article != null)
                    this.props.changedComment(this.editcomment, this.observ);

                e.currentTarget.value = "";

                this.editcomment = "";

                if (e.preventDefault)
                    e.preventDefault();

                return false;

            }

        }

    }

    deleteComment(id: string) {

        if (this.props.article != null)
            this.props.removeComment(id);

    }

    editComment(id: string, curValue: string) {

        this.observ = curValue;
        this.editcomment = id;        
        this.forceUpdate();

    }

    changeSel(e: any) {

        this.selnarrator = e.target.value;

    }

    public render() {

        

        if (this.curid != this.props.match.params.id) {
            
            this.contentReady = true;
            
        }

        if (this.props.hasArticle && this.props.article != null && this.props.article.Paragraphs.length > 0) {

            /* Monta o parágrafo */
            
            var primeiro: string = this.props.article.Paragraphs[0].trim();
            var capital: string = primeiro.substring(0, 1);
            primeiro = primeiro.substring(1);
            var jafoiprim: boolean = false;

            var primtitle: string = this.props.article.Title.trim();
            var captitle: string = primtitle.substring(0, 1);
            primtitle = primtitle.substring(1);

            /* Lista de narrações */

            var nars: string[] = [];
            var nids: string[] = [];

            for (var i = 0; i < this.props.article.Actions.length; i++) {

                if (this.props.article.Actions[i].Type == 5) {

                    var jat: boolean = false;

                    for (var j = 0; j < nids.length; j++) {

                        if (nids[j] == this.props.article.Actions[i].UserId) {

                            jat = true;
                            break;

                        }

                    }

                    if (!jat) {

                        nids.push(this.props.article.Actions[i].UserId);
                        nars.push(this.props.article.Actions[i].UserName);

                    }

                }

            }

            /* Seleção padrão */

            if (nars.length > 0)
                this.selnarrator = nids[0];

            /* Define o acesso */

            var canEdit: boolean = false;
            var canPublish: boolean = false;

            if (this.props.validToken && this.props.user != null) {

                if (this.props.article.Status == 1 && this.props.article.Authors.Authored.Id == this.props.user.Account.Id) {
                    canEdit = true;
                }

                if (this.props.article.Authors.Revised.Id == this.props.user.Account.Id) {
                    canEdit = true;
                }

                if (this.props.article.Status == 2 && this.props.user.Account.Revisor && this.props.article.Authors.Authored.Id != this.props.user.Account.Id) {
                    canEdit = true;
                }

                if (this.props.user.Account.Profile >= 7) {
                    canEdit = true;
                    canPublish = true;
                }

            }

            return (

                <div>

                    <Header />

                    <WaitPanel isContentReady={this.contentReady} />

                    <div className="container open-line"></div>

                    <div className="tdc-video-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="post type-post status-publish format-standard has-post-thumbnail category-entrepreneurs">

                                <div className="td-post-header">

                                    <ShowCategories categ={this.props.article.Categories} link="article-cateogry" />

                                    <header className="td-post-title">
                                        <h1 className="entry-title">{this.props.article.Title}</h1>
                                        <div className="td-module-meta-info">

                                            <ShowAuthors authors={this.props.article.Authors} />

                                            {canEdit && this.props.article.Status < 2 &&
                                                <div className="td-post-author-name">
                                                    <NavLink to={"/edit-article/" + this.props.article.Id} className="gerar-artigo-agora">
                                                        &nbsp; {RacMsg.Get(RacMsg.Id.EditArticle)}  &nbsp; <i className="fa fa-chevron-right"></i>
                                                    </NavLink>
                                                </div>
                                            }

                                            {canEdit && this.props.article.Status >= 2 && this.props.article.Status <= 3 &&
                                                <div className="td-post-author-name">
                                                    <NavLink to={"/edit-article/" + this.props.article.Id} className="gerar-artigo-agora">
                                                        &nbsp; {RacMsg.Get(RacMsg.Id.ReviseArticle)} &nbsp; <i className="fa fa-chevron-right"></i>
                                                    </NavLink>
                                                </div>
                                            }

                                            {canPublish &&
                                                <div className="td-post-author-name">
                                                    <NavLink to={"/edit-article/" + this.props.article.Id} className="gerar-artigo-agora-adm">
                                                        &nbsp; {RacMsg.Get(RacMsg.Id.EditArticle)} (ADM) &nbsp; <i className="fa fa-chevron-right"></i>
                                                    </NavLink>
                                                </div>
                                            }

                                        </div>
                                    </header>
                                </div>

                                {this.props.article.Target.Id != "" &&

                                    <div className="td-post-content news-post-content">

                                        <a href={this.props.article.Target.Link}>{this.props.article.Target.Link}</a>

                                    </div>

                                }

                                <div className="container open-line"></div>
                                <div className="container open-line"></div>

                                <div className="td-post-content news-post-content">
                                    <div className="td-post-featured-image">
                                        <img className="news-featured-image" src={"/api/Article/Image?id=" + this.props.article.Id} ></img>
                                    </div>
                                    {(this.props.article.Status == 3 || this.props.article.Status == 4 || this.props.article.Status == 5) &&

                                        <p className="narration-author">

                                            <span className="dropcap dropcap3">{captitle}</span>
                                            {primtitle}
                                            &nbsp;
                                                                          
                                            <br /><br /><br /><br />

                                            <span className="dropcap dropcap3">{RacMsg.Get(RacMsg.Id.ArticleSalutionFirstLetter)}</span>
                                            {RacMsg.Get(RacMsg.Id.ArticleSalutionRestOf)}
                                             
                                            <br /><br />

                                        </p>

                                    }
                                    <p>
                                        <span className="dropcap dropcap3">{capital}</span>
                                        {primeiro}
                                    </p>

                                    {

                                        this.props.article.Paragraphs.map(par => {

                                            if (jafoiprim)
                                            {

                                                return <p>{par}</p>

                                            }
                                            else
                                            {

                                                jafoiprim = true;
                                                return <span></span>
                                                
                                            }

                                        })
                                    
                                    }

                                    {(this.props.article.Status == 3 || this.props.article.Status == 4 || this.props.article.Status == 5) &&

                                        <div>

                                            <p className="narration-author">
                                                <span className="dropcap dropcap3">{RacMsg.Get(RacMsg.Id.ArticleEndFirstLetter)}</span>
                                                {RacMsg.Get(RacMsg.Id.ArticleEndRestOf)} &nbsp;
                                        
                                                {this.props.article.Authors.Suggested.Id != "" &&
                                                    <div>
                                                        {this.props.article.Authors.SuggestedLabel} {this.props.article.Authors.Suggested.Name} &nbsp;
                                                    </div>
                                                }
                                                {this.props.article.Authors.Authored.Id != "" &&
                                                    <div>
                                                        {this.props.article.Authors.AuthoredLabel} {this.props.article.Authors.Authored.Name} &nbsp;
                                                    </div>
                                                }
                                                {this.props.article.Authors.Revised.Id != "" &&
                                                    <div>
                                                        {this.props.article.Authors.RevisedLabel} {this.props.article.Authors.Revised.Name} &nbsp;
                                                    </div>
                                                }
                                                {this.props.user != null && (this.props.article.Status == 3) &&
                                                    <div>
                                                        e {this.props.article.Authors.NarratedLabel} {this.props.user.Account.Name} &nbsp;
                                                    </div>
                                                }
                                                {this.props.article.Authors.Narrated.Id != "" && (this.props.article.Status == 4 || this.props.article.Status == 5) &&
                                                    <div>
                                                        e {this.props.article.Authors.NarratedLabel} {this.props.article.Authors.Narrated.Name} &nbsp;
                                                    </div>
                                                }

                                                <br /><br /><br /><br />

                                                <span className="dropcap dropcap3">{RacMsg.Get(RacMsg.Id.ArticleLikeFirstLetter)}</span>
                                                {RacMsg.Get(RacMsg.Id.ArticleLikeRestOf)}

                                            </p>

                                        </div>

                                    }

                                    {this.props.article.Links.length > 0 &&

                                        <div>

                                            <br />
                                            <p>
                                                {RacMsg.Get(RacMsg.Id.AssociatedLinks)}:
                                            </p>

                                            {

                                            this.props.article.Links.map(par => {

                                                    return <p><a href={par.Link}>{par.Link}</a></p>

                                                })

                                            }

                                        </div>

                                    }

                                    <div className="news-category-image-box col-lg-12 col-md-12 col-sm-12 col-xs-12 action-obs">

                                        <span className="comment-header">
                                            {RacMsg.Get(RacMsg.Id.CommentsForThisArticle)}
                                        </span>


                                    </div>

                                    {this.props.article.Comments.map(n => {

                                        if (this.editcomment == n.Id) {
                                            
                                            return (

                                                <div className="news-category-image-box col-lg-12 col-md-12 col-sm-12 col-xs-12 action-obs">

                                                    <div className="news-category-image-box col-lg-2 col-md-2 col-sm-2 col-xs-2">

                                                        <img src={'/api/Avatar/' + n.IncludedById} className="comment-user-img" ></img>

                                                        <span className="date-for-comment">
                                                            {n.Included}
                                                        </span>

                                                        <span className="user-name-for-comment">
                                                            {n.IncludedBy}
                                                        </span>

                                                    </div>

                                                    <div className="news-category-image-box col-lg-10 col-md-10 col-sm-10 col-xs-10">

                                                        <div className="form-group">

                                                            <input type="text" className="form-control" name={"edt" + n.Id} id={"edt" + n.Id} placeholder="" onChange={this.obsChanged} onKeyDown={this.keyDown} defaultValue={this.observ} />

                                                        </div>

                                                    </div>

                                                </div>

                                            );

                                        } else {

                                            return (

                                                <div className="news-category-image-box col-lg-12 col-md-12 col-sm-12 col-xs-12 action-obs">

                                                    <div className="news-category-image-box col-lg-2 col-md-2 col-sm-2 col-xs-2">

                                                        <img src={'/api/Avatar/' + n.IncludedById} className="comment-user-img" ></img>

                                                        <span className="date-for-comment">
                                                            {n.Included}
                                                        </span>

                                                        <span className="user-name-for-comment">
                                                            {n.IncludedBy}
                                                        </span>

                                                    </div>

                                                    {this.props.user != null && this.props.user.Account.Id == n.IncludedById &&

                                                        <div className="news-category-image-box col-lg-9 col-md-9 col-sm-9 col-xs-9">

                                                            <span className="comment-comment">
                                                                {n.Comment}
                                                            </span>

                                                        </div>

                                                    }
                                                    {this.props.user != null && this.props.user.Account.Id == n.IncludedById &&

                                                        <div className="news-category-image-box col-lg-1 col-md-1 col-sm-1 col-xs-1">

                                                        <span className="comment-comment">
                                                                <img src={editPng} className="comment-button" onClick={() => { this.editComment(n.Id, n.Comment) }} alt="Editar comentário"></img>
                                                                <img src={deletePng} className="comment-button" onClick={() => { this.deleteComment(n.Id) }} alt="Remover comentário"></img>
                                                            </span>

                                                        </div>

                                                    }
                                                    {this.props.user == null || this.props.user.Account.Id != n.IncludedById &&

                                                        <div className="news-category-image-box col-lg-10 col-md-10 col-sm-10 col-xs-10">

                                                            <span className="comment-comment">
                                                                {n.Comment}
                                                            </span>

                                                        </div>

                                                    }

                                                </div>

                                            );

                                        }

                                    })}

                                    {this.props.user != null && this.editcomment == "" &&

                                        <div className="news-category-image-box col-lg-12 col-md-12 col-sm-12 col-xs-12 action-obs">

                                            <div className="news-category-image-box col-lg-2 col-md-2 col-sm-2 col-xs-2">

                                                <img src={'/api/Avatar/' + this.props.user.Account.Id} className="comment-user-img" ></img>

                                                <span className="date-for-comment">
                                                    Agora
                                                </span>

                                                <span className="user-name-for-comment">
                                                     {this.props.user.Account.Name}
                                                </span>

                                            </div>

                                            <div className="news-category-image-box col-lg-10 col-md-10 col-sm-10 col-xs-10">

                                                <div className="form-group">

                                                    <input type="text" className="form-control" name="newcmt" id="newcmt" placeholder="Inclua um comentário..." onChange={this.obsChanged} onKeyDown={this.keyDown} />

                                                </div>

                                            </div>

                                        </div>

                                    }

                                </div>
                            </div>

                            <div className="news-category-image-box col-lg-12 col-md-12 col-sm-12 col-xs-12 action-obs">

                                <span className="comment-header">
                                    {RacMsg.Get(RacMsg.Id.ActionsFromThisArticle)}
                                </span>


                            </div>

                            <div className="td_block_inner td-column-2">

                                {

                                    this.props.article.Actions.map((n, idx) => {

                                        return (

                                            <ShowArticleAction index={idx} />

                                        );

                                    })

                                }

                            </div>

                            {canPublish &&

                                <div className="td_block_inner td-column-2">

                                {this.props.article != null && this.props.article.Status < 2 &&

                                    <a onClick={() => this.publish(2)} className="btn-publish-adm">Aprovado por ADM. Pedir revisão</a>

                                }
                                {this.props.article != null && this.props.article.Status < 3 &&

                                    <a onClick={() => this.publish(3)} className="btn-publish-adm">Revisado por ADM. Pedir narração</a>
                                    
                                }
                                {this.props.article != null && this.props.article.Status < 4 &&

                                    <a onClick={() => this.publish(4)} className="btn-publish-adm">Narrado por ADM. Pedir produção</a>

                                }
                                {this.props.article != null && this.props.article.Status < 5 &&

                                    <a onClick={() => this.publish(5)} className="btn-publish-adm">Produzido por ADM. Pedir publicação</a>

                                }
                                {this.props.article != null && this.props.article.Status < 6 &&

                                    <a onClick={() => this.publish(6)} className="btn-publish-adm">Publicar por ADM</a>

                                }
                                {this.props.article != null && this.props.article.Status < 7 &&

                                    <a onClick={() => this.publish(7)} className="btn-publish-adm">Remover por ADM</a>

                                }

                                </div>

                            }

                            {this.props.article != null && this.props.article.Status == 2 && this.props.validToken && this.props.user != null && this.props.user.Account.Revisor == 1 && this.props.article.Authors.Authored.Id != this.props.user.Account.Id && 

                                <div>

                                    <div className="form-group">
                                  
                                        <span>Narrador preferencial:</span>

                                        <select className="form-control" onChange={this.changeSel}>

                                            <option value="0">Geral</option>

                                            {

                                                this.props.narrators.map((n, idx) => {

                                                    return (

                                                        <option value={n.Id}>{n.Name}</option>

                                                    );

                                                })

                                            }

                                        </select>

                                    </div>

                                    <a onClick={() => this.publish(3)} className="btn-publish">Revisado. Encaminhar para narração.</a>
                                    <a onClick={() => this.publish(7)} className="btn-publish">Texto muito ruim. Descartar.</a>

                                </div>
                            
                            }

                            {this.props.article != null && this.props.article.Status == 3 && this.props.validToken && this.props.user != null && this.props.user.Account.Narrator == 1 &&

                                <div>

                                    <ReactDropzone onDrop={this.handleDrop} ref={this.setDropzoneRef} preventDropOnDocument={true} accept="audio/mpeg, audio/mp3, audio/mp4, audio/vnd.wav, audio/ogg, audio/wav, audio/x-wav, audio/m4a" disableClick={true} >
                                        {(x: DropzoneRenderArgs) => {

                                            return (
                                                <div className="target-audio-container" onClick={() => { if (this.dropzone != null) this.dropzone.open() }}>

                                                    <div {...x.getRootProps({ className: 'dropzone-target-audio' })}>

                                                        <input {...x.getInputProps()} />

                                                        {

                                                            this.files.map(n => {

                                                                return (

                                                                    <div>
                                                                        {n.name} - {n.size} bytes
                                                                    </div>

                                                                );

                                                            })

                                                        }

                                                        <b>Arraste ou clique para carregar arquivos de audio</b>

                                                    </div>

                                                </div>

                                            );
                                        }}
                                    </ReactDropzone>

                                    <a onClick={() => this.publish(3)} className="btn-publish">Incluir narração</a>

                                </div>

                            }

                            {this.props.article != null && this.props.article.Status == 4 && this.props.validToken && this.props.user != null && this.props.user.Account.Producer == 1 &&

                                <div>

                                    <div className="form-group">

                                        <span>Narrador escolhido:</span>

                                        <select className="form-control" onChange={this.changeSel}>

                                            {

                                                nids.map((n, idx) => {

                                                    return (

                                                        <option value={n}>{nars[idx]}</option>

                                                    );

                                                })

                                            }

                                        </select>

                                    </div>
                                
                                    <div className="form-group">

                                        Arquivos para a produção:

                                        <ul>
                                            <li>Arquivo com o vídeo em si, preferencialmente formato .mp4</li>
                                            <li>Arquivo com a thumbnail, preferencialmente formato .jpg</li>
                                        </ul>

                                    </div>

                                    <ReactDropzone onDrop={this.handleDrop} ref={this.setDropzoneRef} preventDropOnDocument={true} accept="video/mp4, image/jpg, image/png" disableClick={true} >
                                        {(x: DropzoneRenderArgs) => {

                                            return (
                                                <div className="target-audio-container" onClick={() => { if (this.dropzone != null) this.dropzone.open() }}>

                                                    <div {...x.getRootProps({ className: 'dropzone-target-audio' })}>

                                                        <input {...x.getInputProps()} />

                                                        {

                                                            this.files.map(n => {

                                                                return (

                                                                    <div>
                                                                        {n.name} - {n.size} bytes
                                                                        </div>

                                                                );

                                                            })

                                                        }

                                                        <b>Arraste ou clique para carregar arquivos de vídeo</b>

                                                    </div>

                                                </div>

                                            );
                                        }}
                                    </ReactDropzone>

                                    <a onClick={() => this.publish(5)} className="btn-publish">Produzido. Pedir publicação</a>

                                </div>

                            }

                        </div>
                    </div>

                    <Footer />

                    <ErrorMsg />

                </div>

            );

        } else {

            return (

                <div>

                    <Header />

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

                    <Footer />

                    <ErrorMsg />

                </div>

            );

        }

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.article } },
    { ...AccountStore.actionCreators, ...ArticleStore.actionCreators }
)(Article as any) as typeof Article;
