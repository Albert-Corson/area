import Vue from "vue"
import App from "./App.vue"
import "./registerServiceWorker"
import router from "./router"
import store from "./store"
import axios from "@/services/http"
import vueSmoothScroll from "vue2-smooth-scroll"
import { AxiosStatic } from "axios"

Vue.use(vueSmoothScroll)
Vue.config.productionTip = false

Vue.prototype.$axios = axios

declare module "vue/types/vue" {
  export interface Vue {
    $axios: AxiosStatic
  }
}

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app")
