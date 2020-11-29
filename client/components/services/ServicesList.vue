<template>
  <div class="services-list">
    <mosaic-list>
      <service name="Youtube" empty="true" id="1"></service>
      <service name="GitHub" id="2"></service>
      <service name="Weather" id="3"></service>
      <service name="Netflix" id="4"></service>
      <service name="Wikipédia" id="5"></service>
    </mosaic-list>
  </div>
</template>

<script>
import { Component, Prop, Vue } from 'vue-property-decorator'
import MosaicList from '~/components/MosaicList.vue'
import Service from '~/components/services/Service.vue'

@Component({
  name: 'ServicesList',
  components: {
    MosaicList,
    Service
  }
})
export default class ServicesList extends Vue {
  mounted() {

  }
}

</script>
