import React from 'react';
import {TextInput as RNTextInput, StyleSheet, View} from 'react-native';
// @ts-ignore
import InsetShadow from 'react-native-inset-shadow';

interface Props {
  value: string;
  onChange: (value: string) => void;
  placeholder: string;
  containerStyle?: Record<string, number| string>;
  secure?: boolean;
}

const TextInput = ({value, onChange, placeholder, containerStyle = {}, secure = false}: Props): JSX.Element => {
  return (
    <View style={[styles.container, containerStyle]}>
      <InsetShadow
        shadowRadius={7}
        top={false}
        left={false}
        shadowColor={'#ffffff'}
        containerStyle={styles.shadowStyle}>
        <InsetShadow
          shadowRadius={7}
          bottom={false}
          right={false}
          shadowColor={'#A6ABBD'}
          containerStyle={styles.shadowStyle}>
          <RNTextInput
            value={value}
            onChangeText={(text) => onChange(text)}
            placeholder={placeholder}
            style={styles.input}
            secureTextEntry={secure}
            placeholderStyle={styles.placeholder}
            placeholderTextColor={'#545454'}
          />
        </InsetShadow>
      </InsetShadow>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    width: 200,
    height: 50,
    borderRadius: 25,
  },
  input: {
    height: '100%',
    width: '100%',
    paddingHorizontal: 20,
    backgroundColor: '#e6e6e9',
    fontFamily: 'Dosis',
    fontSize: 17,
  },
  shadowStyle: {
    borderRadius: 25,
  },
  placeholder: {
    fontFamily: 'Dosis',
    color: 'black'
  },
});

export default TextInput;
