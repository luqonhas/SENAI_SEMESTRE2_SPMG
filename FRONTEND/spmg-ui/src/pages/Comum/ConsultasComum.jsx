// Libs
import React, {Component} from 'react';

// Components
import Header from '../../components/Header';
// import Footer from '../../components/footer/footer'
import Consulta from '../../components/Consulta';

// Styles
import '../../assets/css/styles.css';

// Imgs
import agendadas from '../../assets/img/botao-agendadas.svg';
import realizadas from '../../assets/img/botao-realizadas.svg';
import canceladas from '../../assets/img/botao-canceladas.svg';

class ConsultasComum extends Component{
    constructor(props){
        super(props);
        this.state = {
            listaConsultas : [],
            descricao : '',
            idConsultaAlterada : 0
        }
    }

    render() {
        return(
            <div>
                <Header />
                <section className="consultas-bg">
                    <div className="consultas">
                        <div className="paciente-botoes-consulta">
                            <button><img src={agendadas}></img></button>
                            <button><img src={realizadas}></img></button>
                            <button><img src={canceladas}></img></button>
                        </div>

                        <Consulta />

                    </div>
                </section>
            </div>
        )
    }
}

export default ConsultasComum;