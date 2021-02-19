import React from 'react'
import {TextInput as RNTextInput, StyleSheet, View} from 'react-native'
import InsetShadowContainer from './InsetShadowContainer'

interface Props {
  value: string;
  onChange: (value: string) => void;
  placeholder: string;
  containerStyle?: Record<string, number| string>;
  secure?: boolean;
}

const TextInput = ({
  value, onChange, placeholder, containerStyle = {}, secure = false,
}: Props): JSX.Element => (
  <View style={[styles.container, containerStyle]}>
    <InsetShadowContainer>
      <RNTextInput
        value={value}
        onChangeText={(text) => onChange(text)}
        placeholder={placeholder}
        style={styles.input}
        secureTextEntry={secure}
        placeholderStyle={styles.placeholder}
        placeholderTextColor="#545454"
      />
    </InsetShadowContainer>
  </View>
)

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
    color: 'black',
  },
})

export default TextInput
