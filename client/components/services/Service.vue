<template>
  <div class="service clickable hoverable">
    <icon :src="icon" width="70" height="70"/>
    <div class="service-name">{{ name }}</div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import Icon from '~/components/Icon.vue'

@Component({
  name: 'Service',
  components: {
    Icon
  }
})
export default class Service extends Vue {
  @Prop({ required: true }) readonly name!: string
  @Prop({ required: true }) readonly id!: number
  @Prop({ default: false }) readonly empty!: boolean

  get icon() {
    return (this.empty ? '/svg/empty-service.svg' : '/svg/service.svg')
  }
}
</script>

<style lang="scss" scoped>
.service {
  position: relative;
  display: inline-block;
  padding: .5em 1em;


  .service-name {
    text-align: center;
  }
}
</style>
