import React, { Component } from 'react';
import { Image, ScrollView, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import api from '../services/API';
import AsyncStorage from '@react-native-async-storage/async-storage';
import jwtDecode from 'jwt-decode';

export default class Dashboard extends Component {
    constructor(props){
      super(props);
      this.state = {
        qntdConsultas : 0,
        qntdMedicos : 0,
        qntdPacientes : 0,
        qntdEspecialidades : 0,
        qntdClinicas : 0,
        qntdUsuarios : 0,

        permissao : ''
      }
    }

    buscarDadosStorage = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token')
        console.warn(jwtDecode(valorToken))
  
        if (valorToken !== null) {
          this.setState({permissao : jwtDecode(valorToken).role})
        }

        console.warn(this.state.permissao)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    contarConsultas = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token');

        let URL = '/consultas/todos';

        if (jwtDecode(valorToken).role !== '1') {
          URL = '/consultas/minhas';
        }

        const response = await api.get(URL, {
          headers : {'Authorization' : 'Bearer ' + valorToken}
        });

        const dadosAPI = response.data.length;

        this.setState({qntdConsultas : dadosAPI})

        console.warn(this.state.qntdConsultas)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    contarMedicos = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token');

        const response = await api.get('/medicos/todos', {
          headers : {'Authorization' : 'Bearer ' + valorToken}
        });

        const dadosAPI = response.data.length;

        this.setState({qntdMedicos : dadosAPI})

        console.warn(this.state.qntdMedicos)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    contarPacientes = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token');

        const response = await api.get('/pacientes/todos', {
          headers : {'Authorization' : 'Bearer ' + valorToken}
        });

        const dadosAPI = response.data.length;

        this.setState({qntdPacientes : dadosAPI})

        console.warn(this.state.qntdPacientes)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    contarEspecialidades = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token');

        const response = await api.get('/especialidades/todos', {
          headers : {'Authorization' : 'Bearer ' + valorToken}
        });

        const dadosAPI = response.data.length;

        this.setState({qntdEspecialidades : dadosAPI})

        console.warn(this.state.qntdEspecialidades)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    contarClinicas = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token');

        const response = await api.get('/clinicas/todos', {
          headers : {'Authorization' : 'Bearer ' + valorToken}
        });

        const dadosAPI = response.data.length;

        this.setState({qntdClinicas : dadosAPI})

        console.warn(this.state.qntdClinicas)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    contarUsuarios = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token');

        const response = await api.get('/usuarios/todos', {
          headers : {'Authorization' : 'Bearer ' + valorToken}
        });

        const dadosAPI = response.data.length;

        this.setState({qntdUsuarios : dadosAPI})

        console.warn(this.state.qntdUsuarios)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    render() {   
      return(
        <ScrollView style={styles.dash_container}>

          {/* CARD CONSULTAS */}
          <TouchableOpacity onPress={() => this.props.navigation.navigate('Consultas')}>
            <View style={styles.dash_card_consultas}>
              <View style={styles.dash_card_consultas_textos}>
                <Text style={styles.dash_card_consultas_textos_qntd}>{this.state.qntdConsultas}</Text>
                <Text style={styles.dash_card_consultas_textos_title}>consultas</Text>
              </View>

              <View style={styles.dash_card_consultas_img}>
                <Image source={require('../../assets/img/dashboard-consultas.png')} style={{width: 100, height: 100, tintColor: '#364958'}} />
              </View>
            </View>
          </TouchableOpacity>
          
          {/* CARD MÉDICOS */}
          {
            this.state.permissao === '1' ?
            <View style={styles.dash_card_consultas}>
              <View style={styles.dash_card_consultas_textos}>
                <Text style={styles.dash_card_consultas_textos_qntd}>{this.state.qntdMedicos}</Text>
                <Text style={styles.dash_card_consultas_textos_title}>médicos</Text>
              </View>

              <View style={styles.dash_card_consultas_img}>
                <Image source={require('../../assets/img/dashboard-medicos.png')} style={{width: 100, height: 100, tintColor: '#364958'}} />
              </View>
            </View> : ''
          }

          {/* CARD ESPECIALIDADES */}
          {
            this.state.permissao === '1' ?
            <View style={styles.dash_card_consultas}>
              <View style={styles.dash_card_consultas_textos}>
                <Text style={styles.dash_card_consultas_textos_qntd}>{this.state.qntdEspecialidades}</Text>
                <Text style={styles.dash_card_consultas_textos_title}>especialidades</Text>
              </View>

              <View style={styles.dash_card_consultas_img}>
                <Image source={require('../../assets/img/dashboard-especialidades.png')} style={{width: 100, height: 100, tintColor: '#364958'}} />
              </View>
            </View> : ''
          }

          {/* CARD PACIENTES */}
          {
            this.state.permissao === '1' ?
            <View style={styles.dash_card_consultas}>
              <View style={styles.dash_card_consultas_textos}>
                <Text style={styles.dash_card_consultas_textos_qntd}>{this.state.qntdPacientes}</Text>
                <Text style={styles.dash_card_consultas_textos_title}>pacientes</Text>
              </View>

              <View style={styles.dash_card_consultas_img}>
                <Image source={require('../../assets/img/dashboard-pacientes.png')} style={{width: 100, height: 100, tintColor: '#364958'}} />
              </View>
            </View> : ''
          }

          {/* CARD CLINICAS */}
          {
            this.state.permissao === '1' ?
            <View style={styles.dash_card_consultas}>
              <View style={styles.dash_card_consultas_textos}>
                <Text style={styles.dash_card_consultas_textos_qntd}>{this.state.qntdClinicas}</Text>
                <Text style={styles.dash_card_consultas_textos_title}>clínicas</Text>
              </View>

              <View style={styles.dash_card_consultas_img}>
                <Image source={require('../../assets/img/dashboard-clinicas.png')} style={{width: 100, height: 100, tintColor: '#364958'}} />
              </View>
            </View> : ''
          }

          {/* CARD USUÁRIOS */}
          {
            this.state.permissao === '1' ?
            <View style={styles.dash_card_consultas}>
              <View style={styles.dash_card_consultas_textos}>
                <Text style={styles.dash_card_consultas_textos_qntd}>{this.state.qntdUsuarios}</Text>
                <Text style={styles.dash_card_consultas_textos_title}>usuários</Text>
              </View>

              <View style={styles.dash_card_consultas_img}>
                <Image source={require('../../assets/img/dashboard-usuarios.png')} style={{width: 100, height: 100, tintColor: '#364958'}} />
              </View>
            </View> : ''
          }

        </ScrollView>
      )
    }

    componentDidMount = () => {
      this.contarConsultas();
      this.contarMedicos();
      this.contarPacientes();
      this.contarEspecialidades();
      this.contarClinicas();
      this.contarUsuarios();
      this.buscarDadosStorage();
    }
}

const styles = StyleSheet.create({
  dash_container: {
    flex: 1,
    backgroundColor: '#FAFAFA',
    left: 38,
    // alignItems: 'center'
  },

  dash_card_consultas: {
    width: 285,
    height: 145,
    backgroundColor: '#FFF',
    borderRadius: 10,
    marginTop: 10,
    borderColor: '#CCC',
    borderWidth: 1,
    shadowOffset:{  width: 0,  height: 5,  },
    shadowColor: 'black',
    shadowOpacity: .25,
    shadowRadius: 10,
    marginBottom: 10,
    padding: 15,
    flexDirection: 'row',
    justifyContent: 'space-between'
  },

  dash_card_consultas_textos: {
    justifyContent: 'space-between',
  },

  dash_card_consultas_textos_qntd: {
    fontSize: 40,
    fontWeight: '700',
    marginTop: -8,
    color: '#36CC9A'
  },

  dash_card_consultas_textos_title: {
    fontSize: 18,
    fontWeight: '700',
    color: '#364958'
  },

  dash_card_consultas_img: {
    justifyContent: 'center',
    marginRight: -10
  }

})