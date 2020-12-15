<template>
  <clickable><hoverable>
    <div class="service-list-item">
      <nuxt-link class="cover" :to="`/${ name }/${ id }`"/>
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
  @Prop({ required: false, default: true }) readonly registered!: boolean
  @Prop({ required: false, default: false }) readonly empty!: boolean

  get icon() {
    if (!this.registered) {
      return "/svg/unuse-service.svg"
    }
    if (this.empty) {
      return "/svg/empty-service.svg"
    } 
    return "/svg/service.svg"
  }
}
</script>

<style lang="scss" scoped>
.cover {
  width : 100%;
  height : 100%;
  left: 0;
  top: 0;
  position : absolute;
}

.service-list-item {
  position: relative;
  display: inline-block;
  padding: .5em 1em;
  text-align: center;

  a {
    color: inherit;
    text-decoration: none;
  }

  .service-name {
    text-align: center;
  }
}
</style>
