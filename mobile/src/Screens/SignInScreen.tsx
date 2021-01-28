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
import {Auth} from '../Api/API';
import {RootStackParamList} from '../Navigation/StackNavigator';
import {StackNavigationProp} from '@react-navigation/stack';

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

interface State {
  email: string;
  password: string;
}

const SignInScreen = ({navigation}: Props): JSX.Element => {
  const [credentials, setCredentials] = useState<State>({
    email: '',
    password: '',
  });
  const [error, setError] = useState<string>('');

  const onPress = (): void => {
    /*
     * Only for testing purposes
     */
    navigation.navigate('Widgets');
    return;

    if (!credentials.email.length || !credentials.password.length) {
      setError('You must type your email and password');
      return;
    }

    Auth.SignIn(credentials)
      .then(({status, body}) => {
        console.log(body);
        if (status >= 400) {
          setError(body?.detail ?? 'An error occurred');
        } else {
          navigation.navigate('Widgets');
          setError('');
          setCredentials({email: '', password: ''});
        }
      })
      .catch(() => setError('An error occurred'));
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <View style={{flex: 1, justifyContent: 'space-between'}}>
          <View style={styles.inner}>
            <Text style={styles.title}>Area</Text>
            <Text style={styles.error}>{error}</Text>
            <TextInput
              value={credentials.email}
              placeholder={'Email...'}
              onChange={(email) => setCredentials({...credentials, email: email.toLowerCase()})}
              containerStyle={styles.input}
            />
            <TextInput
              value={credentials.password}
              placeholder={'Password...'}
              onChange={(password) => setCredentials({...credentials, password})}
              containerStyle={styles.input}
              secure={true}
            />
            <GradientFlatButton
              value={'Sign in'}
              width={325}
              containerStyle={{margin: 10}}
              onPress={onPress}
            />
            <TextButton
              value={'Register'}
              style={{fontSize: 19}}
              containerStyle={styles.textButton}
              onPress={() => navigation.navigate('SignUp')}
            />
          </View>
          <View>
            <TextButton
              value={'Forgot password ?'}
              style={{fontSize: 15}}
              containerStyle={[styles.textButton, {marginBottom: 25}]}
              onPress={() => navigation.navigate('Help')}
            />
          </View>
        </View>
      </TouchableWithoutFeedback>
    </KeyboardAvoidingView>
  );
};

export default SignInScreen;
