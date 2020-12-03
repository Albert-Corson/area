<template>
  <div class="services page">
    <service-list :services="services"
      v-on:create="create"/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import ServiceStore from '~/store/modules/ServiceStore'
import ServiceList from '~/components/services/ServiceList.vue'
import ExtendedServiceModel from '~/components/services/ExtendedServiceModel'
import ServiceModel from '~/api/models/ServiceModel'


@Component({
  components: {
    ServiceList
  }
})
export default class ServicesPage extends Vue {
  public reload() {
    // TODO
    this.$toasted.info('Reload services list...')
  }

  public create(id : number) {
    console.log("WANT CREATION OF SERVICE", id);
  }

  public createExtendedServices(registred : Array<ServiceModel>, all : Array<ServiceModel>) {
    return all.map((service) => {
      let extended : ExtendedServiceModel = service
      extended.empty = false;
      extended.registered = registred.some((knownService) => {
        return knownService.id === service.id;
      });
      return extended;
    })
  }

  public get services() : Array<ExtendedServiceModel> {
    return this.createExtendedServices(ServiceStore.registeredServices, ServiceStore.services);
  }

  // hooks
  mounted() {
    ServiceStore.fetchServices()
    ServiceStore.fetchRegisteredServices()
  }
}
</script>
