import React, {useContext} from 'react'
import {RouteProp} from '@react-navigation/native'
import {StackNavigationProp} from '@react-navigation/stack'
import {WebView, WebViewMessageEvent, WebViewNavigation} from 'react-native-webview'
import {RootStackParamList} from '../Navigation/StackNavigator'
import {SafeAreaView} from 'react-native-safe-area-context'
import RootStoreContext from '../Stores/RootStore'
import {observer} from 'mobx-react-lite'
import {API_HOST, API_PORT} from '@env'

interface Props {
    navigation: StackNavigationProp<RootStackParamList, 'ServiceAuth'>;
    route: RouteProp<RootStackParamList, 'ServiceAuth'>;
}

const ServiceAuthScreen = observer(({navigation, route}: Props): JSX.Element => {
  const userStore = useContext(RootStoreContext).user
  const {
    authUrl, 
    callback, 
    tokenRequired, 
    method,
    body,
  } = route.params

  return (
    <SafeAreaView style={{height: '100%'}}>
      <WebView
        source={{
          uri: `http://${API_HOST}:${API_PORT}/api${authUrl}?${tokenRequired ? `token=${userStore.userJWT?.accessToken}&` : ''}redirect_url=https://google.com`,
          method,
          body,
          headers: {
            'Content-Type': 'application/json',
          }
        }}
        userAgent="Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_1 like Mac OS X; en-us) AppleWebKit/532.9 (KHTML, like Gecko) Version/4.0.5 Mobile/8B117 Safari/6531.22.7 (compatible; Googlebot-Mobile/2.1; +http://www.google.com/bot.html)"
        onNavigationStateChange={callback}
      >
      </WebView>
    </SafeAreaView>
  )
})

export default ServiceAuthScreen