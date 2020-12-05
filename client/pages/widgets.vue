<template>
  <div class="widgets page">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  components: {
  }
})
export default class WidgetsPage extends Vue {
  //methods
  public reload() {
    // TODO
    this.$toasted.info('Reload services list...')
  }

  // hooks
  mounted() {

  }
}
</script>
