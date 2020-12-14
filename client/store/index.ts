import { Store } from 'vuex'
import { initializeStores } from '~/store/init'

const initializer = (store: Store<any>) => initializeStores(store)

export const plugins = [ initializer ]
export * from '~/store/init'
