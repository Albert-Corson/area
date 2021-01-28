import React, {useState} from 'react';
import {
  Text,
  KeyboardAvoidingView,
  TouchableWithoutFeedback,
  Keyboard,
  View,
  Platform,
} from 'react-native';
import TextInput from '../Components/TextInput';
import GradientFlatButton from '../Components/GradientFlatButton';
import TextButton from '../Components/TextButton';
import {Form as styles} from '../StyleSheets/Form';
import windowPadding from '../StyleSheets/WindowPadding';
import Auth from '../Api/Auth';
import {StackNavigationProp} from '@react-navigation/stack';
import {RootStackParamList} from '../Navigation/StackNavigator';

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

interface Credentials {
  email: string;
  firstName: string;
  lastName: string;
  userName: string;
  password: string;
}

const SignUpScreen = ({navigation}: Props) => {
  const [credentials, setCredentials] = useState<Credentials>({
    email: '',
    firstName: '',
    lastName: '',
    userName: '',
    password: '',
  });
  const [error, setError] = useState<string>('');

  const onPress = () => {
    if (Object.keys(credentials).filter((key) => !credentials[key].length).length) {
      setError('You must complete complete form');
      return;
    }
    Auth.SignUp(credentials)
      .then(({status, body}) => {
        if (status < 400) {
          navigation.navigate('Login');
          setError('');
        } else {
          setError(body?.detail ?? 'An error occurred');
        }
      })
      .catch(_ => setError('An error occurred'));
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <View style={styles.inner}>
          <Text style={{...styles.title, fontSize: 60}}>Register</Text>
          <Text style={styles.error}>{error}</Text>
          <TextInput
            value={credentials.email}
            placeholder={'Email...'}
            onChange={(email) => setCredentials({...credentials, email})}
            containerStyle={styles.input}
          />
          <TextInput
            value={credentials.userName}
            placeholder={'Pseudo...'}
            onChange={(userName) => setCredentials({...credentials, userName})}
            containerStyle={styles.input}
          />
          <View style={{flexDirection: 'row'}}>
            <TextInput
              value={credentials.firstName}
              placeholder={'Firstname...'}
              onChange={(firstName) => setCredentials({...credentials, firstName})}
              containerStyle={{...styles.input, width: '47%'}}
            />
            <TextInput
              value={credentials.lastName}
              placeholder={'Lastname...'}
              onChange={(lastName) => setCredentials({...credentials, lastName})}
              containerStyle={{...styles.input, width: '47%'}}
            />
          </View>
          <TextInput
            value={credentials.password}
            placeholder={'Password...'}
            onChange={(password) => setCredentials({...credentials, password})}
            containerStyle={styles.input}
            secure={true}
          />
          <GradientFlatButton
            value={'Sign up'}
            width={325}
            onPress={onPress}
            containerStyle={{margin: 10}}
          />
          <View style={{flexDirection: 'row'}}>
            <TextButton
              value={'Already registered ?'}
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

export default SignUpScreen;
