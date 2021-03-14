<template>
  <div class="service-card">
    <h2>{{ service.name }}</h2>
    <button v-if="isAuthenticated === false" @click="subscribe">
      Subscribe
    </button>
    <button v-else @click="unsubscribe">
      Unsubscribe
    </button>
  </div>
</template>

<script>
export default {
  name: "service-card",
  props: {
    service: Object
  },
  computed: {
    isAuthenticated() {
      const match = this.$store.state.Service.myServices.find(service => {
        return service.id === this.service.id
      })
      return Boolean(match)
    }
  },
  methods: {
    subscribe() {
      this.$store
        .dispatch("Service/signinToService", this.service.id)
        .then(console.log)
        .catch(console.error)
    },
    unsubscribe() {
      this.$store
        .dispatch("Service/signoutFromService", this.service.id)
        .then(console.log)
        .catch(console.error)
    }
  }
}
</script>

<style lang="scss" scoped>
.service-card {
  flex: 1;
  padding: 1rem 1rem;
  border: 1px solid #333;
  border-radius: 15px;
}
</style>
