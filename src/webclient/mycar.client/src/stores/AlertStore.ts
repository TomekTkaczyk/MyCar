import { defineStore } from 'pinia'

interface Alert {
    message: string;
    type: string;
}

interface AlertState {
    alert: Alert | null;
}

export const useAlertStore = defineStore('alert', {
    state: (): AlertState => ({
        alert: null,
    }),
    actions: {
        success(message: string) {
            this.alert = { message, type: 'alert-success' };
        },
        error(message: string) {
            this.alert = { message, type: 'alert-success' };
        },
        clear() {
            this.alert = null;
        },
    }
});
