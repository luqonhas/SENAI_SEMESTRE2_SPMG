// Libs
import { Component } from "react";
import axios from "axios";
import {parseJwt} from '../../services/Auth';

// Imgs
import background from '../../assets/img/login-background.svg';
import logo from '../../assets/img/login-logo.svg';
import doutores from '../../assets/img/login-doutores.svg';

// Styles
import '../../assets/css/styles.css';
import '@fortawesome/fontawesome-free/css/all.min.css';



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

    limparCampos = () => {
        this.setState({
            email : '',
            senha : ''
        })
    }

    atualizarEmail = (email) => {
        this.setState({[email.target.name] : email.target.value})
    }

    atualizarSenha = (senha) => {
        this.setState({[senha.target.name] : senha.target.value})
    }

    componentDidMount = () => {
        const inputs = document.querySelectorAll(".login-input");

        function addcl(){
            let parent = this.parentNode.parentNode;
            parent.classList.add("focus");
        }

        function remcl(){
            let parent = this.parentNode.parentNode;
            if(this.value == ""){
                parent.classList.remove("focus");
            }
        }

        inputs.forEach(input => {
            input.addEventListener("focus", addcl);
            input.addEventListener("blur", remcl);
        });
    }

    render() {
        return(
            <main>
                <div className="login">
                    
                    <div className="login-left-logo">
                        <img src={background} draggable="false" className="login-background" />

                        <div className="login-imagens">
                            <img src={logo} draggable="false" className="login-logo" />
                            <img src={doutores} draggable="false" className="login-medico" />
                        </div>
                        
                    </div>

                    <div className="login-right-form">
                        <div className="login-form">
                            <div className="login-form-center">
                                <p>Login</p>
                                <div className="login-formulario">
                                    <form onSubmit={this.gerarToken} className="login-form-main">
                                        <div className="login-input-div one">
                                            <div className="i">
                                                <i className="fas fa-user"></i>
                                            </div>

                                            <div className="login-div">
                                                <h5>E-mail</h5>
                                                <input 
                                                type="email"
                                                name="email"
                                                value={this.state.email}
                                                onChange={this.atualizarEmail}
                                                className="login-input" 
                                                />
                                            </div>
                                        </div>

                                        <div className="login-input-div pass">
                                            <div className="i">
                                                <i className="fas fa-lock"></i>
                                            </div>

                                            <div className="login-div">
                                                <h5>Senha</h5>
                                                <input 
                                                type="password"
                                                name="senha"
                                                value={this.state.senha}
                                                onChange={this.atualizarSenha}
                                                className="login-input" 
                                                />
                                            </div>
                                        </div>

                                        <a href="#" className="login-a">[ esqueci senha ]</a>
                                        {/* <input type="submit" className="login-btn" value="Entrar" /> */}
                                        {
                                            this.state.isLoading === true && (<input value="Entrando..." type="submit" className="login-btn" disabled />)
                                        }
                                        {
                                            this.state.isLoading === false && (<input value="Entrar" className="login-btn" type="submit" disabled={this.state.email === "" || this.state.senha === "" ? "none" : ""} />)
                                        }
                                        <div className="login-erro">
                                            <p>{this.state.erroMensagem}</p>
                                        </div>
                                    </form>
                                </div>

                            </div>
                        </div>

                        <div className="login-links">
                            <div className="login-a1">
                                <div className="login-linha"></div>
                                <a href="#" className="login-link-a1">criar conta</a>
                            </div>

                            <div className="login-a2">
                                <div className="login-linha"></div>
                                <a href="#" className="login-link-a2">ajuda</a>
                            </div>
                        </div>
                    </div>
                    
                </div>
            </main>
        )
    }
}

export default Login;