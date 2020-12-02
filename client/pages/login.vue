<template>
  <div class="page login">
    <form @submit.prevent="login">
      <h1>Sign in</h1>
      <input type="text" name="username" placeholder="Username" v-model="username"/>
      <input type="password" name="password" placeholder="Password" v-model="password"/>
      <input type="submit" value="Sign in"/>
    </form>
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
  // data
  public username: string = ''
  public password: string = ''

  // methods
  public login() {
    AuthStore.getToken({ username: this.username, password: this.password })
  }
}
</script>

<style lang="scss" scoped>
.page.login {
  display: flex;
  align-items: center;
  justify-content: center;
}

form {
  text-align: center;
  padding: 4em 2em;
  background: var(--main-bg-color);
  box-shadow: 0 3px 0 0 var(--focus-color);
  border-radius: 5px;

  h1 {
    margin: 0 0 2rem 0;
    padding: 0;
  }

  input {
    background: var(--secondary-bg-color);
    outline: var(--focus-color);
    display: block;
    min-width: 350px;
    font: inherit;
    padding: .85rem 1rem;
    margin: .75rem 0;
    border: transparent 2px solid;
    border-radius: 3px;
    transition: filter,border-color .2s ease-in-out;
    color: inherit;

    &:focus {
      border-color: var(--focus-color);
    }

    &:hover {
      filter: brightness(120%);
    }
  }
}
</style>
