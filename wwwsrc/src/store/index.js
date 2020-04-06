import Vue from "vue";
import Vuex from "vuex";
import Axios from "axios";
import router from "../router";

Vue.use(Vuex);

let baseUrl = location.host.includes("localhost")
  ? "https://localhost:5001/"
  : "/";

let api = Axios.create({
  baseURL: baseUrl + "api/",
  timeout: 3000,
  withCredentials: true
});

export default new Vuex.Store({
  state: {
    publicKeeps: {},
    activeKeep: {},
    yourKeeps: {},

  },
  mutations: {
    setPublicKeeps(state, payload) {
      let data = {}
      payload.forEach(each => {
        data[each.id] = each;
      });
      state.publicKeeps = data;
    },
    addPublicKeep(state, payload) {
      Vue.set(state.publicKeeps, payload.id, payload)
    },
    removeKeep(state, id) {
      Vue.delete(state.publicKeeps, id)
      state.publicKeeps = state.publicKeeps.filter(k => k.id != id);
    },
    editKeep(state, payload) {
      Vue.set(state.publicKeeps, payload.id, payload)
    },
    setActiveKeep(state, payload) {
      state.activeKeep = payload;
    },
    viewBump(state, id) {
      state.publicKeeps[id].views += 1;
    },
    addYourKeep(state, payload) {
      Vue.set(state.yourKeeps, payload.id, payload)
    }
  },



  actions: {
    setBearer({ }, bearer) {
      api.defaults.headers.authorization = bearer;
    },
    resetBearer() {
      api.defaults.headers.authorization = "";
    },
    //#region points
    async getKeeps({ commit }) {
      let res = await api.get("keeps")
      commit("setPublicKeeps", res.data);
    },
    async removeKeep({ commit }, id) {
      await api.delete(`keeps/${id}`)
      commit("removeKeep", id);
    },
    async editKeep({ commit }, data) {
      let res = await api.put(`keeps/${data.id}`, data);
      commit("editKeep", res.data)
    },
    async setActiveKeep({ commit, state }, id) {
      api.post(`keeps/${id}/view`)
      if (state.publicKeeps[id]) {
        commit("viewBump", id)
        commit("setActiveKeep", state.publicKeeps[id])
        return
      }
      let res = await api.get(`keeps/${id}`);
      commit("setActiveKeep", res.data);
      if (!res.data.isPrivate) {
        commit("addPublicKeep", res.data);
      }
    },
    async createKeep({ commit }, newKeep) {
      let res = await api.post("keeps", newKeep)

      if (!res.data.isPrivate) {
        commit("addPublicKeep", res.data)
      }
      commit("addYourKeep", res.data)
      router.push({ name: "keep", params: { id: res.data.id } })
    }
    //#endregion
  }
});
