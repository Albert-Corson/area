<template>
  <div class="widget-preview">
    <span class="widget-name">{{ widget.name }}</span>
    <div class="widget" :class="{ locked: widget.requires_auth }">
      <div class="controls">
        <div
          class="unsubscribe-button"
          @click="unsubscribe"
          title="Remove this widget from your dashboard"
        ></div>
        <div
          class="refresh-button"
          v-if="!widget.requires_auth"
          @click="refresh"
          title="Update widget feed"
        ></div>
      </div>
      <div v-if="widget.requires_auth" @click="signinToService">
        <p>
          This widget is locked
        </p>
        <p>
          Please click this card to sign in to the associated service
        </p>
      </div>
      <div v-else-if="data.items && data.items.length === 0">
        Nothing to be displayed
      </div>
      <slider v-else :items="data.items" transition="fade">
        <template v-slot="{ item, visible }">
          <widget-view
            :widget="item"
            :visible="visible"
            @refresh="refresh"
            @unsubscribe="unsubscribe"
          />
        </template>
      </slider>
    </div>
  </div>
</template>

<script>
import WidgetView from "@/components/widgets/WidgetView"
import Slider from "@/components/Slider"

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
    unsubscribe() {
      this.$store
        .dispatch("Widget/unsubscribeFromWidget", this.widget.id)
        .then(() => this.$router.go(0))
    },
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
  height: 15rem;
  width: 15rem;
  margin: 1rem;

  border-radius: $borderRadius;
  box-shadow: $upShadow, $downShadow;

  position: relative;

  &.locked > div:not(.controls) {
    opacity: 1;
    transition: 0.15s opacity ease-in-out;
    &:hover {
      opacity: 0.6;
    }
    cursor: pointer;

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

  .refresh-button,
  .unsubscribe-button {
    cursor: pointer;
    position: absolute;
    background-color: rgb(55, 55, 55);
    opacity: 0.4;
    z-index: 3;
    color: $bgColor;
    height: 2rem;
    width: 2rem;
    border-radius: 90px;
    transition: opacity 0.2s ease-out;
    background-repeat: no-repeat;
    background-position: center;

    &:hover {
      opacity: 1;
    }
  }

  .refresh-button {
    background-size: 60%;
    transform: translate(-75%, -25%);
    top: 0;
    left: 100%;
    background-image: url("../../assets/refresh.svg");
  }

  .unsubscribe-button {
    background-size: 40%;
    transform: translate(-25%, -25%);
    top: 0;
    left: 0;
    background-image: url("../../assets/cross.svg");
  }

  & > * {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;

    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    text-align: center;

    padding: 1rem;
  }
}
</style>
