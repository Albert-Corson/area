import React from 'react';
import {TouchableOpacity, Text, View, StyleSheet, GestureResponderEvent} from 'react-native';
import Shadow from '../StyleSheets/Shadow';

interface Props {
  width: number;
  height: number;
  onPress: (event: GestureResponderEvent) => void;
  value: string;
  containerStyle: Record<string, number| string>;
}

const FlatButton = ({width = 200, height = 50, onPress, value, containerStyle = {}}: Props): JSX.Element => (
  <View style={containerStyle}>
    <View style={Shadow.upShadow}>
      <View style={Shadow.downShadow}>
        <TouchableOpacity
          activeOpacity={.4}
          style={{...styles.button, width, height}}
          onPress={onPress}>
          <Text style={styles.text}>{value}</Text>
        </TouchableOpacity>
      </View>
    </View>
  </View>
);

const styles = StyleSheet.create({
  button: {
    backgroundColor: '#e6e6e9',
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 25,
    paddingHorizontal: 30,
  },
  text: {
    fontFamily: 'DosisSemiBold',
    fontSize: 20,
    color: '#545454',
  },
});

export default FlatButton;
