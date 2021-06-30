// Libs
import React, {Component} from 'react';
import {Link} from 'react-router-dom';

// Imgs
import background from '../assets/img/header-background.svg';
import logo from '../assets/img/header-logo.svg';
import start from '../assets/img/header-tag-start.svg';
import end from '../assets/img/header-tag-role-end.svg';
import avatar from '../assets/img/header-avatar-base.png';
import ponta from '../assets/img/header-submenu-sharp.svg';

// Services
import {logout, parseJwt} from '../services/Auth';
import { menuToggle } from '../services/Toggle';



class Header extends Component{

    render(){
        return(
            <header className="header">
                <div className="header-background">
                    <img src={background} className="header-poly" draggable="false"/>
                    <div className="header-content">

                        <div className="header-logo">
                            <img src={logo} draggable="false" />
                        </div>

                        <div className="header-nav">
                            <img src={start}  />
                            {/* <div className="header-tag-name">
                                <p>Bem vindo(a)</p>
                            </div> */}

                            {/* <div className="header-tag-middle"></div> */}
                            <div className="header-tag">
                                <p>{parseJwt().role === '1' ? 'administrador' : '' || parseJwt().role === '2' ? 'médico' : '' || parseJwt().role === '3' ? 'paciente' : ''}</p>
                            </div>
                            <img src={end}  />
                            <div className="header-avatar" >
                                <li><a><img src={avatar} className="header-avatar-img" onClick={menuToggle}  /></a>
                                    <div className="header-avatar-submenu-dropdown">
                                        <img className="header-ponta" src={ponta} />

                                        <ul className="header-avatar-submenu">
                                            <div className="header-submenu-op1">
                                                {/* <img src={user} aria-hidden="true" /> */}
                                                <div className="far fa-user-circle"></div>
                                                <li><a href="#">Perfil</a></li>
                                            </div>
                                            <div className="header-submenu-op2">
                                                <div className="fas fa-columns"></div>
                                                <li><Link to="/dashboard">Painel de Controle</Link></li>
                                            </div>
                                            <div className="header-submenu-op3">
                                                <div className="fas fa-cog" aria></div>
                                                <li><a href="#">Configurações</a></li>
                                            </div>
                                            <div className="header-submenu-saida">
                                                <li><Link onClick={logout} to="/">Sair</Link></li>
                                            </div>
                                        </ul>
                                    </div>
                                </li>
                            </div>
                        </div>
                    </div>
                </div>
            </header>
        )
    }
}

export default Header;