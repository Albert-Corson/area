import React from 'react';
import {createStackNavigator} from '@react-navigation/stack';
import SignInScreen from '../Screens/SignInScreen';
import SignUpScreen from '../Screens/SignUpScreen';
import HelpScreen from '../Screens/HelpScreen';
import DashboardScreen from '../Screens/DashboardScreen';

export type RootStackParamList = {
  Login: undefined;
  SignUp: undefined;
  Help: undefined;
  Dashboard: undefined;
};

const Stack = createStackNavigator<RootStackParamList>();

const StackNavigator = (): JSX.Element => (
  <Stack.Navigator screenOptions={{headerShown: false}}>
    <Stack.Screen name={'Login'} component={SignInScreen} />
    <Stack.Screen name={'SignUp'} component={SignUpScreen} />
    <Stack.Screen name={'Help'} component={HelpScreen} />
    <Stack.Screen name={'Dashboard'} component={DashboardScreen} />
  </Stack.Navigator>
);

export default StackNavigator;