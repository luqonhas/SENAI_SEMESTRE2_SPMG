// import {Link} from 'react-router-dom';
import {Component} from "react";
import axios from "axios";
import {parseJwt, usuarioAutenticado} from '../../services/auth/auth';
import ModalAlert from "../../components/modalAlert/modalAlert";

class Login extends Component {
    constructor(props) {
      super(props);
      this.state = {
        email : '',
        senha : '',
        erroMensagem : '',
        isLoading : false,
        isSuccessful : false,
        modalAlert : false
      };
    }


    
    efetuaLogin = (event) => {
        event.preventDefault();

        this.setState({ erroMensagem : "", isLoading: true });

        axios.post("http://localhost:5000/api/login", {
            email : this.state.email,
            senha : this.state.senha
        })

        .then((resposta) => {
            if (resposta.status === 200) {
                localStorage.setItem("usuario-login", resposta.data.token)
                console.log("Meu token é: " + resposta.data.token);
                
                this.setState({isLoading : false})
                this.setState({ isSuccessful: true });
                this.setState({ modalAlert: true });

                // let base64 = localStorage.getItem("usuario-login").split(".")[1]

                console.log(parseJwt());
                console.log(parseJwt().role);
                
                switch (parseJwt().role) {
                    case "1":
                        console.log(usuarioAutenticado());
                        this.props.history.push('/');
                        break;
                    
                    case "2":
                        console.log(usuarioAutenticado());
                        this.props.history.push('/medico/consultas');
                        break;

                    case "3":
                        console.log(usuarioAutenticado());
                        this.props.history.push('/paciente/consultas');
                        break;
                
                    default:
                        console.log(usuarioAutenticado());
                        this.props.history.push('/');
                        break;
                }
            }
        })

        .catch(() => {
            this.setState({
              erroMensagem: "E-mail ou senha inválidos! Tente novamente.",
              isLoading: false,
              isSuccessful : false,
              modalAlert : true
            });
        });
    }



    atualizaStateCampo = (campo) => {
        this.setState({ [campo.target.name]: campo.target.value });    
    };

    onModalClick() {
        this.setState({ modalAlert: false });
    }



    render() {
        return (
            <div>
                {this.state.modalAlert ? (
                    this.state.isSuccessful ? (
                        <ModalAlert 
                            titulo="Sucesso!"
                            descricao="Login efetuado com sucesso!"
                            onClick={() => this.onModalClick()}
                        />
                    )
                    :
                    (
                        <ModalAlert 
                            titulo="Erro!"
                            descricao="Login não efetuado! Verifique seu e-mail ou senha."
                            onClick={() => this.onModalClick()}
                        />
                    )
                ) : null}
                
                {/* faz a chamada para a função de login quando o botão é pressionado */}
                <form onSubmit={this.efetuaLogin}>
                    <div className="item">
                        <input
                            className="input__login"
                            id="login__email"
                            // email
                            type="text"
                            name="email"
                            // define que o input email recebe o valor do state "email"
                            value={this.state.email}
                            // faz a chamada para a função que atualiza o state, conforme o usuário altera o valor do input
                            onChange={this.atualizaStateCampo}
                            placeholder="email"
                        />
                    </div>

                    <div className="item">
                        <input
                            className="input__login"
                            id="login__password"
                            // email
                            type="password"
                            name="senha"
                            // define que o input email recebe o valor do state "email"
                            value={this.state.senha}
                            // faz a chamada para a função que atualiza o state, conforme o usuário altera o valor do input
                            onChange={this.atualizaStateCampo}
                            placeholder="senha"
                        />
                    </div>

                    <div className="item">
                        {/* exibe a mensagem de erro ao entrar com as credenciais erradas */}
                        <p style={{ color: "red" }}>{this.state.erroMensagem}</p>

                        {/* verifica se a requisição está em andamento, se estiver, o botão será desabilitado */}
                        {
                            // caso "isLoading" seja true, renderiza o botão desabilitado com o texto "Loading..."
                            this.state.isLoading === true && (<button className="btn btn__login" id="btn__login" type="submit" disabled>Carregando...</button>)
                        }

                        {
                            // caso "isLoading" seja false, renderiza o botão habilitado com o texto "Login"
                            this.state.isLoading === false && (<button type="submit" className="btn btn__login" id="btn__login" disabled={this.state.email === "" || this.state.senha === "" ? "none" : ""}>Login</button>)
                        }

                        {/* <button type="submit">
                            Login
                        </button> */}
                    </div>
                </form>
            </div>
        )
    }
        
}

export default Login;