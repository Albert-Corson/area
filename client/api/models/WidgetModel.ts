import ServiceModel from './ServiceModel'

export default interface WidgetModel {
  id: number
  name: string
  parent_service: ServiceModel
}