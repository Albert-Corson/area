<template>
  <div class="default">
    <dashboard-nav-bar />
    <dashboard-header />
    <!-- <router-link to="/login">To Login</router-link> -->
    <nuxt />
    <dashboard-footer />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import DashboardNavBar from "../components/navbar/navbar.vue"
import DashboardFooter from "../components/footer/footer.vue"
import DashboardHeader from "../components/header/header.vue"

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

<style>
:root {
  --main-bg-color: #1B2224;
  --secondary-bg-color: #222B2E;
  --focus-color: #2eb3982a;
  --active-color: #E4E4E4;
  --inactive-color: #474747;
  --input-radius: 5px;
}

/* todo create image that follow cursor */

* {
  box-sizing: border-box;
}

body {
  margin: 0;
  padding: 0;
  color: var(--active-color);
}

.clickable {
  cursor: pointer;
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
  gap: 0px 0px;
  grid-template-areas:
    "header header"
    "nav-bar page"
    "footer footer";
}

.page
{
  grid-area: page;
  background-color: var(--secondary-bg-color);
}

</style>