<template>
  <div class="services-list">
    <mosaic-list>
      <service v-for="service in services" :key="service.id"
               :id="service.id"
               :name="service.name"
               />
    </mosaic-list>
  </div>
</template>

<script>
import { Component, Prop, Vue } from 'vue-property-decorator'
import MosaicList from '@components/MosaicList.vue'
import Service from '@components/services/Service.vue'

import ServiceStore from '@stores/ServiceStore'

@Component({
  name: 'ServicesList',
  components: {
    MosaicList,
    Service
  }
})
export default class ServicesList extends Vue {
  // computed
  get services() {
    return ServiceStore.services
  }

  // hooks
  mounted() {
    ServiceStore.fetchServices()
  }
}

</script>
