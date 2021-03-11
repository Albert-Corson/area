import { AuthRepository } from "@/repositories"

const AuthModule = {
  namespaced: true,

  state: {},

  mutations: {},

  actions: {
    async signin(_, payload) {
      const response = await AuthRepository.signin(payload)
      return response
    }
  }
}

export default AuthModule
