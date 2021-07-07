// Libs
import React, {Component} from 'react';
import {Link} from 'react-router-dom';



class SettingButton extends Component{
    render(){
        const URL = window.location.pathname;
        console.log(URL);
        return(
            <>
                <div className="settings-content-btn-main">

                    {/* EDITAR PERFIL */}
                    <div className={URL === '/conta/editar' ? 'settings-content-btn settings-btn-active' : 'settings-content-btn'} >
                        <Link to="/conta/editar" className={"settings-content-btn-link"}>
                            <p>Editar Perfil</p>
                            <p>Suas informações pessoais</p>
                        </Link>
                    </div>
                    
                    {/* ALTERAR SENHA */}
                    <div className={URL === '/conta/senha/alterar' ? 'settings-content-btn settings-btn-active' : 'settings-content-btn'}>
                        <Link to="/conta/senha/alterar" className="settings-content-btn-link">
                            <p>Alterar Senha</p>
                            <p>Alterando a sua senha de segurança</p>
                        </Link>
                    </div>
                    
                    {/* TEMAS */}
                    <div className={URL === '/conta/temas' ? 'settings-content-btn settings-btn-active' : 'settings-content-btn'}>
                        <Link to="#" className="settings-content-btn-link">
                            <p>Temas</p>
                            <p>Personalize o SPMG do seu jeito</p>
                        </Link>
                    </div>

                </div>

            </>
                
        )
    }
}

export default SettingButton;