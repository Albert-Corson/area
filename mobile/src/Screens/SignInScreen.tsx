import React, {useContext,} from 'react';
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
import {RootStackParamList} from '../Navigation/StackNavigator';
import {StackNavigationProp} from '@react-navigation/stack';
import RootStoreContext from '../Stores/RootStore';
import { observer } from 'mobx-react-lite';

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const SignInScreen = observer(({navigation}: Props): JSX.Element => {
  const store = useContext(RootStoreContext).auth;

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <View style={{flex: 1, justifyContent: 'space-between'}}>
          <View style={styles.inner}>
            <Text style={styles.title}>Area</Text>
            <Text style={styles.error}>{store.error}</Text>
            <TextInput
              value={store.email}
              placeholder={'Email...'}
              onChange={(val) => store.email = val}
              containerStyle={styles.input}
            />
            <TextInput
              value={store.password}
              placeholder={'Password...'}
              onChange={(val) => store.password = val}
              containerStyle={styles.input}
              secure={true}
            />
            <GradientFlatButton
              value={'Sign in'}
              width={325}
              containerStyle={{margin: 10}}
              onPress={async () => await store.signIn()}
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
              containerStyle={{...styles.textButton, marginBottom: 25}}
              onPress={() => navigation.navigate('Help')}
            />
          </View>
        </View>
      </TouchableWithoutFeedback>
    </KeyboardAvoidingView>
  );
});

export default SignInScreen;
