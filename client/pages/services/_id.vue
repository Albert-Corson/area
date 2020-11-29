<template>
  <div class="page service">
    <widget-list :widgets="widgets">
    </widget-list>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import ServiceStore from '~/store/modules/ServiceStore'
import ServiceModel from '~/api/models/ServiceModel'
import WidgetStore from '~/store/modules/WidgetStore'
import WidgetModel from '~/api/models/WidgetModel'
import WidgetList from '~/components/widgets/WidgetList.vue'

@Component({
  name: 'Service',
  components: {
    WidgetList
  },
  validate({ params }) {
    const id = parseInt(params.id)
    return !isNaN(id)
  }
})
export default class ServicePage extends Vue {
  public id?: number
  public service?: ServiceModel
  public widgets: Array<WidgetModel> = []

  async beforeCreate() {
    this.id = parseInt(this.$route.params.id)
    this.service = await ServiceStore.fetchService(this.id)
    const widgets = await WidgetStore.fetchWidgets(this.id)
    if (widgets !== undefined) {
      this.widgets = widgets
    }
  }
}
</script>

<style lang="scss" scoped>
</style>
