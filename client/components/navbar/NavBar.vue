<template>
  <div class="nav-bar">
     <div class="title">Places</div>
     <div class="buttons">
       <div v-if="authenticated">
         <nav-button title="Home" icon="/svg/home.svg" to="/"></nav-button>
         <nav-button title="Services" icon="/svg/services.svg" to="/services"></nav-button>
         <nav-button title="Widgets" icon="/svg/widgets.svg" to="/widgets"></nav-button>
         <nav-button title="Profil" icon="/svg/profil.svg" to="/"></nav-button>
         <nav-button title="Sign out" icon="/svg/logout.svg" to="/logout"></nav-button>
       </div>
       <div v-else>
         <nav-button title="Sign in" icon="/svg/profil.svg" to="/login"></nav-button>
       </div>
     </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import NavButton from "~/components/navbar/NavButton.vue"
import { AuthStore } from '~/store'

@Component({
  name: 'NavBar',
  components: {
    NavButton
  }
})
export default class NavBar extends Vue {
  // computed
  get authenticated() {
    return AuthStore.authenticated
  }
}
</script>

<style scoped>

.nav-bar {
  grid-area: nav-bar;
}

.title {
  padding: 5px 0px 5px 10px;
  margin: 0 2px;
  font-weight: bold;
  font-size: 1.25em;
}

.buttons {
  padding: .75em 0;
}

</style>
