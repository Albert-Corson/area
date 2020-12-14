<template>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'

import { AuthStore } from '~/store'

@Component({
  name: 'Logout'
})
export default class Logout extends Vue {
  // lifecycle
  beforeMount() {
    AuthStore.logout()
      .then(_ => {
        // clear session data
        localStorage.clear()
        this.$router.push('/login')
        // reload stores
        window.location.reload()
      })
  }
}
</script>
