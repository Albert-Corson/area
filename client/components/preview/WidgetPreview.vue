<template>
  <div class="widget-preview">
    <preview-title :text="widget.name"></preview-title>
    <widget-applet-factory :widgetId="widget.id"></widget-applet-factory>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import PreviewTitle from '~/components/preview/Title.vue'
import WidgetAppletFactory from '~/components/widgets-applets/WidgetAppletFactory.vue'
import { WidgetStore, ServiceStore } from '~/store'

@Component({
  name: 'WidgetPreview',
  components: {
    PreviewTitle,
    WidgetAppletFactory
  }
})
export default class WidgetPreview extends Vue {
  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async fetchData() {
    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id
    })
    if (res?.code === 401) {
      const data: string | null = await ServiceStore.registerService(this.widget.service.id)
      if (data) {
        const authPopup = window.open(data)
      }
    }
  }

  // watchers
  @Watch('widget')
  widgetWatcher() {
    this.fetchData()
  }

  mounted() {
    this.fetchData()
  }
}
</script>

<style lang="scss">

</style>
