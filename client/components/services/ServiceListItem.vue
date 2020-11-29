<template>
  <clickable><hoverable>
    <div class="service-list-item">
      <nuxt-link :to="`/services/${ id }`">
        <icon :src="icon" width="70" height="70"/>
        <div class="service-name">{{ name }}</div>
      </nuxt-link>
    </div>
  </hoverable></clickable>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import Icon from '~/components/Icon.vue'
import Clickable from '~/components/Clickable.vue'
import Hoverable from '~/components/Hoverable.vue'

@Component({
  name: 'ServiceListItem',
  components: {
    Icon,
    Clickable,
    Hoverable
  }
})
export default class ServiceListItem extends Vue {
  @Prop({ required: true }) readonly name!: string
  @Prop({ required: true }) readonly id!: number
  @Prop({ default: false }) readonly empty!: boolean

  get icon() {
    return (this.empty ? '/svg/empty-service.svg' : '/svg/service.svg')
  }
}
</script>

<style lang="scss" scoped>
.service-list-item {
  position: relative;
  display: inline-block;
  padding: .5em 1em;

  a {
    color: inherit;
    text-decoration: none;
  }

  .service-name {
    text-align: center;
  }
}
</style>
