import {WidgetParam} from './Widgets'

export interface Response<T = any> {
  error?: string;
  successful: boolean;
  data: T;
}

export interface Parameters {
  params?: WidgetParam[];
  item?: any;
  items?: any[];
}