import { Mutation, Action, VuexModule, getModule, Module } from 'vuex-module-decorators'
import { store } from '~/store'
import AuthToken from '~/api/models/AuthToken'
import { $api } from '~/globals/api'

@Module({
  dynamic: true,
  store,
  name: 'auth',
  stateFactory: true,
  namespaced: true
})
export class AuthModule extends VuexModule {
  // state
  _token?: AuthToken

  // getters
  public get token() {
    return this._token
  }

  public get authenticated() {
    return Boolean(this._token)
  }

  // mutations
  @Mutation
  private setToken(token: AuthToken) {
    this._token = token
  }

  // actions
  @Action({ rawError: true })
  public async login(username: string, password: string) {
    const response = await $api.auth.login(username, password)
    if (response.successful) {
      this.context.commit('setToken', response.data!)
    }
  }
}

export default getModule(AuthModule)
