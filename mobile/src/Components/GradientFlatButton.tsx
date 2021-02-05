import React from 'react';
import {
  TouchableOpacity, Text, View, StyleSheet, GestureResponderEvent,
} from 'react-native';
import {LinearGradient} from 'expo-linear-gradient';

interface Props {
  width?: number | string;
  height?: number | string;
  onPress: (event: GestureResponderEvent | undefined) => void;
  value: string;
  containerStyle?: Record<string, number| string>;
}

const GradientFlatButton = ({
  width = 200, height = 50, onPress, value, containerStyle = {},
}: Props): JSX.Element => (
  <View style={containerStyle}>
    <View style={styles.upShadow}>
      <View style={styles.downShadow}>
        <TouchableOpacity
          activeOpacity={0.4}
          style={{width, height}}
          onPress={onPress}
        >
          <LinearGradient
            colors={['#d564a8', '#5469ca']}
            start={[0, 1]}
            end={[1, 1]}
            style={styles.button}
          >
            <Text style={styles.text}>{value}</Text>
          </LinearGradient>
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
    borderRadius: 15,
    paddingHorizontal: 5,
    height: '100%',
  },
  upShadow: {
    shadowColor: '#ffffff',
    shadowOffset: {
      width: -10,
      height: -10,
    },
    shadowOpacity: 0.25,
    shadowRadius: 5,
  },
  downShadow: {
    shadowColor: '#A6ABBD',
    shadowOffset: {
      width: 10,
      height: 10,
    },
    shadowOpacity: 0.25,
    shadowRadius: 5,
  },
  text: {
    fontFamily: 'DosisSemiBold',
    fontSize: 20,
    color: 'white',
  },
});

export default GradientFlatButton;
