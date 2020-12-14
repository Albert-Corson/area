<template>
  <div class="widget-applet-factory">
    <component :is="`WidgetApplet${ widgetId }`"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import WidgetApplet1 from '~/components/widgets-applets/WidgetApplet1.vue'
import WidgetApplet2 from '~/components/widgets-applets/WidgetApplet2.vue'
import WidgetApplet3 from '~/components/widgets-applets/WidgetApplet3.vue'
import WidgetApplet4 from '~/components/widgets-applets/WidgetApplet4.vue'
import WidgetApplet5 from '~/components/widgets-applets/WidgetApplet5.vue'
import WidgetApplet6 from '~/components/widgets-applets/WidgetApplet6.vue'
import WidgetApplet7 from '~/components/widgets-applets/WidgetApplet7.vue'
import WidgetApplet8 from '~/components/widgets-applets/WidgetApplet8.vue'
import WidgetApplet9 from '~/components/widgets-applets/WidgetApplet9.vue'
import WidgetApplet10 from '~/components/widgets-applets/WidgetApplet10.vue'
import WidgetApplet11 from '~/components/widgets-applets/WidgetApplet11.vue'
import WidgetApplet12 from '~/components/widgets-applets/WidgetApplet12.vue'

@Component({
  name: 'WidgetAppletFactory',
  components: {
    WidgetApplet1,
    WidgetApplet2,
    WidgetApplet3,
    WidgetApplet4,
    WidgetApplet5,
    WidgetApplet6,
    WidgetApplet7,
    WidgetApplet8,
    WidgetApplet9,
    WidgetApplet10,
    WidgetApplet11,
    WidgetApplet12
  }
})
export default class WidgetAppletFactory extends Vue {
  // props
  @Prop({ required: true, validator: v => (v >= 1 && v <= 12) }) readonly widgetId!: number
}
</script>

<style lang="scss" scoped>

</style>
