import React from 'react'
import {View, Text, StyleSheet} from 'react-native'
import {Service} from '../Types/Widgets'
import FlatButton from './FlatButton'
import GradientFlatButton from './GradientFlatButton'
import ModalContainer from './ModalContainer'

interface Props {
    service: Service | null;
    onPress: () => Promise<void>;
    onCancel: () => void;
}

const ServiceLoginPrompt = ({service, onPress, onCancel}: Props): JSX.Element => (
  <ModalContainer visible={!!(service != null)}>
    <View style={styles.verticalContainer}>
      <Text style={styles.title}>{`${service?.name} requires authentication`}</Text>
      <View style={styles.btnContainer}>
        <GradientFlatButton
          value={'Login'}
          width={150}
          onPress={onPress}
        />
        <FlatButton
          value={'Cancel'}
          width={75}
          onPress={onCancel}
        />
      </View>
    </View>
  </ModalContainer>
)

export default ServiceLoginPrompt

const styles = StyleSheet.create({
  verticalContainer: {
    flex: 1,
    flexDirection: 'column',
  },
  title: {
    width: '100%',
    textAlign: 'center',
    fontSize: 20,
    fontFamily: 'DosisBold',
    marginBottom: 25,
  },
  btnContainer: {
    flex: 1,
    flexDirection: 'row',
    justifyContent: 'space-between'
  },
})