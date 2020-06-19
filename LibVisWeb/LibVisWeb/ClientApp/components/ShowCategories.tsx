import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import { LoginResultModel } from '../models/Account';
import { CategInfo } from 'ClientApp/models/News';

type ShowCategoriesProps =
    AccountStore.AccountState
    & { categ: CategInfo, link: string }
    & typeof AccountStore.actionCreators;

class ShowCategories extends React.Component<ShowCategoriesProps, {}> {
        
    public render() {
        
        

        return (

            <div className="td-module-meta-info">
                <ul className="td-category">
                    {this.props.categ.Categories.map(m => {
                        return (
                            <li className="entry-category">
                                <NavLink to={"/" + this.props.link + "/" + m.Label}>{m.Category}</NavLink>
                            </li>)
                    })
                    }
                </ul>
            </div>

        );

    }
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, }
)(ShowCategories as any) as any;
