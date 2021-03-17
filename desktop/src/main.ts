import Vue from "vue"
import App from "./App.vue"
import "./registerServiceWorker"
import router from "./router"
import store from "./store"
import axios from "@/services/http"
import vueSmoothScroll from "vue2-smooth-scroll"
// @ts-ignore
import VuePlyr from "vue-plyr"
import { AxiosStatic } from "axios"

import "vue-plyr/dist/vue-plyr.css"

Vue.use(vueSmoothScroll)
Vue.use(VuePlyr, {
  plyr: {
    controls: [],
    muted: true,
    resetOnEnd: true,
    ratio: "1:1"
  }
})
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
