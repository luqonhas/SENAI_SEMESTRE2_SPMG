import React, { Component } from 'react';
import { Image, FlatList, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import api from '../services/API';
import AsyncStorage from '@react-native-async-storage/async-storage';
import jwtDecode from 'jwt-decode';



export default class Consultas extends Component {
    constructor(props){
      super(props);
      this.state = {
        listaConsultas : []
      }
    }

    buscarConsultas = async () => {
      try {
        const valorToken = await AsyncStorage.getItem('user-token');

        let URL = '/consultas/todos';

        if (jwtDecode(valorToken).role !== '1') {
          URL = '/consultas/minhas';
        }

        const response = await api.get(URL, {
          headers : {'Authorization' : 'Bearer ' + valorToken}
        });

        const dadosAPI = response.data;

        this.setState({listaConsultas : dadosAPI})

        console.warn(this.state.listaConsultas)
      } 
      
      catch (error) {
        console.warn(error)
      }
    }

    

    renderItem = ({item}) => (
      
      <View style={styles.consultas_card}>
        {
          item.idSituacaoNavigation.situacao === 'Agendada' ? 
          <View style={styles.consultas_card_situacao_agendada}>
          <Text style={styles.consultas_card_situacao_text}>{item.idSituacaoNavigation.situacao}</Text>
          </View> : ''
        }
        {
          item.idSituacaoNavigation.situacao === 'Realizada' ? 
          <View style={styles.consultas_card_situacao_realizada}>
          <Text style={styles.consultas_card_situacao_text}>{item.idSituacaoNavigation.situacao}</Text>
          </View> : ''
        }
        {
          item.idSituacaoNavigation.situacao === 'Cancelada' ? 
          <View style={styles.consultas_card_situacao_cancelada}>
          <Text style={styles.consultas_card_situacao_text}>{item.idSituacaoNavigation.situacao}</Text>
          </View> : ''
        }
        

        <View style={styles.consultas_card_dia}>
          <View style={styles.consultas_card_dia_padrao}>
            <View style={styles.consultas_card_dia_padrao_background}>
              <Text style={styles.consultas_card_dia_padrao_background_text}>{'Data'.toUpperCase()}</Text>
            </View>

            <Text style={styles.consultas_card_dia_padrao_background_text_out}>{Intl.DateTimeFormat('pt-BR').format(new Date(item.dataConsulta))}</Text>
          </View>

          <View style={styles.consultas_card_dia_padrao}>
            <View style={styles.consultas_card_dia_padrao_background}>
              <Text style={styles.consultas_card_dia_padrao_background_text}>{'Hora'.toUpperCase()}</Text>
            </View>

            <Text style={styles.consultas_card_dia_padrao_background_text_out}>{item.horaConsulta}</Text>
          </View>
        </View>

        <View style={styles.consultas_card_paciente}>
          <Text style={styles.consultas_card_paciente_text_title}>{'Paciente'.toUpperCase()}</Text>

          <View style={styles.consultas_card_paciente_linha}>
            <Image source={require('../../assets/img/consulta-paciente.png')} style={{width: 42, height: 42, borderRadius: 50, borderColor: '#36CC9A', borderWidth: 2}} />
            <Image source={require('../../assets/img/consulta-paciente-start.png')} style={{width: 21, height: 43, marginLeft: -10}} />
            <View style={styles.consultas_card_paciente_barra}>
              <Text style={styles.consultas_card_paciente_barra_text}>{item.idPacienteNavigation.nomePaciente}</Text>
            </View>
          </View>
        </View>

        <View style={styles.consultas_card_medico}>
          <Text style={styles.consultas_card_medico_text_title}>{'Doutor & Especialidade'.toUpperCase()}</Text>
          
          <View style={styles.consultas_card_medico_linha}>
            <View style={styles.consultas_card_medico_barra}>
              <Text style={styles.consultas_card_medico_barra_text}>{item.idMedicoNavigation.nomeMedico}</Text>
            </View>
            <Image source={require('../../assets/img/consulta-medico-start.png')} style={{width: 21, height: 43}} />
            <Image source={require('../../assets/img/consulta-medico.png')} style={{width: 42, height: 42, borderRadius: 50, borderColor: '#1481BA', borderWidth: 2, marginLeft: -10}} />
          </View>

          <View style={styles.consultas_card_medico_especialidade}>
            <Text style={styles.consultas_card_medico_especialidade_text}>{item.idMedicoNavigation.idEspecialidadeNavigation.nomeEspecialidade}</Text>
          </View>
        </View>

        <View style={styles.consultas_card_descricao}>
          <Text style={styles.consultas_card_descricao_text_title}>{'Descrição'.toUpperCase()}</Text>
          
          <View style={styles.consultas_card_descricao_linha}>
            <Text style={styles.consultas_card_descricao_text}>{item.descricao}</Text>
          </View>
        </View>

      </View>
    )
    
    render() {
      return(
        <>
            <FlatList contentContainerStyle={styles.consultas_container} data={this.state.listaConsultas} keyExtractor={item => item.idConsulta} renderItem={this.renderItem} />
        </>
      )
    }

    componentDidMount = () => {
      this.buscarConsultas();
    }
      
}

const styles = StyleSheet.create({
  consultas_container: {
    flex: 1,
    backgroundColor: '#FAFAFA',
    alignItems: 'center'
  },

  consultas_card: {
    width: 285,
    height: 370,
    backgroundColor: '#FFF',
    borderRadius: 10,
    marginTop: 10,
    borderColor: '#CCC',
    borderWidth: 1,
    shadowOffset:{  width: 0,  height: 5,  },
    shadowColor: 'black',
    shadowOpacity: .25,
    shadowRadius: 10,
    marginBottom: 10
  },

  // situacao
  consultas_card_situacao_agendada: {
    justifyContent: 'center',
    alignItems: 'center',
    padding: 5,
    backgroundColor: '#36CC9A',
    borderTopLeftRadius: 9,
    borderTopRightRadius: 9,
    width: 283,
    height: 30
  },

  consultas_card_situacao_realizada: {
    justifyContent: 'center',
    alignItems: 'center',
    padding: 5,
    backgroundColor: '#1481BA',
    borderTopLeftRadius: 9,
    borderTopRightRadius: 9,
    width: 283,
    height: 30
  },

  consultas_card_situacao_cancelada: {
    justifyContent: 'center',
    alignItems: 'center',
    padding: 5,
    backgroundColor: '#364958',
    borderTopLeftRadius: 9,
    borderTopRightRadius: 9,
    width: 283,
    height: 30
  },

  consultas_card_situacao_text: {
    color: 'white',
    fontWeight: '700',
    fontSize: 16,
    textTransform: 'upperCase'
  },

  // dias card
  consultas_card_dia: {
    flexDirection: 'row',
    padding: 10
  },

  consultas_card_dia_padrao: {
    marginRight: 20,
  },

  consultas_card_dia_padrao_background: {
    backgroundColor: '#364958',
  },

  consultas_card_dia_padrao_background_text: {
    color: 'white',
    fontSize: 15,
    fontWeight: '700'
  },
  
  consultas_card_dia_padrao_background_text_out: {
    color: '#364958',
    fontWeight: '800'
  },

  // pacientes card
  consultas_card_paciente: {
    padding: 10,
    paddingTop: 0
  },

  consultas_card_paciente_text_title: {
    fontSize: 15,
    fontWeight: '700',
    color: '#36CC9A'
  },

  consultas_card_paciente_linha: {
    flexDirection: 'row'
  },
  
  consultas_card_paciente_barra: {
    width: 213,
    height: 43,
    backgroundColor: '#36CC9A',
    padding: 4
  },

  consultas_card_paciente_barra_text: {
    textTransform: 'uppercase',
    fontWeight: '700',
    color: 'white',
  },

  // medicos card
  consultas_card_medico: {
    padding: 10,
    paddingTop: 0
  },

  consultas_card_medico_text_title: {
    fontSize: 15,
    fontWeight: '700',
    color: '#1481BA'
  },

  consultas_card_medico_linha: {
    flexDirection: 'row'
  },
  
  consultas_card_medico_barra: {
    width: 213,
    height: 43,
    backgroundColor: '#1481BA',
    padding: 4
  },

  consultas_card_medico_barra_text: {
    textTransform: 'uppercase',
    fontWeight: '700',
    color: 'white'
  },

  consultas_card_medico_especialidade: {
    width: 265,
    height: 15,
    backgroundColor: '#1481BA',
    marginTop: 3,
    justifyContent: 'center',
    paddingLeft: 5,
    paddingRight: 5
  },

  consultas_card_medico_especialidade_text: {
    fontSize: 12,
    fontWeight: '700',
    color: 'white',
    textTransform: 'uppercase'
  },

  // descrição
  consultas_card_descricao: {
    padding: 10,
    paddingTop: 0
  },

  consultas_card_descricao_text_title: {
    fontSize: 15,
    fontWeight: '700',
    color: '#364958'
  },

  consultas_card_descricao_linha: {
    width: 265,
    height: 85,
    borderColor: '#364958',
    borderWidth: 2,
    borderBottomLeftRadius: 9,
    borderBottomRightRadius: 9,
    padding: 5
  }


})

// style={styles.consultas_}