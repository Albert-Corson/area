import React from 'react';
import {TouchableOpacity, Text, View, StyleSheet, GestureResponderEvent} from 'react-native';
import Shadow from '../StyleSheets/Shadow';
import DropShadowContainer from './DropShadowContainer';
import InsetShadowContainer from './InsetShadowContainer';

interface Props {
  width?: number;
  height?: number | string;
  active?: boolean;
  onPress: (event: GestureResponderEvent) => void;
  value: string | (() => JSX.Element);
  containerStyle?: Record<string, number | string>;
}

const FlatButton = ({width = 200, height = 50, onPress, value, containerStyle = {}, active = false}: Props): JSX.Element => (
  <View style={containerStyle}>
    {active ? (
      <InsetShadowContainer>
        <TouchableOpacity
          activeOpacity={.4}
          style={[
            styles.button,
            {width, height}
          ]}
          onPress={onPress}>
          {typeof value === 'string' ? (
            <Text style={styles.text}>{value}</Text>
          ) : (
            value()
          )}
        </TouchableOpacity>
      </InsetShadowContainer>
    ) : (
      <DropShadowContainer>
        <TouchableOpacity
          activeOpacity={.4}
          style={[
            styles.button,
            {width, height}
          ]}
          onPress={onPress}>
          {typeof value === 'string' ? (
            <Text style={styles.text}>{value}</Text>
          ) : (
            value()
          )}
        </TouchableOpacity>
      </DropShadowContainer>
    )}
  </View>
);

const styles = StyleSheet.create({
  button: {
    backgroundColor: '#e6e6e9',
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 25,
  },
  text: {
    fontFamily: 'LouisGeorgeCafe',
    fontSize: 18,
    color: '#545454',
  },
  shadowStyle: {
    borderRadius: 25,
  },
});

export default FlatButton;
