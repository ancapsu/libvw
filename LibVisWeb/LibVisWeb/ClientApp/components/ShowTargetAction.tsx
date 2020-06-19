import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as TargetStore from '../store/Target';

const suggestedImg: string = require('../theme/newspaper/img/suggested.png');
const revisedImg: string = require('../theme/newspaper/img/revised.png');
const createdImg: string = require('../theme/newspaper/img/created.png');
const approvedImg: string = require('../theme/newspaper/img/approved.png');
const publishedImg: string = require('../theme/newspaper/img/published.png');
const removedImg: string = require('../theme/newspaper/img/removed.png');

type ShowTargetActionProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & { index: number }
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators;

class ShowTargetAction extends React.Component<ShowTargetActionProps, {}> {
        
    contentReady: boolean;

    constructor(props: ShowTargetActionProps, context: any) {

        super(props, context);

        this.contentReady = true;

        this.setAward = this.setAward.bind(this);

    }

    setAward(actionId: string, awardId: string, t: number) {
        
        if (this.props.validToken && this.props.user != null && this.props.user.Account.Profile >= 7) {

            this.props.registerGrant(actionId, this.props.lang, awardId, t);

        }

    }

    public render() {

        

        var canEdit: boolean = false;

        if (this.props.validToken && this.props.user != null && this.props.user.Account.Profile >= 7) {

            canEdit = true;

        }

        if (this.props.target != null) {

            return (

                <div className="news-category-main">

                    <div className="news-category-image-box col-lg-6 col-md-6 col-sm-6 col-xs-12">
                        {this.props.target.Actions[this.props.index].Type == 1 &&
                            <img src={suggestedImg} className="action-itens"></img>
                        }
                        {this.props.target.Actions[this.props.index].Type == 2 &&
                            <img src={revisedImg} className="action-itens"></img>
                        }
                        {this.props.target.Actions[this.props.index].Type == 3 &&
                            <img src={createdImg} className="action-itens"></img>
                        }
                        {this.props.target.Actions[this.props.index].Type == 4 &&
                            <img src={approvedImg} className="action-itens"></img>
                        }
                        {this.props.target.Actions[this.props.index].Type == 5 &&
                            <img src={publishedImg} className="action-itens"></img>
                        }
                        {this.props.target.Actions[this.props.index].Type == 6 &&
                            <img src={removedImg} className="action-itens"></img>
                        }
                        {this.props.target.Actions[this.props.index].TypeName} por {this.props.target.Actions[this.props.index].UserName} em {this.props.target.Actions[this.props.index].Date}
                    </div>
                    {canEdit &&
                        <div className="news-category-image-box col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            {this.props.awards.map((n, idx) => {

                                var tem: boolean = false;

                                if (this.props.target != null) {

                                    for (var i = 0; i < this.props.target.Actions[this.props.index].Grants.length; i++) {

                                        if (this.props.target.Actions[this.props.index].Grants[i].AwardId == n.Id)
                                            tem = true;

                                    }

                                }

                                if (tem) {

                                    return (

                                        <a onClick={() => { if (this.props.target != null) { this.setAward(this.props.target.Actions[this.props.index].Id, n.Id, 0); }}}>
                                            <img src={'/dist/img/a/' + n.Id + '.png'} title={n.Name} className="awards-itens"></img>
                                        </a>

                                    );

                                } else {

                                    return (

                                        <a onClick={() => { if (this.props.target != null) { this.setAward(this.props.target.Actions[this.props.index].Id, n.Id, 1) } }}>
                                            <img src={'/dist/img/a/' + n.Id + '_off.png'} title={n.Name} className="awards-itens"></img>
                                        </a>

                                    );

                                }


                            })

                            }
                        </div>
                    }
                    {!canEdit &&
                        <div className="news-category-image-box col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            {this.props.awards.map((n, idx) => {

                                var tem: boolean = false;

                                if (this.props.target != null) {

                                    for (var i = 0; i < this.props.target.Actions[this.props.index].Grants.length; i++) {

                                        if (this.props.target.Actions[this.props.index].Grants[i].AwardId == n.Id)
                                            tem = true;

                                    }

                                }

                                if (tem) {

                                    return (

                                        <img src={'/dist/img/a/' + n.Id + '.png'} title={n.Name} className="awards-itens"></img>

                                    );

                                } else {

                                    return (

                                        <p></p>

                                    );

                                }


                            })

                            }
                        </div>
                    }
                </div>

            );

        } else {

            return (

                <div className="news-category-main">

                </div>

            );

        }

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.target } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators }
)(ShowTargetAction as any) as any;
