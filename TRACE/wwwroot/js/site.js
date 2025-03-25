document.addEventListener("DOMContentLoaded", function () {
    setTimeout(function () {
        document.getElementById("loader").style.display = "none";
    }, 2000);
});

document.addEventListener("DOMContentLoaded", function () {
    setTimeout(function () {
        let loader2 = document.getElementById("loader2");
        let loader3 = document.getElementById("loader3");

        if (loader2) loader2.style.display = "none";
        if (loader3) loader3.style.display = "none";
    }, 4000);
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

function toggleDropdown(event) {
    event.stopPropagation();
    let dropdown = event.currentTarget.querySelector(".cads-dropdown");
    dropdown.style.display = dropdown.style.display === "block" ? "none" : "block";
}

function toggleCollapse(event) {
    event.stopPropagation();

    let parentLi = event.currentTarget;
    let dropdown = parentLi.nextElementSibling;

    if (dropdown && dropdown.classList.contains("cads-dropdown")) {

        document.querySelectorAll(".has-dropdown").forEach(item => {
            if (item !== parentLi) {
                item.classList.remove("open");
                item.nextElementSibling.style.maxHeight = "0px";
            }
        });


        parentLi.classList.toggle("open");
        if (parentLi.classList.contains("open")) {
            dropdown.style.maxHeight = dropdown.scrollHeight + "px";
        } else {
            dropdown.style.maxHeight = "0px";
        }
    }
}


document.querySelector("header").addEventListener("mouseleave", function () {
    document.querySelectorAll(".has-dropdown").forEach(item => {
        item.classList.remove("open");
        item.nextElementSibling.style.maxHeight = "0px";
    });
});

//CAUSING ISSUE DO NOT TOUCH
//document.addEventListener("DOMContentLoaded", function () {
//    let goBackLink = document.getElementById("goBackLink");

//    if (document.referrer) {
//        let referrerUrl = new URL(document.referrer);

//        if (referrerUrl.origin === window.location.origin) {
//            let pageName = referrerUrl.pathname.split("/").filter(Boolean).pop() || "Home";
//            pageName = pageName.replace(/[-_]/g, " ");
//            pageName = pageName.charAt(0).toUpperCase() + pageName.slice(1);

//            goBackLink.href = document.referrer;
//            goBackLink.textContent = pageName;
//        } else {
//            goBackLink.style.display = "none";
//        }
//    } else {
//        goBackLink.style.display = "none";
//    }
//});
