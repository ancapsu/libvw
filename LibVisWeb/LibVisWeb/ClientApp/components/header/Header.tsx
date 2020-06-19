import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import RacMsg from '../../message/racmsg';
import * as AccountStore from '../../store/Account';
import HeaderTop from './HeaderTop';
import HeaderLogo from './HeaderLogo';
import HeaderMenu from './HeaderMenu';

type HeaderProps =
    AccountStore.AccountState
    & typeof AccountStore.actionCreators;

class Header extends React.Component<HeaderProps, {}> {

    //
    //   Inicialização da parte dinamica
    //
    componentWillMount() {

        this.props.validateLoginToken();
                
    }

    public render() {

        return (

            <div className="tda-container-black tda-container-size">
                <div className="tda-container-half-yellow tda-container-size">
                    <div className="td-container td-header-row tda-header-row-yellow tda-container-size">
                        <div className="tda-header-chess-pattern tda-container-size">    
                                                        
                            <HeaderTop />

                            <HeaderLogo />

                            <HeaderMenu />

                        </div>
                    </div>
                </div>
            </div>
            
        );

    }

}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => { return { ...state.account, ...state.toastr } },
    { ...AccountStore.actionCreators }
)(Header as any);

