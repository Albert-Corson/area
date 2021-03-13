import React, {useContext, useEffect, useState} from 'react'
import {StackNavigationProp} from '@react-navigation/stack'
import {observer} from 'mobx-react-lite'
import {SafeAreaView, Text, Image, StyleSheet, View, ActivityIndicator, FlatList} from 'react-native'
import {RootStackParamList} from '../Navigation/StackNavigator'
import RootStoreContext from '../Stores/RootStore'
import DropShadowContainer from '../Components/DropShadowContainer'
import GradientFlatButton from '../Components/GradientFlatButton'
import TextInput from '../Components/TextInput'
import {Form as formStyles} from '../StyleSheets/Form'
import FlatButton from '../Components/FlatButton'
import {Device, DeviceInfo} from '../Stores/UserStore'
import {DeviceItem} from '../Components/Devices'


interface Props {
  navigation: StackNavigationProp<RootStackParamList>;
}

const ProfileScreen = observer(({navigation}: Props): JSX.Element => {
  const userStore = useContext(RootStoreContext).user
  const authStore = useContext(RootStoreContext).auth
  const [devices, setDevices] = useState<DeviceInfo | undefined>()

  const logout = async () => {
    await authStore.logout()
    
    navigation.reset({
      index: 0,
      routes: [{name: 'Login'}]
    })
  }

  console.log(devices)

  useEffect(() => {
    (async () => setDevices(await userStore.devices()))()
  }, [])

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
        {!devices ? (
          <ActivityIndicator size="large" />
        ) : (
          <View style={{alignItems: 'center', marginVertical: 50}}>
            <Text style={{fontFamily: 'Dosis', fontSize: 16}}>Devices</Text>
            <View style={{height: 300}}>
              <FlatList
                style={{marginVertical: 15}}
                contentContainerStyle={{height: '100%'}}
                data={devices.devices}
                renderItem={({item}: {item: Device}) => <DeviceItem {...item} isCurrent={item.id === devices.current} />}
                keyExtractor={(item: Device) => item.id.toString()}
              />
            </View>
          </View>
        )}
        <View>
          <GradientFlatButton value={'Logout'} onPress={logout} />
        </View>
      </View>

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