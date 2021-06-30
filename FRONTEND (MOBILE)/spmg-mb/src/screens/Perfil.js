import React, { Component } from 'react';
import { Image, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import jwtDecode from 'jwt-decode';

export default class Perfil extends Component {
    constructor(props){
      super(props);
      this.state = {
        nome : '',
        email : ''
      }
    }

    render() {
        return(
            <View style={{flex: 1, backgroundColor: 'white'}}>
              <Text>Perfil</Text>
            </View>
        )
    }
}