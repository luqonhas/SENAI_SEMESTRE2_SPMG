// Libs
import { Component } from "react";
import axios from "axios";
import {parseJwt} from '../../services/Auth';

// Imgs
import logo from '../../assets/img/login/logologin.svg';
import poly from '../../assets/img/login/poly.svg';
import doutores from '../../assets/img/login/doutoress.svg';
import icone1 from '../../assets/img/login/iconezin.svg';
import icone2 from '../../assets/img/login/cadeado.svg';

// Styles
import '../../assets/css/login.css';

class Login extends Component {
    constructor(props){
        super(props);
        this.state = {
            email : "",
            senha : "",
            erroMensagem : "",
            isLoading : false
        }
    }

    gerarToken = (event) => {
        event.preventDefault();

        this.setState({ erroMensagem : "", isLoading: true });

        axios.post("http://localhost:5000/api/login", {
            email : this.state.email,
            senha : this.state.senha
        })

        .then((response => {
            if (response.status === 200) {
                localStorage.setItem("user-token", response.data.token)

                this.setState({isLoading : false});
                this.setState({isSuccessful : true});

                switch (parseJwt().role) {
                    case "1":
                        this.props.history.push('/administrador/dashboard');
                        break;

                    case "2":
                        this.props.history.push('/medico/consultas');
                        break;

                    case "3":
                        this.props.history.push('/paciente/consultas');
                        break;
                
                    default:
                        this.props.history.push('/');
                        break;
                }
            }
        }))
        .catch(() => {
            this.setState({
                erroMensagem : "E-mail ou senha inválidos! Tente novamente.",
                isLoading : false
            })
        })
    }

    atualizarEmail = (email) => {
        this.setState({[email.target.name] : email.target.value})
    }

    atualizarSenha = (senha) => {
        this.setState({[senha.target.name] : senha.target.value})
    }

    render() {
        return(
            <body className="body-login">
                <main className="main-login">
                    <div className="left-logo">
                        <img src={poly} draggable="false" class="poly" />
                        <div className="imagens">
                            <img src={logo} draggable="false" className="logo" />
                            <img src={doutores} draggable="false" className="medico" />
                        </div>
                    </div>

                    <div className="right-form">
                        <div class="form">
                            <div className="form-center">
                                <p>Login</p>
                                <div className="formulario">
                                    <form className="form-login" onSubmit={this.gerarToken}>
                                        <div className="email">
                                            <img src={icone1} />
                                            <input
                                            type="email"
                                            name="email"
                                            value={this.state.email}
                                            onChange={this.atualizarEmail}
                                            placeholder="Digite seu e-mail"
                                            />
                                        </div>

                                        <div className="senha">
                                            <img src={icone2} />
                                            <input 
                                            type="password"
                                            name="senha"
                                            value={this.state.senha}
                                            onChange={this.atualizarSenha}
                                            placeholder="Digite sua senha"
                                            />
                                        </div>
                                        <div className="botao">
                                            <a href="#"> [ esqueci a senha ] </a>
                                            {
                                                this.state.isLoading === true && (<button type="submit" disabled>Entrando...</button>)
                                            }
                                            {
                                                this.state.isLoading === false && (<button type="submit" disabled={this.state.email === "" || this.state.senha === "" ? "none" : ""}>Entrar</button>)
                                            }
                                        </div>
                                    </form>
                                </div>
                                
                            </div>
                        </div>

                        <div class="links">
                            <div class="a1">
                                <div class="linha"></div>
                                <a href="#">criar conta</a>
                            </div>

                            <div class="a2">
                                <div class="linha"></div>
                                <a href="#">ajuda</a>
                            </div>
                            
                        </div>
                    </div>
                </main>
            </body>
        )
    }
}

export default Login;