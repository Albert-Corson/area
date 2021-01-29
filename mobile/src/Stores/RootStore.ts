import React, {createContext} from 'react';
import {Block} from '../Types/Block';
import {GridStore} from './GridStore';

export class RootStore {
    gridStore: GridStore = new GridStore(this);
}

export default createContext(new RootStore());