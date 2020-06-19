import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';

const youtubeImg: string = require('../../theme/newspaper/img/youtube16.png');
const bitchuteImg: string = require('../../theme/newspaper/img/bitchute16.png');
const facebookImg: string = require('../../theme/newspaper/img/facebook16.png');
const mindsImg: string = require('../../theme/newspaper/img/minds16.png');
const twitterImg: string = require('../../theme/newspaper/img/twitter16.png');
const gabImg: string = require('../../theme/newspaper/img/gab16.png');

type HeaderTopProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators;    

class HeaderTop extends React.Component<HeaderTopProps, {}> {

    public render() {

        return (

            <div className="td-container td-header-row">

                <div className="tda-header-social-leftmargin">

                    <span className="tda-social-icon-wrap">
                        <a target="_blank" href="https://youtube.com/ancapsu" title="Youtube">
                            <img src={youtubeImg} ></img>
                        </a>
                    </span>
                    <span className="tda-social-icon-wrap">
                        <a target="_blank" href="https://bitchute.com/ancapsu" title="Bitchute">
                            <img src={bitchuteImg} ></img>
                        </a>
                    </span>

                    <span className="tda-social-icon-wrap">
                        <a target="_blank" href="https://www.facebook.com/pageancapsu/" title="Facebook">
                            <img src={facebookImg} ></img>
                        </a>
                    </span>
                    <span className="tda-social-icon-wrap">
                        <a target="_blank" href="https://minds.com/ancapsu" title="Minds">
                            <img src={mindsImg} ></img>
                        </a>
                    </span>

                    <span className="tda-social-icon-wrap">
                        <a target="_blank" href="https://twitter.com/ancapsu" title="Twitter">
                            <img src={twitterImg} ></img>
                        </a>
                    </span>
                    <span className="tda-social-icon-wrap">
                        <a target="_blank" href="https://gab.ai/ancapsu" title="Gab">
                            <img src={gabImg} ></img>
                        </a>
                    </span>
                    
                </div>

            </div>

        );

    }

}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(HeaderTop as any);