// Libs
import React, {Component} from 'react';
import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs'; // npm install @react-navigation/material-bottom-tabs react-native-paper react-native-vector-icons
import { FontAwesome5, SimpleLineIcons } from '@expo/vector-icons';
import { StyleSheet, Image, TouchableOpacity, View } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';

// Screens
import Dashboard from './Dashboard';
import Consultas from './Consultas';
import Perfil from './Perfil';



const bottomTab = createMaterialBottomTabNavigator();

export default class Navigation extends Component {
    logout = async () => {
        try {
            await AsyncStorage.removeItem('user-token');
            this.props.navigation.navigate('Login');
        } catch (error) {
            console.warn(error)
        }
    }

    render() {
        return(
            <>
                <View style={styles.header}>
                    <View style={styles.header_content}>
                        <Image source={require('../../assets/img/header-logo.svg')} style={{width: 86, height: 29, }} />
                        <TouchableOpacity onPress={this.logout}>
                            <FontAwesome5 name="sign-out-alt" size={24} color='#FFF'  />
                        </TouchableOpacity>
                        {
                            
                            // (userAuthenticated.role === '3' && (
                            //     <>
                            //         <Text style={styles.header_title}>{userAuthenticated.nomePaciente}</Text>
                            //         <FontAwesome5 style={styles.header_img} name='user-plus' size={24} color='#3E4954' />
                            //     </>
                            // ))
                        }
                    </View>
                </View>
                <bottomTab.Navigator
                    shifting
                    initialRouteName='Dashboard'
                    activeColor="#36CC9A"
                    inactiveColor="#878787"
                    barStyle={{ backgroundColor: "#364958", height: 55 }}
                    showIcon={true}
                >
                    <bottomTab.Screen
                    name="Dashboard"
                    component={Dashboard}
                    options={{
                        tabBarLabel: "Home",
                        tabBarIcon: ({ color }) => (
                        <FontAwesome5 name="house-user" size={24} color={color} />
                        ),
                    }}
                    />
                    <bottomTab.Screen
                    name="Consultas"
                    component={Consultas}
                    options={{
                        tabBarLabel: "Consultas",
                        tabBarIcon: ({ color }) => (
                        <FontAwesome5 name="notes-medical" size={24} color={color} />
                        ),
                    }}
                    />
                    <bottomTab.Screen
                    name="Perfil"
                    component={Perfil}
                    options={{
                        tabBarLabel: "Perfil",
                        tabBarIcon: ({ color }) => (
                        <FontAwesome5 name="id-badge" size={24} color={color} />
                        ),
                    }}
                    />
                </bottomTab.Navigator>
            </>
        )
    }
}

const styles = StyleSheet.create({
    header: {
        height: 50,
        backgroundColor: '#36CC9A',
        justifyContent: 'center',
        alignItems: "flex-end",
    },

    header_content: {
        flexDirection: "row",
        alignItems: 'center',
        height: 35,
        alignSelf: 'center',
        width: '78%',
        justifyContent: 'space-between'
    },

});