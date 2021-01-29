import React, {useContext, useRef} from 'react';
import {StyleSheet} from 'react-native';
import Animated, {
  useAnimatedStyle,
  useSharedValue,
  useAnimatedGestureHandler,
  withSpring,
  runOnJS,
} from 'react-native-reanimated';
import {PanGestureHandler, TapGestureHandler, State, TapGestureHandlerGestureEvent} from 'react-native-gesture-handler';
import {Size} from '../Types/Block';
import RootStoreContext from '../Stores/RootStore';
import Grid from '../Tools/Grid';


interface Props {
  index: number;
  backgroundColor: string;
  containerStyle: Record<string, number | string>;
}

const DraggableContainer = ({
  index,
  backgroundColor,
  containerStyle,
}: Props): JSX.Element => {
  const store = useContext(RootStoreContext).grid;
  //drag
  const translateY = useSharedValue<number>(0);
  const translateX = useSharedValue<number>(0);
  const elevation = useSharedValue<number>(1);
  // multiple gesture handlers refs
  const panRef = useRef(null);
  const tapRef = useRef(null);
  // block info
  const size = store.getBlockSize(index);
  const modifying = store.isBlockMutable(index);
  const gridSize = store.blocks.length;

  const onDrop = (index: number, offsetX: number, offsetY: number): void => {
    const [x, y]: number[] = [index % 2, Math.floor(index / 2)];
    const hoveredBlockIndex: number = Grid.getHoveredBlockIndex(x, y, offsetX, offsetY);

    if (
      index != hoveredBlockIndex &&
      hoveredBlockIndex >= 0 &&
      hoveredBlockIndex < store.blocks.length
    ) {
      store.swithAtIndexes(index, hoveredBlockIndex);
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

  const onTap = (e: TapGestureHandlerGestureEvent) => {
    if (e.nativeEvent.state !== State.ACTIVE) {
      return;
    }
    store.setBlockSize(index, size != Size.full ? size << 1 : Size.normal);
  };

  const onDragEvent = useAnimatedGestureHandler({
    onStart: onStartDrag,
    onActive: onActiveDrag,
    onEnd: onEndDrag,
  });

  const dragStyle = useAnimatedStyle(() => {
    return {
      transform: [
        {translateX: translateX.value},
        {translateY: translateY.value},
      ]
    };
  });

  const boxStyle = useAnimatedStyle(() => {
    return {
      zIndex: elevation.value,
      elevation: elevation.value,
    };
  });

  const widgetStyle = {...styles.box, backgroundColor};

  if (size !== Size.normal) {
    widgetStyle.width = 375;
  }
  if (size === Size.full) {
    widgetStyle.height = 375;
  }

  return (
    <Animated.View style={[containerStyle, boxStyle]}>
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
              </Animated.View>
            </PanGestureHandler>
          </Animated.View>
        </TapGestureHandler>
      ) : (
        <Animated.View style={[widgetStyle]}
        >
        </Animated.View>
      )}
    </Animated.View>
  );
};

export default DraggableContainer;

const styles = StyleSheet.create({
  box: {
    height: 175,
    width: 175,
    borderRadius: 25,
    backgroundColor: 'black'
  }
});