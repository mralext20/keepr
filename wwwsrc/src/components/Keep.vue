<template>
  <div class="card col-md-2 col-12">
    <img class="card-img-top" :src="data.img" alt />
    <div class="card-body">
      <div v-if="!editing">
        <h4 class="card-title">{{data.name}}</h4>
        <p class="card-text">{{data.description}}</p>
      </div>
      <div v-else>
        <input v-model="edited.name" placeholder="name" />
        <input v-model="edited.description" placeholder="description" />
        <input v-model="edited.img" placeholder="image url" />
        <button class="btn btn-success" @click="submitEdit">Send</button>
      </div>
      <div v-if="sub == data.userId">
        <button class="btn btn-danger" @click="remove">Delete</button>
        <button class="btn btn-warning" @click="editing = !editing">edit</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      editing: false,
      edited: {}
    };
  },
  mounted() {
    this.edited = { ...this.data };
  },
  props: ["data"],
  computed: {
    sub() {
      return this.$auth.userInfo.sub;
    }
  },
  methods: {
    remove() {
      this.$store.dispatch("removeKeep", this.data.id);
    },
    submitEdit() {
      this.$store.dispatch("editKeep", this.edited);
      this.editing = false;
    }
  }
};
</script>

<style>
</style>