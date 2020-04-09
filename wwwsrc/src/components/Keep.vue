<template>
  <div>
    <div
      class="modal fade"
      id="shareModal"
      tabindex="-1"
      role="dialog"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Copy link to share</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <p>Click the button bellow to cop the link to this keep.</p>
            <p class="text-success">{{shareMessage}}</p>
          </div>
          <div class="modal-footer">
            <input ref="input" :value="href" readonly />
            <button type="button" @click="copyLinkToClipboard" class="btn btn-primary">Copy Link</button>
          </div>
        </div>
      </div>
    </div>
    <div class="card">
      <div class="embed-responsive embed-responsive-16by9">
        <a v-if="onSingleKeep" :href="data.img">
          <img class="card-img-top embed-responsive-item" :src="data.img" />
        </a>
        <img v-else class="card-img-top embed-responsive-item" :src="data.img" />
      </div>
      <div class="card-body">
        <router-link :to="{name:'keep', params:{id:data.id}}">
          <div v-if="!editing">
            <h4 class="card-title">{{data.name}}</h4>
            <p class="card-text">{{data.description}}</p>
            <p class="card-text">
              Views: {{data.views}}
              keeps: {{data.keeps}}
              shares: {{data.shares}}
            </p>
          </div>
          <div v-else>
            <input v-model="edited.name" placeholder="name" />
            <input v-model="edited.description" placeholder="description" />
            <input v-model="edited.img" placeholder="image url" />
            <button class="btn btn-success" @click="submitEdit">Send</button>
          </div>
        </router-link>
        <div class="dropdown">
          <button
            class="btn btn-success"
            data-toggle="modal"
            data-target="#shareModal"
            v-if="$route.name == 'keep'"
          >share</button>
          <button v-if="sub == data.userId" class="btn btn-danger" @click="remove">Delete</button>
          <button v-if="sub == data.userId" class="btn btn-warning" @click="editing = !editing">edit</button>
          <button
            v-if="data.vaultKeepId"
            class="btn btn-danger"
            @click="$emit('remove-from-vault')"
          >Remove from Vault</button>
          <button
            v-if="vaults && Object.keys(vaults).length > 0"
            class="btn btn-secondary dropdown-toggle"
            type="button"
            data-toggle="dropdown"
          >Add To Vault</button>
          <div class="dropdown-menu">
            <a
              v-for="vault in vaults"
              @click="addToVault(vault.id)"
              :key="vault.id"
              class="dropdown-item"
            >{{vault.name}}</a>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import "bootstrap";
export default {
  data() {
    return {
      sharing: false,
      shareMessage: "",
      editing: false,
      edited: {}
    };
  },
  mounted() {
    this.edited = { ...this.data };
  },
  props: ["data", "vaults"],
  computed: {
    sub() {
      return this.$auth.userInfo.sub;
    },
    onSingleKeep() {
      return this.$route.name == "keep";
    },
    href() {
      return self.location.href;
    }
  },
  methods: {
    remove() {
      this.$store.dispatch("removeKeep", this.data.id);
      this.$emit("RemoveKeep");
    },
    submitEdit() {
      this.$store.dispatch("editKeep", this.edited);
      this.editing = false;
    },
    addToVault(vaultId) {
      this.$store.dispatch("addToVault", { vaultId, keep: this.data });
    },
    copyLinkToClipboard() {
      this.$refs.input.select();
      self.document.execCommand("copy");
      this.shareMessage = "Copied to ClipBoard";
      setTimeout(() => {
        this.shareMessage = "";
      }, 2000);
      this.$store.dispatch("shareBump", this.data.id);
    }
  }
};
</script>

<style scoped>
.embed-responsive .card-img-top {
  object-fit: cover;
}
/*  https://stackoverflow.com/a/54766582/3236881  */
</style>