import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import RacMsg from '../message/racmsg';

export default class Layout extends React.Component<{}, {}> {

    public render() {

        return (

            <div>                
                {this.props.children}                
            </div>

        );

    }

}
