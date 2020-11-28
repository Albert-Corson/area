import { Mutation, Action, VuexModule, getModule, Module } from 'vuex-module-decorators'
import { store } from '~/store'
import { $api } from '~/globals/api'
import ServiceModel from '~/api/models/ServiceModel'

@Module({
  dynamic: true,
  store,
  name: 'service',
  stateFactory: true,
  namespaced: true
})
class ServiceModule extends VuexModule {
  // state
  private _services: Array<ServiceModel> = []
  private _registeredServices: Array<ServiceModel> = []

  // getters
  public get services() {
    return this._services
  }

  public get registeredServices() {
    return this._registeredServices
  }

  // mutations
  @Mutation
  private setServices(services: Array<ServiceModel>) {
    this._services = services
  }

  @Mutation
  private setRegisteredServices(services: Array<ServiceModel>) {
    this._registeredServices = services
  }

  // actions
  @Action
  public async fetchServices() {
    const response = await $api.service.listServices()
    if (response.successful) {
      this.context.commit('setServices', response.data!)
    }
  }

  @Action
  public async fetchRegisteredServices() {
    const response = await $api.service.listRegisteredServices()
    if (response.successful) {
      this.context.commit('setRegisteredServices', response.data!)
    }
  }

  @Action
  public async fetchService(serviceId: number) {
    const response = await $api.service.getService(serviceId)
    if (response.successful) {
      // TODO ?
      return response.data!
    }
  }

  @Action
  public async registerService(serviceId: number, username: string, password: string) {
    const response = await $api.service.registerService(serviceId, username, password)
    if (response.successful) {
      // TODO
    }
  }

  @Action
  public async unregisterService(serviceId: number) {
    const response = await $api.service.unregisterService(serviceId)
    if (response.successful) {
      // TODO
    }
  }
}

export default getModule(ServiceModule)
