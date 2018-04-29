import React, { Component } from 'react';
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom';
import Home from './Home';
import Demo from './Demo';

class App extends Component {
  render() {
    return (
    <Router>
       <Switch>
          <Route exact path='/' component={Home} />
          <Route path='/demo' component={Demo} />
          <Route path='*' component={Home} />
        </Switch>
      </Router>
    );
  }
}

export default App;