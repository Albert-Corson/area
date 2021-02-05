import {observable, action, makeAutoObservable, runInAction} from 'mobx';
import {Widget} from '../Types/Widgets';
import {Response} from '../Types/API';
import {RootStore} from './RootStore';
import absFetch from '../Tools/Network';

enum Action {
    add,
    remove
} 

export class WidgetStore {
    @observable private _widgets: Widget[] = [];
    @observable private _subscribedWidgets: Widget[] = [];

    constructor(private _rootStore: RootStore) {
      makeAutoObservable(this);
    }

    public get widgets(): Widget[] {
      return this._widgets;
    }

    public get availableWidgets(): Widget[] {
      return this.processAvailableWidgets();
    }

    public updateWidgets = async (): Promise<boolean> => {
      const values: boolean[] = await Promise.all([this.fetchSubscribedWidgets(), this.fetchWidgets()]);

      return !values.includes(false);
    }

    private processAvailableWidgets = (): Widget[] => {
      const subscribedIds: number[] = this._subscribedWidgets.map((obj: Widget) => obj.id);

      return this._widgets.filter((widget: Widget) => !subscribedIds.includes(widget.id));
    }

    @action
    private fetchWidgets = async (): Promise<boolean> => {
      try {
        const res = await absFetch({
          route: '/widgets', headers: {
            'Authorization': `Bearer ${this._rootStore.user.userJWT?.accessToken}`
          }
        });
        const json: Response<Widget[]> = await res.json();

        if (json.successful) {
          runInAction(() => this._widgets = json.data);

          return true;
        }
      } catch (e) {
        console.warn('Error occurred while fetching API: ');
        console.warn(e);
      }

      return false;
    }

    @action
    private fetchSubscribedWidgets = async (): Promise<boolean> => {
      try {
        const res = await absFetch({
          route: '/widgets/me', headers: {
            'Authorization': `Bearer ${this._rootStore.user.userJWT?.accessToken}`
          }
        });
        const json: Response<Widget[]> = await res.json();

        if (json.successful) {
          runInAction(() => {
            this._subscribedWidgets = json.data;
            this._rootStore.grid.setBlocks(json.data);
          });

          return true;
        }
      } catch (e) {
        console.warn('Error occurred while fetching API: ');
        console.warn(e);
      }

      return false;
    }

    @action
    private widgetSubscribtion = async (widgetId: number, action: Action): Promise<void> => {
        const res = await absFetch({
            route: `/widgets/${widgetId}`,
            method: action === Action.add ? 'post' : 'delete',
            headers: {
              'Authorization': `Bearer ${this._rootStore.user.userJWT?.accessToken}`
            }
          });
          const json: Response = await res.json();
    
          if (json.successful) {
            this.updateWidgets();
          }
    }

    @action
    public subscribeToWidget = async (widgetId: number): Promise<void> => this.widgetSubscribtion(widgetId, Action.add);

    @action
    public unsubscribeToWidget = async (widgetId: number): Promise<void> => this.widgetSubscribtion(widgetId, Action.remove);
}
