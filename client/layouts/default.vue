<template>
  <div class="default">
    <dashboard-nav-bar />
    <dashboard-header />
    <nuxt />
    <dashboard-footer />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import DashboardNavBar from "../components/navbar/NavBar.vue"
import DashboardFooter from "../components/footer/Footer.vue"
import DashboardHeader from "../components/header/Header.vue"

@Component({
  name: 'DefaultLayout',
  components: {
    DashboardNavBar,
    DashboardFooter,
    DashboardHeader
  }
})
export default class DefaultLayout extends Vue {
  // data
}
</script>

<style lang="scss">
:root {
  --main-bg-color: #1B2224;
  --secondary-bg-color: #222B2E;
  --focus-color: #2eb398;
  --active-color: #E4E4E4;
  --inactive-color: #474747;
}

/* todo create image that follow cursor */

* {
  box-sizing: border-box;
}

body {
  margin: 0;
  padding: 0;
  color: var(--active-color);
  font-family: sans-serif;
}

.clickable, .hoverable {
  cursor: pointer;
}

.hoverable {
  transition: background .1s ease-in-out;

  &:hover {
    background: var(--focus-color);
  }
}

</style>

<style scoped>

.default {
  background-color: var(--main-bg-color);
  height: 100vh;
  width: 100vw;
  display: grid;
  grid-template-columns: 0.75fr 5fr;
  grid-template-rows: 0.75fr 10fr 0.75fr;
  grid-gap: 0px 0px;
  grid-template-areas:
    "header header"
    "nav-bar page"
    "footer footer";
}

.page {
  grid-area: page;
  background-color: var(--secondary-bg-color);
  border: 3px solid #2EB398;
}

</style>
