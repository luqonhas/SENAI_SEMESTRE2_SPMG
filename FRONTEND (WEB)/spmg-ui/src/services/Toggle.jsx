export const menuToggle = () => {
    const toggleMenu = document.querySelector('.header-avatar-submenu-dropdown');
    toggleMenu.classList.toggle('active')
}


// FUNÇÃO NÃO UTILIZADA, MAS ÚTIL PRO FUTURO

export const settingSelector = () => {
    const URL = window.location.pathname;
    console.log(URL);

    const btns = document.querySelector('.settings-content-btn-main').querySelectorAll('.settings-content-btn');
    console.log(btns);
    
    switch (URL) {
        case '/conta/editar':
            btns.forEach(btn => btn.classList.add('settings-btn-active'))
            break;

        case '/conta/senha/alterar':
            btns[0].forEach(btn => btn.classList.add('settings-btn-active'))
            break;
    
        default:
            btns.forEach(btn => btn.classList.add('settings-btn-active'));
            this.classList.add('settings-btn-active')
            break;
    }

    btns.forEach(x => {
        x.addEventListener('click', function(){
            btns.forEach(btn => btn.classList.remove('settings-btn-active'));
            
            this.classList.add('settings-btn-active')
        })
    })
}