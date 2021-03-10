import axios, { AxiosResponse, AxiosRequestConfig } from "axios"

const $axios = axios.create({
  baseURL: `http://${process.env.VUE_APP_API_HOST}:${process.env.VUE_APP_API_PORT}`,
  timeout: 3000
})
$axios.interceptors.request.use((config: AxiosRequestConfig) => {
  // TODO: add Authorization header
  return config
})
$axios.interceptors.response.use((response: AxiosResponse) => {
  return response.data
})

export default $axios
