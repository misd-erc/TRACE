document.addEventListener("DOMContentLoaded", function () {
    setTimeout(function () {
        document.getElementById("loader").style.display = "none";
    }, 2000);
});
const _notifmenu = document.getElementById('notifDropdown');
const _profilemenu = document.getElementById('profileDropdown');
function toggleProfileMenu() {
    _notifmenu.classList.remove('show');
    _profilemenu.classList.toggle('show');
}

function toggleNotifMenu() {
    _profilemenu.classList.remove('show')
    _notifmenu.classList.toggle('show');
}
