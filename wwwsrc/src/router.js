import Vue from "vue";
import Router from "vue-router";
// @ts-ignore
import Home from "./views/Home.vue";
// @ts-ignore
import Keep from "./views/Keep.vue";
// @ts-ignore
import CreateKeep from "./views/CreateKeep.vue";
// @ts-ignore
import Vault from "./views/Vault.vue";
// @ts-ignore
import CreateVault from "./views/CreateVault.vue";
// @ts-ignore
import Dashboard from "./views/Dashboard.vue";
import { authGuard } from "@bcwdev/auth0-vue";

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: "/",
      name: "home",
      component: Home
    },
    {
      path: "/dashboard",
      name: "dashboard",
      component: Dashboard,
      beforeEnter: authGuard
    },
    {
      path: "/dashboard/vaults",
      name: "dashboard vaults",
      component: Dashboard,
      beforeEnter: authGuard
    },
    {
      path: "/dashboard/keeps",
      name: "dashboard keeps",
      component: Dashboard,
      beforeEnter: authGuard
    },
    {
      path: "/create",
      name: "create",
      component: CreateKeep,
      beforeEnter: authGuard
    },
    {
      path: "/create-vault",
      name: "createVault",
      component: CreateVault,
      beforeEnter: authGuard
    },
    {
      path: "/keep/:id",
      name: "keep",
      component: Keep
    },
    {
      path: "/vault/:id",
      name: "vault",
      component: Vault,
      beforeEnter: authGuard
    }
  ]
});
