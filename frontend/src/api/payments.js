import http from "./http";

export async function createPayment(payload) {
    const { data } = await http.post("/api/payments", payload);
    return data;
}

export async function getPayments(params) {
    const { data } = await http.get("/api/payments", { params });
    return data;
}

export async function getStats(params) {
    const { data } = await http.get("/api/payments/stats", { params });
    return data;
}