import axios from "axios";

const http = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:5000",
    timeout: 15000,
});

http.interceptors.request.use((config) => {
    const apiKey = import.meta.env.VITE_API_KEY;
    if (apiKey) config.headers["X-Api-Key"] = apiKey;
    config.headers["X-Timezone-Offset"] = String(new Date().getTimezoneOffset());
    return config;
});

export default http;