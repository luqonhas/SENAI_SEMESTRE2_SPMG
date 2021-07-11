// Libs
import React, {Component} from 'react';
import axios from "axios";

// Services
import {parseJwt} from '../../services/Auth';
import {uri} from '../../services/Connection';

// Components
import Header from '../../components/Header';
import SettingButton from '../../components/SettingButton';

// Styles
import '../../assets/css/styles.css';



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

class Editar extends Component{
    constructor(props){
        super(props);
        this.state = {
            listaPacientes : [],
            listaMedicos : [],
            listaUsuarios : [],
            email : '',
            telefone : '',
            endereco : '',

            mensagemEmail : '',
            mensagemTelefone : '',
            mensagemEndereco : ''
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



    atualizarEstadoEmail = async (event) => {
        await this.setState({
            [event.target.name] : event.target.value
        })
        console.log(this.state.email)
    }



    editarEmail = (event) => {
        event.preventDefault();

        this.setState({mensagemEmail : ''})
        
        axios.patch('http://localhost:5000/api/usuarios/email', {
            email : this.state.email
        },     
        {
            headers : {
                'Authorization' : 'Bearer ' + localStorage.getItem('user-token')
            }
        })

        .then(response => {
            if(response.status === 204){
                this.setState({ mensagemEmail : 'E-mail atualizado!'})
            }
        })

        .catch(erro => {
            this.setState({mensagem : 'E-mail já existente!'})
        })

        .then(this.buscarUsuarios)
    }



    alterarFoto = (event) => {
        event.preventDefault();

        let formData = new FormData();
        formData.append("arquivo", event.target.files[0]);

        fetch('http://localhost:5000/api/usuarios/foto/alterar', {
            method: "PUT",
            headers: {
                authorization: 'Bearer ' + localStorage.getItem("user-token"),
            },
            body: formData,
        })

        .then((response) => response.json())

        .then(console.log("stonks"))

        .then(window.location.reload(false))

        .catch(erro => console.log(erro));
    }



    componentDidMount(){
        this.buscarPacientes();
        this.buscarMedicos();
        this.buscarUsuarios();
    }



    render(){
        return(
            <>
                <Header />

                <div className="settings-background">
                    <div className="settings-titulo">
                        <h1>Configurações</h1>
                    </div>

                    <div className="settings-content-background">
                        <SettingButton />

                        <div className="settings-content-editar">

                            <form className="settings-content-editar-foto">

                                <div className="settings-content-editar-foto-icon-text">
                                    <div className="settings-content-editar-foto-base-icon">
                                        {
                                            this.state.listaUsuarios.map(foto => {
                                                return(
                                                    <div className="settings-content-editar-foto-icon-img">
                                                        <img src={`${uri}/FotosPerfil/${foto.foto}`} draggable="false"  />
                                                    </div>
                                                )
                                            })
                                        }
                                    </div>

                                    <div className="settings-content-editar-foto-text">
                                        <p>Carregue uma nova foto</p>
                                        <p>Tamanho máximo: 4MB</p>
                                    </div>
                                </div>

                                <input type="file" id="inputImage" onChange={(event) => {this.alterarFoto(event)}} className="settings-content-editar-foto-btn" />
                            </form>


                            <form className="settings-content-editar-info" onSubmit={this.editarEmail}>
                                <p className="settings-content-editar-info-title-area">Altere suas informações aqui</p>

                                {
                                    this.state.listaPacientes.map(paciente => {
                                        return(
                                            <>
                                                {
                                                    parseJwt().role === '3' ?
                                                    <div key={paciente.idUsuario} className="settings-content-editar-info-inputs">
                                                        <div className="settings-content-editar-info-inputs-email">
                                                            <p className="settings-content-editar-info-inputs-text">E-mail</p>
                                                            <input minLength="8" value={paciente.idUsuarioNavigation.email} type="text" onChange={this.atualizarEstadoEmail} id={paciente.idUsuario} name="email" />
                                                        </div>

                                                        <div className="settings-content-editar-info-inputs-telefone">
                                                            <p className="settings-content-editar-info-inputs-text">Telefone</p>
                                                            <input value={formataTelefone(paciente.telefonePaciente)} placeholder={paciente.telefonePaciente === '           ' && 'Adicionar telefone...'} type="text" />
                                                        </div>

                                                        <div className="settings-content-editar-info-inputs-endereco">
                                                            <p className="settings-content-editar-info-inputs-text">Endereço</p>
                                                            <input value={paciente.endereco} type="text" />
                                                        </div>
                                                    </div> : ''
                                                }
                                            </>
                                        )
                                    })
                                }
                                
                                {
                                    this.state.listaUsuarios.map(user => {
                                        return(
                                            <>
                                                {
                                                    parseJwt().role === '1' || parseJwt().role === '2' ?
                                                    <div className="settings-content-editar-info-inputs">
                                                        <div className="settings-content-editar-info-inputs-email-med-adm">
                                                            <p className="settings-content-editar-info-inputs-text">E-mail</p>
                                                            <input minLength="8" type="email" onChange={this.atualizarEstadoEmail} id={user.idUsuario} value={user.email} name="email" />
                                                        </div>
                                                    </div> : ''
                                                }
                                            </>
                                        )
                                    })
                                }

                                <button type="submit">Enviar</button>

                            </form>

                        </div>
                    </div>

                </div>

            </>
        )
    }

}

export default Editar;