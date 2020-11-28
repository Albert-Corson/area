import ServiceModel from './ServiceModel'

export default interface WidgetModel {
  id: number
  name: string
  service: ServiceModel
}