import React, {Component} from 'react';
import logo from '../assets/img/menu-logo.svg';
import sair from '../assets/img/menu-sair.svg';
import {logout} from '../services/Auth';

class Header extends Component{
    render(){
        return(
            <header className="paciente-menu-bg">
                <div className="paciente-menu">
                    <div className="paciente-logo">
                        <img src={logo}></img>
                    </div>
                    <nav className="paciente-nav">
                        <ul>
                            <li><a>Bem vindo(a)<br/><span>paciente</span></a></li>
                            <li><a>Consultas</a></li>
                            <li><a>Meus dados</a></li>
                            <li><a href="/" onClick={logout}><img src={sair}/></a></li>
                        </ul>
                    </nav>
                </div>
            </header>
        )
    }
}

export default Header;