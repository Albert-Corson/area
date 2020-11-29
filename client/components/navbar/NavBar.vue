<template>
  <div class="nav-bar">
     <div class="title">Places</div>
     <div class="buttons">
      <nav-button
        v-for="button in navButtons"
        :key="button.title"
        :icon="button.icon" 
        :title="button.title" 
        :to="button.to"
      ></nav-button>
     </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import NavButton from "~/components/navbar/NavButton.vue"

interface INavButton {
  icon: any,
  title : string,
  to: string
}

@Component({
  name: 'NavBar',
  components: {
    NavButton
  }
})
export default class NavBar extends Vue {
  // data
  public navButtons : Array<INavButton> = [
    {
      icon: "svg/home.svg",
      title: "Home",
      to: "/"
    },
    {
      icon: "svg/services.svg",
      title: "Services",
      to: "/"
    },
    {
      icon: "svg/widgets.svg",
      title: "Widgets",
      to: "/"
    },
    {
      icon: "svg/profil.svg",
      title: "Profil",
      to: "/about"
    }
  ]

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
