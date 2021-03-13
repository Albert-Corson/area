import { UserRepository } from "@/repositories"

const UserModule = {
  namespaced: true,

  state: {
    id: undefined,
    username: null,
    email: null,
    devices: [],
    currentDevice: null
  },

  mutations: {
    SET_INFO(state, payload) {
      state.id = payload.id
      state.username = payload.username
      state.email = payload.email
    },
    SET_DEVICES(state, payload) {
      state.devices = payload
    },
    SET_CURRENT_DEVICE(state, payload) {
      state.currentDevice = payload
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
    },

    async listDevices({ commit }) {
      const response = await UserRepository.listDevices()
      if (response.successful) {
        commit("SET_DEVICES", response.data.devices)
        console.log(response.data.devices)
        commit(
          "SET_CURRENT_DEVICE",
          response.data.devices.find(
            device => device.id === response.data.current_device
          )
        )
      }
      return response
    }
  }
}

export default UserModule
