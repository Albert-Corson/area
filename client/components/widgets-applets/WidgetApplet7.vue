<template>
<<<<<<< HEAD
  <div class="widget-applet-7">
    <h1>Widget Applet 7</h1>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'nuxt-property-decorator'
=======
  <v-app class="widget-applet-7">
    <div>
      <h1>{{ widget.name }}</h1>
    </div>
    <div v-if="tracks.length">
      <div class="track-container" 
           :style="`background-image: url(${ track.image })`"
           v-for="track in tracks" 
           :key="track.follower">
        <a :href="track.link" target="_blank">
          <div class="track-info">
            <div class="track-name">{{track.header}}</div>
          </div>
        </a>
      </div>
    </div>
    <div v-else>
      <p>No favorite tracks</p>
    </div>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'
import MosaicList from '~/components/MosaicList.vue'

interface Track
{
  artists: Array<string>
  popularity: number
  preview: string
  header: string
  link: string
  image: string
}
>>>>>>> c374adcb92df025e9ba11ff04ac52a2bf23d2e8f

@Component({
  name: 'WidgetApplet7',
  components: {
<<<<<<< HEAD
  }
})
export default class WidgetApplet7 extends Vue {
=======
    MosaicList
  }
})
export default class WidgetApplet7 extends Vue {
  // data
  public tracks: Array<Track> = [];

  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id
    })
    if (!res.items) return
    this.tracks = res.items
  }

  // lifecycle
  beforeMount() {
    this.reload()
  }
>>>>>>> c374adcb92df025e9ba11ff04ac52a2bf23d2e8f
}

</script>

<<<<<<< HEAD
=======
<style lang="scss" scoped>
h1 {
  margin-bottom: 1em;
}

.track-info {
  display: flex;
  flex: 1;
  justify-content: space-evenly;
  color: #c18181;
  font-weight: bold;
}

.track-container {
  display: flex;
  justify-content: center;
  align-items: center;
  background-size: cover;
  min-height: 100px;
  margin-top: 10px;
}

.mosaic-list {
  margin-top: 1em;
}
</style>
>>>>>>> c374adcb92df025e9ba11ff04ac52a2bf23d2e8f
