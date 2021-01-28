import React from 'react';
import {StyleSheet} from 'react-native';

const Shadow = StyleSheet.create({
  upShadow: {
    shadowColor: '#ffffff',
    shadowOffset: {
      width: -10,
      height: -10,
    },
    shadowOpacity: 0.25,
    shadowRadius: 5,
  },
  downShadow: {
    shadowColor: '#A6ABBD',
    shadowOffset: {
      width: 10,
      height: 10,
    },
    shadowOpacity: 0.25,
    shadowRadius: 5,
  },
});

export default Shadow;
