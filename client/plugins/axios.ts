import { initializeAxios } from '~/globals/axios'
import { Plugin } from '@nuxt/types'
import AuthStore from '~/store/modules/AuthStore'

const axiosPlugin: Plugin = ({ $axios }) => {
  initializeAxios($axios)

  const token = AuthStore.token?.access_token
  if (token) {
    $axios.setToken(token, 'Bearer')
  }
}

export default axiosPlugin
