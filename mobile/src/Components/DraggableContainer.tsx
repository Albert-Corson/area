import React, {useContext, useRef} from 'react'
import {
  Dimensions,
  StyleSheet,
  View,
  TouchableOpacity,
  Text,
} from 'react-native'
import Animated, {
  useAnimatedStyle,
  useSharedValue,
  useAnimatedGestureHandler,
  withSpring,
  runOnJS,
} from 'react-native-reanimated'
import {
  PanGestureHandler,
  TapGestureHandler,
  State,
  TapGestureHandlerGestureEvent,
  GestureHandlerGestureEventNativeEvent,
  PanGestureHandlerEventExtra,
} from 'react-native-gesture-handler'
import {observer} from 'mobx-react-lite'
import {Entypo} from '@expo/vector-icons'
import {Size} from '../Types/Block'
import RootStoreContext from '../Stores/RootStore'
import Grid from '../Tools/Grid'
import DropShadowContainer from './DropShadowContainer'
import {TimePicker} from 'react-native-simple-time-picker'
import ModalContainer from './ModalContainer'
import FlatButton from './FlatButton'
import {pure} from 'recompose'

type Event = GestureHandlerGestureEventNativeEvent & PanGestureHandlerEventExtra;

interface Props {
  index: number;
  name: string;
  children?: React.ReactChild;
}

