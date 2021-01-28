import React, { useState, useRef } from 'react';
import { SafeAreaView, StyleSheet, View, Text, ScrollView } from 'react-native';
import BadgeButton from '../Components/BadgeButton';
import DraggableContainer from '../Components/DraggableContainer';
import { FontAwesome } from '@expo/vector-icons';
import BottomSheet from 'reanimated-bottom-sheet';
import { WidgetSelector, WidgetSelectorHeader } from '../Components/WidgetSelector';
import Grid from '../Tools/Grid';
import { Block, Size } from '../Types/Block';
import InsetShadow from 'react-native-inset-shadow';

interface Widget extends Block {
  color: string;
}

const widgetsData: Widget[] = [
  { color: '#69a58b' },
  { color: '#9b3e63' },
  { color: '#97697d', size: Size.full },
  { color: '#dda022' },
  { color: '#dd5555' },
];

const fillBlock: Widget = { color: '#00000000', unactive: true };

const WidgetsScreen = (): JSX.Element => {
  const [modifying, setModifying] = useState<boolean>(false);
  const [widgets, setWidgets] = useState<readonly Widget[]>(
    Grid.fillGrid<Widget>(widgetsData, fillBlock, 8)
  );
  const sheetRef = useRef<BottomSheet>(null);

  const onPress = (): void => setModifying(!modifying);

  const onMove = (index: number, offsetX: number, offsetY: number): void => {
    const [x, y]: number[] = [index % 2, Math.floor(index / 2)];
    const hoveredBlockIndex: number = Grid.getHoveredBlockIndex(x, y, offsetX, offsetY);

    if (
      index != hoveredBlockIndex &&
      hoveredBlockIndex >= 0 &&
      hoveredBlockIndex < widgets.length
    ) {
      setWidgets(Grid.switchElem<Widget>(widgets, index, hoveredBlockIndex));
    }
  };

  const setSize = (index: number, size: number): void => {
    if (index < 0 || index >= widgets.length) {
      return;
    }

    const copy: Widget[] = [...widgets];

    copy[index].size = size;

    for (let i = 0; i < copy.length - 1; ++i) {
      const position: number = Grid.indexToPosition(copy, i);

      if (position % 2 && (copy[i].size ?? Size.normal) !== Size.normal) {
        copy.splice(i, 0, fillBlock)
      }
      if (size === Size.extended && copy[i + 1].unactive) {
        copy.splice(i + 1, 1);
        ++i;
      }
    }

    setWidgets(Grid.fillGrid<Widget>(copy, fillBlock, 8));
  }

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
            onPress={onPress}
            active={modifying}
            icon={() => (
              <FontAwesome name="pencil-square-o" size={17} color={'#666666'} />
            )} />
        </View>
        <InsetShadow>
          <ScrollView showsVerticalScrollIndicator={false}>
            <View style={styles.container}>
              {widgets.map((widget, index) => (
                <DraggableContainer
                  key={index}
                  size={widget.size}
                  setSize={setSize}
                  zIndex={widgets.length}
                  backgroundColor={widget.color}
                  index={index}
                  modifying={widget.unactive ? !widget.unactive : modifying}
                  onMove={onMove}
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
};

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