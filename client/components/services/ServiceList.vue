<template>
  <div class="services-list">
    <mosaic-list>
      <service-list-item v-for="service in services" :key="service.id"
               :id="service.id"
               :name="service.name"
               />
    </mosaic-list>
  </div>
</template>

<script>
import { Component, Prop, Vue } from 'vue-property-decorator'
import MosaicList from '@components/MosaicList.vue'
import ServiceListItem from '@components/services/ServiceListItem.vue'

import ServiceStore from '@stores/ServiceStore'

@Component({
  name: 'ServiceList',
  components: {
    MosaicList,
    ServiceListItem
  }
})
export default class ServiceList extends Vue {
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
