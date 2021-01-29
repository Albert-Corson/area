import React, {useRef, useContext} from 'react';
import {SafeAreaView, StyleSheet, View, ScrollView} from 'react-native';
import BadgeButton from '../Components/BadgeButton';
import DraggableContainer from '../Components/DraggableContainer';
import {FontAwesome} from '@expo/vector-icons';
import BottomSheet from 'reanimated-bottom-sheet';
import {WidgetSelector, WidgetSelectorHeader} from '../Components/WidgetSelector';
import InsetShadow from 'react-native-inset-shadow';
import RootStoreContext from '../Stores/RootStore';
import {observer} from 'mobx-react-lite';

const WidgetsScreen = observer((): JSX.Element => {
  const store = useContext(RootStoreContext).gridStore;
  const sheetRef = useRef<BottomSheet>(null);
  
  return (
    <>
      <SafeAreaView style={styles.safeView}>
        <View style={styles.modifier}>
          <BadgeButton
            onPress={() => sheetRef.current!.snapTo(0)}
            icon={() => (
              <FontAwesome name="plus" size={17} color={'#666666'} />
            )} />
          <BadgeButton
            onPress={store.toggleEditionMode}
            active={store.modifying}
            icon={() => (
              <FontAwesome name="pencil-square-o" size={17} color={'#666666'} />
            )} />
        </View>
        <InsetShadow>
          <ScrollView showsVerticalScrollIndicator={false}>
            <View style={styles.container}>
              {store.blocks.map((widget, index) => (
                <DraggableContainer
                  key={index}
                  backgroundColor={widget.color}
                  index={index}
                  containerStyle={styles.draggable}
                />
              ))}
            </View>
          </ScrollView>
        </InsetShadow>
      </SafeAreaView>
      <BottomSheet
        ref={sheetRef}
        snapPoints={['80%', '50%', '0%']}
        initialSnap={2}
        renderContent={WidgetSelector}
        renderHeader={WidgetSelectorHeader}
      />
    </>
  );
});

export default WidgetsScreen;

const styles = StyleSheet.create({
  safeView: {
    backgroundColor: '#e7e7e7',
    height: '100%',
  },
  container: {
    width: 'auto',
    alignItems: 'flex-start',
    justifyContent: 'flex-start',

    marginHorizontal: 7.5,

    flexDirection: 'row',
    flexWrap: 'wrap',

    paddingBottom: 50,
  },
  draggable: {
    margin: 15,
  },
  modifier: {
    height: 30,
    justifyContent: 'space-between',
    flexDirection: 'row',
    marginHorizontal: 30,
    marginVertical: 15
  },
});