import Vue from 'vue'
import { Mutation, Action, VuexModule, getModule, Module } from 'vuex-module-decorators'
import WidgetModel from '~/api/models/WidgetModel'
import { $api } from '~/globals/api'
import { ServiceStore } from '~/store'

@Module({
  name: 'modules/WidgetModule',
  stateFactory: true,
  namespaced: true
})
class WidgetModule extends VuexModule {
  // state
  private _widgets: Array<WidgetModel> = []
  private _registeredWidgets: Array<WidgetModel> = []

  // getters
  public get widgets() {
    return this._widgets
  }

  public get registeredWidget() {
    return this._registeredWidgets
  }

  // mutations
  @Mutation
  private setWidgets(widgets: Array<WidgetModel>) {
    // merge without duplicates
    const newWidgets = widgets.filter(w => this._widgets
      .find(i => i.id === w.id) === undefined
    )
    this._widgets = this._widgets.concat(newWidgets)
  }

  @Mutation
  private setRegisteredWidgets(widgets: Array<WidgetModel>) {
    // merge without duplicates
    const newWidgets = widgets.filter(w => this._registeredWidgets
      .find(i => i.id === w.id) === undefined
    )
    this._registeredWidgets = this._registeredWidgets.concat(newWidgets)
  }

  // actions
  @Action
  public async fetchWidgets(serviceId?: number) {
    const response = await $api.widget.listWidgets(serviceId)
    if (response.successful) {
      this.setWidgets(response.data!)
    } else {
      Vue.toasted.error('Error while fetching widgets')
    }
    return response.data
  }

  @Action
  public async fetchRegisteredWidgets(serviceId?: number) {
    try {
      const response = await $api.widget.listRegisteredWidgets(serviceId)
      if (response.successful) {
        this.setRegisteredWidgets(response.data!)
        return response.data
      }
    } catch (e) {
      Vue.toasted.error('Error while fetching widgets')
    }
  }

  @Action
  public async fetchWidgetData({ widgetId, params }: { widgetId: number, params?: object }) {
    try {
      const response = await $api.widget.fetchWidgetData(widgetId, params)
      if (response.successful) {
        // TODO ?
        return response.data
      }
    } catch (e) {
      const res = e.response
      if (res?.data?.error) {
        Vue.toasted.error(res.data.error)
      } else {
        Vue.toasted.error('Error while fetching widget data')
      }
      if (res?.status === 401) {
        // widget requires authentication
        const data = await ServiceStore.registerService(widgetId)
        if (data) {
          window.open(data)
        }
      }
      return { code: res?.status, ...res?.data }
    }
  }

  @Action
  public async registerWidget(widgetId: number) {
    try {
      const response = await $api.widget.registerWidget(widgetId)
      if (response.successful) {
        // TODO
        return true
      }
    } catch (e) {
      Vue.toasted.error('Error while subscribing to a widget')
    }
    return false
  }

  @Action
  public async unregisterWidget(widgetId: number) {
    try {
      const response = await $api.widget.unregisterWidget(widgetId)
      if (response.successful) {
        // TODO
        return true
      }
    } catch (e) {
      Vue.toasted.error('Error while unsubscribing from a widget')
    }
    return false
  }
}

export default WidgetModule
