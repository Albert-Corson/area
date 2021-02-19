import React, {useContext} from 'react'
import {RouteProp} from '@react-navigation/native'
import {StackNavigationProp} from '@react-navigation/stack'
import {WebView, WebViewMessageEvent, WebViewNavigation} from 'react-native-webview'
import {RootStackParamList} from '../Navigation/StackNavigator'
import {SafeAreaView} from 'react-native-safe-area-context'
import RootStoreContext from '../Stores/RootStore'
import {observer} from 'mobx-react-lite'

interface Props {
    navigation: StackNavigationProp<RootStackParamList, 'ServiceAuth'>;
    route: RouteProp<RootStackParamList, 'ServiceAuth'>;
}

const ServiceAuthScreen = observer(({navigation, route}: Props): JSX.Element => {
  const store = useContext(RootStoreContext).widget
  const {authUrl, widgetId} = route.params

  const onMessage = (event: WebViewMessageEvent) => {
    const {data} = event.nativeEvent

    if (data.includes('Success! You can now close this page!')) {
      if (widgetId >= 0) {
        store.subscribeToWidget(widgetId)
      }
      navigation.goBack()
    }
  }

  return (
    <SafeAreaView style={{height: '100%'}}>
      <WebView
        javaScriptEnabled={true}
        injectedJavaScript={'window.ReactNativeWebView.postMessage(document.body.innerHTML)'}
        source={{uri: authUrl}}
        onMessage={onMessage}>
      </WebView>
    </SafeAreaView>
  )
})

export default ServiceAuthScreen