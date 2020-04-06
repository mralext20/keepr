<template>
  <div class="dashboard">
    <h1>WELCOME TO THE DASHBOARD</h1>
    public {{ yourVaults }} user {{ yourKeeps }}
  </div>
</template>

<script>
import { onAuth } from "@bcwdev/auth0-vue";
export default {
  async mounted() {
    await onAuth();
    await this.$store.dispatch("setBearer", this.$auth.bearer);
    this.$store.dispatch("getYourVaults");
    this.$store.dispatch("getYourKeeps");
  },
  computed: {
    yourKeeps() {
      return this.$store.state.yourKeeps;
    },
    yourVaults() {
      return this.$store.state.yourVaults;
    }
  }
};
</script>

<style></style>
