export default {
  state: {
    main: 0
  },

  getters: {
    count(state) {
      return state.main
    }
  },

  mutations: {
    DECREMENT_MAIN_COUNTER(state) {
      state.main--
    },
    INCREMENT_MAIN_COUNTER(state) {
      state.main++
    }
  },

  actions: {
    increment({ commit }) {
      commit("INCREMENT_MAIN_COUNTER")
    },
    decrement({ commit }) {
      commit("DECREMENT_MAIN_COUNTER")
    }
  }
}
