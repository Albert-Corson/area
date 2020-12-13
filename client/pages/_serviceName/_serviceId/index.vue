<template>
  <div class="page service">
    <widget-list
      :widgets="widgets"
      v-on:select="select"/>
    <widget-preview
      v-if="selectedWidget"
      :widget="selectedWidget"/>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import ServiceModel from '~/api/models/ServiceModel'
import { WidgetStore } from '~/store'
import WidgetModel from '~/api/models/WidgetModel'
import WidgetList from '~/components/widgets/WidgetList.vue'
import WidgetPreview from '~/components/preview/WidgetPreview.vue'

@Component({
  name: 'Service',
  components: {
    WidgetList
  },
  validate({ params }) {
    const id = parseInt(params.serviceId)
    return !isNaN(id)
  }
})
export default class ServicePage extends Vue {
  public id?: number
  public service?: ServiceModel
  public selectedWidget: WidgetModel | null = null
  public widgets: Array<WidgetModel> = []

  // methods
  public reload() {
    WidgetStore.fetchWidgets(this.id)
      .then(widgets => this.widgets = widgets!)
  }

  public select(id: number) {
    this.selectedWidget = this.widgets.find(w => w.id === id) ?? null
  }

  // hooks
  beforeCreate() {
    this.id = parseInt(this.$route.params.serviceId)
  }

  mounted() {
    this.reload()
  }
}
</script>

<style lang="scss" scoped>
.page.service {
  display: flex;
  flex-direction: row;
}

.widget-list {
  flex: 1;
}

.widget-preview {
  flex: 1 1;
  max-width: 700px;
  background: black;
}
</style>
