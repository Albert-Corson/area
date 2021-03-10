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
  interval?: number | NodeJS.Timeout;
}

export interface Interval {
  hours: number;
  minutes: number;
}

const DEBOUNCE = 500

export class WidgetStore {
  @observable private _widgets: RefreshableWidget[] = [];
  @observable private _subscribedWidgets: RefreshableWidget[] = [];
  @observable private _currentWidget: RefreshableWidget | null = null;
  @observable private _currentInterval: Interval;

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

  @action
  public updateWidget = async (widgetId: number, queryParams: Record<string, string>): Promise<boolean> => {
    let queryString = ''

    Object.keys(queryParams).map((key, i) => {
      queryString += `${i !== 0 ? '&' : ''}${key}=${queryParams[key]}`
    })

    try {
      const res = await absFetch({
        route: `/widgets/${widgetId}?${queryString}`,
        headers: {
          Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
        },
      })
      const json: Response<RefreshableWidget> = await res.json()

      if (json.successful) {
        const widget = this._widgets.filter(widget => widget.id === widgetId)[0]

        runInAction(() => {
          this._widgets[this._widgets.indexOf(widget)] = json.data
        })

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

          this._subscribedWidgets.map((widget) => {
            if (widget.interval) return
            
            widget.hours = 0
            widget.minutes = 10
            setInterval(() => {
              this.updateParameter(widget.id)
            }, (10 * 60000) + DEBOUNCE)
          })

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
      body: JSON.stringify({
        serviceId: this._currentWidget.service.id,
        redirect_url: 'xhttps://imgur.com/',
      }),
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
        if (!json[i].successful) {
          return widget
        }
          
        widget.params = json[i].data
  
        return widget
      })
      
      this._rootStore.grid.setBlocks([...this._subscribedWidgets])
    })
  }

  @action
  public updateParameter = async (widgetId: number): Promise<void> => {
    const headers = {
      Authorization: `Bearer ${this._rootStore.user.userJWT?.accessToken}`,
    }

    const res = await absFetch({
      route: `/widgets/${widgetId}`,
      headers
    })

    const json = await res.json()

    runInAction(() => {
      const widget = this._subscribedWidgets.filter(widget => widget.id === widgetId)[0]
      widget.params = json.data

      this._rootStore.grid.setBlock(widget)
    })
  }

  @action
  public setRefreshDelay = (): void => {
    if (!this._currentWidget) return

    const widgetIndex = this._subscribedWidgets.indexOf(this._currentWidget)

    if (widgetIndex < 0) return

    const {hours, minutes} = this._currentInterval

    runInAction(() => {
      this.subscribedWidgets[widgetIndex].hours = hours
    
      this.subscribedWidgets[widgetIndex].minutes = minutes
  
      if (this.subscribedWidgets[widgetIndex].interval) {
        clearInterval(this.subscribedWidgets[widgetIndex].interval as number)
      }
  
      this.subscribedWidgets[widgetIndex].interval = setInterval(() => {
        this.updateParameter(this.subscribedWidgets[widgetIndex].id)
      }, (hours * 3600000 + minutes * 60000) + DEBOUNCE)
    })
  }

  public get currentInterval(): Interval {
    return this._currentInterval
  }

  @action
  public setCurrentInterval = (interval: Interval): void => {
    this._currentInterval = interval
  }
}
