<template>
  <div class="page index">
    <service-list :services="services"/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import ServiceList from '~/components/services/ServiceList.vue'
import { ServiceStore } from '~/store'

@Component({
  components: {
    ServiceList
  }
})
export default class IndexPage extends Vue {
  // computed
  public get services() {
    return ServiceStore.registeredServices
  }

  // methods
  public reload() {
    ServiceStore.fetchRegisteredServices()
  }

  // hooks
  mounted() {
    this.reload()
  }
}
</script>
