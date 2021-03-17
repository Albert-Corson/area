<template>
  <div class="profile">
    <h1>My account</h1>
    <h2>My devices</h2>
    <devices-list :devices="devices" :currentDevice="currentDevice" />
    <h2>Account details</h2>
    <account-details />
    <h2>Delete my account</h2>
    <button class="gradient" @click="deleteAccount">Delete my account</button>
  </div>
</template>

<script>
import DevicesList from "@/components/profile/DevicesList"
import AccountDetails from "@/components/profile/AccountDetails"

export default {
  name: "profile",
  components: {
    DevicesList,
    AccountDetails
  },
  computed: {
    devices() {
      const devices = [...this.$store.state.User.devices]
      return devices.sort((a, b) => a.last_used > b.last_used)
    },
    currentDevice() {
      return this.$store.state.User.currentDevice
    }
  },
  created() {
    this.$store.dispatch("User/listDevices")
  },
  methods: {
    deleteAccount() {
      if (confirm("Are you sure you want to delete your account ?")) {
        this.$store
          .dispatch("User/deleteAccount")
          .then(() => this.$router.push("/auth/signout"))
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.profile {
  padding-top: 1em;
}
</style>
