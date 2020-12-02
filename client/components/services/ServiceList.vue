<template>
  <div class="service-list">
    <mosaic-list>
      <service-list-item v-for="service in services" :key="service.id"
               :id="service.id"
               :name="service.name"
               />
    </mosaic-list>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import MosaicList from '~/components/MosaicList.vue'
import ServiceListItem from '~/components/services/ServiceListItem.vue'
import ServiceModel from '~/api/models/ServiceModel'

@Component({
  name: 'ServiceList',
  components: {
    MosaicList,
    ServiceListItem
  }
})
export default class ServiceList extends Vue {
  // props
  @Prop({ required: true }) readonly services!: Array<ServiceModel>
}

</script>
