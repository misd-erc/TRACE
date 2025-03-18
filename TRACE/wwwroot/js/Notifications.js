function showTab2(tabName) {
    document.querySelectorAll('.cms-content-notif').forEach(function (content) {
        content.classList.remove('active');
    });

    document.querySelectorAll('.cms-nav li').forEach(function (navItem) {
        navItem.classList.remove('active');
    });

    document.getElementById(tabName).classList.add('active');

    event.target.classList.add('active');
}

//BUTTON FUNCTIONS FOR "ALL" NOTIFICATIONS
document.getElementById("selectAllBtn").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#all input[type='checkbox']");

    checkboxes.forEach(checkbox => {
        checkbox.checked = true;
    });

    document.getElementById("deselectall").removeAttribute("disabled");
    document.getElementById("archiveselected").removeAttribute("disabled");
});

document.getElementById("deselectall").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#all input[type='checkbox']");

    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });

    this.setAttribute("disabled", "true");
    document.getElementById("archiveselected").setAttribute("disabled", "true");
});

document.querySelectorAll("#all input[type='checkbox']").forEach(checkbox => {
    checkbox.addEventListener("change", function () {
        const anyChecked = document.querySelector("#all input[type='checkbox']:checked");

        if (anyChecked) {
            document.getElementById("deselectall").removeAttribute("disabled");
            document.getElementById("archiveselected").removeAttribute("disabled");
        } else {
            document.getElementById("deselectall").setAttribute("disabled", "true");
            document.getElementById("archiveselected").setAttribute("disabled", "true");
        }
    });
});


//BUTTON FUNCTIONS FOR "UNREAD" NOTIFICATIONS
document.getElementById("selectUnreadBtn").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#unread input[type='checkbox']");

    checkboxes.forEach(checkbox => {
        checkbox.checked = true;
    });

    document.getElementById("deselectunread").removeAttribute("disabled");
    document.getElementById("archiveselectedunread").removeAttribute("disabled");
});

document.getElementById("deselectunread").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#unread input[type='checkbox']");

    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });

    this.setAttribute("disabled", "true");
    document.getElementById("archiveselectedunread").setAttribute("disabled", "true");
});

document.querySelectorAll("#unread input[type='checkbox']").forEach(checkbox => {
    checkbox.addEventListener("change", function () {
        const anyChecked = document.querySelector("#unread input[type='checkbox']:checked");

        if (anyChecked) {
            document.getElementById("deselectunread").removeAttribute("disabled");
            document.getElementById("archiveselectedunread").removeAttribute("disabled");
        } else {
            document.getElementById("deselectunread").setAttribute("disabled", "true");
            document.getElementById("archiveselectedunread").setAttribute("disabled", "true");
        }
    });
});


//BUTTON FUNCTIONS FOR "ARCHIVED" NOTIFICATIONS
document.getElementById("selectarchiveBtn").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#archive input[type='checkbox']");

    checkboxes.forEach(checkbox => {
        checkbox.checked = true;
    });

    document.getElementById("deselectarchive").removeAttribute("disabled");
    document.getElementById("unarchiveselected").removeAttribute("disabled");
});

document.getElementById("deselectarchive").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#archive input[type='checkbox']");

    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });

    this.setAttribute("disabled", "true");
    document.getElementById("unarchiveselected").setAttribute("disabled", "true");
});

document.querySelectorAll("#archive input[type='checkbox']").forEach(checkbox => {
    checkbox.addEventListener("change", function () {
        const anyChecked = document.querySelector("#archive input[type='checkbox']:checked");

        if (anyChecked) {
            document.getElementById("deselectarchive").removeAttribute("disabled");
            document.getElementById("unarchiveselected").removeAttribute("disabled");
        } else {
            document.getElementById("deselectarchive").setAttribute("disabled", "true");
            document.getElementById("unarchiveselected").setAttribute("disabled", "true");
        }
    });
});