// Libs
import AsyncStorage from '@react-native-async-storage/async-storage';
import React, {Component} from 'react';
import { Image, ImageBackground, StyleSheet, TouchableOpacity, Text, View } from 'react-native';
import { TextInput } from 'react-native-gesture-handler';

// API
import api from '../services/API';

// Imgs
// import Logo from '../../assets/img/login-logo';



export default class Login extends Component { 
    constructor(props){
        super(props);
        this.state = {
            email : '',
            senha : '',
            erro : '',
            idLoading : false
        }
    }

    validarLogin = () => {
        const email = this.state.email;
        if (email == null || email.length === 0) {
            this.setState({erro : 'O e-mail é obrigatório!'});
            return false;
        }

        const senha = this.state.senha;
        if (senha == null || senha.length === 0) {
            this.setState({erro : 'A senha é obrigatória!'});
            return false;
        }

        return true;
    }

    logar = async () => {
        if (this.validarLogin()) {
            console.warn(this.state.email + ' ' +  this.state.senha);
            
            this.setState({isLoading : true})
    
            const response = await api.post('/login', {
                email : this.state.email,
                senha : this.state.senha
            })

            .then(response => {
                switch (response.status) {
                    case 200:
                        const token = response.data.token;

                        console.warn(token);

                        AsyncStorage.setItem('user-token', token);

                        this.props.navigation.navigate('Navigation');

                        break;

                    case 404:
                        response.json().then(result => {
                            result => {
                                this.setState({erro : result})
                            }
                        })
                        break;
                
                    default:
                        response.json().then(result => {
                            result => {
                                this.setState({erro : result})
                            }
                        })
                        break;
                }
            })

            .catch(erro => {
                this.setState({erro : 'E-mail ou senha incorretos!'})
            })

            this.setState({isLoading : false})

        }
    }

    render() {
        return(
            <ImageBackground source={require('../../assets/img/login-background.png')} style={StyleSheet.absoluteFillObject} >
                <View style={styles.overlay}>
                    <View style={styles.main}>
                        {/* <View style={styles.mainImgLogin}>
                            <Logo />
                        </View> */}
                        <Image source={require('../../assets/img/login-logo.png')} style={styles.mainImgLogin} />
                        
                        <TextInput 
                        style={styles.inputLogin}
                        placeholder='seu e-mail'
                        placeholderTextColor='#9a999a'
                        keyboardType='email-address'
                        onChangeText={email => this.setState({email})}
                        />

                        <TextInput 
                        style={styles.inputLogin}
                        placeholder='sua senha'
                        placeholderTextColor='#9a999a'
                        secureTextEntry={true}
                        passwordRules
                        onChangeText={senha => this.setState({senha})}
                        />

                        <TouchableOpacity
                            style={styles.btnLogin}
                            onPress={this.logar}
                            activeOpacity={0.5}
                            disabled={this.state.isLoading}
                        >
                            <Text style={styles.btnLoginText}>{'Login'.toUpperCase()}</Text>
                        </TouchableOpacity>

                        <Text style={{color: 'white', textAlign: 'center', backgroundColor: '#364958', fontWeight: 'bold', marginTop: 6, position:'relative', paddingLeft: 24, paddingRight: 24}}>{this.state.erro}</Text>
                        
                    </View>
                </View>
            </ImageBackground>

        )
    }
}

const styles = StyleSheet.create({
    overlay: {
        ...StyleSheet.absoluteFillObject,
        backgroundColor: 'rgba(45,192,159,.75)',
        
    },

    mainImgLogin: {
        height: 50,
        width: 210,
        margin: 40,
        marginTop: 0,
        zIndex: 1,
        marginBottom: 50
    },

    main: {
        width: '100%',
        height: '100%',
        justifyContent: 'center',
        alignItems: 'center'
    },

    inputLogin: {
        width: 240,
        marginBottom: 20,
        fontSize: 18,
        height: 40,
        color: '#364958',
        backgroundColor: 'white',
        borderRadius: 10,
        padding: 10
    },

    btnLogin: {
        justifyContent: 'center',
        alignItems: 'center',
        height: 38,
        width: 240,
        backgroundColor: '#1481BA',
        borderColor: '#1481BA',
        borderRadius: 50,
        borderWidth: 1,
        fontWeight: '800'
    },

    btnLoginText: {
        fontSize: 14,
        color: '#FFF',
        letterSpacing: 3,
        fontWeight: '800',
        textTransform: 'capitalize'
    }

})

// style={styles.overlay}