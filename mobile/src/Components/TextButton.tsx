import React from 'react'
import {
  TouchableOpacity,
  Text,
  View,
  StyleSheet,
  GestureResponderEvent,
} from 'react-native'

interface Props {
  value: string;
  onPress: (event: GestureResponderEvent) => void;
  style?: Record<string, number| string>;
  containerStyle?: Record<string, number| string>;
}

const TextButton = ({
  value, onPress, style = {}, containerStyle = {},
}: Props): JSX.Element => (
  <View style={containerStyle}>
    <TouchableOpacity onPress={onPress}>
      <Text style={[styles.text, style]}>{value}</Text>
    </TouchableOpacity>
  </View>
)

const styles = StyleSheet.create({
  text: {
    fontFamily: 'DosisLight',
    color: '#545454',
  },
})

export default TextButton
