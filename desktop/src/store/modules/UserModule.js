import { UserRepository } from "@/repositories"

const UserModule = {
  namespaced: true,

  state: {},

  mutations: {},

  actions: {
    async signup(_, payload) {
      console.log("signup -> store")
      const response = await UserRepository.signup(payload)
      return response
    }
  }
}

export default UserModule
