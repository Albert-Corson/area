import VuexPersistence from 'vuex-persist'

// @ts-ignore
export default ({ store }) => {
  new VuexPersistence({
    storage: window.localStorage
  }).plugin(store)
}
