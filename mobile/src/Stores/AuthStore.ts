import {makeAutoObservable, observable, action} from 'mobx'
import {RootStore} from './RootStore'
import absFetch from '../Tools/Network'
import {Response} from '../Types/API'

export class AuthStore {
  @observable private _email = '';
  @observable private _username = '';
  @observable private _password = '';
  @observable private _confirm = '';
  @observable private _error = '';

  constructor(private _rootStore: RootStore) {
    makeAutoObservable(this)
  }

  public get email(): string {
    return this._email.toLowerCase()
  }

  @action
  public set email(value: string) {
    this._email = value
  }

  public get username(): string {
    return this._username
  }

  @action
  public set username(value: string) {
    this._username = value
  }

  public get password(): string {
    return this._password
  }

  @action
  public set password(value: string) {
    this._password = value
  }

  public get confirm(): string {
    return this._confirm
  }

  @action
  public set confirm(value: string) {
    this._confirm = value
  }

  public get error(): string {
    return this._error
  }

  @action
  public set error(value: string) {
    this._error = value
  }

  @action
  public signUp = async (): Promise<boolean> => {
    if (this._password !== this._confirm) {
      this._error = 'Passwords must be equals'
      return false
    } if (!this._email.length || !this._username.length || !this._password.length) {
      this._error = 'All entry are required'
      return false
    }

    const res = await absFetch({
      route: '/users',
      method: 'post',
      body: JSON.stringify({
        email: this._email,
        username: this._username,
        password: this._password,
      }),
    })

    try {
      const body = await res.json()

      if (!body.successful) {
        this.error = body.error ?? 'An error occurred'
        return false
      }

      this.password = ''
      this.error = ''
      return true
    } catch {
      console.warn('error while parsing json')
    }
    return false
  };

  @action
  public signIn = async (): Promise<boolean> => {
    if (!this._username.length || !this._password.length) {
      this._error = 'All entries are required'
      return false
    }

    const res = await absFetch({
      route: '/auth/token',
      method: 'post',
      body: JSON.stringify({
        identifier: this._username,
        password: this._password,
      }),
    })

    try {
      const body = await res.json()

      if (!body.successful) {
        this.error = body.error ?? 'Error occured'
        return false
      }

      this.password = ''
      this.email = ''
      this.error = ''

      this._rootStore.user.loadUser()

      return await this._rootStore.user.storeUser(
        body.data.refresh_token,
        body.data.access_token,
        body.data.expires_in,
        this.username,
      )
    } catch (e) {
      console.warn(e)
      this.error = 'Error occured'
    }

    return false
  };

  @action
  public askForTokens = async (code: string): Promise<boolean> => {
    const res = await absFetch({
      route: '/auth/code',
      method: 'post',
      body: JSON.stringify({
        code,
      }),
    })

    const json: Response = await res.json()
    
    if (json.successful) {
      return await this._rootStore.user.storeUser(
        json.data.refresh_token,
        json.data.access_token,
        json.data.expires_in,
        'Profile',
      )
    }

    return false
  };

  @action
  public resetPassword = (): void => {
    console.warn('Not implemented yet')
  };

  @action
  public logout = async (): Promise<void> => {
    const headers = {
      Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
    }

    this._rootStore.user.removeCurrentUser()

    const deviceRes = await absFetch({
      route: '/users/me/devices',
      headers,
    })

    const deviceJson: Response = await deviceRes.json()
    const currentDevice = deviceJson?.data?.current_device

    if (currentDevice == undefined) return

    await absFetch({
      route: `/users/me/devices/${currentDevice}`,
      method: 'delete',
      headers,
    })
  }
}
