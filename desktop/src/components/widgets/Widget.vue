<template>
  <div class="widget">
    <div v-if="widget.requires_auth" @click="signinToService">
      This widget is locked
    </div>
    <div v-else>
      {{ widget.name }}
    </div>
  </div>
</template>

<script>
export default {
  name: "widget",
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
      this.$store
        .dispatch("Widget/callWidget", this.widget.id)
        .then(({ data }) => (this.data = data))
    }
  },
  methods: {
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
  min-height: 15rem;
  width: 15rem;
  margin: 1rem;
  cursor: pointer;

  border-radius: $borderRadius;
  box-shadow: $upShadow, $downShadow;

  opacity: 1;
  transition: 0.15s opacity ease-in-out;
  &:hover {
    opacity: 0.6;
  }

  .widget-name {
    display: block;
    font-weight: bolder;
    margin-bottom: 0.3rem;
  }
}
</style>
