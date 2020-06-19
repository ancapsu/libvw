import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import { LoginResultModel } from '../models/Account';
import { AuthorInfo } from 'ClientApp/models/News';

type ShowAuthorsProps =
    AccountStore.AccountState
    & { authors: AuthorInfo }
    & typeof AccountStore.actionCreators;

class ShowAuthors extends React.Component<ShowAuthorsProps, {}> {
        
    public render() {
        
        

        var canEdit: boolean = false;

        if (this.props.validToken && this.props.user != null) {

            if (this.props.authors.Authored.Id == this.props.user.Account.Id) {
                canEdit = true;
            }

            if (this.props.user.Account.Profile >= 7) {
                canEdit = true;
            }

        }

        return (

            <div className="td-post-author-name">

                {this.props.authors.Suggested.Id != "" && 
                    <div>
                        <div className="td-author-by">&nbsp;{this.props.authors.SuggestedLabel}&nbsp;</div>
                        <NavLink to={"/user/" + this.props.authors.Suggested.Id} className="news-author-name">{this.props.authors.Suggested.Name}</NavLink>
                    </div>
                }
                {this.props.authors.Authored.Id != "" && 
                    <div>
                        <div className="td-author-by">&nbsp;{this.props.authors.AuthoredLabel}&nbsp;</div>
                        <NavLink to={"/user/" + this.props.authors.Authored.Id} className="news-author-name">{this.props.authors.Authored.Name}</NavLink>
                    </div>
                }
                {this.props.authors.Revised.Id != "" &&
                    <div>
                        <div className="td-author-by">&nbsp;{this.props.authors.RevisedLabel}&nbsp;</div>
                        <NavLink to={"/user/" + this.props.authors.Revised.Id} className="news-author-name">{this.props.authors.Revised.Name}</NavLink>
                    </div>
                }
                {this.props.authors.Narrated.Id != "" &&
                    <div>
                        <div className="td-author-by">&nbsp;{this.props.authors.NarratedLabel}&nbsp;</div>
                        <NavLink to={"/user/" + this.props.authors.Narrated.Id} className="news-author-name">{this.props.authors.Narrated.Name}</NavLink>
                    </div>
                }
                {this.props.authors.Produced.Id != "" &&
                    <div>
                        <div className="td-author-by">&nbsp;{this.props.authors.ProducedLabel}&nbsp;</div>
                        <NavLink to={"/user/" + this.props.authors.Produced.Id} className="news-author-name">{this.props.authors.Produced.Name}</NavLink>
                    </div>
                }
                <div className="td-author-line"> - </div>
                <div className="entry-date updated td-module-date"><b>{this.props.authors.StatusText}</b> {this.props.authors.DateLabel} {this.props.authors.Date}</div>

            </div>

        );

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, }
)(ShowAuthors as any) as any;
