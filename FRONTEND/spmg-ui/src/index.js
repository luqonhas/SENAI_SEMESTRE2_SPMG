import React from 'react';
import ReactDOM from 'react-dom';
import { Route, BrowserRouter as Router, Switch, Redirect } from 'react-router-dom';
import {parseJwt, userAuthentication} from './services/Auth';

import './assets/css/index.css';

import Login from './pages/Login/Login';
import Consultas from './pages/Consultas/Consultas';
import Dashboard from './pages/Dashboard/Dashboard';
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

const PermissaoAdministrador = ({component : Component}) => (
  <Route 
    render = {props =>
      userAuthentication() && parseJwt().role === "1" ?
      <Component {...props} /> :
      <Redirect to="/" />
    }
  />
)

const Permissao = ({component : Component}) => (
  <Route 
    render = {props =>
      userAuthentication() && parseJwt().role === "1" || "2" || "3" ?
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
        <PermissaoPaciente exact path="/paciente/consultas" component={Consultas} />
        <PermissaoMedico exact path="/medico/consultas" component={Consultas} />
        <PermissaoAdministrador exact path="/administrador/consultas" component={Consultas} />
        <Permissao exact path="/dashboard" component={Dashboard} />
        <Route exact path="/notfound" component={NotFound} />
        <Redirect to="/notfound" />
      </Switch>
    </div>
  </Router>
)

ReactDOM.render(routing, document.getElementById('root'));