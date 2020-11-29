<template>
  <div class="page widget">
    {{ id }}
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  name: 'Widget',
  components: {
  },
  validate({ params }) {
    const id = parseInt(params.id)
    return !isNaN(id)
  }
})
export default class WidgetPage extends Vue {
  public id?: number

  async beforeCreate() {
    this.id = parseInt(this.$route.params.id)
  }
}
</script>

<style lang="scss" scoped>
</style>
