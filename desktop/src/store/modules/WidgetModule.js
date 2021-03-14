import { ServiceRepository } from "@/repositories"

const ServiceModule = {
  namespaced: true,

  state: {
    services: [],
    myServices: []
  },

  mutations: {
    SET_SERVICES(state, payload) {
      state.services = payload
    },
    SET_MY_SERVICES(state, payload) {
      state.myServices = payload
    }
  },

  actions: {
    async listServices({ commit }) {
      const response = await ServiceRepository.listServices()
      if (response.successful) {
        commit("SET_SERVICES", response.data)
      }
      return response
    },

    async listMyServices({ commit }) {
      const response = await ServiceRepository.listMyServices()
      if (response.successful) {
        commit("SET_MY_SERVICES", response.data)
      }
      return response
    },

    async getService(_, serviceId) {
      return await ServiceRepository.getService(serviceId)
    },

    async signinToService(_, serviceId) {
      const response = await ServiceRepository.signinToService(serviceId, {
        /* eslint-disable-next-line @typescript-eslint/camelcase */
        redirect_url: `${window.location.origin}/services/callback`,
        state: window.location.pathname
      })
      if (response.successful && response.data.requires_redirect) {
        window.location.href = response.data.redirect_url
      }
    },

    async signoutFromService(_, serviceId) {
      return await ServiceRepository.signoutFromService(serviceId)
    }
  }
}

export default ServiceModule
