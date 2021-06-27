// Libs
import React, {Component} from 'react';

// Components
import Header from '../../components/Header';
import Consulta from '../../components/Consulta';

// Styles
import '../../assets/css/styles.css';



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
            <main>
                <Header />

                <div className="consultas-background">
                    <div className="consultas-titulo">
                        <h1>Consultas</h1>
                    </div>

                    <div className="consultas-content-background">
                        <Consulta />
                    </div>
                </div>
            </main>
        )
    }
}

export default ConsultasComum;