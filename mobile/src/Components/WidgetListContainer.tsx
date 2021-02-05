import React from 'react';
import {View, ScrollView, StyleSheet, StyleProp, ViewStyle} from 'react-native';
import InsetShadow from 'react-native-inset-shadow';


interface Props {
    children: React.ReactNode;
    containerStyle: StyleProp<ViewStyle>;
    bounce?: boolean;
}

const WidgetListContainer = ({children, containerStyle, bounce = true}: Props): JSX.Element => (
  <InsetShadow shadowColor={'#A6ABBD'}>
    <ScrollView showsVerticalScrollIndicator={false} bounces={bounce}>
      <View style={containerStyle}>

        {children}

      </View>
    </ScrollView>
  </InsetShadow>
);

export default WidgetListContainer;