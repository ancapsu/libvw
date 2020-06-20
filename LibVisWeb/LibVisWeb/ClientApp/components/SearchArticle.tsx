import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import SearchArticleForm from './SearchArticleForm';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import { routerActions } from 'react-router-redux';

type SearchArticleProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators
    & typeof routerActions
    & RouteComponentProps<{ search: string }>;

class SearchArticle extends React.Component<SearchArticleProps, {}> {

    public render() {

        

        return (

            <div>

                <Header />

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">

                        <SearchArticleForm search={this.props.match.params.search} />

                        <ErrorMsg />

                    </div>
                </div>

                <Footer />

            </div>

        );

    }

}

export default connect(
    (state: ApplicationState) => { return { ...state.account } },
    { ...AccountStore.actionCreators, ...routerActions }
)(SearchArticle as any) as typeof SearchArticle;
