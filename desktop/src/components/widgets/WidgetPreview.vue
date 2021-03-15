<template>
  <div
    v-if="widget.requires_auth"
    class="widget locked"
    @click="signinToService"
  >
    <p>
      This widget is locked
    </p>
    <p>
      Please click this card to sign in to the associated service
    </p>
  </div>
  <slider v-else :items="data.items" class="widget">
    <template v-slot="{ item, visible }">
      <widget-view :widget="item" :visible="visible" />
    </template>
  </slider>
</template>

<script>
import WidgetView from "@/components/widgets/WidgetView"
import Slider from "@/components/widgets/Slider"

export default {
  name: "widget",
  components: {
    WidgetView,
    Slider
  },
  props: {
    widget: Object
  },
  data() {
    return {
      data: {}
    }
  },
  created() {
    if (this.widget.requires_auth === false) {
      this.refresh()
      if (this.widget.frequency > 0) {
        window.setInterval(() => {
          this.refresh()
        }, this.widget.frequency * 1000)
      }
    }
  },
  methods: {
    refresh() {
      this.$store
        .dispatch("Widget/callWidget", this.widget.id)
        .then(({ data }) => (this.data = data))
    },
    signinToService() {
      this.$store
        .dispatch("Service/signinToService", this.widget.service.id)
        .then(() => this.$store.dispatch("Widget/listMyWidgets"))
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars.scss";

.widget {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;

  padding: 1rem;
  height: 15rem;
  width: 15rem;
  margin: 1rem;
  cursor: pointer;

  border-radius: $borderRadius;
  box-shadow: $upShadow, $downShadow;

  /* opacity: 1; */
  /* transition: 0.15s opacity ease-in-out; */
  /* &:hover { */
  /*   opacity: 0.6; */
  /* } */
  position: relative;

  &.locked {
    &::after {
      content: "";
      position: absolute;
      left: 0;
      right: 0;
      width: 100%;
      height: 100%;
      background-image: url("../../assets/lock.svg");
      background-size: 35%;
      background-repeat: no-repeat;
      background-position: center;
      opacity: 0.12;
    }
  }
}
</style>
