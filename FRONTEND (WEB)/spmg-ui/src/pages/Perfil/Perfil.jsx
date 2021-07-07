// Libs
import React, {Component} from 'react';
import axios from "axios";
import {Link} from 'react-router-dom';

// Services
import {parseJwt} from '../../services/Auth';

// Components
import Header from '../../components/Header';

// Styles
import '../../assets/css/styles.css';

// Imgs
import profilepic from '../../assets/img/perfil-profile-pic-teste.png'
import emblema from '../../assets/img/emblema-teste.png'


function formataCPF(cpf){
    // retira os caracteres indesejados
    cpf = cpf.replace(/[^\d]/g, "");

    // realizar a formatação
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
}

function formataRG(rg){
    // retira os caracteres indesejados
    rg = rg.replace(/[^\d]/g, "");

    // realiza a formatação
    return rg.replace(/(\d{2})(\d{3})(\d{3})(\d{1})/, "$1.$2.$3-$4");
}

function formataTelefone(fone) {
    let response = fone.replace(/\D/g, "");
    response = response.replace(/^0/, "");

    if (response.length > 11) {
        response = response.replace(/^(\d\d)(\d{5})(\d{4}).*/, "($1) $2-$3");
    } else if (response.length > 7) {
        response = response.replace(/^(\d\d)(\d{5})(\d{0,4}).*/, "($1) $2-$3");
    } else if (response.length > 2) {
        response = response.replace(/^(\d\d)(\d{0,5})/, "($1) $2");
    } else if (fone.trim() !== "") {
        response = response.replace(/^(\d*)/, "($1");
    }
    return response;
}

function formataCRM(crm){
    // retira os caracteres indesejados
    crm = crm.replace(/[^\d]/g, "");

    // realizar a formatação
    return crm.replace(/(\d{2})(\d{3})(\d{0})/, "$1.$2/$3");
}

class Perfil extends Component{
    constructor(props){
        super(props);
        this.state = {
            listaPacientes : [],
            listaMedicos : []
        }
    }

