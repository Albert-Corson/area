<template>
  <div class="service-list">
    <mosaic-list>
      <service-list-item v-for="service in services" :key="service.id"
               :id="service.id"
               :name="service.name"
               :registered="service.registered"
               :empty="service.empty"
               v-on:create="create"
               />
    </mosaic-list>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import MosaicList from '~/components/MosaicList.vue'
import ServiceListItem from '~/components/services/ServiceListItem.vue'
import ServiceModel from '~/api/models/ServiceModel'
import ExtendedServiceModel from './ExtendedServiceModel'

@Component({
  name: 'ServiceList',
  components: {
    MosaicList,
    ServiceListItem
  }
})
export default class ServiceList extends Vue {
  // props
  @Prop({ required: true }) readonly services!: Array<ServiceModel | ExtendedServiceModel>

  public create(id : number) {
    this.$emit("create", id);
  }
}

</script>
