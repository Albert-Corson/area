import React from 'react'
import {createStackNavigator} from '@react-navigation/stack'
import SignInScreen from '../Screens/SignInScreen'
import SignUpScreen from '../Screens/SignUpScreen'
import HelpScreen from '../Screens/HelpScreen'
import DashboardScreen from '../Screens/DashboardScreen'
import ProfileScreen from '../Screens/ProfileScreen'
import ServiceAuthScreen from '../Screens/ServiceAuthScreen'
import OAuthSignInScreen from '../Screens/OAuthSignInScreen'

export type RootStackParamList = {
  Login: undefined;
  SignUp: undefined;
  Help: undefined;
  Dashboard: undefined;
  Profile: undefined;
  ServiceAuth: {authUrl: string, widgetId: number};
  OAuthSignIn: {
    url?: string;
  };
};

const Stack = createStackNavigator<RootStackParamList>()

const StackNavigator = (): JSX.Element => {
  return (
    <Stack.Navigator
      screenOptions={{headerShown: false}}
      initialRouteName="Login"
    >
      <Stack.Screen name="Login" component={SignInScreen} />
      <Stack.Screen name="SignUp" component={SignUpScreen} />
      <Stack.Screen name="Help" component={HelpScreen} />
      <Stack.Screen name="Profile" component={ProfileScreen} />
      <Stack.Screen name="Dashboard" component={DashboardScreen} />
      <Stack.Screen name="ServiceAuth" component={ServiceAuthScreen} />
      <Stack.Screen name="OAuthSignIn" component={OAuthSignInScreen} />
    </Stack.Navigator>
  )
}

export default StackNavigator
