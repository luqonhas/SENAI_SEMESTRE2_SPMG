// Libs
import React, {Component} from 'react';
import axios from 'axios';
import {Link} from 'react-router-dom';

// Services
import {parseJwt} from '../services/Auth';

// Imgs
import medico from '../assets/img/medico-consulta.svg';

class Consulta extends Component{
    constructor(props) {
        super(props);
        this.state = {
            listaConsultas : [],
            descricao : '',
            idConsultaAlterada : 0,
            mensagem : ''
        }
    }



    buscarConsultas = () => {
        // define a URL padrão (método que lista as consultas linkadas com o id do usuário logado)
        let URL = 'http://localhost:5000/api/consultas/minhas';

        // se o usuário logado tiver a permissão '1' (administrador), o método da URL é trocado para o método de listar todas as consultas
        if (parseJwt().role === '1') {
            URL = 'http://localhost:5000/api/consultas/todas';
        }

        axios(URL, {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ listaConsultas : response.data})
            }
        })

        .catch(erro => {console.log(erro)})
    }



    atualizarEstadoDescricao = async (event) => {
        await this.setState({
            [event.target.name] : event.target.value, idConsultaAlterada : event.target.id
        })

        this.limparCampos()
    }



    limparCampos = () => {
        this.setState({ mensagem : '' })
    }



    editarDescricao = (event) => {
        event.preventDefault();

        this.setState({mensagem : ''})
        
        axios.patch('http://localhost:5000/api/consultas/descricao/' + this.state.idConsultaAlterada, {
            descricao : this.state.descricao
        },     
        {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .catch(erro => {console.log(erro)})

        .then(this.buscarConsultas)
    }



    componentDidMount(){
        this.buscarConsultas();
    }



    render() {
        return(
            <main>
                {
                    this.state.listaConsultas.map(consulta => {
                        return(
                            <form key={consulta.idConsulta} onSubmit={this.editarDescricao} className="paciente-consultas-bg">
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
                                            <textarea readOnly={parseJwt().role === '3' ? 'none' : ''} onChange={this.atualizarEstadoDescricao} id={consulta.idConsulta} name="descricao" rows="3">{consulta.descricao}</textarea>
                                            {
                                                parseJwt().role === '2' ?
                                                <button type="submit">EDITAR</button> :
                                                ''
                                            }
                                            {/* <button>EDITAR</button> */}
                                        </div>
                                    </div>
                                </div>
                            </form>
                        )
                    })
                }
            </main>
        )
    }
}

export default Consulta;