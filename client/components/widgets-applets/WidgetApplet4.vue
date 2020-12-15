<template>
  <v-app class="widget-applet-4">
    <h1>{{ widget.name }}</h1>
    <div>
      <v-text-field
        v-model="width"
        filled
        clearable
        label="width"
        type="number"
      />
      <v-text-field
        v-model="height"
        filled
        clearable
        label="height"
        type="number"
      />
      <v-btn
        color="accent"
        @click="updatePicsum"
      >
      Validate
      </v-btn>
    </div>
    <div class="image-container" v-if="picsum">
      <a :href="picsum.link" target="_blank">
          <img
          :src="picsum.image"
          alt="img"
          />
      </a>
    </div>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'
import MosaicList from '~/components/MosaicList.vue'

interface Picsum {
  image?: string
  link?: string

}

@Component({
  name: 'WidgetApplet1',
  components: {
    MosaicList
  }
})
export default class WidgetApplet4 extends Vue {
  // data
  public picsum: Picsum | null = null;
  public width: number = 500;
  public height: number  = 500;

  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const params: { width: number, height: number } = {width: this.width, height: this.height};
    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id,
      params
    })
    this.picsum = {}; 
    this.picsum.link = res.item.link;
    this.picsum.image = res.item.image;
  }

  public updatePicsum() {
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

.image-container {
  overflow: auto;
}

</style>
