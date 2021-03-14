import React, {useContext, useState, useEffect} from 'react'
import {observer} from 'mobx-react-lite'
import {View, Text, StyleSheet, Dimensions} from 'react-native'
import {RootStore} from '../Stores/RootStore'
import WidgetListContainer from './WidgetListContainer'
import Widget from './Widget'
import {StackNavigationProp} from '@react-navigation/stack'
import {RootStackParamList} from '../Navigation/StackNavigator'
import ServiceLoginPrompt from './ServiceLoginPrompt'
import Animated, {useAnimatedStyle, useSharedValue, withSpring} from 'react-native-reanimated'
import RootStoreContext from '../Stores/RootStore'
import {RefreshableWidget} from '../Stores/WidgetStore'
import {TouchableOpacity} from 'react-native-gesture-handler'
import DropShadowContainer from './DropShadowContainer'
import FlatButton from './FlatButton'
import GradientFlatButton from './GradientFlatButton'
import ModalContainer from './ModalContainer'

interface WidgetSelectorProps {
  store: RootStore;
  navigation: StackNavigationProp<RootStackParamList>;
  sheetRef: any;
}

const WidgetSelector = observer(({store, navigation, sheetRef}: WidgetSelectorProps): JSX.Element => {
  const widgetStore = store.widget
  const [showModal, setShowModal] = useState<boolean>(false)

  const onTap = (index: number): void => {
    const {subscribedWidgets, availableWidgets} = store.widget

    if (index < 0 || index >= availableWidgets.length) return

    const widget = availableWidgets[index]

    if (subscribedWidgets.includes(widget)) return

    if (widget.requires_auth) {
      store.widget.currentWidget = widget
      setShowModal(!!(store.widget.currentWidget?.service != null))
    } else {
      store.widget.subscribeToWidget(widget.id)
    }
  }

  const onPromptPress = async () => {
    const authUrl = await store.widget.serviceAuthentication()

    // sheetRef?.current?.snapTo(1)

    navigation.navigate('ServiceAuth', {
      authUrl: authUrl ?? '',
      widgetId: store.widget.currentWidget?.id ?? -1
    })

    store.widget.currentWidget = null
    setShowModal(false)
  }

  const onPromptCancel = () => {
    store.widget.currentWidget = null
    setShowModal(false)
  }

  const SIZE = Dimensions.get('window').width / 2.5
  const MARGIN = ((SIZE * (2.5 - 2.14)) / 4)
  
  const Widgets = () => (
    <>
      {widgetStore.availableWidgets.filter(widget => widget.name).map((widget, index) => (
        <TouchableOpacity
          key={index}
          activeOpacity={.5}
          onPressOut={() => onTap(index)}
        >
          <DropShadowContainer>
            <View style={[{width: SIZE, height: SIZE, margin: MARGIN, zIndex: 11}]}>
              <Widget widgetStore={widgetStore} item={widget} subscribed={false} />
            </View>
          </DropShadowContainer>
        </TouchableOpacity>
      ))}
    </>
  )

  return (
    <>
      <ModalContainer visible={showModal}>
        <View style={styles.verticalContainer}>
          <Text style={styles.title}>{`${store.widget.currentWidget?.service?.name} requires authentication`}</Text>
          <View style={styles.btnContainer}>
            <GradientFlatButton
              value={'Login'}
              width={150}
              onPress={onPromptPress}
            />
            <FlatButton
              value={'Cancel'}
              width={75}
              onPress={onPromptCancel}
            />
          </View>
        </View>
      </ModalContainer>

      <WidgetListContainer containerStyle={[styles.container, styles.selector]} bounce={false}>
        {!widgetStore.availableWidgets.length && (
          <Text style={styles.headerTitle}>{'No new widget available, you got\'em all!'}</Text>
        )}
        <Widgets />
      </WidgetListContainer>
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
  headerTitle: {
    width: '100%',
    textAlign: 'center',
    fontSize: 20,
    fontFamily: 'DosisBold',
    marginHorizontal: 20,
  },
  box: {
    borderRadius: 25,
    backgroundColor: '#e6e6e9',
  },
  verticalContainer: {
    flex: 1,
    flexDirection: 'column',
  },
  title: {
    width: '100%',
    textAlign: 'center',
    fontSize: 20,
    fontFamily: 'DosisBold',
    marginBottom: 25,
  },
  btnContainer: {
    flex: 1,
    flexDirection: 'row',
    justifyContent: 'space-between'
  },
})
