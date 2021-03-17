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
    path: "/auth/signin",
    name: "Sign in",
    meta: { requiresAuth: false },
    component: () => {
      return import(/* webpackChunkName: "signin" */ "../views/auth/Signin.vue")
    }
  },
  {
    path: "/auth/signup",
    name: "Sign up",
    meta: { requiresAuth: false },
    component: () => {
      return import(/* webpackChunkName: "signup" */ "../views/auth/Signup.vue")
    }
  },
  {
    path: "/auth/signout",
    name: "Sign out",
    meta: { requiresAuth: false },
    component: () => {
      return import(
        /* webpackChunkName: "signout" */ "../views/auth/Signout.vue"
      )
    }
  },
  {
    path: "/auth/callback",
    name: "Auth callback",
    meta: { requiresAuth: false },
    component: () => {
      return import(
        /* webpackChunkName: "callback" */ "../views/auth/Callback.vue"
      )
    }
  },
  {
    path: "/services/callback",
    name: "Services auth callback",
    meta: { requiresAuth: true },
    component: () => {
      return import(
        /* webpackChunkName: "callback" */ "../views/services/Callback.vue"
      )
    }
  },
  {
    path: "/users/me",
    name: "User profile",
    meta: { requiresAuth: true },
    component: () => {
      return import(
        /* webpackChunkName: "callback" */ "../views/users/Profile.vue"
      )
    }
  },
  {
    path: "*",
    name: "Not found",
    meta: { requiresAuth: false },
    component: () => {
      return import(/* webpackChunkName: "404" */ "../views/404.vue")
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
    router.push("/auth/signout")
  } else {
    next()
  }
})

export default router