const DraggableContainer = observer(({
  index,
  children,
  name,
}: Props): JSX.Element => {
  const store = useContext(RootStoreContext)
  // drag
  const translateY = useSharedValue<number>(0)
  const translateX = useSharedValue<number>(0)
  const elevation = useSharedValue<number>(1)
  // multiple gesture handlers refs
  const panRef = useRef(null)
  const tapRef = useRef(null)
  // block info
  const size = store.grid.getBlockSize(index)
  const gridSize = store.grid.blocks.length
  const modifying = store.grid.isBlockMutable(index)

  const item = store.widget.subscribedWidgets[index]

  const SIZE = Dimensions.get('window').width / 2.5
  const MARGIN = (SIZE * (2.5 - 2.14)) / 4

  const onDrop = (index: number, offsetX: number, offsetY: number): void => {
    const [x, y]: number[] = [index % 2, Math.floor(index / 2)]
    const hoveredBlockIndex: number = Grid.getHoveredBlockIndex(x, y, offsetX, offsetY)

    if (
      index != hoveredBlockIndex
      && hoveredBlockIndex >= 0
      && hoveredBlockIndex < store.grid.blocks.length
    ) {
      store.grid.swithAtIndexes(index, hoveredBlockIndex)
    }
  }

  const onStartDrag = (_: Event, ctx: any) => {
    'worklet'

    elevation.value = gridSize + 1

    ctx.offsetX = translateX.value
    ctx.offsetY = translateY.value
  }

  const onActiveDrag = (event: Event, ctx: any) => {
    'worklet'

    translateX.value = event.translationX + ctx.offsetX
    translateY.value = event.translationY + ctx.offsetY
  }

  const onEndDrag = () => {
    'worklet'

    runOnJS(onDrop)(index, translateX.value, translateY.value)

    translateY.value = withSpring(0, {
      damping: 60,
      stiffness: 500,
    })
    translateX.value = withSpring(0, {
      damping: 60,
      stiffness: 500,
    })
    elevation.value = gridSize
  }

  const onDragEvent = useAnimatedGestureHandler({
    onStart: onStartDrag,
    onActive: onActiveDrag,
    onEnd: onEndDrag,
  })

  const onTap = (e: TapGestureHandlerGestureEvent): void => {
    if (e.nativeEvent.state !== State.ACTIVE) return

    store.grid.setBlockSize(index, size != Size.full ? size << 1 : Size.normal)
  }

  const deleteWidget = () => {
    store.widget.unsubscribeToWidget(store.grid.blocks[index].id)
  }

  const changeRefreshTime = () => {
    store.widget.setRefreshDelay()
    store.grid.closeTimePicker()
  }

  const dragStyle = useAnimatedStyle(() => ({
    transform: [
      {translateX: translateX.value},
      {translateY: translateY.value},
    ],
  }))

  const boxStyle = useAnimatedStyle(() => ({
    zIndex: elevation.value,
    elevation: elevation.value,
  }))

  const widgetStyle = {
    ...styles.box,
    width: size !== Size.normal ? SIZE * 2.14 : SIZE,
    height: size === Size.full ? SIZE * 2.14 : SIZE,
  }

  if (store.grid.blocks[index].unactive) {
    return (
      <View style={[
        widgetStyle,
        {
          backgroundColor: 'transparent',
          margin: MARGIN,
        },
      ]}
      />
    )
  }

  return (
    <>
      <Animated.View style={[
        {marginHorizontal: MARGIN, marginVertical: MARGIN -  5},
        boxStyle
      ]}>
        {!modifying && <Text style={styles.title}>{name}</Text>}
        <DropShadowContainer>
          {modifying ? (
            <TapGestureHandler
              onHandlerStateChange={onTap}
              numberOfTaps={2}
              ref={tapRef}
              simultaneousHandlers={panRef}
            >
              <Animated.View>
                <PanGestureHandler
                  ref={panRef}
                  simultaneousHandlers={tapRef}
                  maxPointers={1}
                  onGestureEvent={onDragEvent}
                >
                  <Animated.View style={[
                    widgetStyle,
                    dragStyle,
                  ]}
                  >
                    <TouchableOpacity style={[styles.badgeBtn, styles.deleteBtn]} onPress={deleteWidget}>
                      <Entypo name="cross" size={15} color="#e6e6e9" />
                    </TouchableOpacity>

                    <TouchableOpacity style={[styles.badgeBtn, styles.clockBtn]} onPress={() => {
                      store.grid.openTimePicker()
                      store.widget.currentWidget = item
                      store.widget.setCurrentInterval({hours: item.hours || 0, minutes: item.minutes || 1})
                    }}>
                      <Entypo name="clock" size={15} color="#e6e6e9" />
                    </TouchableOpacity>

                    {children}

                  </Animated.View>
                </PanGestureHandler>
              </Animated.View>
            </TapGestureHandler>
          ) : (
            <Animated.View style={widgetStyle}>

              {children}
          
            </Animated.View>
          )}
        </DropShadowContainer>
        <ModalContainer visible={store.grid.isTimePickerVisible()} containerStyle={{height: 300, maxWidth: 350}}>
          <View style={styles.timePickerContainer}>
            <TimePicker value={store.widget.currentInterval} onChange={store.widget.setCurrentInterval} />
          </View>
          <View style={{flexDirection: 'row', justifyContent: 'space-between'}}>
            <FlatButton 
              containerStyle={{margin: 5}}
              value="Confirm" 
              width={150} 
              onPress={changeRefreshTime} 
            />
            <FlatButton 
              containerStyle={{margin: 5}}
              value="Cancel" 
              width={75} 
              onPress={store.grid.closeTimePicker} 
            />
          </View>
        </ModalContainer>
      </Animated.View>
    </>
  )
})

export default pure(DraggableContainer)

const styles = StyleSheet.create({
  timePickerContainer: {
    width: '100%',
    overflow: 'hidden', 
    alignItems: 'center', 
    justifyContent: 'center',
  },
  box: {
    height: 175,
    width: 175,
    borderRadius: 25,
    backgroundColor: '#e6e6e9',
  },
  container: {
    margin: 15,
  },
  badgeBtn: {

    width: 20,
    height: 20,
    borderRadius: 20,
    zIndex: 20,

    position: 'absolute',
    margin: 5,
    justifyContent: 'center',
    alignItems: 'center',
  },
  deleteBtn: {
    right: -10,
    top: -10,

    backgroundColor: '#f04e4677',
  },
  clockBtn: {
    left: -10,
    top: -10,

    backgroundColor: '#2B2B2BCC',
  },
  modBtn: {
    bottom: -10,
    right: -10,

    backgroundColor: '#2B2B2BCC',
  },
  title: {
    textAlign: 'center', 
    fontSize: 11,
    fontFamily: 'DosisLight',
    padding: 0,
    marginTop: 0,
    marginBottom: 2,
  },
})
