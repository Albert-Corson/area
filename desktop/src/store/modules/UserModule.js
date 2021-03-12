import { UserRepository } from "@/repositories"

const UserModule = {
  namespaced: true,

  state: {
    id: undefined,
    username: null,
    email: null
  },

  mutations: {
    SET_INFO(state, payload) {
      state.id = payload.id
      state.username = payload.username
      state.email = payload.email
    }
  },

  actions: {
    async fetchInfo({ commit }) {
      const response = await UserRepository.fetchInfo()
      if (response.successful) {
        commit("SET_INFO", response.data)
      }
      return response
    },

    async signup(_, payload) {
      const response = await UserRepository.signup(payload)
      return response
    }
  }
}

export default UserModule
