import React from 'react'
import { Link } from 'react-router-dom'
import ReactDOM from 'react-dom';
import { Container, Row, Col, Card, CardHeader, CardBody, CardText, Button, Label, Media } from 'reactstrap';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Nav, NavItem, NavLink, TabContent, TabPane } from 'reactstrap';
import { Table } from 'reactstrap';
import classnames from 'classnames';

const chromelylogo = require('./assets/img/chromely.png');
const reactlogo = require('./assets/img/logo.svg');

class Demo extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      info: { objective: 'Chromely Main Objectives', platform: 'Platforms', version: 'Version' },
      modal: false,
      activeTab: 'get'
    };

    this.toggleModal = this.toggleModal.bind(this);
  }

  toggleModal() {
    this.setState({
      modal: !this.state.modal
    });
  }

  setTabActive(tab) {
    if (this.state.activeTab !== tab) {
      this.setState({
        activeTab: tab
      });
    }
  }

  render() {
    const { info } = this.state;

    return (
      <Container>
          <div className="centerBlock">
          <Row>
              <Col>
                  <Media object src={chromelylogo} className="img-rounded" alt="Chromely Logo" width="120" height="120"  style={{marginTop: '20px'}} >
                  </Media>
              </Col>
              <Col>
                <Media  object src={reactlogo} className="react-logo" alt="react logo" width="120" height="120" >
                </Media>
              </Col>
            </Row>

            <Row className="centerBlock">
              <span className="text-primary text-center"><h2>demo panel</h2></span>
            </Row>

            <br />

            <Row>
              <Link to="/"><button className="btn btn-primary" style={{margin: "#5px"}}>Back</button></Link>
            </Row>

            <br />

            <Row className="centerBlock">
              <Button type="button" className="btn btn-light" onClick={this.toggleModal} style={{margin: "#5px"}}>RegisterAsyncJsObject Demo</Button>
              <a href="https://github.com/mattkol/Chromely" className="btn btn-default" role="button" style={{margin: "#5px"}}>more info</a>
            </Row>

            <Row className="centerBlock">
              <Card>
                <CardHeader className="card-header card bg-primary text-white">Chromely Main objective</CardHeader>
                <CardBody>
                  <CardText>{ info.objective }</CardText>
                </CardBody>
              </Card>

              <Card>
                <CardHeader className="card-header card bg-primary text-white">Platforms</CardHeader>
                <CardBody>
                  <CardText>{ info.platform }</CardText>
                </CardBody>
              </Card>

              <Card>
                <CardHeader className="card-header card bg-primary text-white">Current CefSharp/Chromium Version</CardHeader>
                <CardBody>
                  <CardText>{ info.version }</CardText>
                </CardBody>
              </Card>

            </Row>
      </div>

      <div>
        <Modal className="modal-lg" isOpen={this.state.modal} toggle={this.toggleModal}>
          <ModalHeader>.NET/JavaScript Integration (RegisterAsyncJsObject) Demo</ModalHeader>
          <ModalBody>
            <div>
            <Nav pills>
              <NavItem>
                <NavLink className={classnames({ active: this.state.activeTab === 'get1' })}
                 onClick={() => { this.setTabActive('get1'); }}>Get1</NavLink>
              </NavItem>
              <NavItem>
                <NavLink className={classnames({ active: this.state.activeTab === 'get2' })}
                  onClick={() => { this.setTabActive('get2'); }}>Get2</NavLink>
              </NavItem>
              <NavItem>
                <NavLink className={classnames({ active: this.state.activeTab === 'post' })}
                  onClick={() => { this.setTabActive('post'); }}>Post</NavLink>
              </NavItem>
            </Nav>

            <TabContent activeTab={this.state.activeTab}>
              <TabPane tabId="get1">
              <div>
                    <Row style={{margin: "#5px"}}>
                       &ensp;&ensp;&ensp;Route Path:&ensp;/democontroller/movies &ensp; (Restful Service in Local Assembly)&ensp;<button id="buttonBoundObjectRun1" type="button" class="btn btn-primary btn-sm" onclick="boundObjectRun1()">Run</button>
                    </Row>
                    <br/><br/>
                    <Row>
                      <Table>
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
                        <tbody></tbody>
                      </Table>
                    </Row>
                </div>
              </TabPane>
              <TabPane tabId="get2">
                 <Row style={{margin: "#5px"}}>
                    &ensp;&ensp;&ensp;Route Path:&ensp;/democontroller/movies &ensp; (Restful Service in Local Assembly)&ensp;<button id="buttonBoundObjectRun1" type="button" class="btn btn-primary btn-sm" onclick="boundObjectRun1()">Run</button>
                    </Row>
                    <br/><br/>
                    <Row>
                      <Table>
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
                        <tbody></tbody>
                      </Table>
                    </Row>
              </TabPane>
              <TabPane tabId="post">
                 <Row>
                  &ensp;&ensp;&ensp;Route Path:&ensp;/democontroller/savemovies&ensp;(Restful Service in Local Assembly)&ensp;<button id="buttonBoundObjectRun3" type="button" class="btn btn-primary btn-sm" onclick="boundObjectRun3()">Run</button>
                </Row>
                <br/><br/>
                <Row>
                  <div id="boundObjectResult3"></div>
                </Row>
              </TabPane>
            </TabContent>

          </div>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={this.toggleModal}>Close</Button>
          </ModalFooter>
        </Modal>
      </div>

    </Container>
    )
  }

}

export default Demo
