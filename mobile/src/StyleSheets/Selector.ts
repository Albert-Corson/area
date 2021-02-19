import {StyleSheet} from 'react-native'
import windowPadding from './WindowPadding'

const Selector = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
  },
  subContainer: {
    width: 285,
    marginHorizontal: windowPadding,
  },
  selector: {
    width: '100%',
    flexDirection: 'row',
    flexWrap: 'wrap',
    backgroundColor: '#e6e6e9',
    borderRadius: 25,
    paddingBottom: 15,
  },
  title: {
    fontFamily: 'Dosis',
    fontSize: 22,
    color: '#9e9e9e',
    marginVertical: 10,
  },
  item: {
    width: 75,
    height: 75,
    borderRadius: 20,
    marginLeft: 15,
    marginTop: 15,
    backgroundColor: '#e7e7e7',
    alignItems: 'center',
    justifyContent: 'center',
  },
  icon: {
    width: 50,
    height: 50,
  },
  insetShadow: {
    width: '100%',
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 20,
  },
})

export default Selector
