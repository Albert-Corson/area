<template>
  <div class="default">
    <router-link to="/login">To Login</router-link>
    <nuxt/>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'

@Component({
  name: 'DefaultLayout',
  components: {
  }
})
export default class DefaultLayout extends Vue {
  // data
  items = [
    {
      icon: 'mdi-apps',
      title: 'Welcome'
    }
  ] 
  inputUser: string = ""

  @Prop(String) readonly title: string

  public get sortedItems() {
    return this.items
  }

  toto(value) {

  }
}
</script>

<style scoped>
:root {

}
</style>