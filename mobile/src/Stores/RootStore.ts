import {GridStore} from './GridStore';
import {AuthStore} from './AuthStore';
import {UserStore} from './UserStore';
import {WidgetStore} from './WidgetStore';
import {createContext} from 'react';

export class RootStore {
  public user: UserStore = new UserStore(this);

  public auth: AuthStore = new AuthStore(this);

  public widget: WidgetStore = new WidgetStore(this);

  public grid: GridStore = new GridStore(this);

  constructor() {
    this._init();
  }

  private _init = async (): Promise<void> => {
    if (!this.user.userJWT) {
      await this.user.loadCurrentUser();
      await this.user.refreshToken();
      await this.widget.updateWidgets();
    }
  };
}

export default createContext(new RootStore());
