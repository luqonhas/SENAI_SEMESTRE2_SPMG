// Libs
import React, {Component} from 'react';
import axios from "axios";
import {Link} from 'react-router-dom';

// Components
import Header from '../../components/Header';

// Services
import {parseJwt} from '../../services/Auth';

// Imgs
import consultas from '../../assets/img/dashboard-consultas.svg';
import medicos from '../../assets/img/dashboard-medicos.svg';
import pacientes from '../../assets/img/dashboard-pacientes.svg';
import especialidades from '../../assets/img/dashboard-especialidades.svg';
import clinicas from '../../assets/img/dashboard-clinicas.svg';
import usuarios from '../../assets/img/dashboard-usuarios.svg';



const RoleLinkConsultas = () => {
    var startURL = parseJwt().role === '1' ? '/administrador/' : '' || parseJwt().role === '2' ? '/medico/' : '' || parseJwt().role === '3' ? '/paciente/' : ''
    return startURL + 'consultas';
}

class Dashboard extends Component{
    constructor(props){
        super(props);
        this.state = {
            qntdConsultas : 0,
            qntdMedicos : 0,
            qntdPacientes : 0,
            qntdEspecialidades : 0,
            qntdClinicas : 0,
            qntdUsuarios : 0
        }
    }

    contarConsultas = () => {
        let URL = 'http://localhost:5000/api/consultas/minhas';

        if (parseJwt().role === '1') {
            URL = 'http://localhost:5000/api/consultas/todos';
        }

        axios(URL, {
            headers: {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ qntdConsultas : response.data.length})
            }
        })

        .then(() => console.log('aqui '+ this.state.qntdConsultas))

        .catch(erro => console.log(erro));
    }

    contarMedicos = () => {
        let URL = 'http://localhost:5000/api/medicos/todos';

        axios(URL, {
            headers: {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ qntdMedicos : response.data.length})
            }
        })

        .then(() => console.log('aqui '+ this.state.qntdMedicos))

        .catch(erro => console.log(erro));
    }

    contarPacientes = () => {
        let URL = 'http://localhost:5000/api/pacientes/todos';

        axios(URL, {
            headers: {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ qntdPacientes : response.data.length})
            }
        })

        .then(() => console.log('aqui '+ this.state.qntdPacientes))

        .catch(erro => console.log(erro));
    }

    contarEspecialidades = () => {
        let URL = 'http://localhost:5000/api/especialidades/todos';

        axios(URL, {
            headers: {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ qntdEspecialidades : response.data.length})
            }
        })

        .then(() => console.log('aqui '+ this.state.qntdEspecialidades))

        .catch(erro => console.log(erro));
    }

    contarClinicas = () => {
        let URL = 'http://localhost:5000/api/clinicas/todos';

        axios(URL, {
            headers: {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ qntdClinicas : response.data.length})
            }
        })

        .then(() => console.log('aqui '+ this.state.qntdClinicas))

        .catch(erro => console.log(erro));
    }

    contarUsuarios = () => {
        let URL = 'http://localhost:5000/api/usuarios/todos';

        axios(URL, {
            headers: {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ qntdUsuarios : response.data.length})
            }
        })

        .then(() => console.log('aqui '+ this.state.qntdUsuarios))

        .catch(erro => console.log(erro));
    }

    componentDidMount(){
        this.contarConsultas();
        this.contarMedicos();
        this.contarPacientes();
        this.contarEspecialidades();
        this.contarClinicas();
        this.contarUsuarios();
    }

    render() {
        return(
            <main>
                <Header />

                <div className="dash-background">
                    <div className="dash-titulo">
                        <h1>Painel de Controle</h1>
                    </div>
                    
                    <div className="dash-cards-background">
                        {/* CONSULTAS */}
                        <div className="dash-card-consultas">
                            <Link to={RoleLinkConsultas()} className="dash-consultas-link">
                                <div className="dash-consultas-texto">
                                    <p>{this.state.qntdConsultas}</p>
                                    <p>consultas</p>
                                </div>
                                <div className="dash-consultas-img">
                                    <img draggable="false" src={consultas} />
                                </div>
                            </Link>
                        </div>

                        {
                                    parseJwt().role === '1' ?
                                    // MÉDICOS
                                    <div className="dash-card-medicos">
                                        <Link to={RoleLinkConsultas()} className="dash-medicos-link">
                                            <div className="dash-medicos-texto">
                                                <p>{this.state.qntdMedicos}</p>
                                                <p>médicos</p>
                                            </div>
                                            <div className="dash-medicos-img">
                                                <img draggable="false" src={medicos} />
                                            </div>
                                        </Link>
                                    </div> : ''
                        }
                        
                        {
                            parseJwt().role === '1' ?
                            // PACIENTES
                            <div className="dash-card-pacientes">
                                <Link to={RoleLinkConsultas()} className="dash-pacientes-link">
                                    <div className="dash-pacientes-texto">
                                        <p>{this.state.qntdPacientes}</p>
                                        <p>pacientes</p>
                                    </div>
                                    <div className="dash-pacientes-img">
                                        <img draggable="false" src={pacientes} />
                                    </div>
                                </Link>
                            </div> : ''
                        }

                        {
                            parseJwt().role === '1' ?
                            // ESPECIALIDADES
                            <div className="dash-card-especialidades">
                                <Link to={RoleLinkConsultas()} className="dash-especialidades-link">
                                    <div className="dash-especialidades-texto">
                                        <p>{this.state.qntdEspecialidades}</p>
                                        <p>especialidades</p>
                                    </div>
                                    <div className="dash-especialidades-img">
                                        <img draggable="false" src={especialidades} />
                                    </div>
                                </Link>
                            </div> : ''
                        }

                        {
                            parseJwt().role === '1' ?
                            // CLÍNICAS
                            <div className="dash-card-clinicas">
                                <Link to={RoleLinkConsultas()} className="dash-clinicas-link">
                                    <div className="dash-clinicas-texto">
                                        <p>{this.state.qntdClinicas}</p>
                                        <p>clínicas</p>
                                    </div>
                                    <div className="dash-clinicas-img">
                                        <img draggable="false" src={clinicas} />
                                    </div>
                                </Link>
                            </div> : ''
                        }

                        {
                            parseJwt().role === '1' ?
                            // USUÁRIOS
                            <div className="dash-card-usuarios">
                                <Link to={RoleLinkConsultas()} className="dash-usuarios-link">
                                    <div className="dash-usuarios-texto">
                                        <p>{this.state.qntdUsuarios}</p>
                                        <p>usuários</p>
                                    </div>
                                    <div className="dash-usuarios-img">
                                        <img draggable="false" src={usuarios} />
                                    </div>
                                </Link>
                            </div> : ''
                        }

                    </div>

                </div>
            </main>
        )
    }
}

export default Dashboard;