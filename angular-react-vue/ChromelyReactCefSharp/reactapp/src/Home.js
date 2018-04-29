import React from 'react'
import { Link } from 'react-router-dom'
import { Container, Row, Col, Label, Media } from 'reactstrap';

const chromelylogo = require('./assets/img/chromely.png');
const reactlogo = require('./assets/img/logo.svg');

const Home = () => (

    <Container>
      <div className="centerBlock">
       <Row>
          <Col>
              <Media object src={chromelylogo} className="img-rounded" alt="Chromely Logo" width="200" height="200"  style={{marginTop: '20px'}} >
              </Media>
          </Col>
          <Col>
             <Media  object src={reactlogo} className="react-logo" alt="react logo" width="240" height="240" >
            </Media>
          </Col>
        </Row>

        <Row className="centerBlock">
          <span className="text-primary text-center"><h2>chromely + reactjs</h2></span>
        </Row>

        <Row className="centerBlock">
           <p className="text-muted text-center">Build .NET/.NET CORE HTML5 Desktop Apps</p>
        </Row>

        <Row className="centerBlock">
            <Label for="info">RegisterAsyncJsObject/Http Demos:</Label>
             <Link to="/demo"><button id="buttonDemoRun"  className="btn btn-primary" style={{margin: '5px'}}>Run</button></Link>
        </Row>

  </div>

</Container>
)

export default Home
