import React, {useRef} from 'react';
import {Dimensions, StyleSheet, View} from 'react-native';
import {TapGestureHandler, TapGestureHandlerGestureEvent} from 'react-native-gesture-handler';
import Animated from 'react-native-reanimated';
import DropShadowContainer from './DropShadowContainer';

interface Props {
  index: number;
  onTap: (e: TapGestureHandlerGestureEvent, index: number) => void;
  renderItem: () => React.ReactNode;
}

const StaticContainer = ({
  index,
  onTap,
  renderItem,
}: Props) => {
  const tapRef = useRef(null);

  const SIZE = Dimensions.get('window').width / 2.5;
  const MARGIN = ((SIZE * (2.5 - 2.14)) / 4);

  return (
    <View style={{margin: MARGIN}}>
      <DropShadowContainer>
        <TapGestureHandler
          onHandlerStateChange={(e) => onTap(e, index)}
          numberOfTaps={1}
          ref={tapRef}
        >
          <Animated.View style={[styles.box, {width: SIZE, height: SIZE}]}>

            {renderItem()}

          </Animated.View>
        </TapGestureHandler>
      </DropShadowContainer>
    </View>
  );
};

export default StaticContainer;

const styles = StyleSheet.create({
  box: {
    borderRadius: 25,
    backgroundColor: '#e6e6e9',
  },
});
