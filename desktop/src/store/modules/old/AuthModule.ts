// import { Mutation, Action, VuexModule, Module } from 'vuex-module-decorators'
// import Vue from 'vue'
// import AuthTokenModel from '~/api/models/AuthTokenModel'
// import { $api } from '~/globals/api'
// import { $axios } from '~/globals/axios'
//
// @Module({
//   name: 'modules/AuthModule',
//   stateFactory: true,
//   namespaced: true
// })
// class AuthModule extends VuexModule {
//   // state
//   private _token: AuthTokenModel | null = null
//
//   // getters
//   public get token() {
//     return this._token
//   }
//
//   public get authenticated() {
//     return Boolean(this._token)
//   }
//
//   // mutations
//   @Mutation
//   private setToken(token: AuthTokenModel | null) {
//     this._token = token
//     $axios.setToken(token?.access_token ?? false, 'Bearer')
//   }
//
//   // actions
//   @Action
//   public async login({ username, password }: { username: string, password: string }) {
//     return $api.auth.getToken(username, password)
//       .then(response => {
//         if (response?.successful) {
//           Vue.toasted.success('Successfully logged in')
//           this.setToken(response.data!)
//         }
//       })
//       .catch(_ => Vue.toasted.error('Error while logging in'))
//   }
//
//   @Action
//   public async refreshToken() {
//     if (!this.authenticated) {
//       return
//     }
//     try {
//       const response = await $api.auth.refreshToken(this._token?.refresh_token!)
//       if (response.successful) {
//         this.setToken(response.data!)
//       }
//     } catch (e) {
//       Vue.toasted.error('Error while refreshing authorization token')
//     }
//   }
//
//   @Action
//   public async revokeToken() {
//     try {
//       const response = await $api.auth.revokeToken()
//       if (response.successful) {
//         Vue.toasted.success('Successfully revoked authorization tokens')
//         // TODO
//       }
//     } catch (e) {
//       Vue.toasted.error('Error while revoking token')
//     }
//   }
//
//   @Action
//   public logout() {
//     return new Promise((resolve) => {
//       this.setToken(null)
//       resolve()
//     })
//   }
//
// }
//
// export default AuthModule
