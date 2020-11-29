<template>
  <div class="nav-button clickable hoverable">
    <nuxt-link class="link" :to="to" />
    <icon :src="icon" width="32" height="32" />
    <p>{{ title }}</p>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import Icon from "~/components/Icon.vue"

@Component({
  name: 'NavButton',
  components: {
    Icon
  }
})
export default class NavButton extends Vue {
  // data
    @Prop({required: true}) readonly icon!: string
    @Prop({required: true}) readonly title!: string
    @Prop({required: true}) readonly to!: string

}
</script>

<style scoped lang="scss">

.nav-button {
  position: relative;
  width: 100%;
  padding: .75em 0 .75em 1.25em;
  display: flex;
  align-items: center;
  min-width: 200px;

  .link {
    position: absolute;
    width: 100%;
    height: 100%;
    left: 0;
    top: 0;
  }

  p {
    margin-left: 15px;
  }
}

</style>
