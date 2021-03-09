import axios from "axios"

const $axios = axios.create({
  baseURL: `http://${process.env.VUE_APP_API_HOST}:${process.env.VUE_APP_API_PORT}`,
  timeout: 3000
})

export default $axios
