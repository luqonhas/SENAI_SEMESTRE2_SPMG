import React, { Component } from "react";
import axios from "axios";
import '../../assets/css/styles.css';
import Header from '../../components/header/header';

// import logo from '../../assets/img/menu-logo.svg';
// import sair from '../../assets/img/menu-sair.svg';
import agendadas from '../../assets/img/botao-agendadas.svg';
import realizadas from '../../assets/img/botao-realizadas.svg';
import canceladas from '../../assets/img/botao-canceladas.svg';
import medico from '../../assets/img/medico-consulta.svg';

// function DataFormatada(){
//     return new Intl.DateTimeFormat('pt-br', {day: 'numeric', month: 'numeric', year: 'numeric'}).format()
// }

class PacienteConsultas extends Component{
    constructor(props) {
        super(props);
        this.state = {
            listaConsultas : []
        }
    }

    buscarConsultas = () => {
        axios("http://localhost:5000/api/consultas", {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('usuario-login')
            }
        })
        .then(resposta => {
            if (resposta.status === 200) {
                this.setState({listaConsultas : resposta.data, })
            }
            console.log(this.state.listaConsultas)
        })
        .catch(erro => console.log(erro));
    }

    componentDidMount(){
        this.buscarConsultas();
    }



    render(){
        return(
            <main>
                <Header nome={this.state.nomePaciente} logout={this.logout} />

                <section className="consultas-bg">
                    <div className="consultas">
                        <div className="paciente-botoes-consulta">
                            <button><img src={agendadas}></img></button>
                            <button><img src={realizadas}></img></button>
                            <button><img src={canceladas}></img></button>
                        </div>

                        {
                            this.state.listaConsultas.map(consulta => {
                                return(
                                    <div key={consulta.idConsulta} className="paciente-consultas-bg">
                                        <div className="paciente-consultas">
                                            <div className="paciente-tipo-consulta">
                                                <p>{consulta.idSituacaoNavigation.situacao}</p>
                                            </div>

                                            <div className="paciente-info-consulta">
                                                <div className="paciente-nome-data-consulta">
                                                    <img src={medico} />
                                                    <div className="paciente-texto">
                                                        <div className="paciente-texto-top">
                                                            <p><strong>Paciente:</strong> {consulta.idPacienteNavigation.nomePaciente}</p>
                                                            <p><strong>Médico:</strong> {consulta.idMedicoNavigation.nomeMedico}</p>
                                                        </div>
                                                        <div className="paciente-texto-bottom">
                                                            <p>{new Date(consulta.dataConsulta).toLocaleDateString()}</p>
                                                            <p><strong>Especialidade:</strong> {consulta.idMedicoNavigation.idEspecialidadeNavigation.nomeEspecialidade}</p>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div className="paciente-hora-consulta">
                                                    <p>{consulta.horaConsulta}</p>
                                                    <p>horas</p>
                                                </div>
                                            </div>

                                            <hr className="paciente-linha-divisao" />

                                            <div className="paciente-desc-consulta">
                                                <h2>DESCRIÇÃO</h2>
                                                <div className="paciente-text-button">
                                                    <textarea readOnly>{consulta.descricao}</textarea>
                                                    <button>EDITAR</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                )
                            })
                        }

                    </div>
                </section>
            </main>
        )
    }
}

export default PacienteConsultas;