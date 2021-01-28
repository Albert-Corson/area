import React from 'react';
import {TouchableOpacity, StyleSheet} from 'react-native';

interface BadgeButtonType {
    active?: boolean;
    icon: () => JSX.Element
    onPress: () => void;
}

const BadgeButton = ({active = false, icon, onPress}: BadgeButtonType): JSX.Element => (
  <TouchableOpacity
    style={[styles.button, {backgroundColor: active ? '#92D0FF' : '#cccccc'}]}
    onPress={onPress}
    activeOpacity={0.8}>
    {icon()}
  </TouchableOpacity>
);

export default BadgeButton;

const styles = StyleSheet.create({
  button: {
    justifyContent: 'center',
    alignItems: 'center',

    height: '100%',
    width: 60,
    borderRadius: 20,
  },
});