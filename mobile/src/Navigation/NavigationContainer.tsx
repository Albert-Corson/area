import React from 'react'
import {NavigationContainer as Container} from '@react-navigation/native'
import StackNavigator from './StackNavigator'

const NavigationContainer = (): JSX.Element => (
  <Container>
    <StackNavigator />
  </Container>
)

export default NavigationContainer
