import Vue from 'vue'
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
    // merge without duplicates
    const newServices = services.filter(s => this._services
      .find(i => i.id === s.id) === undefined
    )
    this._services = this._services.concat(newServices)
  }

  @Mutation
  private setRegisteredServices(services: Array<ServiceModel>) {
    // merge without duplicates
    const newServices = services.filter(s => this._registeredServices
      .find(i => i.id === s.id) === undefined
    )
    this._registeredServices = this._registeredServices.concat(newServices)
  }

  // actions
  @Action
  public async fetchServices() {
    const response = await $api.service.listServices()
    if (response.successful) {
      this.context.commit('setServices', response.data!)
    } else {
      Vue.toasted.error('Error while fetching services')
    }
    return response.data
  }

  @Action
  public async fetchRegisteredServices() {
    const response = await $api.service.listRegisteredServices()
    if (response.successful) {
      this.context.commit('setRegisteredServices', response.data!)
    } else {
      Vue.toasted.error('Error while fetching services')
    }
    return response.data
  }

  @Action
  public async fetchService(serviceId: number) {
    const response = await $api.service.getService(serviceId)
    if (response.successful) {
      // TODO ?
    } else {
      Vue.toasted.error('Error while fetching service data')
    }
    return response.data
  }

  @Action
  public async registerService(serviceId: number, username: string, password: string) {
    const response = await $api.service.registerService(serviceId, username, password)
    if (response.successful) {
      // TODO
      return true
    } else {
      Vue.toasted.error('Error while subscribing to a service')
      return false
    }
  }

  @Action
  public async unregisterService(serviceId: number) {
    const response = await $api.service.unregisterService(serviceId)
    if (response.successful) {
      // TODO
      return true
    } else {
      Vue.toasted.error('Error while unsubscribing from a service')
      return false
    }
  }
}

export default getModule(ServiceModule)
