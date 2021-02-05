import {observable, action, makeAutoObservable, runInAction} from 'mobx';
import {RootStore} from './RootStore';
import * as SecureStore from 'expo-secure-store';
import absFetch from '../Tools/Network';
import {Response} from '../Types/API';

export interface UserJWT {
    refreshToken: string | null | undefined;
    accessToken: string | null | undefined;
    expiresIn: number | null | undefined;
    username: string | null | undefined;
}

export class UserStore {
    @observable private _userJWT: UserJWT;

    constructor(private _rootStore: RootStore) {
      makeAutoObservable(this);
    }

    public get userJWT(): UserJWT | undefined {
      return this._userJWT;
    }

    @action
    public storeUser = async (
      refreshToken: string,
      accessToken: string,
      expiresIn: number,
      username: string
    ): Promise<boolean> => {
      this._userJWT = {
        refreshToken,
        accessToken,
        expiresIn,
        username
      };

      if (!(await SecureStore.isAvailableAsync())) {
        return false;
      }

      const config: SecureStore.SecureStoreOptions = {
        keychainAccessible: SecureStore.WHEN_UNLOCKED_THIS_DEVICE_ONLY
      };

      Object.keys(this._userJWT).forEach(async key => {
        const value = this._userJWT[key as keyof UserJWT];

        await SecureStore.setItemAsync(key, value ? value.toString() : '', config);
      });

      return true;
    }

    @action
    public loadCurrentUser = async (): Promise<UserJWT | null> => {
      try {
        const refreshToken = await SecureStore.getItemAsync('refreshToken'),
          accessToken = await SecureStore.getItemAsync('accessToken'),
          expiresIn = parseInt(await SecureStore.getItemAsync('expiresIn') ?? ''),
          username = await SecureStore.getItemAsync('username');

        runInAction(() => this._userJWT = {refreshToken, accessToken, expiresIn, username});

        return this._userJWT;
      } catch {
        return null;
      }
    };

    @action
    public refreshToken = async (): Promise<boolean> => {
      if (!this._userJWT?.refreshToken) return false;

      const res = await absFetch({
        route: '/auth/refresh',
        method: 'post',
        body: JSON.stringify({
          refresh_token: this._userJWT.refreshToken
        })
      });
      const json: Response = await res.json();
      const {refresh_token, access_token, expires_in} = json.data;

      await this.storeUser(refresh_token, access_token, expires_in, this._userJWT.username || '');

      return json.successful;
    }
}