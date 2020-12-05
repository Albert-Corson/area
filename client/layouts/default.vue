<template>
  <div class="default">
    <dashboard-nav-bar />
    <dashboard-header v-on:reload="reload"/>
    <nuxt ref="page"/>
    <dashboard-footer />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import DashboardNavBar from "~/components/navbar/NavBar.vue"
import DashboardFooter from "~/components/footer/Footer.vue"
import DashboardHeader from "~/components/header/Header.vue"

@Component({
  name: 'DefaultLayout',
  components: {
    DashboardNavBar,
    DashboardFooter,
    DashboardHeader
  }
})
export default class DefaultLayout extends Vue {
  // methods
  public reload() {
    // @ts-ignore
    this.$refs.page?.$children?.[0]?.reload?.()
  }
}
</script>

<style lang="scss">
:root {
  --main-bg-color: #1B2224;
  --secondary-bg-color: #222B2E;
  --focus-color: #2eb398;
  --active-color: #E4E4E4;
  --inactive-color: #474747;
  --active-filter-color : invert(100%) sepia(0%) saturate(7495%) hue-rotate(273deg) brightness(87%) contrast(106%); 
}

* {
  box-sizing: border-box;
}

body {
  margin: 0;
  padding: 0;
  color: var(--active-color);
  font-family: sans-serif;
}

</style>

<style scoped>

.default {
  background-color: var(--main-bg-color);
  height: 100vh;
  display: grid;
  grid-template-columns: 0.75fr 5fr;
  grid-template-rows: 1fr 10fr 0.75fr;
  grid-gap: 0px 0px;
  grid-template-areas:
    "header header"
    "nav-bar page"
    "footer page";
}

.page {
  grid-area: page;
  background-color: var(--secondary-bg-color);
  border: 3px solid #2EB398;
  border-bottom-width: 0px;
  border-right-width: 0px;
  overflow-y: auto;
}

</style>
