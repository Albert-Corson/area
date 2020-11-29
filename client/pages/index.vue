<template>
  <div class="page index">
    <service-list/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import ServiceList from '~/components/services/ServiceList.vue'

@Component({
  components: {
    ServiceList
  }
})
export default class IndexPage extends Vue {
  mounted() {

  }
}
</script>
