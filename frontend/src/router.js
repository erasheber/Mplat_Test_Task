import { createRouter, createWebHistory } from "vue-router";
import CreatePaymentView from "./views/CreatePaymentView.vue";
import PaymentsHistoryView from "./views/PaymentsHistoryView.vue";
import PaymentsStatsView from "./views/PaymentsStatsView.vue";

const routes = [
    { path: "/", redirect: "/create" },
    { path: "/create", component: CreatePaymentView },
    { path: "/history", component: PaymentsHistoryView },
    { path: "/stats", component: PaymentsStatsView },
];

export default createRouter({
    history: createWebHistory(),
    routes,
});