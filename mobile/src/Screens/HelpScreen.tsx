import React, {useContext, useState} from 'react';
import {
  View,
  Text,
  KeyboardAvoidingView,
  Keyboard,
  TouchableWithoutFeedback,
  Platform,
} from 'react-native';
import TextInput from '../Components/TextInput';
import TextButton from '../Components/TextButton';
import GradientFlatButton from '../Components/GradientFlatButton';
import {Form as styles} from '../StyleSheets/Form';
import windowPadding from '../StyleSheets/WindowPadding';
import {RootStackParamList} from '../Navigation/StackNavigator';
import {StackNavigationProp} from '@react-navigation/stack';
import RootStoreContext from '../Stores/RootStore';
import {observer} from 'mobx-react-lite';

interface Props {
  navigation: StackNavigationProp<RootStackParamList>
}

const HelpScreen = observer(({navigation}: Props): JSX.Element => {
  const store = useContext(RootStoreContext).auth;
  
  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <View style={styles.inner}>
          <View>
            <Text style={[styles.title, {fontSize: 30, fontFamily: 'DosisBold'}]}>Recover password</Text>
          </View>
          <TextInput
            value={store.email}
            placeholder={'Email...'}
            onChange={(val) => store.email = val}
            containerStyle={styles.input}
          />
          <GradientFlatButton
            value={'Envoyer un mail'}
            width={325}
            containerStyle={{margin: 10}}
            onPress={store.resetPassword}
          />
          <View style={{flexDirection: 'row'}}>
            <TextButton
              value={'Connexion'}
              containerStyle={{
                ...styles.textButton,
                marginHorizontal: windowPadding,
                alignItems: 'flex-end'
              }}
              onPress={() => navigation.navigate('Login')}
            />
          </View>
        </View>
      </TouchableWithoutFeedback>
    </KeyboardAvoidingView>
  );
});

export default HelpScreen;
