import React, {useContext} from 'react';
import {createStackNavigator} from '@react-navigation/stack';
import SignInScreen from '../Screens/SignInScreen';
import SignUpScreen from '../Screens/SignUpScreen';
import HelpScreen from '../Screens/HelpScreen';
import DashboardScreen from '../Screens/DashboardScreen';
import ProfileScreen from '../Screens/ProfileScreen';
import RootStoreContext from '../Stores/RootStore';

export type RootStackParamList = {
  Login: undefined;
  SignUp: undefined;
  Help: undefined;
  Dashboard: undefined;
  Profile: undefined;
};

const Stack = createStackNavigator<RootStackParamList>();

const StackNavigator = (): JSX.Element => {
  const store = useContext(RootStoreContext);

  return (
    <Stack.Navigator
      screenOptions={{headerShown: false}}
      initialRouteName={store.user.userJWT?.accessToken ? 'Dashboard' : 'Login'}
    >
      <Stack.Screen name={'Login'} component={SignInScreen} />
      <Stack.Screen name={'SignUp'} component={SignUpScreen} />
      <Stack.Screen name={'Help'} component={HelpScreen} />
      <Stack.Screen name={'Profile'} component={ProfileScreen} />
      <Stack.Screen name={'Dashboard'} component={DashboardScreen} />
    </Stack.Navigator>
  );
};

export default StackNavigator;