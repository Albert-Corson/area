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
  public get widget() {
    return this._widgets
  }

  public get registeredWidget() {
    return this._registeredWidgets
  }

  // mutations
  @Mutation
  private setWidgets(widgets: Array<WidgetModel>) {
    this._widgets = widgets
  }

  @Mutation
  private setRegisteredWidgets(widgets: Array<WidgetModel>) {
    this._registeredWidgets = widgets
  }

  // actions
  @Action
  public async fetchWidgets() {
    const response = await $api.widget.listWidgets()
    if (response.successful) {
      this.context.commit('setWidgets', response.data!)
    }
  }

  @Action
  public async fetchRegisteredWidgets() {
    const response = await $api.widget.listRegisteredWidgets()
    if (response.successful) {
      this.context.commit('setRegisteredWidgets', response.data!)
    }
  }

  @Action
  public async fetchWidgetData(widgetId: number, params?: object) {
    const response = await $api.widget.fetchWidgetData(widgetId, params)
    if (response.successful) {
      // TODO ?
      return response.data!
    }
  }

  @Action
  public async registerWidget(widgetId: number, params?: object) {
    const response = await $api.widget.registerWidget(widgetId, params)
    if (response.successful) {
      // TODO
    }
  }

  @Action
  public async unregisterWidget(widgetId: number) {
    const response = await $api.widget.unregisterWidget(widgetId)
    if (response.successful) {
      // TODO
    }
  }
}

export default getModule(WidgetModule)
