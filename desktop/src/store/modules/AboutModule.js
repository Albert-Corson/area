import { AboutRepository } from "@/repositories"

const AboutModule = {
  namespaced: true,

  state: {
    about: {}
  },

  mutations: {
    SET_ABOUT(state, about) {
      state.about = about
    }
  },

  actions: {
    async fetchAbout({ commit }) {
      const response = await AboutRepository.fetch()
      if (response.successful) {
        commit("SET_ABOUT", { data: response.data })
      }
    }
  }
}

export default AboutModule
