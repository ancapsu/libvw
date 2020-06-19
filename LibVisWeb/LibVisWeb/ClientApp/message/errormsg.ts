import * as React from 'react';
import * as Toastr from 'react-redux-toastr'
import ReduxToastr from 'react-redux-toastr';
import { ApplicationState } from 'ClientApp/store';
import { connect } from 'react-redux';

type ErrorMsgProps =
    Toastr.ToastrState
    & typeof Toastr.actions;

class ErrorMsg extends React.Component<ErrorMsgProps, {}> {

    public render() {

        var toastrFactory = React.createFactory(ReduxToastr);
        return toastrFactory({
            timeOut: 5000,
            newestOnTop: true
        });

    }

}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.toastr,  
    {}                                          
)(ErrorMsg as any);
