// Libs
import React from "react";
import { StyleSheet, Text, View } from "react-native";
import { FontAwesome5 } from "@expo/vector-icons";

// Services
import { userAuthenticated, logout } from "../services/Auth";



export default class Header extends Component {
    render() {
        return(
            <View style={styles.header}>
                <View style={styles.header_content}>
                    {
                        (userAuthenticated.role === '3' && (
                            <>
                                <Text style={styles.header_title}>{userAuthenticated.nomePaciente}</Text>
                                <FontAwesome5 style={styles.header_img} name='user-plus' size={24} color='#3E4954' />
                            </>
                        )) ||

                        (userAuthenticated.role === '2' && (
                            <>
                                <Text style={styles.header_title}>{userAuthenticated.nomeMedico}</Text>
                                <FontAwesome5 style={styles.header_img} name='user-md' size={24} color='#3E4954' />
                            </>
                        ))
                    
                    }
                </View>
            </View>
        )
    }
}

const styles = StyleSheet.create({
    header: {
        height: 100,
        paddingBottom: 16,
        backgroundColor: "#E8EFEF",
        flexDirection: "row",
        justifyContent: "flex-end",
        alignItems: "flex-end",
    },

    header_content: {
        flexDirection: "row",
        alignItems: "center",
        marginRight: 20,
    },

    header_title: {
        fontSize: 16,
        color: "#3F3D56",
        paddingRight: 16,
    },

    header_img: {
        borderWidth: 1,
        borderRadius: 6,
        padding: 8,
    }
});