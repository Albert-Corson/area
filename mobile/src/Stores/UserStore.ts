import {
  observable, action, makeAutoObservable, runInAction,
} from 'mobx'
import * as SecureStore from 'expo-secure-store'
import {RootStore} from './RootStore'
import absFetch from '../Tools/Network'
import {Response} from '../Types/API'

export interface UserJWT {
  refreshToken: string | null | undefined;
  accessToken: string | null | undefined;
  expiresIn: number | null | undefined;
  username: string | null | undefined;
}

export interface User {
  id: number | undefined;
  email: string | undefined;
  username: string | undefined;
}

export interface Device {
  id: number;
  last_used: number;
  country: string;
  device: string;
  browser: string;
  browser_version: string;
  os: string;
  os_version: string;
  architecture: string;
}

export interface DeviceInfo {
  current: number;
  devices: Device[]
}

export class UserStore {
  private _userJWT: UserJWT;
  private _user: User;

  constructor(private _rootStore: RootStore) {
    makeAutoObservable(this)
  }

  public get userJWT(): UserJWT | undefined {
    return this._userJWT
  }

  public async devices(): Promise<DeviceInfo> {
    const res = await absFetch({
      route: '/users/me/devices',
      headers: {
        Authorization: `Bearer ${this._userJWT?.accessToken}`
      },
    })
    const json: Response<DeviceInfo> = await res.json()


    return json.data
  }

  @action
  public storeUser = async (
    refreshToken: string,
    accessToken: string,
    expiresIn: number,
    username: string,
  ): Promise<boolean> => {
    this._userJWT = {
      refreshToken,
      accessToken,
      expiresIn,
      username,
    }

    if (!(await SecureStore.isAvailableAsync())) {
      return false
    }

    const config: SecureStore.SecureStoreOptions = {
      keychainAccessible: SecureStore.WHEN_UNLOCKED_THIS_DEVICE_ONLY,
    }

    Object.keys(this._userJWT).forEach(async (key) => {
      const value = this._userJWT[key as keyof UserJWT]

      await SecureStore.setItemAsync(key, value ? value.toString() : '', config)
    })

    return true
  };

  @action
  public loadCurrentUser = async (): Promise<UserJWT | null> => {
    try {
      const refreshToken = await SecureStore.getItemAsync('refreshToken')
      const accessToken = await SecureStore.getItemAsync('accessToken')
      const expiresIn = parseInt(await SecureStore.getItemAsync('expiresIn') ?? '')
      const username = await SecureStore.getItemAsync('username')

      runInAction(() => this._userJWT = {
        refreshToken, accessToken, expiresIn, username,
      })

      return this._userJWT
    } catch {
      return null
    }
  };

  @action
  public removeCurrentUser = async (): Promise<void> => {
    await Promise.all([
      SecureStore.deleteItemAsync('refreshToken'),
      SecureStore.deleteItemAsync('accessToken'),
      SecureStore.deleteItemAsync('expiresIn'),
      SecureStore.deleteItemAsync('username')
    ])

    this._user = {
      id: undefined,
      username: undefined,
      email: undefined,
    }
    this._userJWT = {
      refreshToken: undefined,
      accessToken: undefined,
      expiresIn: undefined,
      username: undefined,
    }
  };

  @action
  public refreshToken = async (): Promise<boolean> => {
    if (!this._userJWT?.refreshToken) return false

    const res = await absFetch({
      route: '/auth/refresh',
      method: 'post',
      body: JSON.stringify({
        refresh_token: this._userJWT.refreshToken,
      }),
    })
    const json: Response = await res.json()
    const {refresh_token, access_token, expires_in} = json.data

    await this.storeUser(refresh_token, access_token, expires_in, this._userJWT.username || '')

    return json.successful
  };

  @action
  public loadUser = async (): Promise<void> => {
    if (this._user) return

    const res = await absFetch({
      route: '/users/me',
      headers: {
        Authorization: `Bearer ${this._userJWT?.accessToken}`
      },
    })

    const json: Response = await res.json()

    if (json.successful) {
      runInAction(() => this._user = json.data)
    }
  }

  public get user(): User {
    return this._user
  }
}
