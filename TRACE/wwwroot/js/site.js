document.addEventListener("DOMContentLoaded", function () {
    setTimeout(function () {
        document.getElementById("loader").style.display = "none";
    }, 2000);
});

function toggleProfileMenu() {
    const menu = document.getElementById('profileDropdown');
    menu.classList.toggle('show');
}