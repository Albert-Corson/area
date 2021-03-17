import $axios from "@/services/http"
import { Response, Widget, WidgetCallResponse, Status } from "@/types/models"

export const WidgetRepository = {
  listWidgets(serviceId?: number): Promise<Response<Array<Widget>>> {
    let url = "/api/widgets"
    if (serviceId !== undefined) {
      url += `?serviceId=${serviceId}`
    }
    return $axios.get(url)
  },

  listMyWidgets(serviceId?: number): Promise<Response<Array<Widget>>> {
    let url = "/api/widgets/me"
    if (serviceId !== undefined) {
      url += `?serviceId=${serviceId}`
    }
    return $axios.get(url)
  },

  callWidget(
    widgetId: number,
    params?: object
  ): Promise<Response<WidgetCallResponse>> {
    let url = `/api/widgets/${widgetId}`
    let qparams = ""
    if (params) {
      for (const key of Object.keys(params)) {
        qparams += qparams === "" ? "?" : "&"
        // @ts-ignore
        qparams += `${key}=${params[key]}`
      }
      url += qparams
    }
    return $axios.get(url)
  },

  subscribeToWidget(widgetId: number): Promise<Status> {
    return $axios.post(`/api/widgets/${widgetId}`)
  },

  unsubscribeFromWidget(widgetId: number): Promise<Status> {
    return $axios.delete(`/api/widgets/${widgetId}`)
  }
}
