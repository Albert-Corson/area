<template>
    <div class="header-glitter">
        <icon src="/svg/device.svg" width="30" height="30"/>
        <hoverable
            v-for="(section, index) in breadCrumbs"
            :key="index"
        >
          <div class="glitter-element" @click="changeRoute(section, index)">
            {{ section.name }}
          </div>
        </hoverable>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'nuxt-property-decorator'
import Hoverable from '~/components/Hoverable.vue'
import Icon from '~/components/Icon.vue'

interface RouteSection {
  name: string
  id?: number
};

@Component({
  name: 'HeaderGlitter',
  components: {
    Icon,
    Hoverable
  }
})
export default class HeaderGlitter extends Vue {

  // data
  public breadCrumbs : Array<RouteSection> = [];

  // watchers
  @Watch("$route")
  public urlWatcher(to : any) {
    this.updateBreadCrumb(to.path);
  }

  // methods
  public updateBreadCrumb(url : string) {
    const urlSections: Array<string> = url
      .split('/')
      .filter(section => section.length !== 0)
    const routeSections: Array<RouteSection> = urlSections
      .map(name => ({ name }) as RouteSection)
      .reduce((result, value, index, array) => {
        const isNumber = !isNaN(parseInt(value.name))
        if (isNumber && index > 0) {
          array[index - 1].id = parseInt(value.name)
        } else {
          result.push(value)
        }
        return result
      }, [] as Array<RouteSection>)
      this.breadCrumbs = [ { name: '/' }, ...routeSections ]
  }

  public changeRoute(section: RouteSection, index: number) {
    const targetRouteSections = this.breadCrumbs.slice(0, index + 1)

    let target = '/'
    targetRouteSections.forEach(section => {
      if (section.name === '/') {
        return
      }
      target += `${ section.name }/`
      if (section.id !== undefined) {
        target += `${ section.id }/`
      }
    })
    this.$router
      .replace(target)
      .catch(e => {
        // do nothing
      })
  }

  // hooks
  beforeMount() {
    this.updateBreadCrumb(this.$route.path)
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
