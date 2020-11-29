<template>
  <div class="page index">
    <services-list/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import ServicesList from '~/components/services/ServicesList.vue'

@Component({
  components: {
    ServicesList
  }
})
export default class IndexPage extends Vue {
  mounted() {

  }
}
</script>
