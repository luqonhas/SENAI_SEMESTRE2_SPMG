export const menuToggle = () => {
    const toggleMenu = document.querySelector('.header-avatar-submenu-dropdown');
    toggleMenu.classList.toggle('active')
}

export const menuToggleOff = () => {
    const toggleMenu = document.querySelector('.header-avatar-submenu-dropdown');
    toggleMenu.remove.classList('active')
}