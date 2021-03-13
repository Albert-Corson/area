import React from 'react'
import {Text, View, StyleSheet} from 'react-native'
import DropShadowContainer from './DropShadowContainer'
import InsetShadowContainer from './InsetShadowContainer'

interface Props {
  country: string;
  isCurrent: boolean;
  last_used: number;
  browser: string;
}

const DeviceItem = ({country, isCurrent, last_used, browser}: Props) => (
  <View style={styles.container}>
    <InsetShadowContainer>
      <View style={styles.item}>
        <Text>hello {new Date(last_used)}</Text>
      </View>
    </InsetShadowContainer>
  </View>
)

export {DeviceItem}

const styles = StyleSheet.create({
  container: {
    height: 50,
  },
  item: {
    flex: 1, 
    width: 200, 
    padding: 15, 
    alignItems: 'center', 
    alignContent: 'center',
  }
})