<template>
  <div class="nav-button clickable">
    <nuxt-link class="link" :to="redirect" />
    <icon :src="src" width="30" height="30" />
    <p>{{title}}</p>
  </div>
</template>


<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import Icon from "../icon.vue"


@Component({
  name: 'NavButton',
  components: {
    Icon
  }
})
export default class NavButton  extends Vue {
  // data
    @Prop({required: true}) readonly src!: string
    @Prop({required: true}) readonly title!: string
    @Prop({required: true}) readonly redirect!: string

}
</script>

<style scoped lang="scss">

.link {
  position: absolute;
  width: 100%;
  height: 100%;
  left: 0;
  top: 0;
}

.nav-button {
  position: relative;
  width: 100%;
  border-radius: var(--input-radius);
  padding: 10px 5%;
  display: flex;
  align-items: center;

  &:hover {
    background: var(--focus-color);
  };
}

p {
  margin-left: 15px;
  font-size: 0.75em;
}

</style>
