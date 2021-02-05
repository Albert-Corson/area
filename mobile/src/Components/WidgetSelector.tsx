import React, {useEffect, useState} from 'react';
import {observer} from 'mobx-react-lite';
import {View, Text, StyleSheet, Dimensions} from 'react-native';
import RootStoreContext, {RootStore} from '../Stores/RootStore';
import {WidgetStore} from '../Stores/WidgetStore';
import DraggableContainer from './DraggableContainer';
import WidgetListContainer from './WidgetListContainer';
import Widget from './Widget';
import {State, TapGestureHandlerGestureEvent} from 'react-native-gesture-handler';
import StaticContainer from './StaticContainer';

interface WidgetSelectorProps {
  store: RootStore;
}

const WidgetSelector = observer(({store}: WidgetSelectorProps): JSX.Element => {
  const onTap = (e: TapGestureHandlerGestureEvent, index: number): void => {
    if (e.nativeEvent.state !== State.ACTIVE) return;
    
    const availableWidgets = store.widget.availableWidgets;
    
    if (index < 0 || index >= availableWidgets.length) return;

    store.widget.subscribeToWidget(availableWidgets[index].id);
  };

  const availableWidgets = store.widget.availableWidgets;

  return (
    <WidgetListContainer containerStyle={[styles.container, styles.selector]} bounce={false}>
      {!availableWidgets.length && (
        <Text style={styles.title}>No new widget available, you got'em all!</Text>
      )}
      {availableWidgets.map((widget, index) => (
        <StaticContainer
          key={index}
          onTap={onTap}
          index={index}
          renderItem={() => <Widget item={widget} />}
        />
      ))}
    </WidgetListContainer>
  );
});

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
    height: 800,
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
    borderTopLeftRadius: 50,
    borderTopRightRadius: 50,
    backgroundColor: 'grey',
  },
  container: {
    width: 'auto',
    alignItems: 'flex-start',
    justifyContent: 'flex-start',

    backgroundColor: '#e7e7e7',

    marginHorizontal: 7.5,

    flexDirection: 'row',
    flexWrap: 'wrap',

    paddingBottom: 50,
  },
  title: {
    width: '100%', 
    textAlign: 'center',
    fontSize: 20,
    fontFamily: 'DosisBold'
  }
});
