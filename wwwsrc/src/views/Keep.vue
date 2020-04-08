<template>
  <div class="container">
    <keep :data="keep" :vaults="vaults" />
  </div>
</template>

<script>
import { onAuth } from "@bcwdev/auth0-vue";
import Keep from "../components/Keep";
export default {
  name: "Keep-vue",
  components: {
    Keep
  },
  async mounted() {
    this.$store.dispatch("setActiveKeep", this.$route.params.id);
    await onAuth();
    await this.$store.dispatch("setBearer", this.$auth.bearer);
    this.$store.dispatch("getYourVaults");
  },
  computed: {
    keep() {
      return this.$store.state.activeKeep;
    },
    vaults() {
      return this.$store.state.yourVaults;
    }
  }
};
</script>

<style>
</style>