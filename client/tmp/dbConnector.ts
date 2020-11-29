import db from '~/tmp/db.json'

export default {
  listUsers() {
    return db.users ?? []
  },

  getUser(userId?: number) {
    if (userId !== undefined) {
      throw new Error(`Permission denied`)
    }
    const user = this.listUsers()[0]
    if (user === undefined) {
      throw new Error(`You are not logged in`)
    }
    return user
  },

  listServices() {
    return db.services ?? []
  },

  listRegisteredServices() {
    const user = this.getUser()
    return this.listServices().filter(s => {
      return db.user_has_service.find(r => {
        return r.user === user.id && r.service === s.id
      })
    })
  },

  getService(serviceId: number) {
    const service = this.listServices().find(s => s.id === serviceId)
    if (service === undefined) {
      throw new Error(`Service ${ serviceId } doesn't exist`)
    }
    return service
  },

  listWidgets(serviceId?: number) {
    let widgets = db.widgets
    if (serviceId !== undefined) {
      // check if service exists
      this.getService(serviceId)
      // @ts-ignore
      widgets = widgets.filter(w => w.service.id === serviceId)
    }
    return widgets
  },

  getWidget(widgetId: number) {
    const widget = this.listWidgets().find(w => w.id === widgetId)
    if (widget === undefined) {
      throw new Error(`Widget ${ widgetId } doesn't exist`)
    }
    return widget
  },

  listRegisteredWidgets(serviceId?: number) {
    const user = this.getUser()
    let widgets = this.listWidgets().filter(w => {
      return db.user_has_widget.find(r => {
        return r.user === user.id && r.widget === w.id
      })
    })
    if (serviceId !== undefined) {
      // check if service exists
      this.getService(serviceId)
      // @ts-ignore
      widgets = widgets.filter(w => w.service.id === serviceId)
    }
    return widgets
  }
}
