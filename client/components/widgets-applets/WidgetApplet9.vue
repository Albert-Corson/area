<template>
  <v-app class="widget-applet-9">
    <h1>{{ widget.name }}</h1>
    <div class="params">
      <v-text-field
        v-model="language"
        label="Language"
        filled
        >
      </v-text-field>
      <v-text-field
        v-model="country"
        label="Country"
        filled
        >
      </v-text-field>

      <div>
        <v-btn
          elevation="2"
          @click="update"
          color="accent"
          >
          Query
        </v-btn>
      </div>
    </div>

    <div v-for="item in items" :key="item.id">
      <div class="news-entry">
        <a :href="item.link" target="_blank">
          <div
            class="preview-image"
            :style="`background-image: url(${ item.image })`">
          </div>
        </a>
        <span class="description">
          {{ item.description }}
        </span>
      </div>
    </div>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'

import MosaicList from '~/components/MosaicList.vue'
import Clickable from '../Clickable.vue'

interface NewsModel {
  source: string
  author: string
  description: string
  published_at: number
  header: string
  content: string
  link: string
  image: string
}

@Component({
  name: 'WidgetApplet9',
  components: {
    MosaicList,
    Clickable
  }
})
export default class WidgetApplet9 extends Vue {
  // data
  public items: Array<NewsModel> = []
  public language: string = ''

  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const params: { language?: string } = {}
    if (this.language)
      params.language = this.language

    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id,
      params
    })
    if (!res.items) return
    // @ts-ignore
    this.items = res.items.map(i => {
      i.isVideo = i.image.endsWith('.mp4')
      return i
    })
    // @ts-ignore
    this.language = res.params.find(p => p.name === 'language')?.value
  }

  public updateSection(language: string) {
    this.language = language
    this.reload()
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
  width: 100px;
  height: 100px;
  background-size: cover;
}

.params {
  margin-bottom: 1em;
}

.news-entry {
  margin-bottom: .5em;
  display: flex;
  text-align: left;

  .preview-image {
    flex: 0 1 auto;
    margin-right: .5em;
  }

  .description {
    flex: 1;
    display: inline-block;
    overflow: hidden;
  }
}
</style>
