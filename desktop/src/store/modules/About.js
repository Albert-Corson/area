import $axios from "@/services/http"

export default {
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
      return $axios
        .get("/api/about")
        .then(res => res.data)
        .then(({ data }) => commit("SET_ABOUT", { data }))
        .catch(console.error)
    }
  }
}
