import React from 'react'
import {Modal, View, StyleSheet, StyleProp} from 'react-native'

interface Props {
    children: React.ReactNode;
    visible: boolean;
    containerStyle?: StyleProp<any>;
}

const ModalContainer = ({children, visible, containerStyle = {}}: Props): JSX.Element => (
  <Modal
    animationType="slide"
    transparent={true}
    visible={visible}>
    <View style={styles.centeredView}>
      <View style={[styles.background, styles.shadow, containerStyle]}>
        {children}
      </View>
    </View>
  </Modal>
)

export default ModalContainer

const styles = StyleSheet.create({
  centeredView: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  background: {
    height: 150,
    paddingVertical: 20,
    paddingHorizontal: 40,
    justifyContent: 'space-around',
    backgroundColor: '#eeeeee',
    borderRadius: 25,
  },
  shadow: {
    shadowColor: '#808491',
    shadowOffset: {
      width: 15,
      height: 15,
    },
    shadowOpacity: 0.5,
    shadowRadius: 7.5,
  },
})