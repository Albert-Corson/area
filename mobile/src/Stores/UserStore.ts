import { observable, action } from "mobx";
import { RootStore } from "./RootStore";
import * as SecureStore from 'expo-secure-store';

export interface UserJWT {
    refreshToken: string | null | undefined;
    accessToken: string | null | undefined;
    expiresIn: number | null | undefined;
}

export class UserStore {
    private _rootStore: RootStore;
    @observable private _userJWT: UserJWT;

    constructor(rootStore: RootStore) {
        this._rootStore = rootStore;
    }

    public get refreshToken(): string | null | undefined {
        return this._userJWT?.refreshToken;
    }

    public get accessToken(): string | null | undefined {
        return this._userJWT?.accessToken;
    }

    public get expiresIn(): number | null | undefined {
        return this._userJWT?.expiresIn;
    }

    @action
    public storeUser = async (refreshToken: string, accessToken: string, expiresIn: number): Promise<boolean> => {
        this._userJWT.refreshToken = refreshToken;
        this._userJWT.accessToken = accessToken;
        this._userJWT.expiresIn = expiresIn;

        if (!(await SecureStore.isAvailableAsync())) {
            return false;
        }

        const config: SecureStore.SecureStoreOptions = {
            keychainAccessible: SecureStore.WHEN_UNLOCKED_THIS_DEVICE_ONLY
        };

        Object.keys(this._userJWT).forEach(async key => {
            const value = this._userJWT[key as keyof UserJWT];

            await SecureStore.setItemAsync(key, value ? value.toString() : '', config)
        });

        return true;
    }

    @action
    public loadCurrentUser = async (): Promise<UserJWT | null> => {
        try {
            this._userJWT.refreshToken = await SecureStore.getItemAsync('refreshToken')
            this._userJWT.accessToken = await SecureStore.getItemAsync('accessToken')
            this._userJWT.refreshToken = await SecureStore.getItemAsync('expiresIn')

            return this._userJWT;
        } catch {
            return null;
        }
    };
}