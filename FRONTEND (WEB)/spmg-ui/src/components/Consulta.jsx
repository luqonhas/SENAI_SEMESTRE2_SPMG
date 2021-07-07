// Libs
import React, {Component} from 'react';
import axios from 'axios';
import {Link} from 'react-router-dom';

// Services
import {parseJwt} from '../services/Auth';

// Components
import Modal from '../components/Modal';

// Imgs
import deletar from '../assets/img/consultas-deletar.svg';
import descricao from '../assets/img/consultas-descricao.svg';
import paciente from '../assets/img/consultas-paciente.svg';
import paciente_start from '../assets/img/consultas-paciente-start.svg';
import medico from '../assets/img/consultas-medico.svg';
import medico_start from '../assets/img/consultas-medico-start.svg';
import editar from '../assets/img/consultas-editar.svg';
import cadastrar from '../assets/img/consultas-cadastrar.svg';
import ordenar from '../assets/img/consultas-ordenar.svg';



class Consulta extends Component{
    constructor(props) {
        super(props);
        this.state = {
            listaConsultas : [],
            listaPacientes : [],
            listaMedicos : [],
            listaSituacoes : [],
            descricao : '',
            idConsultaAlterada : 0,
            dataConsulta : new Date,
            horaConsulta : new Date,
            idMedico : 0,
            idPaciente : 0,
            idSituacao : 0,
            
            isModalOpen: false,
            ordenado : false,
            isLoading : false,
            mensagem : ''
        }
    }



    buscarConsultas = () => {
        // define a URL padrão (método que lista as consultas linkadas com o id do usuário logado)
        let URL = 'http://localhost:5000/api/consultas/minhas';

        // se o usuário logado tiver a permissão '1' (administrador), o método da URL é trocado para o método de listar todas as consultas
        if (parseJwt().role === '1') {
            URL = 'http://localhost:5000/api/consultas/todos';
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



    buscarPacientes = () => {
        if (parseJwt().role === '1') {
            var URL = 'http://localhost:5000/api/pacientes/todos';
        }

        axios(URL, {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ listaPacientes : response.data})
            }
        })

