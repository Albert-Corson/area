import {Parameters} from './API'
import {Block} from './Block'

export interface Service {
  id: number;
  name: string;
}

export interface WidgetParam {
  name: string,
  type: string,
  required: boolean;
  value: string;
}

export interface Widget extends Block {
  id: number;
  name: string;
  description: string;
  requires_auth: boolean;
  service: Service;
  params: Parameters
}
