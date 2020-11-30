<template>
    <div class="header-glitter">
        <icon src="/svg/device.svg" width="30" height="30"/>
        <hoverable
            class="glitter-element"
            v-for="element in breadCrumbs"
            :key="element.index"
        >
          <div>
            {{element}}
          </div>
        </hoverable>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'nuxt-property-decorator'
import Hoverable from '~/components/Hoverable.vue'
import Icon from '~/components/Icon.vue'
import { Route } from './index'

@Component({
  name: 'HeaderGlitter',
  components: {
      Icon,
      Hoverable
  }
})
export default class HeaderGlitter extends Vue {

  public breadCrumbs : Array<string> = [];

  beforeMount() {
    this.getBaliseFromUrl(this.$route.path)
  }

  @Watch("$route")
  public urlWatcher(to : Route) {
    this.getBaliseFromUrl(to.path);
  }

  public getBaliseFromUrl(url : string) {
    let parsedUrl : Array<string> = url.split('/').filter(Element => {
      return Element.length > 0
    });
    console.log(parsedUrl)
    this.breadCrumbs = ["/" , ... parsedUrl]
  }
}
</script>


<style scoped>

.glitter-element {
  display: flex;
  border-radius: 5px;
  align-items: center;
  margin-left: 15px;
  padding: 0px 15px;
  font-weight: bold;
  height: 50px;
  
}

.header-glitter {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  grid-area: header-glitter;
}

</style>