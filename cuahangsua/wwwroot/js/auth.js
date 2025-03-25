document.addEventListener("DOMContentLoaded", function () {
    const showLogin = document.getElementById("showLogin");
    const showRegister = document.getElementById("showRegister");
    const loginForm = document.getElementById("loginForm");
    const registerForm = document.getElementById("registerForm");

    showLogin?.addEventListener("click", function () {
        loginForm.classList.remove("d-none");
        registerForm.classList.add("d-none");
    });

    showRegister?.addEventListener("click", function () {
        loginForm.classList.add("d-none");
        registerForm.classList.remove("d-none");
    });
});
