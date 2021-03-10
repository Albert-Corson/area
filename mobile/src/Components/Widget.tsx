import React, {Component} from 'react'
import {View, Text, StyleSheet, ImageBackground, FlatList} from 'react-native'
import {Widget as WidgetType} from '../Types/Widgets'
import {observer} from 'mobx-react'
import {BlurView} from 'expo-blur'
import {Size} from '../Types/Block'
import {Video} from 'expo-av'
import DoubleTap from './DoubleTap'
import ModalContainer from './ModalContainer'
import FlatButton from './FlatButton'
import GradientFlatButton from './GradientFlatButton'
import TextInput from './TextInput'

interface Props {
  item: WidgetType;
  subscribed?: boolean;
  size?: Size;
  display?: Record<string, any>;
  widgetStore: any;
  modifying?: any;
}

interface State {
  showText: boolean;
  showModal: boolean;
  modifyingQuery: Record<string, string>;
}

const sizes: Record<Size, number> = {
  [Size.normal]: 70,
  [Size.extended]: 250,
  [Size.full]: 500,
}

@observer
class Widget extends Component<Props, State> {
  queries: any | any[]

  constructor(props: Props) {
    super(props)

    this.state = {
      showText: true,
      showModal: false,
      modifyingQuery: {},
    }

    this.queries = Array.isArray(props.item?.params) ? props.item?.params : props.item?.params?.params || []
  }

  truncateString = (str: string): string => {
    const length: number = sizes[this.props.size ?? Size.normal]

    if (!length) return str

    return str && str.length > length ? `${str.slice(0, length)}...` : str
  }

  onTap = (): void => {
    if (!this.props.subscribed || !this.props.display?.image) return

    this.setState({showText: !this.state?.showText})
  }

  onDoubleTap = () => {
    if (!this.props.subscribed || !this.queries.length) return

    this.setState({showModal: true})
  }

  content: () => JSX.Element | boolean = () => this.state.showText && (
    <BlurView intensity={100} style={[styles.centerContent, styles.fullSize]}>
      <View style={styles.contentContainer}>
        <Text style={[styles.text, styles.title, {}]}>
          {this.truncateString(this.props.display?.header ?? this.props.item.name)}
        </Text>

        {(() => {
          const body = (this.props.display?.artists ?? (this.props.display?.genres || []))?.map((item: string, i: number) => (
            <Text style={styles.text} key={i}>
              {this.truncateString(item)}
            </Text>
          ))

          if (body?.length) {
            return body
          }

          if (!this.props.display?.content && !this.props.display?.description && this.props.subscribed) {
            return null
          }

          return (
            <Text style={styles.text}>
              {this.truncateString(this.props.display?.content || this.props.display?.description || this.props.item.description)}
            </Text>
          )
        })()}
      </View>
    </BlurView>
  )

  container: () => JSX.Element = () => (
    <>
      {this.props.display?.image ? (
        <>
          {this.props.display?.image.includes('.mp4') ? (
            <Video
              style={[styles.widget, styles.fullSize]}
              source={{uri: this.props.display?.image}}
              shouldPlay={true}
              isLooping={true}
              volume={0}
              resizeMode="cover">
              {this.content()}
            </Video>
          ) : (
            <ImageBackground style={[styles.widget, styles.fullSize]} source={{uri: this.props.display?.image}}>
              {this.content()}
            </ImageBackground>
          )}
        </>
      ) : (
        <View style={[styles.widget, styles.centerContent, styles.fullSize]}>
          {this.content()}
        </View>
      )}
    </>
  )

  render(): JSX.Element {
    return (
      <>
        <DoubleTap
          onPress={this.onTap}
          onDoublePress={this.onDoubleTap}
          delay={200}
          activeOpacity={1}
        >
  
          {this.container()}
  
        </DoubleTap>
        <ModalContainer visible={this.state.showModal} containerStyle={{height: 175, justifyContent: 'center'}}>
          <FlatList
            style={{height: 50}}
            data={this.queries}
            renderItem={({item, index}) => (
              <TextInput
                containerStyle={{width: '100%', marginVertical: 15}}
                placeholder={item.name}
                value={this.state.modifyingQuery[index]}
                onChange={(text) => {
                  const modifyingQuery = {...this.state.modifyingQuery}
  
                  modifyingQuery[index] = text
                  this.setState({modifyingQuery})
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
                const map = new Map(Object.keys(this.state.modifyingQuery)
                  .filter((key: any) => this.state.modifyingQuery[key]?.length)
                  .map((key: any) => [this.queries[key].name, this.state.modifyingQuery[key]]))
  
                await this.props.widgetStore.updateWidget(this.props.item.id, Object.fromEntries(map))
                this.props.widgetStore.updateParameter(this.props.item.id)
                this.setState({showModal: false})
              }}
            />
            <FlatButton 
              width={75} 
              value="Cancel" 
              onPress={() => this.setState({modifyingQuery: {}, showModal: false})}
            />
          </View>
        </ModalContainer>
      </>
    )
  }
}

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
