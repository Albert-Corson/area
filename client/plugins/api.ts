import { Plugin } from '@nuxt/types'
import { initializeApi } from '~/globals/api' 

import makeAuthRepository from '~/api/auth' 

const apiPlugin: Plugin = ({ $axios }, inject: Function) => {
  const repositories = {
    auth: makeAuthRepository($axios)
  }
  initializeApi(repositories)
  inject('api', repositories)
}

export default apiPlugin
