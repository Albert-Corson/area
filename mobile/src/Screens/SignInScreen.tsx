/* eslint-disable react/display-name */
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
import FlatButton from '../Components/FlatButton'
import {FontAwesome, FontAwesome5} from '@expo/vector-icons'
import {WebViewNavigation} from 'react-native-webview'

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

interface buttonDefinition {
  icon: () => JSX.Element;
  color: string;
  url: string;
}

const SOCIAL_BTNS: buttonDefinition[] = [
  {
    url: '/auth/facebook',
    icon: () => <FontAwesome name="facebook" size={17} color="#666666" />,
    color: '#4267B280',

  },
  {
    url: '/auth/google',
    icon: () => <FontAwesome name="google" size={17} color="#666666" />,
    color: '#DB443780'
  },
  {
    url: '/auth/microsoft',
    icon: () => <FontAwesome5 name="microsoft" size={17} color="#666666" />,
    color: '#73737380'
  },
]

const SignInScreen = observer(({navigation}: Props): JSX.Element => {
  const store = useContext(RootStoreContext)

  const onPress = (authUrl: string) => navigation.navigate('ServiceAuth', {
    authUrl, 
    callback: async (state: WebViewNavigation) => {
      const success = state.url.match(/.*successful=(true|false)/)
      const code = state.url.match(/.*code=([a-zA-Z0-9.-_]*).*/)

      if (!success || !code) return

      if (success[1] === 'true' && await store.auth.askForTokens(code[1])) {
        navigation.navigate('Dashboard')
      } else {
        navigation.navigate('Login')
      }
    },
    method: 'post',
    body: JSON.stringify({
      redirect_url: 'https://google.fr',
      state: null,
    }),
  })

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
                if (await store.auth.signIn()) {
                  navigation.navigate('Dashboard')
                }
              }}
            />
            <View style={{flexDirection: 'row', justifyContent: 'space-around', width: 335, margin: 5}}>
              {SOCIAL_BTNS.map((btn, i) => (
                <FlatButton
                  key={i}
                  height={50}
                  width={100}
                  containerStyle={{borderRadius: 15, backgroundColor: btn.color}}
                  value={btn.icon}
                  onPress={() => onPress(btn.url)}
                />
              ))}
            </View>
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
