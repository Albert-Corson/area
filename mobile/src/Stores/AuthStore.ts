import { resolvePlugin } from "@babel/core";
import { makeAutoObservable, observable, action } from "mobx";
import { RootStore } from "./RootStore";
import absFetch from '../Tools/Network';
import { UserJWT } from "./UserStore";

export class AuthStore {
  private _rootStore: RootStore;
  @observable private _email: string = '';
  @observable private _username: string = '';
  @observable private _password: string = '';
  @observable private _confirm: string = '';
  @observable private _error: string = '';

  constructor(rootStore: RootStore) {
    this._rootStore = rootStore;
    makeAutoObservable(this);
  }

  public get email() {
    return this._email.toLowerCase();
  }

  @action
  public set email(value) {
    this._email = value;
  }

  public get username() {
    return this._username.toLowerCase();
  }

  @action
  public set username(value) {
    this._username = value;
  }

  public get password() {
    return this._password.toLowerCase();
  }

  @action
  public set password(value) {
    this._password = value;
  }

  public get confirm() {
    return this._confirm.toLowerCase();
  }

  @action
  public set confirm(value) {
    this._confirm = value;
  }

  public get error() {
    return this._error;
  }

  @action
  public set error(value) {
    this._error = value;
  }

  @action
  public signUp = async (): Promise<boolean> => {
    if (this._password !== this._confirm) {
      this._error = 'Passwords must be equals';
      return false;
    } else if (!this._email.length || !this._username.length || !this._password.length) {
      this._error = 'All entry are required';
      return false;
    }


    const res = await absFetch({
      route: '/users',
      method: 'post',
      body: JSON.stringify({
        email: this._email,
        username: this._username,
        password: this._password,
      }),
    });

    try {
      const json = await res.json();
      return true;
    } catch {
      console.warn('error while parsing json');
    }
    return false;
  }

  @action
  public signIn = async (): Promise<boolean> => {
    if (!this._email.length || !this._password.length) {
      this._error = 'All entry are required';
      return false;
    }

    const res = await absFetch({
      route: '/auth/token',
      method: 'post',
      body: JSON.stringify({
        username: this._username,
        password: this._password,
      }),
    });

    try {
      const body = await res.json();

      if (!body.successful) {
        this.error = body.error ?? "Error occured";
        return false;
      }

      return await this._rootStore.user.storeUser(
        body.refresh_token,
        body.access_token,
        body.expires_in
      );
    } catch {
      this.error = "Error occured";
    }

    return false;
  };

  @action
  public resetPassword = () => {
    console.warn('Not implemented yet');
  }

};
