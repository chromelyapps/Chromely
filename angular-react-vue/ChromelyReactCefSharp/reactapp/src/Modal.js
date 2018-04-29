import React from 'react';

class Modal extends React.Component {
  render() {
    // Render nothing if the "show" prop is false
    if(!this.props.show) {
      return null;
    }

    return (
        // -- The Modal RegisterAsyncJsObject Modal --
        <div className="modal fade" id="boundJsObjectModal">
          <div className="modal-dialog modal-lg">
            <div className="modal-content">

              // --  Modal Header --
              <div className="modal-header">
                <h4 className="modal-title">.NET/JavaScript Integration Demo (RegisterAsyncJsObject)</h4>
                <button type="button" className="close" data-dismiss="modal">&times;</button>
              </div>

              // --  Modal body --
              <div className="modal-body">
                // -- Nav pills --
                <ul className="nav nav-pills" role="tablist">
                  <li className="nav-item">
                    <a className="nav-link active" data-toggle="pill" href="#sectionA">Get 1</a>
                  </li>
                  <li className="nav-item">
                    <a className="nav-link" data-toggle="pill" href="#sectionB">Get 2</a>
                  </li>
                  <li className="nav-item">
                    <a className="nav-link" data-toggle="pill" href="#sectionC">Post</a>
                  </li>
                </ul>

                // --  Tab panes --
                <div className="tab-content">
                  <div id="sectionA" className="container tab-pane active">
                    <br/>
                    <div className="row">
                      <div className="col-12">
                        Route Path:&ensp;/democontroller/movies &ensp; (Restful Service in Local Assembly)&ensp;<button type="button" className="btn btn-primary btn-sm" >Run</button>
                      </div>
                      <br/><br/>
                      <div className="col-12">
                        <div className='table-responsive'>
                          <table className='table'>
                            <thead>
                              <tr>
                                <th>Id</th>
                                <th>Title</th>
                                <th>Year</th>
                                <th>Votes</th>
                                <th>Rating</th>
                                <th>Date</th>
                                <th>RestfulAssembly</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                              </tr>
                            </tbody>
                          </table>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div id="sectionB" className="container tab-pane fade">
                    <br/>
                    <div className="row">
                      <div className="col-12">
                        Route Path:&ensp;/externalcontroller/movies &ensp;(Restful Service in External Assembly)&ensp;<button type="button" className="btn btn-primary btn-sm">Run</button>
                      </div>
                      <br/> <br/>
                      <div className="col-12">
                        <div className='table-responsive'>
                          <table className='table'>
                            <thead>
                              <tr>
                                <th>Id</th>
                                <th>Title</th>
                                <th>Year</th>
                                <th>Votes</th>
                                <th>Rating</th>
                                <th>Date</th>
                                <th>RestfulAssembly</th>
                              </tr>
                            </thead>
                            <tbody>
                               <tr>
                              </tr>
                            </tbody>
                          </table>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div id="sectionC" className="container tab-pane fade">
                    <br/>
                    <div className="row">
                      <div className="col-12">
                        Route Path:&ensp;/democontroller/savemovies&ensp;(Restful Service in Local Assembly)&ensp;<button type="button" className="btn btn-primary btn-sm">Run</button>
                      </div>
                      <br/><br/>
                      <div className="col-12">
                        <div>postCefQueryResult </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              // -- Modal footer --
              <div className="modal-footer">
                <button type="button" className="btn btn-primary" data-dismiss="modal">Close</button>
              </div>

            </div>
          </div>
        </div>
    );
  }
}


export default Modal;