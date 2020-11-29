<template>
  <clickable><hoverable>
    <div class="service">
      <icon :src="icon" width="70" height="70"/>
      <div class="service-name">{{ name }}</div>
    </div>
  </hoverable></clickable>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import Icon from '~/components/Icon.vue'
import Clickable from '~/components/Clickable.vue'
import Hoverable from '~/components/Hoverable.vue'

@Component({
  name: 'Service',
  components: {
    Icon,
    Clickable,
    Hoverable
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
