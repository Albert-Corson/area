import React, {useState, useContext} from 'react'
import {View, Text, StyleSheet, ImageBackground, FlatList} from 'react-native'
import {Widget as WidgetType} from '../Types/Widgets'
import {observer} from 'mobx-react-lite'
import {BlurView} from 'expo-blur'
import {Size} from '../Types/Block'
import {Video} from 'expo-av'
import DoubleTap from './DoubleTap'
import ModalContainer from './ModalContainer'
import FlatButton from './FlatButton'
import GradientFlatButton from './GradientFlatButton'
import TextInput from './TextInput'
import RootStoreContext from '../Stores/RootStore'
import {pure} from 'recompose'
import InsetShadowContainer from './InsetShadowContainer'

interface Props {
  item: WidgetType;
  subscribed?: boolean;
  size: Size;
}

const sizes: Record<Size, number> = {
  [Size.normal]: 70,
  [Size.extended]: 250,
  [Size.full]: 500,
}

const Widget = observer(({item, size, subscribed = true}: Props): JSX.Element => {
  const [showText, setShowText] = useState<boolean>(true)
  const [showModal, setShowModal] = useState<boolean>(false)
  const [modifyingQuery, setModifyingQuery] = useState<Record<number, string>>({})
  const display = item.params?.item ?? (item.params?.items?.length ? item.params.items[0] : {})
  const queries = Array.isArray(item?.params) ? item?.params : item?.params?.params || []
  const modifying = useContext(RootStoreContext).grid.modifying

  const widgetStore = useContext(RootStoreContext).widget

  const truncateString = (str: string): string => {
    const length = sizes[size]

    if (!length) return str

    return str && str.length > length ? `${str.slice(0, length)}...` : str
  }

  const onTap = (): void => {
    if (!subscribed) return

    setShowText(!showText)
  }

  const onDoubleTap = () => {
    if (modifying || !subscribed || !queries.length) return

    setShowModal(true)
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
    <>
      <DoubleTap
        onPress={onTap}
        onDoublePress={onDoubleTap}
        delay={200}
        activeOpacity={1}
      >

        {container}

      </DoubleTap>
      <ModalContainer visible={showModal} containerStyle={{height: 175, justifyContent: 'center'}}>
        <FlatList
          style={{height: 50}}
          data={queries}
          renderItem={({item, index}) => (
            <TextInput
              containerStyle={{width: '100%', marginVertical: 15}}
              placeholder={item.name}
              value={modifyingQuery[index]}
              onChange={(text) => {
                const cpy = {...modifyingQuery}

                cpy[index] = text
                setModifyingQuery(cpy)
              }}
            />
          )}
          keyExtractor={(_, index) => `query_${index}`}
        />
        <View style={{flex: 1, flexDirection: 'row', justifyContent: 'space-evenly', width: 300}}>
          <GradientFlatButton
            width={200} 
            value="Save" 
            onPress={async () => {
              const map = new Map(Object.keys(modifyingQuery)
                .filter((key: any) => modifyingQuery[key]?.length)
                .map((key: any) => [queries[key].name, modifyingQuery[key]]))

              await widgetStore.updateWidget(item.id, Object.fromEntries(map))
              widgetStore.updateParameter(item.id)
              setShowModal(false)
            }}
          />
          <FlatButton 
            width={75} 
            value="Cancel" 
            onPress={() => {
              setModifyingQuery({})
              setShowModal(false)
            }}
          />
        </View>
      </ModalContainer>
    </>
  )
})

export default pure(Widget)

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
