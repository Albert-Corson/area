import React from 'react';
import {View, Text, StyleSheet} from 'react-native';
import Shadow from '../StyleSheets/Shadow';

const WidgetSelector = (): JSX.Element => (
  <View style={styles.selector}>
        
  </View>
);

const WidgetSelectorHeader = (): JSX.Element => (
  <View style={[styles.header]}>
    <View style={styles.handle}>

    </View>
  </View>
);

export {WidgetSelector, WidgetSelectorHeader};

const styles = StyleSheet.create({
  selector: {
    backgroundColor: '#ebebeb',
    padding: 16,
    height: '100%',
  },
  header: {
    backgroundColor: '#ebebeb',

    borderColor: '#ebebeb',
    borderWidth: 2,
    borderRadius: 50,

    width: '100%',
    height: 30,

    justifyContent: 'center',
    alignItems: 'center',


    shadowColor: '#666',
    shadowOffset: {
      width: -10,
      height: -10,
    },
    shadowOpacity: 0.25,
    shadowRadius: 5,
  },
  handle: {
    height: 5,
    width: 60,
    borderRadius: 50,
    backgroundColor: 'grey',
  },
});
