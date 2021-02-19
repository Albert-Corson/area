import {StyleSheet} from 'react-native'

export const Form = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#e7e7e7',
    alignItems: 'center',
  },
  inner: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: 325,
  },
  input: {
    margin: 10,
    width: '100%',
  },
  title: {
    fontFamily: 'DosisExtraBold',
    fontSize: 80,
    color: '#545454',
    textAlign: 'center',
  },
  textButton: {
    marginTop: 10,
    alignItems: 'center',
  },
  error: {
    fontFamily: 'Dosis',
    fontSize: 16,
    color: '#a35150',
    marginBottom: 10,
  },
})
