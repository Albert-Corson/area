<template>
  <div class="login">
    {{ token }}
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'nuxt-property-decorator'

import AuthStore from '~/store/modules/auth'

@Component({
  name: 'Login',
  components: {},
  async asyncData() {
    await AuthStore.login('', '')
  }
})
export default class Login extends Vue {
  get token() { return AuthStore.token }
}
</script>
