import Vue from "vue"
import VueRouter, { RouteConfig } from "vue-router"

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "Home",
    component: () => {
      return import(/* webpackChunkName: "home" */ "../views/Home.vue")
    }
  },
  {
    path: "/signin",
    name: "Sign in",
    component: () => {
      return import(/* webpackChunkName: "signin" */ "../views/Signin.vue")
    }
  },
  {
    path: "/signup",
    name: "Sign up",
    component: () => {
      return import(/* webpackChunkName: "signup" */ "../views/Signup.vue")
    }
  }
]

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
})

export default router
