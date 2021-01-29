import React, { createContext } from 'react';
import { GridStore } from './GridStore';
import { AuthStore } from './AuthStore';
import { UserStore } from './UserStore';

export class RootStore {
    grid: GridStore = new GridStore(this);
    auth: AuthStore = new AuthStore(this);
    user: UserStore = new UserStore(this);
}

export default createContext(new RootStore());