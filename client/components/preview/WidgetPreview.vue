<template>
  <div class="widget-preview">
    <widget-applet-factory :widget="widget"></widget-applet-factory>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'nuxt-property-decorator'
import WidgetModel from '~/api/models/WidgetModel'
import WidgetAppletFactory from '~/components/widgets-applets/WidgetAppletFactory.vue'
import { WidgetStore, ServiceStore } from '~/store'

@Component({
  name: 'WidgetPreview',
  components: {
    WidgetAppletFactory
  }
})
export default class WidgetPreview extends Vue {
  // props
  @Prop({ required: true }) readonly widget!: WidgetModel
}
</script>

<style lang="scss">
.widget-preview {
  flex: 1 1;
  padding: 2em 1em;
  max-width: 40vw;
  min-width: 500px;
  text-align: center;
  border-left: 3px solid var(--focus-color);
  overflow-y: auto;
}

.vs__search::placeholder,
.vs__dropdown-toggle,
.vs__dropdown-menu {
  background: var(--active-color);
}

.vs__clear,
.vs__open-indicator {
  fill: var(--inactive-color);
}
</style>
