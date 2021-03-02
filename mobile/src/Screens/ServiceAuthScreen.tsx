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
  const widgetStore = useContext(RootStoreContext).widget
  const userStore = useContext(RootStoreContext).user
  const {authUrl, widgetId} = route.params

  const onMessage = async () => {
    if (widgetId >= 0) {
      await widgetStore.subscribeToWidget(widgetId)
    }
    navigation.goBack()
  }

  return (
    <SafeAreaView style={{height: '100%'}}>
      <WebView
        javaScriptEnabled={true}
        injectedJavaScript={'window.ReactNativeWebView.postMessage(document.body.innerHTML)'}
        source={{uri: `http://${API_HOST}:${API_PORT}/api${authUrl}?token=${userStore.userJWT?.accessToken}&redirect_url=https://google.com`}}
        onMessage={onMessage}>
      </WebView>
    </SafeAreaView>
  )
})

export default ServiceAuthScreen