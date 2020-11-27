import { initializeAxios } from '~/globals/axios'
import { Plugin } from '@nuxt/types'

const axiosPlugin: Plugin = ({ $axios, redirect }) => {
  initializeAxios($axios)

  $axios.onRequest(config => {
    console.log(`Making request to ${ config.url }`)
  })

  $axios.onError(error => {
    const code = error.response?.status
    console.error(`HTTP error ${ code }`)
    redirect('/error')
  })
}

export default axiosPlugin
