import React, {useRef, useState} from 'react'
import {View, Text, StyleSheet, ImageBackground, TouchableOpacity} from 'react-native'
import {Widget as WidgetType} from '../Types/Widgets'
import {observer} from 'mobx-react-lite'
import {BlurView} from 'expo-blur'
import {Size} from '../Types/Block'
import {Video} from 'expo-av'
import {FontAwesome} from '@expo/vector-icons'
import DoubleTap from './DoubleTap'

interface Props {
  item: WidgetType;
  subscribed?: boolean;
  size: Size;
}

const sizes = {
  [Size.normal]: 70,
  [Size.extended]: 250,
  [Size.full]: 500,
}

const DOUBLE_PRESS_DELAY = 400

const Widget = observer(({item, size, subscribed = true}: Props): JSX.Element => {
  const [showText, setShowText] = useState<boolean>(true)
  const display = item.params?.item ?? (item.params?.items?.length ? item.params.items[0] : {})
  const queries = item?.params?.params || []

  const truncateString = (str: string): string => {
    const length = sizes[size]

    if (!length) return str

    return str.length > length ? `${str.slice(0, length)}...` : str
  }

  const onTap = (): void => {
    if (!subscribed) return

    setShowText(!showText)
  }

  const onDoubleTap = () => {
    if (!queries.length) return

    // open query modal
  }

  const content: JSX.Element | boolean = showText && (
    <BlurView intensity={100} style={[styles.centerContent, styles.fullSize]}>
      <View style={styles.contentContainer}>
        <Text style={[styles.text, styles.title, {}]}>
          {truncateString(display.header ?? item.name)}
        </Text>

        {(() => {
          const body = (display.artists ?? display.genres)?.map((item: string, i: number) => (
            <Text style={styles.text} key={i}>
              {truncateString(item)}
            </Text>
          ))

          if (body?.length) {
            return body
          }

          if (!display.content && !display.description && subscribed) {
            return null
          }

          return (
            <Text style={styles.text}>
              {truncateString(display.content || display.description || item.description)}
            </Text>
          )
        })()}
      </View>
    </BlurView>
  )

  const container: JSX.Element = (
    <>
      {display.image ? (
        <>
          {display.image.includes('.mp4') ? (
            <Video
              style={[styles.widget, styles.fullSize]}
              source={{uri: display.image}}
              shouldPlay={true}
              isLooping={true}
              volume={0}
              resizeMode="cover">
              {content}
            </Video>
          ) : (
            <ImageBackground style={[styles.widget, styles.fullSize]} source={{uri: display.image}}>
              {content}
            </ImageBackground>
          )}
        </>
      ) : (
        <View style={[styles.widget, styles.centerContent, styles.fullSize]}>
          {content}
        </View>
      )}
    </>
  )

  return (
    <DoubleTap
      onPress={onTap}
      onDoublePress={onDoubleTap}
      delay={200}
      activeOpacity={1}
    >

      {container}

    </DoubleTap>
  )
})

export default Widget

const styles = StyleSheet.create({
  contentContainer: {
    padding: 10,
    overflow: 'hidden'
  },
  text: {
    fontFamily: 'Dosis',

    textAlign: 'center',
  },
  title: {
    fontFamily: 'LouisGeorgeCafeBold',

    marginVertical: 10,
  },
  badge: {
    backgroundColor: 'green',
    paddingHorizontal: 5,
    paddingVertical: 1,
    borderRadius: 50,
  },
  badgeText: {
    fontFamily: 'LouisGeorgeCafeBold',
    color: 'white',
    fontSize: 13,
  },
  widget: {
    borderRadius: 25,

    overflow: 'hidden',
  },
  centerContent: {
    justifyContent: 'space-evenly',
    alignItems: 'center',
  },
  fullSize: {
    width: '100%',
    height: '100%',
  },
  query: {
    width: 20,
    height: 20,
    borderRadius: 20,

    zIndex: 1000,

    position: 'absolute',

    bottom: 0,
    right: 0,

    backgroundColor: '#54545477',

    justifyContent: 'center',
    alignItems: 'center',
  },
})
