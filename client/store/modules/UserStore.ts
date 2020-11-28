import { Mutation, Action, VuexModule, getModule, Module } from 'vuex-module-decorators'
import { store } from '~/store'
import UserModel from '~/api/models/UserModel'
import { $api } from '~/globals/api'

@Module({
  dynamic: true,
  store,
  name: 'user',
  stateFactory: true,
  namespaced: true
})
class UserModule extends VuexModule {
  // state
  private _user?: UserModel

  // getters
  public get user() {
    return this._user
  }

  // mutations
  @Mutation
  private setUser(user: UserModel) {
    this._user = user
  }

  // actions
  @Action
  public async fetchUser() {
    const response = await $api.user.getUser()
    if (response.successful) {
      this.context.commit('setUser', response.data!)
    }
  }

  @Action
  public async createUser(username: string, password: string, email: string) {
    const response = await $api.user.createUser(username, password, email)
    if (response.successful) {
      // TODO
    }
  }

  @Action
  public async deleteUser(userId: number) {
    const response = await $api.user.deleteUser(userId)
    if (response.successful) {
      // TODO
    }
  }
}

export default getModule(UserModule)
