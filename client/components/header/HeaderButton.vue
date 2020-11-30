<template>
    <div class="header-button">
        <div 
            v-for="button in navButtons"
            :key="button.icon"
            @click="button.action"
            class="enable clickable hoverable"
        >
            <icon :src="button.icon" width="30" height="30"/>
        </div>
    </div>
</template>


<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import Icon from "~/components/Icon.vue"

interface IHeaderButton {
  icon: any,
  action : Function,
}

@Component({
  name: 'HeaderButton',
  components: {
    Icon 
  }
})
export default class HeaderButton extends Vue {
  // data
  public navButtons : Array<IHeaderButton> = [
      {
        icon: "/svg/go-previous.svg",
        action: this.goPrevious,
      },
      {
        icon: "/svg/go-next.svg",
        action: this.goNext,
      },
      {
        icon: "/svg/reload.svg",
        action: this.reload,
      }
  ]

  public goNext() {
    this.$router.forward()
  }

  public goPrevious() {
    this.$router.back()
  }

  public reload() {
    // FETCH DATA STORE
    console.log("reload")
  }

}

</script>

<style scoped lang="scss">

.enable > * {
  filter: var(--active-filter-color);
}


.header-button div {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 60%;
  min-width: 45px;
  border-radius: 5px;
}

.header-button
{
  display: flex;
  align-items: center;
  justify-content: space-around;
  grid-area: header-button;
}
</style> >