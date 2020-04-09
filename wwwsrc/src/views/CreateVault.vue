<template>
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-4">
        <form class="form-group" @submit.prevent="submit">
          Name:
          <input class="form-control" v-model="newVault.name" />
          Description:
          <input class="form-control" v-model="newVault.description" />

          <button class="btn btn-primary" type="submit">Create Vault</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Create-vault",
  data() {
    return {
      newVault: {}
    };
  },
  methods: {
    submit() {
      this.$store.dispatch("createVault", this.newVault);
    }
  }
};
</script>

<style>
</style>