<template>
  <div class="nav-bar">
     <div class="title">Places</div>
     <div class="button-cotainers">
      <nav-button v-for="button in navButtons"
                  :key="button.title"
                  :src="button.src" 
                  :title="button.title" 
                  :redirect="button.redirect">
      </nav-button>
     </div>
  </div>
</template>


<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import NavButton from "./navbutton.vue"


interface INavButton
{
  src: any,
  title : string,
  redirect: string
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
      src : "svg/home.svg",
      title : "Home",
      redirect : "/",
    },
    {
      src : "svg/services.svg",
      title : "Services",
      redirect : "/",
    },
    {
      src : "svg/widgets.svg",
      title : "Widgets",
      redirect : "/",
    },
    {
      src : "svg/profil.svg",
      title : "Profil",
      redirect : "/about",
    }
  ]

}
</script>


<style scoped>

.title {
  border-radius: var(--input-radius);
  padding: 5px 0px 5px 10px;
  margin: 0 2px;
  font-weight: bold;
  font-size: 1.25em;
}

.button-cotainers
{
  display: flex;
  flex-direction: column;
  height: 35%;
  justify-content: space-evenly;
  align-items: center;
  grid-area: nav-bar;
}

</style>