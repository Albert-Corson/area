import React from 'react'
import {TouchableOpacity} from 'react-native'

interface Props {
  onPress: () => void;
  onDoublePress: () => void;
  delay?: number;
  activeOpacity?: number;
  children: React.ReactChild
}

const DoubleTap = ({children, onPress, onDoublePress, delay = 200, activeOpacity = 1}: Props): JSX.Element => {
  let time = 0
  let last = 0
  let clickCount = 0

  const handlePress = (): void => {
    time = Date.now()
    clickCount++

    if (time - last < delay) {
      onDoublePress()
      clickCount = 0
    } else if (time - last >= delay) {
      setTimeout(() => {
        if (clickCount === 1) {
          onPress()
          clickCount = 0
        }
      }, delay)

      last = time
    }
  }

  return (
    <TouchableOpacity onPress={handlePress} activeOpacity={activeOpacity}>

      {children}

    </TouchableOpacity>
  )
}

export default DoubleTap
