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
        <div
          class="settings-button"
          v-if="hasParams"
          @click="openSettings"
          title="Edit widget parameters"
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
    <popup v-if="hasParams" title="Widget parameters" ref="settings">
      <template v-slot:body>
        <widget-params-form
          @submit.prevent="callWithParams"
          :params="widget.params"
        />
      </template>
    </popup>
  </div>
</template>

<script>
import WidgetView from "@/components/widgets/WidgetView"
import Popup from "@/components/Popup"
import Slider from "@/components/Slider"
import WidgetParamsForm from "@/components/widgets/WidgetParamsForm"

export default {
  name: "widget",
  components: {
    WidgetView,
    Slider,
    Popup,
    WidgetParamsForm
  },
  props: {
    widget: Object
  },
  data() {
    return {
      data: {}
    }
  },
  computed: {
    hasParams() {
      return this.widget.params.length > 0
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
        .dispatch("Widget/callWidget", { widgetId: this.widget.id })
        .then(({ data }) => (this.data = data))
    },
    callWithParams({ target }) {
      this.$refs.settings.close()
      const payload = { ...Object.fromEntries(new FormData(target).entries()) }
      this.$store
        .dispatch("Widget/callWidget", {
          widgetId: this.widget.id,
          params: payload
        })
        .then(({ data }) => (this.data = data))
    },
    openSettings() {
      if (this.$refs.settings) {
        this.$refs.settings.open()
      }
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
  .unsubscribe-button,
  .settings-button {
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
    background-color: rgb(242, 116, 97);
    background-size: 40%;
    transform: translate(-25%, -25%);
    top: 0;
    left: 0;
    background-image: url("../../assets/cross.svg");
  }

  .settings-button {
    background-size: 60%;
    transform: translate(-75%, 75%);
    top: 0;
    left: 100%;
    background-image: url("../../assets/settings.svg");
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
