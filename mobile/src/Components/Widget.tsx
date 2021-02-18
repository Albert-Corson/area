import React, {useState} from 'react'
import {View, Text, StyleSheet, ImageBackground} from 'react-native'
import {Widget as WidgetType} from '../Types/Widgets'
import {observer} from 'mobx-react-lite'
import {BlurView} from 'expo-blur'
import {TouchableWithoutFeedback} from 'react-native-gesture-handler'
import {Size} from '../Types/Block'
import {Video} from 'expo-av'


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


const Widget = observer(({item, size, subscribed = true}: Props): JSX.Element => {
  const [showText, setShowText] = useState<boolean>(true)
  const display = item.params?.item ?? (item.params?.items?.length ? item.params.items[0] : {})

  console.log(item)

  const truncateString = (str: string): string => {
    const length = sizes[size]

    if (!length) return str

    return str.length > length ? `${str.slice(0, length)}...` : str
  }

  const content: JSX.Element = (
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
              {showText && (
                <BlurView intensity={100} style={[styles.centerContent, styles.fullSize]}>
                  {content}
                </BlurView>
              )}
            </Video>
          ) : (
            <ImageBackground style={[styles.widget, styles.fullSize]} source={{uri: display.image}}>
              {showText && (
                <BlurView intensity={100} style={[styles.centerContent, styles.fullSize]}>
                  {content}
                </BlurView>
              )}
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
    <TouchableWithoutFeedback onPress={() => subscribed && display.image && setShowText(!showText)}>
      {container}
    </TouchableWithoutFeedback>
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
  }
})
