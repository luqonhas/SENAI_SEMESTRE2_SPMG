import React, { Component } from 'react';
import { Image, FlatList, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
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
        qntdUsuarios : 0
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

    render() {
      const valorToken = AsyncStorage.getItem('user-token');
      return(
        <View style={styles.dash_container}>

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

        </View>
      )
    }

    componentDidMount = () => {
      this.contarConsultas();
    }
}

const styles = StyleSheet.create({
  dash_container: {
    flex: 1,
    backgroundColor: '#FAFAFA',
    alignItems: 'center'
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