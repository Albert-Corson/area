import React, { useContext, useRef } from 'react';
import { Dimensions, StyleSheet, View, TouchableOpacity } from 'react-native';
import Animated, {
  useAnimatedStyle,
  useSharedValue,
  useAnimatedGestureHandler,
  withSpring,
  runOnJS,
} from 'react-native-reanimated';
import {
  PanGestureHandler,
  TapGestureHandler,
  State,
  TapGestureHandlerGestureEvent,
} from 'react-native-gesture-handler';
import { Size } from '../Types/Block';
import RootStoreContext from '../Stores/RootStore';
import Grid from '../Tools/Grid';
import { observer } from 'mobx-react-lite';
import DropShadowContainer from './DropShadowContainer';
import { Entypo } from '@expo/vector-icons';

interface Props {
  index: number;
  renderItem?: () => JSX.Element;
}

const DraggableContainer = observer(({
  index,
  renderItem = () => <></>,
}: Props): JSX.Element => {
  const store = useContext(RootStoreContext);
  //drag
  const translateY = useSharedValue<number>(0);
  const translateX = useSharedValue<number>(0);
  const elevation = useSharedValue<number>(1);
  // multiple gesture handlers refs
  const panRef = useRef(null);
  const tapRef = useRef(null);
  // block info
  const size = store.grid.getBlockSize(index);
  const gridSize = store.grid.blocks.length;
  const modifying = store.grid.isBlockMutable(index);

  const SIZE = Dimensions.get('window').width / 2.5;
  const MARGIN = (SIZE * (2.5 - 2.14)) / 4;

  const onDrop = (index: number, offsetX: number, offsetY: number): void => {
    const [x, y]: number[] = [index % 2, Math.floor(index / 2)];
    const hoveredBlockIndex: number = Grid.getHoveredBlockIndex(x, y, offsetX, offsetY);

    if (
      index != hoveredBlockIndex &&
      hoveredBlockIndex >= 0 &&
      hoveredBlockIndex < store.grid.blocks.length
    ) {
      store.grid.swithAtIndexes(index, hoveredBlockIndex);
    }
  };

  const onStartDrag = (_: any, ctx: any) => {
    'worklet';
    elevation.value = gridSize + 1;

    ctx.offsetX = translateX.value;
    ctx.offsetY = translateY.value;
  };

  const onActiveDrag = (event: any, ctx: any) => {
    'worklet';
    translateX.value = event.translationX + ctx.offsetX;
    translateY.value = event.translationY + ctx.offsetY;
  };

  const onEndDrag = (_: any, ctx: any) => {
    'worklet';
    runOnJS(onDrop)(index, translateX.value, translateY.value);

    translateY.value = withSpring(0, {
      damping: 60,
      stiffness: 500,
    });
    translateX.value = withSpring(0, {
      damping: 60,
      stiffness: 500,
    });
    elevation.value = gridSize;
  };

  const onDragEvent = useAnimatedGestureHandler({
    onStart: onStartDrag,
    onActive: onActiveDrag,
    onEnd: onEndDrag,
  });

  const onTap = (e: TapGestureHandlerGestureEvent): void => {
    if (e.nativeEvent.state !== State.ACTIVE) return;

    store.grid.setBlockSize(index, size != Size.full ? size << 1 : Size.normal);
  };

  const deleteWidget = () => {
    store.widget.unsubscribeToWidget(store.grid.blocks[index].id);
  };

  const dragStyle = useAnimatedStyle(() => {
    return {
      transform: [
        { translateX: translateX.value },
        { translateY: translateY.value },
      ]
    };
  });

  const boxStyle = useAnimatedStyle(() => {
    return {
      zIndex: elevation.value,
      elevation: elevation.value,
    };
  });

  const widgetStyle = {
    ...styles.box,
    width: size !== Size.normal ? SIZE * 2.14 : SIZE,
    height: size === Size.full ? SIZE * 2.14 : SIZE,
  };

  if (store.grid.blocks[index].unactive) {
    return (
      <View style={[
        widgetStyle,
        {
          backgroundColor: 'transparent',
          margin: MARGIN,
        }
      ]}
      ></View>
    );
  }

  return (
    <Animated.View style={[{ margin: MARGIN }, boxStyle]}>
      <DropShadowContainer>
        {modifying ? (
          <TapGestureHandler
            onHandlerStateChange={onTap}
            numberOfTaps={2}
            ref={tapRef}
            simultaneousHandlers={panRef}>
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
                  <TouchableOpacity style={styles.deleteBtn} onPress={deleteWidget}>
                    <Entypo name={'cross'} size={15} color={'#e6e6e9'} />
                  </TouchableOpacity>

                  {renderItem()}

                </Animated.View>
              </PanGestureHandler>
            </Animated.View>
          </TapGestureHandler>
        ) : (
          <Animated.View style={widgetStyle}>
            {renderItem()}
          </Animated.View>
        )}
      </DropShadowContainer>
    </Animated.View>
  );
});

export default DraggableContainer;

const styles = StyleSheet.create({
  box: {
    height: 175,
    width: 175,
    borderRadius: 25,
    backgroundColor: '#e6e6e9'
  },
  container: {
    margin: 15,
  },
  deleteBtn: {
    width: 20,
    height: 20,
    borderRadius: 20,

    zIndex: 1000,

    position: 'absolute',

    right: -10,
    top: -10,
    margin: 5,

    backgroundColor: '#f04e4677',

    justifyContent: 'center',
    alignItems: 'center',
  },
});