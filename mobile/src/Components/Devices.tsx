import React from 'react'
import {Text, View, StyleSheet} from 'react-native'
import InsetShadowContainer from './InsetShadowContainer'

interface Props {
  country: string;
  isCurrent: boolean;
  last_used: number;
  browser: string;
}

const numberPadding = (number: number, padding = 2, char = '0') => {
  const strNum = number.toString()
  return strNum.length > padding ? strNum : char.repeat(padding - strNum.length) + strNum
}

const formatDate = (date: Date): string => {
  return `${date.getFullYear()}/${numberPadding(date.getMonth())}/${numberPadding(date.getDay())} ${numberPadding(date.getHours())}:${numberPadding(date.getMinutes())}`
}

const DeviceItem = ({country, isCurrent, last_used, browser}: Props) => {
  const date = new Date(last_used * 1000)

  return (
    <View style={styles.container}>
      <InsetShadowContainer>
        <View style={styles.item}>
          <View style={styles.badge}>
            <Text style={{color: '#fff'}}>{country === 'Unknown' ? 'FR' : country}</Text>
          </View>
          <Text>{browser}</Text>
          <Text>{formatDate(date)}</Text>
        </View>
      </InsetShadowContainer>
    </View>
  )
}

export {DeviceItem}

const styles = StyleSheet.create({
  badge: {
    backgroundColor: '#5469ca',
    borderRadius: 25,
    paddingHorizontal: 5,
  },
  container: {
    height: 50,
  },
  item: {
    flex: 1, 
    flexDirection: 'row',
    width: '100%', 
    padding: 15, 
    alignItems: 'center', 
    justifyContent: 'space-between',
  }
})