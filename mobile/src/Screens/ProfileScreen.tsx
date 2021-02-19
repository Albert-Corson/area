import React, {useContext, useEffect} from 'react'
import {StackNavigationProp} from '@react-navigation/stack'
import {observer} from 'mobx-react-lite'
import {SafeAreaView, Text, Image, StyleSheet, View} from 'react-native'
import {RootStackParamList} from '../Navigation/StackNavigator'
import RootStoreContext from '../Stores/RootStore'
import DropShadowContainer from '../Components/DropShadowContainer'
import GradientFlatButton from '../Components/GradientFlatButton'
import TextInput from '../Components/TextInput'
import {Form as formStyles} from '../StyleSheets/Form'
import FlatButton from '../Components/FlatButton'


interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const ProfileScreen = observer(({navigation}: Props): JSX.Element => {
  const userStore = useContext(RootStoreContext).user
  const authStore = useContext(RootStoreContext).auth

  const logout = async () => {
    console.log('hello')
    if (await authStore.logout()) {
      console.log('goodbye')
      navigation.replace('Login')
    }
  }

  return (
    <SafeAreaView style={styles.safeView}>
      <View style={{alignItems: 'center'}}>
        <DropShadowContainer>
          <View style={styles.avatarContainer}>
            <Image source={require('../../assets/avatar/preview.png')} style={styles.avatar} />
          </View>
        </DropShadowContainer>
        {userStore.user && (
          <>
            <Text style={styles.title}>{userStore.user.username}</Text>
            <Text style={styles.label}>{userStore.user.email}</Text>
          </>
        )}
      </View>
      <View style={{alignItems: 'center'}}>
        <Text style={styles.label}>
          Modify username
        </Text>
        <View style={styles.inlineInputSubmit}>
          <View style={styles.inputContainer}>
            <TextInput
              value={''}
              placeholder={authStore.username}
              onChange={(val) => authStore.username = val}
              containerStyle={formStyles.input}
            />
          </View>
          <FlatButton value="Modify" width={100} height={50} onPress={() => console.log('pressed')} containerStyle={styles.submit} />
        </View>
      </View>
      <GradientFlatButton value={'Logout'} onPress={logout} />
    </SafeAreaView>
  )
})

export default ProfileScreen

const styles = StyleSheet.create({
  inlineInputSubmit: {
    flexDirection: 'row',
  },
  inputContainer: {
    width: '60%'
  },
  submit: {
    paddingVertical: 7.5,
    paddingHorizontal: 15,
  },
  safeView: {
    height: '100%',

    backgroundColor: '#e7e7e7',

    alignItems: 'center',
    justifyContent: 'space-around'
  },
  avatarContainer: {
    borderRadius: 75,
    width: 150,
    height: 150,

    backgroundColor: '#e6e6e9',

    margin: 20,
  },
  avatar: {
    borderRadius: 75,
    width: 150,
    height: 150,
  },
  title: {
    fontFamily: 'LouisGeorgeCafeBold',
    fontSize: 25,
  },
  label: {
    fontFamily: 'LouisGeorgeCafe',
    fontSize: 17,

    paddingHorizontal: 15,
    paddingVertical: 5
  },
})