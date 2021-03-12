import { setBearerToken, unsetBearerToken } from "@/services/http"
import { AuthRepository } from "@/repositories"

const AuthModule = {
  namespaced: true,

  state: {
    accessToken: null,
    refreshToken: null,
    expiresIn: 0
  },

  getters: {
    isAuthenticated(state) {
      return state.accessToken !== null
    }
  },

  mutations: {
    SET_TOKEN(state, payload) {
      state.accessToken = payload.access_token
      state.refreshToken = payload.refresh_token
      state.expiresIn = payload.expires_in
      setBearerToken(payload.access_token)
    },

    UNSET_TOKEN(state) {
      state.accessToken = null
      state.refreshToken = null
      state.expiresIn = 0
      unsetBearerToken()
    }
  },

  actions: {
    async localSignin({ commit }, payload) {
      const response = await AuthRepository.signin(payload)
      if (response.successful) {
        commit("SET_TOKEN", response.data)
        delete response.data
      }
      return response
    },

    async signinWithFacebook() {
      const response = await AuthRepository.signinWithFacebook({
        /* eslint-disable-next-line @typescript-eslint/camelcase */
        redirect_url: `${window.location.origin}/callback`,
        state: window.location.pathname
      })
      if (response.data.requires_redirect) {
        window.location.href = response.data.redirect_url
      }
    },

    async signinWithGoogle() {
      const response = await AuthRepository.signinWithGoogle({
        /* eslint-disable-next-line @typescript-eslint/camelcase */
        redirect_url: `${window.location.origin}/callback`,
        state: window.location.pathname
      })
      if (response.data.requires_redirect) {
        window.location.href = response.data.redirect_url
      }
    },

    async signinWithMicrosoft() {
      const response = await AuthRepository.signinWithMicrosoft({
        /* eslint-disable-next-line @typescript-eslint/camelcase */
        redirect_url: `${window.location.origin}/callback`,
        state: window.location.pathname
      })
      if (response.data.requires_redirect) {
        window.location.href = response.data.redirect_url
      }
    },

    async exchangeAuthCode({ commit }, payload) {
      const response = await AuthRepository.exchangeAuthCode(payload)
      if (response.successful) {
        commit("SET_TOKEN", response.data)
        delete response.data
      }
      return response
    },

    async signout({ commit }) {
      commit("UNSET_TOKEN")
    }
  }
}

export default AuthModule
