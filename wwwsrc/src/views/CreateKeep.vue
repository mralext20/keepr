<template>
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-4">
        <form class="form-group" @submit.prevent="submit">
          Image url:
          <input class="form-control" v-model="newKeep.img" />
          Name:
          <input class="form-control" v-model="newKeep.name" />
          Description:
          <input class="form-control" v-model="newKeep.description" />
          <div class="form-check">
            <label class="form-check-label">
              <input type="checkbox" class="form-check-input" v-model="newKeep.isPrivate" />
              is Private
            </label>
          </div>

          <button class="btn btn-primary" type="submit">Create Point</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      newKeep: {}
    };
  },
  methods: {
    submit() {
      this.$store.dispatch("createKeep", this.newKeep);
    }
  }
};
</script>

<style>
</style>