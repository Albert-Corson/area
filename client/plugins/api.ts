import { Plugin } from '@nuxt/types'
import { initializeApi } from '~/globals/api' 

import makeAuthRepository from '~/api/AuthRepository' 
import makeServiceRepository from '~/api/ServiceRepository' 
import makeUserRepository from '~/api/UserRepository' 
import makeWidgetRepository from '~/api/WidgetRepository' 

const apiPlugin: Plugin = ({ $axios }, inject: Function) => {
  const repositories = {
    auth: makeAuthRepository($axios),
    user: makeUserRepository($axios),
    service: makeServiceRepository($axios),
    widget: makeWidgetRepository($axios)
  }
  initializeApi(repositories)
  inject('api', repositories)
}

export default apiPlugin
