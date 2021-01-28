import React from 'react';
import {createStackNavigator} from '@react-navigation/stack';
import SignInScreen from '../Screens/SignInScreen';
import SignUpScreen from '../Screens/SignUpScreen';
import HelpScreen from '../Screens/HelpScreen';
import WidgetsScreen from '../Screens/WidgetsScreen';

export type RootStackParamList = {
  Login: undefined;
  SignUp: undefined;
  Help: undefined;
  Widgets: undefined;
};

const Stack = createStackNavigator<RootStackParamList>();

const StackNavigator = (): JSX.Element => (
  <Stack.Navigator screenOptions={{headerShown: false}}>
    <Stack.Screen name={'Login'} component={SignInScreen} />
    <Stack.Screen name={'SignUp'} component={SignUpScreen} />
    <Stack.Screen name={'Help'} component={HelpScreen} />
    <Stack.Screen name={'Widgets'} component={WidgetsScreen} />
  </Stack.Navigator>
);

export default StackNavigator;