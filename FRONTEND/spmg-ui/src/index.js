import React from 'react';
import ReactDOM from 'react-dom';
import { Route, BrowserRouter as Router, Switch, Redirect } from 'react-router-dom';
import {parseJwt, userAuthentication} from './services/Auth';

import './assets/css/index.css';

import Login from './pages/Login/Login';
import ConsultasComum from './pages/Comum/ConsultasComum';
import NotFound from './pages/NotFound/NotFound';

const PermissaoPaciente = ({component : Component}) => (
  <Route 
    render = {props =>
      userAuthentication() && parseJwt().role === "3" ?
      <Component {...props} /> :
      <Redirect to="/" />
    }
  />
)

const PermissaoMedico = ({component : Component}) => (
  <Route 
    render = {props =>
      userAuthentication() && parseJwt().role === "2" ?
      <Component {...props} /> :
      <Redirect to="/" />
    }
  />
)

const routing = (
  <Router>
    <div>
      <Switch>
        <Route exact path="/" component={Login} />
        <PermissaoPaciente exact path="/paciente/consultas" component={ConsultasComum} />
        <PermissaoMedico exact path="/medico/consultas" component={ConsultasComum} />
        <Route exact path="/notfound" component={NotFound} />
        <Redirect to="/notfound" />
      </Switch>
    </div>
  </Router>
)

ReactDOM.render(routing, document.getElementById('root'));