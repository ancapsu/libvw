import * as React from 'react';
import { Link, NavLink, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';
import * as AccountStore from '../store/Account';
import * as TargetStore from '../store/Target';
import ErrorMsg from '../message/errormsg';
import Footer from './footer/Footer';
import Header from './header/Header';
import ShowTargetRef from './ShowTargetRef';

const waitGif: string = require('../theme/newspaper/img/wait.gif');

type TargetCategoryProps =
    AccountStore.AccountState
    & TargetStore.TargetState
    & typeof AccountStore.actionCreators
    & typeof TargetStore.actionCreators
    & RouteComponentProps<{ categ: string }>;

class TargetCategory extends React.Component<TargetCategoryProps, {}> {

    contentReady: boolean;
    categ: string;

    constructor(props: TargetCategoryProps, context: any) {

        super(props, context);

        this.contentReady = false;
        this.categ = this.props.match.params.categ;

        this.componentDidMount = this.componentDidMount.bind(this);
        this.componentWillReceiveProps = this.componentWillReceiveProps.bind(this);
        this.LoadMore = this.LoadMore.bind(this);

    }

    componentDidMount() {

        if (!this.props.hasCategory || this.categ != this.props.match.params.categ) {

            this.props.requestTargetByCategory(this.props.match.params.categ, 0, 10);

        } else {

            this.contentReady = true;
            this.forceUpdate();

        }

    }

    componentWillReceiveProps(props: TargetCategoryProps) {

        if (this.categ != props.match.params.categ) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestTargetByCategory(this.props.match.params.categ, 0, 10);
            this.categ = this.props.match.params.categ;

        } else {

            if ((props.hasCategory && props.targets.length != this.props.targets.length) || (!this.props.hasCategory && props.hasCategory)) {

                this.contentReady = true;

            }

        }

    }

    LoadMore() {

        if (this.contentReady) {

            this.contentReady = false;
            this.forceUpdate();

            this.props.requestTargetByCategory(this.props.match.params.categ, this.props.targets.length, 10);

        }

    }

    public render() {

        return (

            <div>

                <Header />

                <div className={(!this.contentReady ? "wait-panel" : "wait-panel-disabled")}>
                    <img src={waitGif} ></img>
                </div>

                <div className="container open-line"></div>

                <div className="tdc-row">
                    <div className="td-business-home-row wpb_row td-pb-row">
                        <div className="vc_column wpb_column vc_column_container tdc-column td-pb-span12">
                            <div className="wpb_wrapper">
                                <div className="td_block_wrap">
                                    <div className="td-block-title-wrap">
                                        <h4 className="td-block-title">
                                            <span className="news-page-headline">{RacMsg.Get(RacMsg.Id.ListOfAgendasWithCategory)} '{this.props.title}'</span>
                                        </h4>
                                    </div>
                                    <div className="td_block_inner td-column-2">
                                        {this.props.description}
                                    </div>
                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>
                                    <div className="td_block_inner td-column-2">

                                        {

                                            this.props.targets.map((n, idx) => {

                                                return (

                                                    <ShowTargetRef index={idx} />

                                                );

                                            })

                                        }

                                    </div>

                                    <div className="container open-line"></div>
                                    <div className="container open-line"></div>

                                    <div className="td_block_inner td-column-2">

                                        {this.props.targets.length < this.props.total &&

                                            <a onClick={this.LoadMore} className="btn-carregar-mais">{RacMsg.Get(RacMsg.Id.LoadMore)}</a>

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
    
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.target } },
    { ...AccountStore.actionCreators, ...TargetStore.actionCreators }
)(TargetCategory as any) as typeof TargetCategory;
