import { Mutation, Action, VuexModule, getModule, Module } from 'vuex-module-decorators'
import Vue from 'vue'
import { store } from '~/store'
import AuthTokenModel from '~/api/models/AuthTokenModel'
import { $api } from '~/globals/api'

@Module({
  dynamic: true,
  store,
  name: 'auth',
  stateFactory: true,
  namespaced: true
})
class AuthModule extends VuexModule {
  // state
  private _token?: AuthTokenModel

  // getters
  public get token() {
    return this._token
  }

  public get authenticated() {
    return Boolean(this._token)
  }

  // mutations
  @Mutation
  private setToken(token: AuthTokenModel) {
    this._token = token
  }

  // actions
  @Action
  public async getToken({ username, password }: { username: string, password: string }) {
    const response = await $api.auth.getToken(username, password)
    if (response.successful) {
      Vue.toasted.success('Successfully logged in')
      this.context.commit('setToken', response.data!)
    }
  }

  @Action
  public async refreshToken() {
    if (!this.authenticated) {
      return
    }
    const response = await $api.auth.refreshToken(this._token?.refresh_token!)
    if (response.successful) {
      this.context.commit('setToken', response.data!)
    }
  }

  @Action
  public async revokeToken() {
    const response = await $api.auth.revokeToken()
    if (response.successful) {
      // TODO
    }
  }

}

export default getModule(AuthModule)
