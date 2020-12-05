<template>
  <div class="header-button">
      <hoverable 
          v-for="button in navButtons"
          :key="button.icon"
      >
        <div class="enabled" @click="button.action">
          <icon :src="button.icon" width="30" height="30"/>
        </div>
      </hoverable>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import Icon from "~/components/Icon.vue"
import Hoverable from '~/components/Hoverable.vue'

interface IHeaderButton {
  icon: any,
  action : Function,
}

@Component({
  name: 'HeaderButton',
  components: {
    Icon,
    Hoverable
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

  // methods
  public goNext() {
    this.$router.forward()
  }

  public goPrevious() {
    this.$router.back()
  }

  public reload() {
    this.$emit('reload')
  }

}
</script>

<style scoped lang="scss">
.header-button {
  display: flex;
  align-items: center;
  justify-content: space-around;
  grid-area: header-button;
}

.header-button > div {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 60%;
  min-width: 45px;
  border-radius: 5px;
}

.enabled > * {
  filter: var(--active-filter-color);
}
</style>