// import Vue from 'vue'
// import { Mutation, Action, VuexModule, getModule, Module } from 'vuex-module-decorators'
// import { $api } from '~/globals/api'
// import ServiceModel from '~/api/models/ServiceModel'
//
// @Module({
//   name: 'modules/ServiceModule',
//   stateFactory: true,
//   namespaced: true
// })
// class ServiceModule extends VuexModule {
//   // state
//   private _services: Array<ServiceModel> = []
//   private _registeredServices: Array<ServiceModel> = []
//
//   // getters
//   public get services() {
//     return this._services
//   }
//
//   public get registeredServices() {
//     return this._registeredServices
//   }
//
//   // mutations
//   @Mutation
//   private setServices(services: Array<ServiceModel>) {
//     // merge without duplicates
//     const newServices = services.filter(s => this._services
//       .find(i => i.id === s.id) === undefined
//     )
//     this._services = this._services.concat(newServices)
//   }
//
//   @Mutation
//   private setRegisteredServices(services: Array<ServiceModel>) {
//     // merge without duplicates
//     const newServices = services.filter(s => this._registeredServices
//       .find(i => i.id === s.id) === undefined
//     )
//     this._registeredServices = this._registeredServices.concat(newServices)
//   }
//
//   // actions
//   @Action
//   public async fetchServices() {
//     try {
//       const response = await $api.service.listServices()
//       if (response.successful) {
//         this.setServices(response.data!)
//         return response.data
//       }
//     } catch (e) {
//       Vue.toasted.error('Error while fetching services')
//     }
//   }
//
//   @Action
//   public async fetchRegisteredServices() {
//     try {
//       const response = await $api.service.listRegisteredServices()
//       if (response.successful) {
//         this.setRegisteredServices(response.data!)
//         return response.data
//       }
//     } catch (e) {
//       Vue.toasted.error('Error while fetching services')
//     }
//   }
//
//   @Action
//   public async fetchService(serviceId: number) {
//     try {
//       const response = await $api.service.getService(serviceId)
//       if (response.successful) {
//         // TODO ?
//         return response.data
//       }
//     } catch (e) {
//       Vue.toasted.error('Error while fetching service data')
//     }
//   }
//
//   @Action
//   public async registerService(serviceId: number) {
//     try {
//       const response = await $api.service.registerService(serviceId)
//       if (response.successful) {
//         // TODO
//         return response.data!
//       }
//     } catch (e) {
//       Vue.toasted.error('Error while subscribing to a service')
//     }
//     return null
//   }
//
//   @Action
//   public async unregisterService(serviceId: number) {
//     try {
//       const response = await $api.service.unregisterService(serviceId)
//       if (response.successful) {
//         // TODO
//         return true
//       }
//     } catch (e) {
//       Vue.toasted.error('Error while unsubscribing from a service')
//     }
//     return false
//   }
// }
//
// export default ServiceModule
