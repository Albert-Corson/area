<template>
  <v-app class="widget-applet-6">
    <div>
      <h1>{{ widget.name }}</h1>
    </div>
    <div v-if="artists.length">
        <div class="artist-container" 
          :style="`background-image: url(${ artist.image })`"
          v-for="artist in artists" 
          :key="artist.follower">
            <div class="artist-info">
              <div class="artist-name">{{artist.name}}</div>
              <div class="artist-followers">{{artist.followers}}</div>
              <div class="artist-genre">{{artist.genre[0]}}</div>
              <div class="artist-name">{{artist.popularity}}</div>
              <div class="artist-name">{{artist.name}}</div>
            </div>
        </div>
    </div>
    <div v-else>
      <p>No favorite artists</p>
    </div>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'
import MosaicList from '~/components/MosaicList.vue'

interface Artist
{
  followers: number
  genre : Array<string>
  name: string
  image: string
  link: string
  popularity: number
}

@Component({
  name: 'WidgetApplet6',
  components: {
    MosaicList
  }
})
export default class WidgetApplet6 extends Vue {
  // data
  public artists : Array<Artist> = [];

  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id
    })
    res?.items?.forEach(element => {
      const artist : Artist = {
        followers : element.followers,
        genre : element.genres,
        name : element.header,
        image : element.image,
        link : element.link,
        popularity : element.popularity
      };
      this.artists.push(artist);
    });
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

.artist-info {
  display: flex;
  flex: 1;
  justify-content: space-evenly;
}

.artist-container {
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
