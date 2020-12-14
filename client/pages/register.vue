<template>
  <div class="page register">
    <form @submit.prevent="register">
      <h1>Sign up</h1>
      <input autofocus type="text" name="username" placeholder="Username" v-model="username"/>
      <input type="email" name="email" placeholder="Email" v-model="email"/>
      <input type="password" name="password" placeholder="Password" v-model="password"/>
      <input type="password" name="confirm-password" placeholder="Confirm password" v-model="confirm_password"/>
      <input type="submit" value="Sign up"/>
    </form>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'nuxt-property-decorator'

import { UserStore } from '~/store'

@Component({
  name: 'register',
  components: {}
})
export default class register extends Vue {
  // data
  public username: string = ''
  public email: string = ''
  public password: string = ''
  public confirm_password: string = ''

  // methods
  public async register() {
    if (this.password !== this.confirm_password) {
      this.$toasted.error('Passwords must be identical')
      return
    }
    try {
      await UserStore.createUser({
        username: this.username,
        password: this.password,
        email: this.email
      })
      this.$router.push('/login')
    } catch (e) {}
  }
}
</script>

<style lang="scss" scoped>
.page.register {
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
