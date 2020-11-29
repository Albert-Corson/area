import Vue from 'vue'
import { Mutation, Action, VuexModule, getModule, Module } from 'vuex-module-decorators'
import { store } from '~/store'
import WidgetModel from '~/api/models/WidgetModel'
import { $api } from '~/globals/api'

@Module({
  dynamic: true,
  store,
  name: 'widget',
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
      this.context.commit('setWidgets', response.data!)
    } else {
      Vue.toasted.error('Error while fetching widgets')
    }
    return response.data
  }

  @Action
  public async fetchRegisteredWidgets(serviceId?: number) {
    const response = await $api.widget.listRegisteredWidgets(serviceId)
    if (response.successful) {
      this.context.commit('setRegisteredWidgets', response.data!)
    } else {
      Vue.toasted.error('Error while fetching widgets')
    }
    return response.data
  }

  @Action
  public async fetchWidgetData(widgetId: number, params?: object) {
    const response = await $api.widget.fetchWidgetData(widgetId, params)
    if (response.successful) {
      // TODO ?
    } else {
      Vue.toasted.error('Error while fetching widget data')
    }
    return response.data
  }

  @Action
  public async registerWidget(widgetId: number, params?: object) {
    const response = await $api.widget.registerWidget(widgetId, params)
    if (response.successful) {
      // TODO
      return true
    } else {
      Vue.toasted.error('Error while subscribing to a widget')
      return false
    }
  }

  @Action
  public async unregisterWidget(widgetId: number) {
    const response = await $api.widget.unregisterWidget(widgetId)
    if (response.successful) {
      // TODO
      return true
    } else {
      Vue.toasted.error('Error while unsubscribing from a widget')
      return false
    }
  }
}

export default getModule(WidgetModule)
