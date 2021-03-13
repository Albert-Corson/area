import Vue from "vue"
import VueRouter, { RouteConfig } from "vue-router"
import store from "@/store"

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "Home",
    meta: { requiresAuth: true },
    component: () => {
      return import(/* webpackChunkName: "home" */ "../views/Home.vue")
    }
  },
  {
    path: "/signin",
    name: "Sign in",
    meta: { requiresAuth: false },
    component: () => {
      return import(/* webpackChunkName: "signin" */ "../views/Signin.vue")
    }
  },
  {
    path: "/signup",
    name: "Sign up",
    meta: { requiresAuth: false },
    component: () => {
      return import(/* webpackChunkName: "signup" */ "../views/Signup.vue")
    }
  },
  {
    path: "/signout",
    name: "Sign out",
    meta: { requiresAuth: false },
    component: () => {
      return import(/* webpackChunkName: "signout" */ "../views/Signout.vue")
    }
  },
  {
    path: "/callback",
    name: "Auth callback",
    meta: { requiresAuth: false },
    component: () => {
      return import(
        /* webpackChunkName: "callback" */ "../views/AuthCallback.vue"
      )
    }
  }
]

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, _, next) => {
  const isAuthenticated = store.getters["Auth/isAuthenticated"]
  if (to.meta.requiresAuth && isAuthenticated === false) {
    router.push("/signout")
  } else {
    next()
  }
})

export default router
