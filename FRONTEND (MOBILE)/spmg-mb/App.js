// Libs
import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';

// Screens
import Login from './src/screens/Login';
import Navigation from './src/screens/Navigation';



const AuthStack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <AuthStack.Navigator headerMode="none">
        <AuthStack.Screen name="Login" component={Login} />
        <AuthStack.Screen name="Navigation" component={Navigation} />
      </AuthStack.Navigator>
    </NavigationContainer>
  );
}
