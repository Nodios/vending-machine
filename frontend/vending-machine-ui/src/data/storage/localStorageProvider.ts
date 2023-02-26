class LocalStorageProvider {
    set<T>(key: string, value: T) {
        localStorage.setItem(key, JSON.stringify(value));
    }

    get<T>(key: string): T | null {
        const raw = localStorage.getItem(key);

        if (raw != null && raw !== '') {
            return JSON.parse(raw) as T;
        }

        return null;
    }

    remove(key: string) {
        localStorage.removeItem(key);
    }

    clear() {
        localStorage.clear();
    }
}

const localStorageProvider = new LocalStorageProvider();

export default localStorageProvider;