        .catch(erro => {console.log(erro)})
    }



    buscarMedicos = () => {
        if (parseJwt().role === '1') {
            var URL = 'http://localhost:5000/api/medicos/todos';
        }

        axios(URL, {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ listaMedicos : response.data})
            }
        })

        .catch(erro => {console.log(erro)})
    }

    buscarSituacoes = () => {
        if (parseJwt().role === '1') {
            var URL = 'http://localhost:5000/api/situacoes/todos';
        }

        axios(URL, {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 200){
                this.setState({ listaSituacoes : response.data})
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

        .then(response => {
            if(response.status === 204){
                this.setState({ mensagem : 'Descrição atualizada com sucesso!'})
            }
        })

        .catch(erro => {
            this.setState({mensagem : 'Não foi possível atualizar a descrição.'})
        })

        .then(this.buscarConsultas)
    }



    atualizarMensagem = (id) => {
        while(this.state.idConsultaAlterada == id){
            return 'ok'
        }
    }



    atualizaEstado = async (event) => {
        await this.setState({
            [event.target.name] : event.target.value
        })
    }



    cancelaModal = () => {
        this.setState({ isModalOpen : false })
        this.setState({ dataConsulta : new Date })
        this.setState({ horaConsulta : new Date })
        this.setState({ idMedico : 0 })
        this.setState({ idPaciente : 0 })
    }



    excluirConsulta = (idConsulta) => {
        axios.delete('http://localhost:5000/api/consultas/deletar/' + idConsulta, {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .catch(erro => {console.log(erro)})

        .then(this.buscarConsultas)
    }



    cadastrarConsulta = (event) => {
        event.preventDefault();

        var consulta = {
            dataAgendamento : new Date(this.state.dataConsulta),
            horaAgendamento : this.state.horaConsulta,
            idMedico : this.state.idMedico,
            idPaciente : this.state.idPaciente
        }

        axios.post('http://localhost:5000/api/consultas/agendar', consulta, {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 201){
                this.setState({isLoading : false});
                this.setState({isModalOpen : false});
            }
        })

        .catch(erro => {
            this.setState({isLoading : false})
        })

        .then(this.buscarConsultas)

        .then(this.cancelaModal)
    }



    componentDidMount(){
        this.buscarConsultas();
        this.buscarMedicos();
        this.buscarPacientes();
        this.buscarSituacoes();
    }



    render() {
        return(
            <div className="consultas-content-background">
                <div className="consultas-btns">
                    <button onClick={() => {this.setState({ordenado : !this.state.ordenado}); this.buscarConsultas()}}>Ordenar por data <img src={ordenar} draggable="false" /></button>
                    {
                        parseJwt().role === '1' ?
                        <button onClick={() => this.setState({isModalOpen : true})}>+ Nova consulta <img src={cadastrar} draggable="false" /></button>
                        : ''
                    }
                </div>

                <div className="consultas-card-consulta-background">
                    {
                        this.state.ordenado && this.state.listaConsultas.sort((a, b) => b.idConsulta - a.idConsulta),

                        this.state.listaConsultas.map(consulta => {
                            return(
                                <form key={consulta.idConsulta} onSubmit={this.editarDescricao} className="consultas-card-consulta">
                                    {
                                        consulta.idSituacaoNavigation.idSituacao === 3 ?
                                        <div className="consultas-card-situacao-agendada">
                                            <p>{consulta.idSituacaoNavigation.situacao}</p>
                                        </div> : ''
                                    }
                                    {
                                        consulta.idSituacaoNavigation.idSituacao === 1 ?
                                        <div className="consultas-card-situacao-realizada">
                                            <p>{consulta.idSituacaoNavigation.situacao}</p>
                                        </div> : ''
                                    }
                                    {
                                        consulta.idSituacaoNavigation.idSituacao === 2 ?
                                        <div className="consultas-card-situacao-cancelada">
                                            <p>{consulta.idSituacaoNavigation.situacao}</p>
                                        </div> : ''
                                    }

                                    <div className="consultas-card-dia-btns">
                                        <div className="consultas-card-dia">
                                            <div className="consultas-card-data">
                                                <div className="consultas-card-data-background">
                                                    <p>data</p>
                                                </div>
                                                <p>{new Date(consulta.dataConsulta).toLocaleDateString()}</p>
                                            </div>

                                            <div className="consultas-card-hora">
                                                <div className="consultas-card-hora-background">
                                                    <p>hora</p>
                                                </div>
                                                <p>{consulta.horaConsulta}</p>
                                            </div>
                                        </div>

                                        <div key={consulta.idConsulta} className="consultas-card-btn-inicio">
                                            {
                                                parseJwt().role === '1' &&
                                                <button type="button" onClick={() => this.excluirConsulta(consulta.idConsulta)}><img src={deletar} draggable="false" /></button>
                                            }
                                        </div>
                                    </div>

                                    <div className="consultas-card-paciente">
                                        <p>paciente</p>
                                        <div className="consultas-card-paciente-linha">
                                            <img src={paciente} draggable="false" />
                                            <img src={paciente_start} draggable="false" />
                                            <div className="consultas-card-paciente-barra">
                                                <p>{consulta.idPacienteNavigation.nomePaciente}</p>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="consultas-card-medico">
                                        <p>doutor(a) & especialidade</p>
                                        <div className="consultas-card-medico-linha">
                                            <div className="consultas-card-medico-barra">
                                                <p>{consulta.idMedicoNavigation.nomeMedico}</p>
                                            </div>
                                            <img src={medico_start} draggable="false" className="consultas-card-medico-ponta" />
                                            <img src={medico} draggable="false" className="consultas-card-medico-img" />
                                        </div>
                                        <div className="consultas-card-medico-especialidade">
                                            <p>{consulta.idMedicoNavigation.idEspecialidadeNavigation.nomeEspecialidade}</p>
                                        </div>
                                    </div>

                                    <div className="consultas-card-descricao">
                                        <p>descrição</p>
                                        <div key={consulta.idConsulta} className="consultas-card-descricao-linha">
                                            <textarea readOnly={parseJwt().role === '3' ? 'none' : ''} onChange={this.atualizarEstadoDescricao} id={consulta.idConsulta} name="descricao" rows="3" className="consultas-card-descricao-campo">{consulta.descricao}</textarea>
                                            {
                                                
                                                parseJwt().role === '1' ?
                                                <button disabled={this.state.descricao === "" ? "none" : ""} type="submit"><img src={editar} draggable="false" /></button> : ''
                                            }
                                            {
                                                parseJwt().role === '2' ?
                                                <button disabled={this.state.descricao === "" ? "none" : ""} type="submit"><img src={editar} draggable="false" /></button> : ''
                                            }
                                            {
                                                parseJwt().role === '3' ?
                                                <div className="consultas-card-editar-block"></div> : ''
                                            }

                                        </div>
                                        {
                                            this.atualizarMensagem(consulta.idConsulta) === 'ok' &&
                                            <div className="consultas-card-editar-sucesso"><p style={{color: '#36CC9A', fontSize: '.8em'}}>{this.state.mensagem}</p></div>
                                        }
                                    </div>
                                </form>
                            )
                        })
                    }
                </div>
                
                {
                    parseJwt().role === '1' && (
                        <Modal isOpen={this.state.isModalOpen}>
                            <form className="modal-consulta" onSubmit={this.cadastrarConsulta}>
                                <div className="modal-consulta-situacao">
                                    <p>agendada</p>
                                </div>

                                <div className="modal-consulta-dia">
                                    <div className="modal-consulta-dia">
                                        <div className="modal-consulta-data">
                                            <div className="modal-consulta-data-background">
                                                <p>data</p>
                                            </div>
                                            <input 
                                            type="date"
                                            name="dataConsulta"
                                            // min={new Date(DateTime.Now())} 
                                            max="2040-01-01" 
                                            step="1"
                                            onChange={this.atualizaEstado}
                                            value={this.state.dataConsulta}
                                            required
                                            />
                                        </div>

                                        <div className="modal-consulta-hora">
                                            <div className="modal-consulta-hora-background">
                                                <p>hora</p>
                                            </div>
                                            <input 
                                            type="time"
                                            name="horaConsulta"
                                            value="13:00" 
                                            step="900"
                                            onChange={this.atualizaEstado}
                                            value={this.state.horaConsulta}
                                            />
                                        </div>
                                    </div>
                                </div>

                                <div className="modal-consulta-paciente">
                                    <p>paciente</p>
                                    <div className="modal-consulta-paciente-linha">
                                        <img src={paciente} draggable="false" />
                                        <img src={paciente_start} draggable="false" />
                                        <div className="modal-consulta-paciente-barra">
                                            {
                                                <select
                                                    name="idPaciente"
                                                    value={this.state.idPaciente}
                                                    onChange={this.atualizaEstado}
                                                    id="idPaciente"
                                                    required
                                                >
                                                    <option value={0}>Selecione um paciente...</option>
                                                    {
                                                        this.state.listaPacientes.map(nomes => {
                                                            return(
                                                                <option key={nomes.idPaciente} value={nomes.idPaciente}>
                                                                    {nomes.nomePaciente}
                                                                </option>
                                                            )
                                                        })
                                                    }
                                                </select>
                                            }
                                        </div>
                                    </div>
                                </div>

                                <div className="modal-consulta-medico">
                                    {/* <p>doutor(a) & especialidade</p> */}
                                    <p>doutor(a)</p>
                                    <div className="modal-consulta-medico-linha">
                                        <div className="modal-consulta-medico-barra">
                                            {
                                                <select
                                                    name="idMedico"
                                                    value={this.state.idMedico}
                                                    onChange={this.atualizaEstado}
                                                    required
                                                >
                                                    <option value={0}>Selecione um médico...</option>
                                                    {
                                                        this.state.listaMedicos.map(nomes => {
                                                            return(
                                                                <option key={nomes.idMedico} value={nomes.idMedico}>
                                                                    {nomes.nomeMedico}
                                                                </option>
                                                            )
                                                        })
                                                    }
                                                </select>
                                            }
                                        </div>
                                        <img src={medico_start} draggable="false" className="modal-consulta-medico-ponta" />
                                        <img src={medico} draggable="false" className="modal-consulta-medico-img" />
                                    </div>
                                    {/* <div className="modal-consulta-medico-especialidade">
                                        {
                                            this.state.listaConsultas.map(esp => {
                                                return(
                                                    
                                                    <p key={esp.idConsulta}>{esp.idMedicoNavigation.idEspecialidadeNavigation.nomeEspecialidade === '' ? 'aguardando...' : ''}</p>
                                                )
                                            })
                                        }
                                    </div> */}
                                </div>

                                <div className="modal-consulta-btns">
                                    <div className="modal-consulta-cadastrar">
                                        {
                                            this.state.isLoading === true && (<button value="aguarde..." type="submit" disabled>aguarde...</button>)
                                        }
                                        {
                                            this.state.isLoading === false && (<button type="submit" disabled={this.state.dataConsulta === new Date || this.state.horaConsulta === new Date || this.state.idMedico === 0 || this.state.idPaciente === 0 ? "none" : ""}>cadastrar</button>)
                                        }
                                    </div>
                                    <div className="modal-consulta-cancelar">
                                        <button onClick={() => this.cancelaModal()} disabled={this.state.isLoading === true ? 'none' : ''}>cancelar</button>
                                    </div>
                                </div>
                            </form>
                        </Modal>
                    )
                }
            </div>
            
        )
    }
}

export default Consulta;