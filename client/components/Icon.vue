<template>
  <div class="icon">
    <img :src="src" alt="icon button" :width="width" :height="height"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'

@Component({
  name: 'Icon',
  components: {
  }
})
export default class Icon  extends Vue {
  // data
    @Prop({required: true}) readonly src!: string
    @Prop({required: true}) readonly width!: number
    @Prop({required: true}) readonly height!: number
}
</script>