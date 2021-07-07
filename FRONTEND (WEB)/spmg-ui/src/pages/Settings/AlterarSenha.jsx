// Libs
import React, {Component} from 'react';
import axios from "axios";
import {Link} from 'react-router-dom';

// Services
import {parseJwt} from '../../services/Auth';
// import {settingSelector} from '../../services/Toggle';

// Components
import Header from '../../components/Header';
import SettingButton from '../../components/SettingButton';

// Styles
import '../../assets/css/styles.css';



class AlterarSenha extends Component{
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

                        <div className="settings-content-altsenha"></div>
                    </div>
                </div>

            </>
        )
    }
}

export default AlterarSenha;