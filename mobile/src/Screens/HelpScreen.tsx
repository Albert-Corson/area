import React, {useState} from 'react';
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

interface Props {
  navigation: StackNavigationProp<RootStackParamList>
}

const HelpScreen = ({navigation}: Props): JSX.Element => {
  const [email, setEmail] = useState('');

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
            value={email}
            placeholder={'Email...'}
            onChange={setEmail}
            containerStyle={styles.input}
          />
          <GradientFlatButton
            value={'Envoyer un mail'}
            width={325}
            containerStyle={{margin: 10}}
            onPress={() => console.log('not implemented')}
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
};

export default HelpScreen;
