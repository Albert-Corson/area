<template>
  <div class="title">
    <h1>{{ text }}</h1>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'

@Component({
  name: 'Title'
})
export default class Title extends Vue {
  @Prop({ required: true }) readonly text!: string
}
</script>

<style lang="scss">

</style>
