<template>
  <div class="home">
    <h1>My dashboard</h1>
    <services-list :services="mySortedServices" :widgets="myWidgets" />
    <button
      class="add-widgets-button"
      @click="openWidgetsList"
      title="Add widgets"
    ></button>
    <popup title="Select widgets to add to your dashboard" ref="popup">
      <template v-slot:body>
        <services-info-list
          :services="sortedServices"
          :widgets="missingWidgets"
          @select="addWidget"
        />
      </template>
    </popup>
  </div>
</template>

<script>
import ServicesList from "@/components/services/ServicesList"
import Popup from "@/components/Popup"
import ServicesInfoList from "@/components/services/ServicesInfoList"

export default {
  name: "home",
  components: {
    ServicesList,
    Popup,
    ServicesInfoList
  },
  created() {
    this.refresh()
    if (this.$store.state.Service.services.length === 0) {
      this.$store.dispatch("Service/listServices")
    }
    if (this.$store.state.Widget.widgets.length === 0) {
      this.$store.dispatch("Widget/listWidgets")
    }
  },
  computed: {
    mySortedServices() {
      return this.$store.getters["Service/mySortedServices"]
    },
    myWidgets() {
      return this.$store.state.Widget.myWidgets
    },
    sortedServices() {
      return this.$store.getters["Service/sortedServices"]
    },
    myServices() {
      return this.$store.state.Service.myServices
    },
    widgets() {
      return this.$store.state.Widget.widgets
    },
    missingWidgets() {
      return this.widgets.filter(needle => {
        return !this.myWidgets.find(widget => {
          return widget.id === needle.id
        })
      })
    }
  },
  methods: {
    refresh() {
      this.$store.dispatch("Service/listMyServices")
      this.$store.dispatch("Widget/listMyWidgets")
    },
    openWidgetsList() {
      this.$refs.popup.open()
    },
    addWidget(widgetId) {
      this.$store
        .dispatch("Widget/subscribeToWidget", widgetId)
        .then(() => this.$router.go(0))
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.home {
  padding-top: 1em;

  .add-widgets-button {
    font-family: sans-serif;
    font-size: 4rem;
    font-weight: 500;
    color: white;
    position: fixed;
    right: 2rem;
    bottom: 2rem;
    padding: 2.25rem;
    background: $accentColor;
    background: linear-gradient(to right, #d564a8, #5469ca);
    border-radius: 90px;
    box-shadow: $upShadow, $downShadow;

    &::after {
      content: "+";
      position: absolute;
      left: 50%;
      top: 50;
      transform: translate(-50%, -50%);
    }
  }
}
</style>
