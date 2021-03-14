import Param from "./Param"
import WidgetCallResponseItem from "./WidgetCallResponseItem"

export interface WidgetCallResponse {
  params: Array<Param>
  items: Array<WidgetCallResponseItem>
}
