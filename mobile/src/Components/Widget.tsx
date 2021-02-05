import React from 'react';
import {View, Text, StyleSheet} from 'react-native';
import {Widget as WidgetType} from '../Types/Widgets';

interface Props {
  item: WidgetType;
}

const Widget = ({item}: Props) => (
  <View style={styles.widget}>
    <Text style={[styles.text, styles.title]}>{item.name}</Text>
    <Text style={styles.text}>{item.description}</Text>
    {item.requires_auth ? (
      <View style={styles.badge}>
        <Text style={[styles.text, styles.badgeText]}>auth</Text>
      </View>
    ) : (
      <View />
    )}
  </View>
);

export default Widget;

const styles = StyleSheet.create({
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
    width: '100%',
    height: '100%',

    justifyContent: 'space-evenly',
    alignItems: 'center',
  },
});
