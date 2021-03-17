import { WidgetRepository } from "@/repositories"

const WidgetModule = {
  namespaced: true,

  state: {
    widgets: [],
    myWidgets: [],
    fetching: false
  },

  getters: {
    serviceWidgets: state => serviceId => {
      return state.widgets.filter(widget => widget.service.id === serviceId)
    }
  },

  mutations: {
    SET_WIDGETS(state, payload) {
      state.widgets = payload
    },
    SET_MY_WIDGETS(state, payload) {
      state.myWidgets = payload
    }
  },

  actions: {
    async listWidgets({ state, commit }) {
      if (state.fetching === true) {
        return null
      }
      state.fetching = true
      const response = await WidgetRepository.listWidgets()
      if (response.successful) {
        commit("SET_WIDGETS", response.data)
        state.fetching = false
      }
      return response
    },

    async listMyWidgets({ commit }) {
      const response = await WidgetRepository.listMyWidgets()
      if (response.successful) {
        commit("SET_MY_WIDGETS", response.data)
      }
      return response
    },

    async callWidget(_, { widgetId, params }) {
      return await WidgetRepository.callWidget(widgetId, params)
    },

    async subscribeToWidget(_, widgetId) {
      return await WidgetRepository.subscribeToWidget(widgetId)
    },

    async unsubscribeFromWidget(_, widgetId) {
      return await WidgetRepository.unsubscribeFromWidget(widgetId)
    }
  }
}

export default WidgetModule
