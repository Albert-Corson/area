<template>
  <v-app class="widget-applet-2">
    <h1>{{ widget.name }}</h1>
    <!-- <v-select -->
    <!--   filled -->
    <!--   v-model="sort" -->
    <!--   :items="sortOptions" -->
    <!--   item-text="label" -->
    <!--   item-value="code" -->
    <!--   @change="updateSort" -->
    <!--   > -->
    <!-- </v-select> -->
    <mosaic-list>
      <div v-for="item in items" :key="item.id">
        <a :href="item.link" target="_blank">
          <div v-if="item.isVideo" class="preview-video">
            <video width="150" height="150">
              <source :src="item.image" type="video/mp4">
              Your browser does not support the video tag.
            </video>
          </div>
          <div
            v-else
            class="preview-image"
            :style="`background-image: url(${ item.image })`">
          </div>
        </a>
      </div>
    </mosaic-list>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'

import MosaicList from '~/components/MosaicList.vue'

interface PostModel {
  header: string
  image: string
  link: string
  isVideo: boolean
}

@Component({
  name: 'WidgetApplet2',
  components: {
    MosaicList
  }
})
export default class WidgetApplet2 extends Vue {
  // data
  public items: Array<PostModel> = []
  public sort: string = ''

  public sortOptions = [
    { label: 'Newest', code: 'newest' }
  ]

  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const params: { section?: string } = {}
    if (this.sort)
      params.section = this.sort

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
    this.section = res.params.find(p => p.name === 'section')?.value
  }

  public updateSort(sort: string) {
    this.sort = sort
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
  width: 150px;
  height: 150px;
  background-size: cover;
}
</style>
