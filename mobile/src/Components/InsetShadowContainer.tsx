import React from 'react'
import {StyleSheet} from 'react-native'
import InsetShadow from 'react-native-inset-shadow'

interface Props {
  children: React.ReactNode
}

const InsetShadowContainer = ({children}: Props) => (
  <InsetShadow
    shadowRadius={7}
    top={false}
    left={false}
    shadowColor="#ffffff"
    containerStyle={styles.shadowStyle}
  >
    <InsetShadow
      shadowRadius={7}
      bottom={false}
      right={false}
      shadowColor="#A6ABBD"
      containerStyle={styles.shadowStyle}
    >

      {children}

    </InsetShadow>
  </InsetShadow>
)

export default InsetShadowContainer

const styles = StyleSheet.create({
  shadowStyle: {
    borderRadius: 15,
  },
})
