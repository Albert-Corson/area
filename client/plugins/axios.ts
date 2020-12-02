import { initializeAxios } from '~/globals/axios'
import { Plugin } from '@nuxt/types'

const axiosPlugin: Plugin = ({ $axios }) => {
  initializeAxios($axios)

  $axios.onRequest(config => {
    console.log(`Making request to ${ config.url }`)
  })

  $axios.onError(_ => {
    console.error(`HTTP error`)
  })
}

export default axiosPlugin
