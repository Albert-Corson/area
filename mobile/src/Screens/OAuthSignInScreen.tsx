import React, {useContext} from 'react'
import {RouteProp} from '@react-navigation/native'
import {StackNavigationProp} from '@react-navigation/stack'
import {WebView, WebViewNavigation} from 'react-native-webview'
import {RootStackParamList} from '../Navigation/StackNavigator'
import {SafeAreaView} from 'react-native-safe-area-context'
import RootStoreContext from '../Stores/RootStore'
import {observer} from 'mobx-react-lite'

interface Props {
    navigation: StackNavigationProp<RootStackParamList, 'OAuthSignIn'>;
    route: RouteProp<RootStackParamList, 'OAuthSignIn'>;
}


const OAuthSignInScreen = observer(({navigation, route}: Props): JSX.Element => {
  const store = useContext(RootStoreContext)
  const {url} = route.params

  const onNavigationStateChange = async (state: WebViewNavigation) => {
    const success = state.url.match(/.*successful=(true|false)/)
    const code = state.url.match(/.*code=([a-zA-Z0-9.\-_]*).*/)

    if (!success || success[1] !== 'true' || !code) return

    const successulTokenClaim = await store.auth.askForTokens(code[1])

    if (successulTokenClaim) {
      navigation.reset({
        index: 0,
        routes: [{name: 'Dashboard'}],
      })
    }
  }

  return (
    <SafeAreaView style={{height: '100%'}}>
      <WebView
        source={{uri: url || ''}}
        userAgent="Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_1 like Mac OS X; en-us) AppleWebKit/532.9 (KHTML, like Gecko) Version/4.0.5 Mobile/8B117 Safari/6531.22.7 (compatible; Googlebot-Mobile/2.1; +http://www.google.com/bot.html)"
        onNavigationStateChange={onNavigationStateChange}
        incognito
      >
      </WebView>
    </SafeAreaView>
  )
})

export default OAuthSignInScreen