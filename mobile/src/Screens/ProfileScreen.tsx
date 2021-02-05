import {StackNavigationProp} from '@react-navigation/stack';
import React from 'react';
import {SafeAreaView, Text} from 'react-native';
import {RootStackParamList} from '../Navigation/StackNavigator';

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const ProfileScreen = ({navigation}: Props): JSX.Element => (
  <SafeAreaView>
    <Text>Hello</Text>
  </SafeAreaView>
);

export default ProfileScreen;
