import { Store } from 'vuex'
import { getModule } from 'vuex-module-decorators'

import AuthModule from '~/store/modules/AuthModule'
import UserModule from '~/store/modules/UserModule'
import ServiceModule from '~/store/modules/ServiceModule'
import WidgetModule from '~/store/modules/WidgetModule'

export let AuthStore: AuthModule
export let UserStore: UserModule
export let ServiceStore: ServiceModule
export let WidgetStore: WidgetModule

export const initializeStores = (store: Store<any>): void => {
  AuthStore = getModule(AuthModule, store)
  UserStore = getModule(UserModule, store)
  ServiceStore = getModule(ServiceModule, store)
  WidgetStore = getModule(WidgetModule, store)
}