// Libs
import React, {Component} from 'react';
import axios from "axios";
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
import { uri } from '../services/Connection';



class Header extends Component{
    constructor(props){
        super(props);
        this.state = {
            listaUsuarios : []
        }
    }

    buscarUsuarios = () => {
        axios('http://localhost:5000/api/usuarios/perfil', {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ listaUsuarios : response.data})
            }
        })

        .catch(erro => {console.log(erro)})
    }
    
    componentDidMount(){
        this.buscarUsuarios();
    }

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
                            {/* <img src={start}  /> */}
                            <div className="header-tag-name">
                                <p>{parseJwt().name}</p>
                            </div>

                            {/* <div className="header-tag-middle"></div> */}
                            {/* <div className="header-tag">
                                <p>{parseJwt().role === '1' ? 'administrador' : '' || parseJwt().role === '2' ? 'médico' : '' || parseJwt().role === '3' ? 'paciente' : ''}</p>
                            </div> */}
                            {/* <img src={end}  /> */}
                            <div className="header-avatar" >
                                {
                                    this.state.listaUsuarios.map(foto => {
                                        return(
                                            <li><a className="header-avatar-img-base"><img src={`${uri}/FotosPerfil/${foto.foto}`} className="header-avatar-img" onClick={menuToggle}  /></a>
                                                <div className="header-avatar-submenu-dropdown">
                                                    <img className="header-ponta" src={ponta} />

                                                    <ul className="header-avatar-submenu">
                                                        <div className="header-submenu-op1">
                                                            {/* <img src={user} aria-hidden="true" /> */}
                                                            <div className="far fa-user-circle"></div>
                                                            <li><Link to="/perfil">Perfil</Link></li>
                                                        </div>
                                                        <div className="header-submenu-op2">
                                                            <div className="fas fa-columns"></div>
                                                            <li><Link to="/dashboard">Painel de Controle</Link></li>
                                                        </div>
                                                        <div className="header-submenu-op3">
                                                            <div className="fas fa-cog" aria></div>
                                                            <li><Link to="/conta/editar">Configurações</Link></li>
                                                        </div>
                                                        <div className="header-submenu-saida">
                                                            <li><Link onClick={logout} to="/">Sair</Link></li>
                                                        </div>
                                                    </ul>
                                                </div>
                                            </li>
                                        )
                                    })
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </header>
        )
    }
}

export default Header;