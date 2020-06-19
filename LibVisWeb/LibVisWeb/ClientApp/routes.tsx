import * as React from 'react';
import { Route } from 'react-router-dom';
import Layout from './components/Layout';
import Home from './components/Home';
import Login from './components/account/Login';
import CreateAccount from './components/account/CreateAccount';
import ConfirmEmail from './components/account/ConfirmEmail';
import ChangePassword from './components/account/ChangePassword';
import ForgotPassword from './components/account/ForgotPassword';
import RecoverPassword from './components/account/RecoverPassword';
import EditProfile from './components/account/EditProfile';
import Main from './components/Main';

import Article from './components/Article';
import EditArticle from './components/EditArticle';
import ArticleList from './components/ArticleList';
import ArticleCategory from './components/ArticleCategory';
import NewArticle from './components/NewArticle';
import ArticleListApproval from './components/ArticleListApproval';
import ArticleListRevision from './components/ArticleListRevision';
import ArticleListNarration from './components/ArticleListNarration';
import ArticleListProduction from './components/ArticleListProduction';
import ArticleListPublish from './components/ArticleListPublish';
import ArticleListTranslation from './components/ArticleListTranslation';
import SearchArticle from './components/SearchArticle';
import ValueDescription from './components/ValueDescription';

import Target from './components/Target';
import EditTarget from './components/EditTarget';
import TargetList from './components/TargetList';
import TargetListAll from './components/TargetListAll';
import TargetCategory from './components/TargetCategory';
import NewTarget from './components/NewTarget';
import SearchTarget from './components/SearchTarget';

import Video from './components/Video';
import EditVideo from './components/EditVideo';
import VideoList from './components/VideoList';
import VideoCategory from './components/VideoCategory';
import NewVideo from './components/NewVideo';
import SearchVideo from './components/SearchVideo';

import User from './components/User';
import Help from './components/Help';
import HelpValues from './components/HelpValues';
import HelpPoints from './components/HelpPoints';
import StyleManual from './components/StyleManual';
import ProductionResources from './components/ProductionResources';
import Accountability from './components/Accountability';
import Admin from './components/Admin';
import ManageUsers from './components/ManageUsers';
import ManageUser from './components/ManageUser';

export const routes = (

    <Layout>
        <Route exact path='/' component={Home as any} />
        <Route path='/login' component={Login as any} />
        <Route path='/create-account' component={CreateAccount as any} />
        <Route path='/confirm-email/:code?/:email?' component={ConfirmEmail as any} />
        <Route path='/change-password' component={ChangePassword as any} />
        <Route path='/forgot-password' component={ForgotPassword as any} />
        <Route path='/recover-password/:code?/:email?' component={RecoverPassword as any} />

        <Route path='/article/:id' component={Article as any} />
        <Route path='/edit-article/:id' component={EditArticle as any} />
        <Route path='/article-list' component={ArticleList as any} />
        <Route path='/article-list-approval' component={ArticleListApproval as any} />
        <Route path='/article-list-revision' component={ArticleListRevision as any} />
        <Route path='/article-list-narration' component={ArticleListNarration as any} />
        <Route path='/article-list-production' component={ArticleListProduction as any} />
        <Route path='/article-list-publish' component={ArticleListPublish as any} />
        <Route path='/article-list-translation' component={ArticleListTranslation as any} />
        <Route exact path='/article-category/:categ' component={ArticleCategory as any} />
        <Route path='/new-article/:target?' component={NewArticle as any} />
        <Route path='/search-article/:search?' component={SearchArticle as any} />
        <Route path='/value-description/:month' component={ValueDescription as any} />

        <Route path='/target/:id' component={Target as any} />
        <Route path='/edit-target/:id' component={EditTarget as any} />
        <Route path='/target-list' component={TargetList as any} />
        <Route path='/target-list-all' component={TargetListAll as any} />
        <Route exact path='/target-category/:categ' component={TargetCategory as any} />
        <Route path='/new-target' component={NewTarget as any} />
        <Route path='/search-target/:search?' component={SearchTarget as any} />

        <Route path='/video/:id' component={Video as any} />
        <Route path='/edit-video/:id' component={EditVideo as any} />
        <Route path='/video-list' component={VideoList as any} />
        <Route path='/video-category/:categ' component={VideoCategory as any} />
        <Route path='/new-video' component={NewVideo as any} />
        <Route path='/search-video/:search?' component={SearchVideo as any} />

        <Route path='/user/:id' component={User as any} />
        <Route path='/main' component={Main as any} />
        <Route path='/edit-profile' component={EditProfile as any} />

        <Route path='/help' component={Help as any} />
        <Route path='/help-points' component={HelpPoints as any} />
        <Route path='/help-values' component={HelpValues as any} />
        <Route path='/style-manual' component={StyleManual as any} />
        <Route path='/production-resources' component={ProductionResources as any} />
        <Route path='/admin' component={Admin as any} />
        <Route path='/accountability' component={Accountability as any} />
        <Route path='/manage-users/:search?' component={ManageUsers as any} />
        <Route path='/manage-user/:id' component={ManageUser as any} />
    </Layout>

);
