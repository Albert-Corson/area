<template>
  <v-app class="widget-applet-12">
    <h1>{{ widget.name }}</h1>
    <div v-if="item" class="joke">
      {{ item.content }}
    </div>
    <v-btn
      elevation="2"
      @click="reload"
      >
      Another one
    </v-btn>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'

interface JokeModel {
  content: string
}

@Component({
  name: 'WidgetApplet12',
})
export default class WidgetApplet12 extends Vue {
  // data
  public item: JokeModel | null = null

  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id
    })
    console.log(res)
    if (!res.item) return
    this.item = res.item
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

.preview-image {
  width: 150px;
  height: 150px;
  background-size: cover;
}

.joke {
  margin-bottom: 2em;
}
</style>
