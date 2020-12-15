<template>
  <v-app class="widget-applet-8">
    <div>
      <h1>{{ widget.name }}</h1>
    </div>
    <div v-if="songs.length">
      <div class="song-container" 
        v-for="song in songs" 
        :key="song.id">
          <div class="song-info">
            <div class="song-title">{{song.artists[0]}}</div>
            <div class="song-artists">{{song.title}}</div>
            <v-btn
              :href="song.preview"
              target="_blank"
              elevation="2"
              icon
            >
              <v-icon dark>
                mdi-eye-outline
              </v-icon>
            </v-btn>
            <v-btn
              :href="song.link"
              target="_blank"
              elevation="2"
              icon
            >
              <v-icon dark>
                mdi-magnify
              </v-icon>
            </v-btn>
          </div>
      </div>
    </div>
    <div v-else>
      <p>No History</p>
    </div>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import { WidgetStore } from '~/store'

interface Song 
{
  artists: Array<string>
  title: string
  link: string 
  preview: string
}

@Component({
  name: 'WidgetApplet8',
  components: {
  }
})
export default class WidgetApplet8 extends Vue {
  // data
    public songs : Array<Song> = [];
  
  // props
  @Prop({ required: true }) readonly widget!: WidgetModel

  // methods
  public async reload() {
    const res = await WidgetStore.fetchWidgetData({
      widgetId: this.widget.id
    })
    res?.items?.forEach(element => {
      console.log(element)
      const song : Song = {
        artists : element.artists,
        title : element.header,
        link : element.link,
        preview : element.preview,
      };
      this.songs.push(song);
    });

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


.song-info {
  display: flex;
  flex: 1;
  align-items: center;
  justify-content: space-evenly;
}

.song-container {
  color: #c3c3c3;
  background: #a8a8a893;
  border-radius: 15px;
  position: relative;
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
