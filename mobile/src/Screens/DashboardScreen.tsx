import React, {useRef, useContext, useEffect} from 'react';
import {SafeAreaView, StyleSheet, View} from 'react-native';
import {FontAwesome} from '@expo/vector-icons';
import BottomSheet from 'reanimated-bottom-sheet';
import {observer} from 'mobx-react-lite';
import {StackNavigationProp} from '@react-navigation/stack';
import DraggableContainer from '../Components/DraggableContainer';
import {WidgetSelector, WidgetSelectorHeader} from '../Components/WidgetSelector';
import RootStoreContext from '../Stores/RootStore';
import FlatButton from '../Components/FlatButton';
import {RootStackParamList} from '../Navigation/StackNavigator';
import WidgetListContainer from '../Components/WidgetListContainer';
import Widget from '../Components/Widget';
import GradientFlatButton from '../Components/GradientFlatButton';

interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const WidgetsScreen = observer(({navigation}: Props): JSX.Element => {
  const store = useContext(RootStoreContext);
  const sheetRef = useRef<BottomSheet>(null);

  useEffect(() => {
    store.widget.updateWidgets();
  }, []);

  return (
    <>
      <SafeAreaView style={styles.safeView}>
        <View style={styles.modifier}>
          <FlatButton
            height="100%"
            width={60}
            value={() => (
              <FontAwesome name="plus" size={17} color="#666666" />
            )}
            onPress={() => {
              if (store.grid.modifying) {
                store.grid.toggleEditionMode();
              }
              sheetRef.current!.snapTo(store.grid.adding ? 2 : 0);

              store.grid.toggleAdditionMode();
            }}
            active={store.grid.adding}
          />

          <GradientFlatButton
            height="100%"
            width={120}
            value={store.user.userJWT?.username ?? 'Profile'}
            onPress={() => navigation.navigate('Profile')}
          />

          <FlatButton
            height="100%"
            width={60}
            value={() => (
              <FontAwesome name="pencil-square-o" size={17} color="#666666" />
            )}
            onPress={() => {
              store.grid.toggleEditionMode();
              if (store.grid.adding) {
                sheetRef.current!.snapTo(2);
                store.grid.toggleAdditionMode();
              }
            }}
            active={store.grid.modifying}
          />
        </View>
        <WidgetListContainer containerStyle={styles.container}>
          {store.grid.blocks.map((widget, index) => (
            <DraggableContainer
              key={index}
              index={index}
              renderItem={() => <Widget item={widget} />}
            />
          ))}
        </WidgetListContainer>
      </SafeAreaView>
      <BottomSheet
        ref={sheetRef}
        snapPoints={['80%', '50%', '0%']}
        initialSnap={2}
        renderContent={() => (<WidgetSelector store={store} />)}
        renderHeader={WidgetSelectorHeader}
        onOpenStart={store.grid.toggleAdditionMode}
        onCloseStart={store.grid.toggleAdditionMode}
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

    marginHorizontal: 8,

    flexDirection: 'row',
    flexWrap: 'wrap',

    paddingBottom: 50,
  },
  modifier: {
    height: 30,
    justifyContent: 'space-between',

    flexDirection: 'row',
    marginHorizontal: 30,
    marginVertical: 15,
  },
});
