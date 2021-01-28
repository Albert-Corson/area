class Store {
  constructor() {
    this.id_ = null;
    this.token_ = null;
  }

  get id() {
    return this.id_;
  }

  set id(id) {
    this.id_ = id;
  }

  get token() {
    return this.token_;
  }

  set token(token) {
    this.token_ = token;
  }
}

const instance = new Store();

export default instance;
