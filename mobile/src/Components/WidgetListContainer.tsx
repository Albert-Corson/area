import React, {useContext, useCallback} from 'react'
import {
  View, ScrollView, StyleProp, ViewStyle, RefreshControl,
} from 'react-native'
import InsetShadow from 'react-native-inset-shadow'
import RootStoreContext from '../Stores/RootStore'

interface Props {
  children: React.ReactNode;
  containerStyle: StyleProp<ViewStyle>;
  bounce?: boolean;
}

const wait = (timeout: number) => {
  return new Promise(resolve => setTimeout(resolve, timeout))
}

const WidgetListContainer = ({children, containerStyle, bounce = true}: Props): JSX.Element => {
  const store = useContext(RootStoreContext)
  const [refreshing, setRefreshing] = React.useState(false)

  const onRefresh = useCallback(() => {
    setRefreshing(true)
    store.widget.updateParameters().then(() => setRefreshing(false))
  }, [])

  return (
    <InsetShadow shadowColor="#A6ABBD">
      <ScrollView
        showsVerticalScrollIndicator={false}
        bounces={bounce}
        refreshControl={
          <RefreshControl
            refreshing={refreshing}
            onRefresh={onRefresh}
          />
        }>
        <View style={containerStyle}>

          {children}

        </View>
      </ScrollView>
    </InsetShadow>
  )
}

export default WidgetListContainer
