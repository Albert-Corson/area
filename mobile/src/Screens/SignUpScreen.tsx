import React, {useContext} from 'react';
import {
  Text,
  KeyboardAvoidingView,
  TouchableWithoutFeedback,
  Keyboard,
  View,
  Platform,
} from 'react-native';
import {StackNavigationProp} from '@react-navigation/stack';
import {observer} from 'mobx-react-lite';
import TextInput from '../Components/TextInput';
import GradientFlatButton from '../Components/GradientFlatButton';
import TextButton from '../Components/TextButton';
import {Form as styles} from '../StyleSheets/Form';
import windowPadding from '../StyleSheets/WindowPadding';
import {RootStackParamList} from '../Navigation/StackNavigator';
import RootStoreContext from '../Stores/RootStore';

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const SignUpScreen = observer(({navigation}: Props) => {
  const store = useContext(RootStoreContext).auth;

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <View style={styles.inner}>
          <Text style={{...styles.title, fontSize: 60}}>Register</Text>
          <Text style={styles.error}>{store.error}</Text>
          <TextInput
            value={store.email}
            placeholder="Email..."
            onChange={(val) => store.email = val}
            containerStyle={styles.input}
          />
          <TextInput
            value={store.username}
            placeholder="Username..."
            onChange={(val) => store.username = val}
            containerStyle={styles.input}
          />
          <TextInput
            value={store.password}
            placeholder="Password..."
            onChange={(val) => store.password = val}
            containerStyle={styles.input}
            secure
          />
          <TextInput
            value={store.confirm}
            placeholder="Confirm password..."
            onChange={(val) => store.confirm = val}
            containerStyle={styles.input}
            secure
          />
          <GradientFlatButton
            value="Sign up"
            width={325}
            onPress={async () => {
              if (await store.signUp()) {
                navigation.navigate('Login');
              }
            }}
            containerStyle={{margin: 10}}
          />
          <View style={{flexDirection: 'row'}}>
            <TextButton
              value="Already registered ?"
              containerStyle={{
                ...styles.textButton,
                marginHorizontal: windowPadding,
                alignItems: 'flex-end',
              }}
              onPress={() => navigation.navigate('Login')}
            />
          </View>
        </View>
      </TouchableWithoutFeedback>
    </KeyboardAvoidingView>
  );
});

export default SignUpScreen;
