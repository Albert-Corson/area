<template>
  <div class="login">
    {{ token }}
    <router-link to="/"><div>FOLDER TO GO TO ABOUT</div></router-link>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'nuxt-property-decorator'

import AuthStore from '~/store/modules/AuthStore'

@Component({
  name: 'Login',
  components: {}
})
export default class Login extends Vue {
  beforeMount() {
    this.$store.dispatch('auth/login', { username: '', password: '' })
    AuthStore.getToken('', '')
  }

  get token() { return AuthStore.token }
}
</script>
