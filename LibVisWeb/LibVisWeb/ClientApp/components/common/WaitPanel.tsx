import * as React from 'react';

const waitGif: string = require('../../theme/newspaper/img/wait.gif');

type WaitPanelProps ={
    isContentReady: Boolean;
}

export default (props: WaitPanelProps) => (
    <div className={(!props.isContentReady ? "wait-panel" : "wait-panel-disabled")}>
        <img src={waitGif} ></img>
    </div>
);
