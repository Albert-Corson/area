import {
  observable, action, makeAutoObservable, runInAction,
} from 'mobx'
import {Widget} from '../Types/Widgets'
import {Response} from '../Types/API'
import {RootStore} from './RootStore'
import absFetch from '../Tools/Network'

enum Action {
  add,
  remove,
}

interface RefreshableWidget extends Widget {
  hours?: number;
  minutes?: number;
  interval?: NodeJS.Timeout;
}

export class WidgetStore {
  @observable private _widgets: RefreshableWidget[] = [];
  @observable private _subscribedWidgets: RefreshableWidget[] = [];
  @observable private _currentWidget: RefreshableWidget | null = null;

  constructor(private _rootStore: RootStore) {
    makeAutoObservable(this)
  }

  public get widgets(): RefreshableWidget[] {
    return this._widgets
  }

  public get subscribedWidgets(): RefreshableWidget[] {
    return this._subscribedWidgets
  }

  public get currentWidget(): RefreshableWidget | null {
    return this._currentWidget
  }

  @action
  public set currentWidget(value: RefreshableWidget | null) {
    this._currentWidget = value
  }

  public get availableWidgets(): RefreshableWidget[] {
    return this.processAvailableWidgets()
  }

  public updateWidgets = async (): Promise<boolean> => {
    const values: boolean[] = await Promise.all([this.fetchSubscribedWidgets(), this.fetchWidgets()])

    return !values.includes(false)
  };

  public updateWidget = async (index: number): Promise<boolean> => {
    try {
      const res = await absFetch({
        route: `/widgets/${index}`,
        headers: {
          Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
        },
      })
      const json: Response<RefreshableWidget> = await res.json()

      if (json.successful) {
        runInAction(() => this._widgets[index] = json.data)

        return true
      }
    } catch (e) {
      console.warn('Error occurred while fetching API: ')
      console.warn(e)
    }

    return false
  };

  private processAvailableWidgets = (): RefreshableWidget[] => {
    const subscribedIds: number[] = this._subscribedWidgets.map((obj: RefreshableWidget) => obj.id)

    return this._widgets.filter((widget: RefreshableWidget) => !subscribedIds.includes(widget.id))
  };

  @action
  private fetchWidgets = async (): Promise<boolean> => {
    try {
      const res = await absFetch({
        route: '/widgets',
        headers: {
          Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
        },
      })
      const json: Response<RefreshableWidget[]> = await res.json()

      if (json.successful) {
        runInAction(() => this._widgets = json.data)

        return true
      }
    } catch (e) {
      console.warn('Error occurred while fetching API: ')
      console.warn(e)
    }

    return false
  };

  @action
  private fetchSubscribedWidgets = async (): Promise<boolean> => {
    try {
      const res = await absFetch({
        route: '/widgets/me',
        headers: {
          Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
        },
      })
      const json: Response<RefreshableWidget[]> = await res.json()

      if (json.successful) {
        runInAction(() => {
          this._subscribedWidgets = json.data
          this._rootStore.grid.setBlocks(json.data)
        })

        return true
      }
    } catch (e) {
      console.warn('Error occurred while fetching API: ')
      console.warn(e)
    }

    return false
  };

  @action
  private widgetSubscribtion = async (widgetId: number, action: Action): Promise<void> => {
    const res = await absFetch({
      route: `/widgets/${widgetId}`,
      method: action === Action.add ? 'post' : 'delete',
      headers: {
        Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
      },
    })
    const json: Response = await res.json()

    if (json.successful) {
      this.updateWidgets()
    }
  };

  @action
  public subscribeToWidget = async (widgetId: number): Promise<void> => this.widgetSubscribtion(widgetId, Action.add);

  @action
  public unsubscribeToWidget = async (widgetId: number): Promise<void> => this.widgetSubscribtion(widgetId, Action.remove);

  @action
  public serviceAuthentication = async (): Promise<string | undefined> => {
    if (!this._currentWidget?.service) return

    const res = await absFetch({
      route: `/services/${this._currentWidget.service.id}`,
      method: 'post',
      headers: {
        Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
      },
    })
    const json: Response = await res.json()

    if (json.successful) {
      return json.data
    }
  }

  @action
  public updateParameters = async (): Promise<void> => {
    const headers = {
      Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
    }

    const results = await Promise.all(
      this._subscribedWidgets.map((widget) => {
        return absFetch({
          route: `/widgets/${widget.id}`,
          headers
        })
      })
    )

    const json = await Promise.all(
      results.map(res => res.json())
    )

    runInAction(() => {
      this._subscribedWidgets.forEach((widget, i) => {
        widget.params = json[i].data
  
        return widget
      })
      this._rootStore.grid.setBlocks(this._subscribedWidgets)
    })
  }

  @action
  public setRefreshDelay = (widgetIndex: number, hours: number, minutes: number): void => {
    if (widgetIndex < 0 || widgetIndex >= this._subscribedWidgets.length) return

    this.subscribedWidgets[widgetIndex].hours = hours
    
    this.subscribedWidgets[widgetIndex].minutes = minutes

    this.subscribedWidgets[widgetIndex].interval = setInterval(() => {
      this.updateWidget(widgetIndex)
    }, hours * 3600000 + minutes * 60000)
  }
}