    buscarPacientes = () => {
        axios('http://localhost:5000/api/pacientes/perfil', {
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
        axios('http://localhost:5000/api/medicos/perfil', {
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

    componentDidMount(){
        this.buscarPacientes();
        this.buscarMedicos();
    }

    render(){
        return(
            <>
                <Header />
                
                <div className="perfil-background">
                    <div className="perfil-titulo">
                        <h1>Perfil</h1>
                    </div>

                    <div className="perfil-content-background">
                        <div className="perfil-card-main">
                            <div className="perfil-card-main-foto">
                                <img draggable="false" src={profilepic} />
                            </div>

                            <div className="perfil-card-main-bottom-foto">
                                <div className="perfil-card-main-text">
                                    <p>{parseJwt().name}</p>
                                    {
                                        parseJwt().role !== '1' ? <p>{parseJwt().role !== '2' ? 'Paciente' : 'Médico'}</p> : ''
                                    }
                                    
                                </div>

                                <div className="perfil-card-main-btn">
                                    <Link to="/conta/editar">Editar perfil</Link>
                                </div>
                            </div>
                        </div>

                        <div className="perfil-card-info">
                            <div className="perfil-card-info-title">
                                <p>Suas informações pessoais</p>
                            </div>
                            
                            
                            {/* ADM */}
                            {
                                parseJwt().role === '1' ? 
                                <div className="perfil-card-info-inputs">
                                    <div className="perfil-card-info-inputs-email">
                                        <p className="perfil-card-info-inputs-title">E-mail</p>
                                        <input value={parseJwt().email} readOnly disabled type="text" />
                                    </div>
                                </div> : ''
                            }

                            
                            {/* MÉDICO */}
                            {
                                this.state.listaMedicos.map(medico => {
                                    return(
                                        <>
                                            {
                                                parseJwt().role === '2' ?
                                                <div key={medico.idMedico} className="perfil-card-info-inputs">
                                                    <div className="perfil-card-info-inputs-crm">
                                                        <p className="perfil-card-info-inputs-title">CRM</p>
                                                        <input value={formataCRM(medico.crm)+'SP'} readOnly disabled type="text" />
                                                    </div>

                                                    <div className="perfil-card-info-inputs-clinica">
                                                        <p className="perfil-card-info-inputs-title">Clínica</p>
                                                        <input value={medico.idClinicaNavigation.nomeFantasia} readOnly disabled type="text" />
                                                    </div>

                                                    <div className="perfil-card-info-inputs-especialidade">
                                                        <p className="perfil-card-info-inputs-title">Especialidade</p>
                                                        <input value={medico.idEspecialidadeNavigation.nomeEspecialidade} readOnly disabled type="text" />
                                                    </div>
                                                    
                                                    <div className="perfil-card-info-inputs-email">
                                                        <p className="perfil-card-info-inputs-title">E-mail</p>
                                                        <input value={parseJwt().email} readOnly disabled type="text" />
                                                    </div>
                                                </div> : ''
                                            }
                                        </>
                                    )
                                })
                            }

                            {/* PACIENTE */}
                            {
                                this.state.listaPacientes.map(paciente => {
                                    return(
                                        <>
                                            {
                                                parseJwt().role === '3' ? 
                                                <div className="perfil-card-info-inputs">
                                                    <div className="perfil-card-info-inputs-rg">
                                                        <p className="perfil-card-info-inputs-title">RG</p>
                                                        <input value={formataRG(paciente.rg)} readOnly disabled type="text" />
                                                    </div>

                                                    <div className="perfil-card-info-inputs-cpf">
                                                        <p className="perfil-card-info-inputs-title">CPF</p>
                                                        <input value={formataCPF(paciente.cpf)} readOnly disabled type="text" />
                                                    </div>

                                                    <div className="perfil-card-info-inputs-telefone">
                                                        <p className="perfil-card-info-inputs-title">Telefone</p>
                                                        <input value={formataTelefone(paciente.telefonePaciente)} readOnly disabled type="text" />
                                                    </div>
                                                    
                                                    <div className="perfil-card-info-inputs-data">
                                                        <p className="perfil-card-info-inputs-title">Data de nascimento</p>
                                                        <input value={new Date(paciente.dataNascimento).toLocaleDateString()} readOnly disabled type="text" />
                                                    </div>

                                                    <div className="perfil-card-info-inputs-endereco">
                                                        <p className="perfil-card-info-inputs-title">Endereço</p>
                                                        <input value={paciente.endereco} readOnly disabled type="text" />
                                                    </div>

                                                    <div className="perfil-card-info-inputs-email">
                                                        <p className="perfil-card-info-inputs-title">E-mail</p>
                                                        <input value={parseJwt().email} readOnly disabled type="text" />
                                                    </div>
                                                </div> : ''
                                            }
                                        </>
                                    )
                                })
                            }
                        </div>

                        <div className="perfil-card-emblemas">
                            <div className="perfil-card-emblemas-title">
                                <p>Emblemas</p>
                            </div>
                            
                            <div className="perfil-card-emblemas-emblema-background">
                                {/* CARD EMBLEMA */}
                                <div className="perfil-card-emblemas-emblema">
                                    <div className="perfil-card-emblemas-emblema-base-img">
                                        <div className="perfil-card-emblemas-emblema-img">
                                            <img draggable="false" src={emblema} />
                                        </div>
                                    </div>

                                    <div className="perfil-card-emblemas-emblema-desc">
                                        <p>Usuário com <span>10 ou mais</span> consultas <span>realizadas</span>.</p>
                                    </div>
                                </div>

                                <div className="perfil-card-emblemas-btn">
                                    <button>Seus emblemas</button>
                                </div>
                            </div>
                            
                        </div>

                    </div>
                </div>

            </>
        )
    }
}

export default Perfil;