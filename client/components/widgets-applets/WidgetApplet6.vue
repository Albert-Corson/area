<template>
  <v-app class="widget-applet-6">
    <h1>{{ widget.name }}</h1>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'

@Component({
  name: 'WidgetApplet6',
  components: {
  }
})
export default class WidgetApplet6 extends Vue {
  // data

  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id
    })
    console.log(res)
    // update data
  }

  // lifecycle
  beforeMount() {
    this.reload()
  }
}

</script>

<style lang="scss" scoped>
h1 {
  margin-bottom: 1em;
}

.mosaic-list {
  margin-top: 1em;
}
</style>
