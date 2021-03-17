import React, {useRef, useContext, useEffect} from 'react'
import {SafeAreaView, StyleSheet, View} from 'react-native'
import {FontAwesome} from '@expo/vector-icons'
import BottomSheet from 'reanimated-bottom-sheet'
import {observer} from 'mobx-react-lite'
import {StackNavigationProp} from '@react-navigation/stack'
import DraggableContainer from '../Components/DraggableContainer'
import {WidgetSelector, WidgetSelectorHeader} from '../Components/WidgetSelector'
import RootStoreContext from '../Stores/RootStore'
import FlatButton from '../Components/FlatButton'
import {RootStackParamList} from '../Navigation/StackNavigator'
import WidgetListContainer from '../Components/WidgetListContainer'
import Widget from '../Components/Widget'
import GradientFlatButton from '../Components/GradientFlatButton'
import Animated, {useAnimatedStyle, useSharedValue, withSpring} from 'react-native-reanimated'
import * as Device from 'expo-device'

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const WidgetsScreen = observer(({navigation}: Props): JSX.Element => {
  const store = useContext(RootStoreContext)
  const sheetRef = useRef<BottomSheet>(null)

  const opacity = useSharedValue<number>(1)

  const opacityStyle = useAnimatedStyle(() => ({
    opacity: opacity.value
  }))

  useEffect(() => {
    store.widget.updateWidgets()
      .then(store.widget.updateParameters)
  }, [])

  const topBar: JSX.Element = (
    <View style={styles.modifier}>
      <FlatButton
        height="100%"
        width={60}
        value={() => (
          <FontAwesome name="plus" size={17} color="#666666" />
        )}
        onPress={() => {
          if (store.grid.modifying) {
            store.grid.toggleEditionMode()
          }

          opacity.value = withSpring(store.grid.adding ? 1 : 0)
          sheetRef.current!.snapTo(store.grid.adding ? 1 : 0)

          store.grid.toggleAdditionMode()
        }}
        active={store.grid.adding}
      />

      <GradientFlatButton
        height="100%"
        width={120}
        value={store.user.userJWT?.username ?? 'Profile'}
        onPress={async () => {
          await store.user.loadUser()
          navigation.navigate('Profile')
        }}
      />

      <FlatButton
        height="100%"
        width={60}
        value={() => (
          <FontAwesome name="pencil-square-o" size={17} color="#666666" />
        )}
        onPress={() => {
          store.grid.toggleEditionMode()
          if (store.grid.adding) {
            sheetRef.current!.snapTo(2)
            store.grid.toggleAdditionMode()
          }
        }}
        active={store.grid.modifying}
      />
    </View>
  )

  return (
    <>
      <SafeAreaView style={styles.safeView}>
        {topBar}
        <Animated.View style={opacityStyle}>
          <WidgetListContainer containerStyle={[
            styles.container,
            {
              marginLeft: Device?.modelName?.includes('iPad') ? 25 : 10
            }
          ]}>
            {store.grid.blocks.map((widget, index) => (
              <DraggableContainer
                key={widget.id || `fillBlock${index}`}
                index={index}
                name={widget.name}
              >
                <Widget
                  item={widget}
                  widgetStore={store.widget}
                  subscribed={!store.grid.modifying}
                  modifying={store.grid.modifying}
                  size={store.grid.getBlockSize(index)}
                  display={widget.display}
                />
              </DraggableContainer>
            ))}
          </WidgetListContainer>
        </Animated.View>
      </SafeAreaView>
      <BottomSheet
        ref={sheetRef}
        snapPoints={['90%', '0%']}
        initialSnap={1}
        renderContent={() => (<WidgetSelector store={store} navigation={navigation} sheetRef={sheetRef} />)}
        renderHeader={WidgetSelectorHeader}
        onOpenStart={store.grid.toggleAdditionMode}
        onCloseStart={() => {
          store.grid.toggleAdditionMode()
          opacity.value = withSpring(1)
        }}
      />
    </>
  )
})

export default WidgetsScreen

const styles = StyleSheet.create({
  safeView: {
    backgroundColor: '#e7e7e7',
    height: '100%',
  },
  container: {
    width: 'auto',
    alignItems: 'flex-start',
    justifyContent: 'flex-start',

    flexDirection: 'row',
    flexWrap: 'wrap',

    paddingBottom: 50,
  },
  modifier: {
    height: 30,
    justifyContent: 'space-between',

    flexDirection: 'row',
    marginHorizontal: 30,
    marginVertical: 15,
  },
})
