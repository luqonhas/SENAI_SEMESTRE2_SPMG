export const userAuthentication = () => localStorage.getItem('user-token') !== null;

export const parseJwt = () => {
    const payload = localStorage.getItem('user-token').split('.')[1];

    return JSON.parse(window.atob(payload));
}

export const logout = () => {
    localStorage.removeItem('user-token');
}