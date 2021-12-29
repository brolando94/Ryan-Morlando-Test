import React, { Component } from 'react';
import { variables } from './Variables.js';

export class Artist extends Component {
    /*initialize component*/
    constructor(props) {
        super(props);

        this.state = {
            artists: [],
            modalTitle: "",
            Name: "",
            Id: 0
        }

    }
    /*
    refresh with the new data, once its available 
    convert to json and assign to array
    */
    refreshList() {
        fetch(variables.API_URL + 'artist')
            .then(response => response.json()).then(data => {
                this.setState({ artists: data });
            })
    }

    /*mount component to DOM*/
    componentDidMount() {
        this.refreshList();
    }
    /*Update the Arist Name*/
    changeArtistName = (e) => {
        this.setState({ Name: e.target.value });
    }
    /*bring up modal window for add on click*/
    addClick() {
        this.setState({
            modalTitle: "Add Artist",
            Id: 0,
            Name: ""
        });
    }
    /*bring up modal window for edit on click*/
    editClick(art) {
        this.setState({
            modalTitle: "Edit Artist",
            Id: art.ArtistId,
            Name: art.ArtistName
        });
    }
    /*add new into table on create click in modal*/
    createClick() {
        fetch(variables.API_URL + 'artist', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            /*pass new name as json in method body, once successful
            display message and refresh table*/
            body: JSON.stringify({
                Name: this.state.Name
            })
        })
            .then(res => res.json())
            .then((result) => {
                alert(result);
                this.refreshList();
            }, (error) => {
                alert('failed');
            })
    }

    /*Update record on update click in modal*/
    updateClick() {
        fetch(variables.API_URL + 'artist', {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            /*pass new name as json in method body, once successful
            display message and refresh table*/
            body: JSON.stringify({
                Id: this.state.Id,
                Name: this.state.Name
            })
        })
            .then(res => res.json())
            .then((result) => {
                alert(result);
                this.refreshList();
            }, (error) => {
                alert('failed');
            })
    }

    /*delete record on delete click in modal after user confirmation*/
    deleteClick(Id) {
        if(window.confirm('Are you Sure?'))
        fetch(variables.API_URL + 'artist/'+Id, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
            
        })
            .then(res => res.json())
            .then((result) => {
                alert(result);
                this.refreshList();
            }, (error) => {
                alert('failed');
            })
    }

    render() {
        /*delcare state variable inside render to be used in the html*/
        const {
            artists,
            modalTitle,
            Id,
            Name
        } = this.state;
        return (
            <div>
                {/* Create button to open up modal window to add new */}
                <button type="button"
                    className="btn btn-primary m-2 float-end"
                    data-bs-toggle="modal"
                    data-bs-target="#updateModal"
                    onClick={() => this.addClick()}>
                    Add Artist
                </button>
                {/*create table*/}
                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>
                                ArtistId
                            </th>
                            <th>
                                ArtistName
                            </th>
                            <th>
                                Options
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {/*put database info info into table*/}
                        {artists.map(art =>
                            <tr key={art.ArtistId}>
                                <td>{art.ArtistId}</td>
                                <td>{art.ArtistName}</td>
                                <td>
                                    {/*edit button*/}
                                    <button type="button"
                                        className="btn btn-light mr-1"
                                        data-bs-toggle="modal"
                                        data-bs-target="#updateModal"
                                        onClick={() => this.editClick(art)}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                                            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                                            <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                                        </svg>
                                    </button>
                                    {/*delete button*/}
                                    <button type="button"
                                        className="btn btn-light mr-1"
                                        onClick={()=>this.deleteClick(art.ArtistId)}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash" viewBox="0 0 16 16">
                                            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" />
                                            <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" />
                                        </svg>
                                    </button>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
                {/* Create modal window to access the update and add features */}
                <div className="modal fade" id="updateModal" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{modalTitle}</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close">
                                </button>
                            </div>

                            <div className="Modal-body">
                                <div className="input-group mb-3">
                                    <span className="input-group-text">ArtistName</span>
                                    <input type="text" className="form-control"
                                        value={Name}
                                        onChange={this.changeArtistName} />
                                </div>
                                {/* Create new*/}
                                {Id === 0 ?
                                    <button type="button"
                                        className="btn btn-primary float-start"
                                        onClick={() => this.createClick()}
                                    >Create</button> : null}

                                {/* Update*/}
                                {Id !== 0 ?
                                    <button type="button"
                                        className="btn btn-primary float-start"
                                        onClick={() => this.updateClick()}
                                        >Update</button> : null}
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        )
    }
}