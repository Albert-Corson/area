import React, {useContext} from 'react'
import {RouteProp} from '@react-navigation/native'
import {StackNavigationProp} from '@react-navigation/stack'
import {WebView, WebViewNavigation} from 'react-native-webview'
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


  const onNavigationStateChange = (state: WebViewNavigation) => {
    const success = state.url.match(/.*successful=(true|false)/)

    if (success && success[1] === 'true') {
      if (widgetId >= 0) {
        store.subscribeToWidget(widgetId)
        
        navigation.goBack()
        // navigation.reset({
        //   index: 0,
        //   routes: [{name: 'Dashboard'}],
        // })
      }
    }
  }

  return (
    <SafeAreaView style={{height: '100%'}}>
      <WebView
        source={{uri: authUrl}}
        onNavigationStateChange={onNavigationStateChange}
      >
      </WebView>
    </SafeAreaView>
  )
})

export default ServiceAuthScreen