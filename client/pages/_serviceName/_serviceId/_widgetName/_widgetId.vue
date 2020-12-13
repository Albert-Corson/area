<template>
  <div class="page widget">
    {{ id }}
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'nuxt-property-decorator'

@Component({
  name: 'Widget',
  validate({ params }) {
    const id = parseInt(params.widgetId)
    return !isNaN(id)
  },
  loading: true
})
export default class WidgetPage extends Vue {
  public id?: number

  // methods
  public reload() {
    // TODO: reload widget data
    this.$toasted.info('Reloading widget data...')
  }

  // hooks
  async beforeCreate() {
    this.id = parseInt(this.$route.params.widgetId)
  }

  mounted() {
    this.$nextTick(() => {
      this.$nuxt.$loading.start()
    })
  }
}
</script>

<style lang="scss" scoped>
</style>
