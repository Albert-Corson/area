<template>
  <div class="home">
    <h1>My dashboard</h1>
    <services-list :services="mySortedServices" :widgets="myWidgets" />
    <button
      class="add-widgets-button"
      @click="openWidgetsList"
      title="Add widgets"
    ></button>
    <popup title="Greetings" ref="popup">
      <template v-slot:body>
        AAAAAAAAAAAAAAAAAAAH
      </template>
      <template v-slot:footer>
        <button class="primary">Primary</button>
        <button class="secondary">Secondary</button>
      </template>
    </popup>
  </div>
</template>

<script>
import ServicesList from "@/components/services/ServicesList"
import Popup from "@/components/Popup"

export default {
  name: "home",
  components: {
    ServicesList,
    Popup
  },
  created() {
    this.refresh()
  },
  computed: {
    mySortedServices() {
      return this.$store.getters["Service/mySortedServices"]
    },
    myWidgets() {
      return this.$store.state.Widget.myWidgets
    }
  },
  methods: {
    refresh() {
      this.$store.dispatch("Service/listMyServices")
      this.$store.dispatch("Widget/listMyWidgets")
    },
    openWidgetsList() {
      this.$refs.popup.open()
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

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
</style>
