import axios, { AxiosResponse, AxiosRequestConfig, AxiosError } from "axios"

let accessToken = window.localStorage.getItem("access_token")

export const setBearerToken = (token: string) => {
  accessToken = token
  window.localStorage.setItem("access_token", token)
}
export const unsetBearerToken = () => {
  accessToken = null
  window.localStorage.removeItem("access_token")
}

const $axios = axios.create({
  baseURL: `http://${process.env.VUE_APP_API_HOST}:${process.env.VUE_APP_API_PORT}`,
  timeout: 3000
})
$axios.interceptors.request.use((config: AxiosRequestConfig) => {
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`
  }
  return config
})
$axios.interceptors.response.use(
  (response: AxiosResponse) => {
    return response.data
  },
  (error: AxiosError) => {
    if (error.response?.status === 403) {
      window.location.href = `${window.location.origin}/auth/signout`
    }
  }
)

export default $axios
