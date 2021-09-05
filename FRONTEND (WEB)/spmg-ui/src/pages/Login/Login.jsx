// Libs
import { Component } from "react";
import axios from "axios";
import ReactWebChat, { createDirectLine, createStyleSet } from 'botframework-webchat';

// Services
import {parseJwt} from '../../services/Auth';

// Components
import Modal2 from '../../components/Modal2';

// Imgs
import background from '../../assets/img/login-background.svg';
import logo from '../../assets/img/login-logo.svg';
import doutores from '../../assets/img/login-doutores.svg';
import help from '../../assets/img/login-help-icon.svg';
import loading from '../../assets/img/login-chatbot-time.svg';

// Styles
import '../../assets/css/styles.css';
import '../../assets/css/chatbot.css';
import '@fortawesome/fontawesome-free/css/all.min.css';



class Login extends Component {
    constructor(props){
        super(props);
        this.state = {
            email : "",
            senha : "",
            erroMensagem : "",
            isLoading : false,

            idModalChatBot : false,
            // directLine = new DirectLine({token})
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
                        this.props.history.push('/dashboard');
                        break;

                    case "2":
                        this.props.history.push('/dashboard');
                        break;

                    case "3":
                        this.props.history.push('/dashboard');
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

    cancelaModal = () => {
        this.setState({ idModalChatBot : false })
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
        const styleSet = window.WebChat.createStyleSet({
            //  bubbleBackground: 'rgba(0, 0, 255, .1)',
            //  bubbleFromUserBackground: 'rgba(0, 255, 0, .1)',
             rootHeight: '500px',
             rootWidth: '350px',
             backgroundColor: '#400A6F'
          });

          styleSet.textContent = {
            ...styleSet.textContent,
            fontFamily: "'Arial', 'Arial', sans-serif"
           //  ,
           //  fontWeight: 'bold'
         };
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
                                                autoComplete="off"
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

                                        {/* <input type="submit" className="login-btn" value="Entrar" /> */}
                                        {
                                            this.state.isLoading === true && (<input value="Entrando..." type="submit" className="login-btn" disabled />)
                                        }
                                        {
                                            this.state.isLoading === false && (<input value="Entrar" className="login-btn" type="submit" disabled={this.state.email === "" || this.state.senha === "" ? "none" : ""} />)
                                        }
                                        <a href="#" className="login-a">[ esqueci senha ]</a>
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
                                <a style={{cursor: 'pointer'}} onClick={() => this.setState({idModalChatBot : true})} className="login-link-a2">ajuda</a>
                            </div>
                        </div>
                    </div>
                    
                </div>

                <Modal2 isOpen={this.state.idModalChatBot}>
                    <div className="modal-overlay-chatbot">
                        <div className="modal-chatbot" id="modal-chatbot" onClick={() => document.getElementById('modal-card-chatbot').click() ? '' : this.cancelaModal()}></div>
                        
                        <div id="modal-card-chatbot" className="modal-card-chatbot-background">
                            <div className="modal-card-chatbot-header">
                                <img src={help} draggable="false" />
                                <p>Central de ajuda</p>
                            </div>
                            
                            <div>
                                <iframe className="chatbot" src="https://webchat.botframework.com/embed/spmg-bot?s=JjkQbeXJb1U.zRk4XDD5G-5VuXDiK1R6yjWOMhlNa4vQxYc1lQEs4ns"></iframe>
                            </div>
                            
                        </div>
                    </div>
                </Modal2>

            </main>
        )
    }
}

export default Login;