<template>
  <div class="widget-preview">
    <preview-title :text="widget.name"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import PreviewTitle from '~/components/preview/Title.vue'

@Component({
  name: 'WidgetPreview',
  components: {
    PreviewTitle
  }
})
export default class WidgetPreview extends Vue {
  @Prop({ required: true }) readonly widget!: WidgetModel
}
</script>

<style lang="scss">

</style>
