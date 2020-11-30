<template>
    <div class="header-glitter">
        <icon src="/svg/device.svg" width="30" height="30"/>
        <hoverable
            v-for="(element, index) in breadCrumbs"
            :key="index"
        >
          <div class="glitter-element" @click="changeRoute(index)">
            {{element}}
          </div>
        </hoverable>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'nuxt-property-decorator'
import Hoverable from '~/components/Hoverable.vue'
import Icon from '~/components/Icon.vue'
import { Route } from './'

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
  public urlWatcher(to : any) {
    console.log(to);
    this.getBaliseFromUrl(to.path);
  }

  public getBaliseFromUrl(url : string) {
    let parsedUrl : Array<string> = url.split('/').filter(Element => {
      return Element.length > 0
    });
    console.log(parsedUrl)
    this.breadCrumbs = ["/" , ... parsedUrl]
  }

  public changeRoute(indexDiv : number) {
    let newRoute : string = "";
    let breadCrumbsAsked : Array<string> = this.breadCrumbs.slice(0, indexDiv + 1);

    breadCrumbsAsked.forEach((element) => {
      newRoute = newRoute + element
      if (element[element.length - 1] !== "/")
        newRoute = newRoute + "/"
    })
    this.$router.replace(newRoute).catch((error) => {
      console.log(error);
    })
  }
}
</script>


<style scoped>

.header-glitter .hoverable {
  margin-left: 15px;
}

.glitter-element {
  display: flex;
  align-items: center;
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