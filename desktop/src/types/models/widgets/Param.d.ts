import EnumValue from "./EnumValue"

export interface Param {
  name: string
  type: string
  value?: string
  allowed_values?: Array<EnumValue>
}
