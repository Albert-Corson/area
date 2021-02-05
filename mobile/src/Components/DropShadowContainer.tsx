import React from 'react';
import {View, ViewStyle} from 'react-native';
import Shadow from '../StyleSheets/Shadow';

interface Props {
    children: React.ReactNode;
    style?: ViewStyle;
}

const DropShadowContainer = ({children, style = {}}: Props) => (
  <View style={[Shadow.upShadow, style]}>
    <View style={Shadow.downShadow}>

      {children}

    </View>
  </View>
);

export default DropShadowContainer;