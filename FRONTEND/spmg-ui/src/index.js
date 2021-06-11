import React from 'react';
import ReactDOM from 'react-dom';
import { Route, BrowserRouter as Router, Switch, Redirect } from 'react-router-dom';
import {parseJwt, usuarioAutenticado} from './services/auth/auth';

// CSS:
import './assets/css/index.css';

// PÁGINAS:
import App from './pages/home/App';
import NotFound from './pages/notFound/notFound';
import Login from './pages/login/login';
import PacienteConsultas from './pages/consultas/pacienteConsultas';
// import MedicoConsultas from './pages/consultas/medicoConsultas';

import reportWebVitals from './reportWebVitals';



const PermissaoPaciente = ({component : Component}) => (
  <Route 
    render = {props =>
      usuarioAutenticado() && parseJwt().role === "3" ?
      <Component {...props} /> :
      <Redirect to="/login" />
    }
  />
)

// const PermissaoMedico = ({component : Component}) => (
//   <Route 
//     render = {props =>
//       usuarioAutenticado() && parseJwt().role === "2" ?
//       <Component {...props} /> :
//       <Redirect to="/login" />
//     }
//   />
// )



const routing = (
  <Router>
    <div>
      <Switch>
        <Route exact path="/" component={App} /> {/* Home */}
        <Route path="/login" component={Login} /> {/* Login */}
        <PermissaoPaciente path="/paciente/consultas" component={PacienteConsultas} /> {/* Login */}
        {/* <PermissaoMedico path="/medico/consultas" component={MedicoConsultas} /> */}
        <Route exact path="/notfound" component={NotFound} /> {/* Not Found */}
        <Redirect to="/notfound" /> {/* Redireciona para NotFound caso não encontre nenhuma rota */}
      </Switch>
    </div>
  </Router>
)

ReactDOM.render(routing, document.getElementById('root'));

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
