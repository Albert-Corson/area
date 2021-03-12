<template>
  <div class="providers">
    <button class="google" type="button" @click="signinWithGoogle">
      <img src="../assets/google.svg" />
    </button>
    <button class="facebook" type="button" @click="signinWithFacebook">
      <img src="../assets/facebook.svg" />
    </button>
    <button class="microsoft" type="button" @click="signinWithMicrosoft">
      <img src="../assets/microsoft.svg" />
    </button>
  </div>
</template>

<script>
export default {
  name: "provider-button",
  props: {
    provider: String
  },
  methods: {
    signinWithGoogle() {
      this.$store.dispatch("Auth/signinWithGoogle")
    },
    signinWithFacebook() {
      this.$store.dispatch("Auth/signinWithFacebook")
    },
    signinWithMicrosoft() {
      this.$store.dispatch("Auth/signinWithMicrosoft")
    }
  }
}
</script>

<style lang="scss" scoped>
.providers {
  display: flex;
  align-items: center;
  justify-content: space-evenly;
  margin: 0.5rem auto;

  button {
    flex: 1 0;
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 0.7rem;
    font-weight: 600;
    height: auto;

    &:not(:first-child):not(:last-child) {
      margin: 0 0.7rem;
    }

    img {
      height: 1.2em;
    }

    &.google {
      background: #ffffff;
    }

    &.facebook {
      background: #3b5a98;
      color: white;
    }

    &.microsoft {
      background: #ffffff;
    }
  }
}
</style>
