import React from 'react'
import {observer} from 'mobx-react-lite'
import {View, Text, StyleSheet} from 'react-native'
import {State, TapGestureHandlerGestureEvent} from 'react-native-gesture-handler'
import {RootStore} from '../Stores/RootStore'
import WidgetListContainer from './WidgetListContainer'
import Widget from './Widget'
import StaticContainer from './StaticContainer'
import {StackNavigationProp} from '@react-navigation/stack'
import {RootStackParamList} from '../Navigation/StackNavigator'
import ServiceLoginPrompt from './ServiceLoginPrompt'
import Animated, {useAnimatedStyle, useSharedValue, withSpring} from 'react-native-reanimated'

interface WidgetSelectorProps {
  store: RootStore;
  navigation: StackNavigationProp<RootStackParamList>;
}

const WidgetSelector = observer(({store, navigation}: WidgetSelectorProps): JSX.Element => {
  const {availableWidgets} = store.widget
  const opacity = useSharedValue<number>(1)

  const opacityStyle = useAnimatedStyle(() => ({
    opacity: opacity.value
  }))

  const onTap = (e: TapGestureHandlerGestureEvent, index: number): void => {
    if (e.nativeEvent.state !== State.ACTIVE) return

    const {availableWidgets} = store.widget

    if (index < 0 || index >= availableWidgets.length) return

    const widget = availableWidgets[index]

    if (widget.requires_auth) {
      store.widget.currentWidget = widget
      opacity.value = withSpring(.3)
    } else {
      store.widget.subscribeToWidget(widget.id)
    }
  }

  const onPromptPress = async () => {
    //const authUrl = await store.widget.serviceAuthentication()

    opacity.value = withSpring(1)
    
    navigation.navigate('ServiceAuth', {
      authUrl: `/services/${store.widget.currentWidget?.service.id}/auth`,
      widgetId: store.widget.currentWidget?.id ?? -1
    })

    store.widget.currentWidget = null
  }

  const onPromptCancel = () => {
    store.widget.currentWidget = null
    opacity.value = withSpring(1)

  }


  return (
    <>
      <Animated.View style={opacityStyle}>
        <WidgetListContainer containerStyle={[styles.container, styles.selector]} bounce={false}>
          {!availableWidgets.length && (
            <Text style={styles.title}>{'No new widget available, you got\'em all!'}</Text>
          )}
          {availableWidgets.map((widget, index) => (
            <StaticContainer
              key={index}
              onTap={onTap}
              index={index}
              renderItem={() => <Widget item={widget} subscribed={false} />}
            />
          ))}
        </WidgetListContainer>
      </Animated.View>

      <ServiceLoginPrompt
        onPress={onPromptPress}
        onCancel={onPromptCancel}
        service={store.widget.currentWidget?.service || null}
      />
    </>
  )
})

const WidgetSelectorHeader = (): JSX.Element => (
  <View style={[styles.header]}>
    <View style={styles.handle} />
  </View>
)

export {WidgetSelector, WidgetSelectorHeader}

const styles = StyleSheet.create({
  selector: {
    backgroundColor: '#ebebeb',
    height: 800,
  },
  header: {
    backgroundColor: '#ebebeb',

    borderColor: '#ebebeb',
    borderWidth: 2,
    borderRadius: 50,

    width: '100%',
    height: 30,

    justifyContent: 'center',
    alignItems: 'center',

    shadowColor: '#666',
    shadowOffset: {
      width: -10,
      height: -10,
    },
    shadowOpacity: 0.25,
    shadowRadius: 5,
  },
  handle: {
    height: 5,
    width: 60,
    borderTopLeftRadius: 50,
    borderTopRightRadius: 50,
    backgroundColor: 'grey',
  },
  container: {
    width: 'auto',
    alignItems: 'flex-start',
    justifyContent: 'flex-start',

    backgroundColor: '#e7e7e7',

    marginHorizontal: 7.5,

    flexDirection: 'row',
    flexWrap: 'wrap',

    paddingBottom: 50,
  },
  title: {
    width: '100%',
    textAlign: 'center',
    fontSize: 20,
    fontFamily: 'DosisBold',
  },
})
