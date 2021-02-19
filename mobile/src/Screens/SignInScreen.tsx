import React, {useContext} from 'react'
import {
  View,
  Text,
  KeyboardAvoidingView,
  Keyboard,
  TouchableWithoutFeedback,
  Platform,
} from 'react-native'
import {StackNavigationProp} from '@react-navigation/stack'
import {observer} from 'mobx-react-lite'
import TextInput from '../Components/TextInput'
import TextButton from '../Components/TextButton'
import GradientFlatButton from '../Components/GradientFlatButton'
import {Form as styles} from '../StyleSheets/Form'
import {RootStackParamList} from '../Navigation/StackNavigator'
import RootStoreContext from '../Stores/RootStore'

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const SignInScreen = observer(({navigation}: Props): JSX.Element => {
  const store = useContext(RootStoreContext)

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <View style={{flex: 1, justifyContent: 'space-between'}}>
          <View style={styles.inner}>
            <Text style={styles.title}>Area</Text>
            <Text style={styles.error}>{store.auth.error}</Text>
            <TextInput
              value={store.auth.username}
              placeholder="Email or username..."
              onChange={(val) => store.auth.username = val}
              containerStyle={styles.input}
            />
            <TextInput
              value={store.auth.password}
              placeholder="Password..."
              onChange={(val) => store.auth.password = val}
              containerStyle={styles.input}
              secure
            />
            <GradientFlatButton
              value="Sign in"
              width={325}
              containerStyle={{margin: 10}}
              onPress={async () => {
                // navigation.navigate('Dashboard');
                // return;
                if (await store.auth.signIn()) {
                  navigation.navigate('Dashboard')
                }
              }}
            />
            <TextButton
              value="Register"
              style={{fontSize: 19}}
              containerStyle={styles.textButton}
              onPress={() => navigation.navigate('SignUp')}
            />
          </View>
          <View>
            <TextButton
              value="Forgot password ?"
              style={{fontSize: 15}}
              containerStyle={{...styles.textButton, marginBottom: 25}}
              onPress={() => navigation.navigate('Help')}
            />
          </View>
        </View>
      </TouchableWithoutFeedback>
    </KeyboardAvoidingView>
  )
})

export default SignInScreen
