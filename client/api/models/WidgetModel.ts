import ServiceModel from './ServiceModel'
import NamedModel from "./NamedModel"


export default interface WidgetModel extends NamedModel {
  service: ServiceModel
}