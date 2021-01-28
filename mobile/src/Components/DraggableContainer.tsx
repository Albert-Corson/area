import React, { useRef, useState } from 'react';
import { StyleSheet } from 'react-native';
import Animated, {
  useAnimatedStyle,
  useSharedValue,
  useAnimatedGestureHandler,
  withSpring,
  runOnJS,
} from 'react-native-reanimated';
import { PanGestureHandler, TapGestureHandler, State, GestureHandlerGestureEventNativeEvent, PanGestureHandlerGestureEvent, TapGestureHandlerGestureEvent } from 'react-native-gesture-handler';
import {Size} from '../Types/Block';


interface Props {
  index: number;
  modifying: boolean;
  backgroundColor: string;
  zIndex: number;
  containerStyle: Record<string, number | string>;
  size?: Size;
  setSize: (index: number, size: number) => void;
  onMove: (index: number, offsetX: number, offsetXY: number) => void;
}

const DraggableContainer = ({
  index,
  modifying,
  backgroundColor,
  zIndex,
  containerStyle,
  size = Size.normal,
  setSize,
  onMove
}: Props): JSX.Element => {
  //drag
  const translateY = useSharedValue<number>(0);
  const translateX = useSharedValue<number>(0);
  const elevation = useSharedValue<number>(1);
  // widget growth
  //const width = useSharedValue<number>(175);
  //const height = useSharedValue<number>(175);
  // multiple gesture handlers refs
  const panRef = useRef(null);
  const tapRef = useRef(null);

  const onStartDrag = (_: any, ctx: any) => {
    'worklet';
    elevation.value = zIndex + 1;

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
    runOnJS(onMove)(index, translateX.value, translateY.value);

    translateY.value = withSpring(0, {
      damping: 60,
      stiffness: 500,
    });
    translateX.value = withSpring(0, {
      damping: 60,
      stiffness: 500,
    });
    elevation.value = zIndex;
  };

  const onTap = (e: TapGestureHandlerGestureEvent) => {
    if (e.nativeEvent.state !== State.ACTIVE) {
      return;
    }
    setSize(index, size != Size.full ? size << 1 : Size.normal);
  }

  const onDragEvent = useAnimatedGestureHandler({
    onStart: onStartDrag,
    onActive: onActiveDrag,
    onEnd: onEndDrag,
  });

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

  const widgetStyle = { ...styles.box, backgroundColor }

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

const styles = StyleSheet.create({
  box: {
    height: 175,
    width: 175,
    borderRadius: 25,
    backgroundColor: 'black'
  }
});

export default DraggableContainer;
