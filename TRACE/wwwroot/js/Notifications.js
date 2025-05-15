document.addEventListener("DOMContentLoaded", function () {
    fetchallNotificationsInModule();
});

async function fetchallNotificationsInModule() {
    try {
        const response = await fetch('/Notification/getNotification');
        const notifications = await response.json();

        const allnotifList = document.getElementById('notifList1');
        const unreadnotifList = document.getElementById('unreadnotif');
        const archivednotifList = document.getElementById('archivednotif');

        const allCounter = document.getElementById('allnotifCounter');
        const unreadCounter = document.getElementById('unreadnotifCounter');
        const archivedCounter = document.getElementById('archivednotifCounter');

        // Clear existing content
        allnotifList.innerHTML = '';
        unreadnotifList.innerHTML = '';
        archivednotifList.innerHTML = '';

        if (notifications.length === 0) {
            allnotifList.innerHTML = `<li><span class="ndesc nothing">No notifications available.</span></li>`;
            unreadnotifList.innerHTML = `<li><span class="ndesc nothing">No unread notifications.</span></li>`;
            archivednotifList.innerHTML = `<li><span class="ndesc nothing">No archived notifications.</span></li>`;
            return;
        }

        let unreadCount = 0;
        let archivedCount = 0;

        notifications.forEach(notif => {
            if (!notif.isArchived) {
                const allItem = createNotificationListItem(notif);
                allnotifList.appendChild(allItem);
            }

            if (notif.isArchived) {
                const archivedItem = createNotificationListItem(notif);
                archivednotifList.appendChild(archivedItem);
                archivedCount++;
            } else if (!notif.isRead) {
                const unreadItem = createNotificationListItem(notif);
                unreadnotifList.appendChild(unreadItem);
                unreadCount++;
            }
        });

        // Update counters
        allCounter.textContent = notifications.filter(n => !n.isArchived).length;
        unreadCounter.textContent = unreadCount;
        archivedCounter.textContent = archivedCount;

        // Fallback for empty lists
        if (unreadCount === 0) {
            unreadnotifList.innerHTML = `<li><span class="ndesc nothing">No unread notifications.</span></li>`;
        }
        if (archivedCount === 0) {
            archivednotifList.innerHTML = `<li><span class="ndesc nothing">No archived notifications.</span></li>`;
        }

    } catch (error) {
        console.error('Error loading notifications:', error);
    }
}

function createNotificationListItem(notif) {
    const linotif = document.createElement('li');
    linotif.dataset.notificationid = notif.notificationID;
    linotif.innerHTML = `
        <div class="action-div flex h-center gap-10">
            <input type="checkbox" class="chekin" title="Select this notification." />
        </div>
        <div class="flex h-center space-between notif-item-wrap">
            <div class="notif-content-div">
                <span class='clickupdateread'>${notif.message}</span>
                <span class="notif-status ${notif.isRead ? 'read' : 'unread'}">[${notif.isRead ? 'Read' : 'Unread'}]</span>
            </div>
            <div class="notif-timestamp">
                <span>${formatDate(notif.createdAt)}</span>
            </div>
        </div>
    `;

    const updateReadEl = linotif.querySelector('.clickupdateread');
    if (updateReadEl) {
        updateReadEl.addEventListener('click', async (e) => {
            e.stopPropagation();
            try {
                await fetch('/Notification/MarkAsRead', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(notif.notificationID)
                });

                window.location.href = `/CaseDetails?id=${notif.caseID}`;
            } catch (error) {
                console.error('Failed to mark notification as read:', error);
                window.location.href = `/CaseDetails?id=${notif.caseID}`;
            }
        });
    }

    return linotif;
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('en-US', {
        month: 'short',
        day: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: true
    });
}

function showTab2(tabName) {
    document.querySelectorAll('.cms-content-notif').forEach(function (content) {
        content.classList.remove('active');
    });

    document.querySelectorAll('.cms-nav li').forEach(function (navItem) {
        navItem.classList.remove('active');
    });

    document.querySelectorAll('.cms-content-notif input[type="checkbox"]').forEach(function (checkbox) {
        checkbox.checked = false;
    });

    document.getElementById("deselectall")?.setAttribute("disabled", "true");
    document.getElementById("archiveselected")?.setAttribute("disabled", "true");

    document.getElementById("deselectunread")?.setAttribute("disabled", "true");
    document.getElementById("archiveselectedunread")?.setAttribute("disabled", "true");

    document.getElementById("deselectarchive")?.setAttribute("disabled", "true");
    document.getElementById("unarchiveselected")?.setAttribute("disabled", "true");

    document.getElementById(tabName).classList.add('active');
    event.target.classList.add('active');
}


//BUTTON FUNCTIONS FOR "ALL" NOTIFICATIONS
document.getElementById("selectAllBtn").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#all input[type='checkbox']");
    /*console.log("CHECKED ALL" + checkboxes);*/
    checkboxes.forEach(checkbox => {
        checkbox.checked = true;
    });

    document.getElementById("deselectall").removeAttribute("disabled");
    document.getElementById("archiveselected").removeAttribute("disabled");
});

document.getElementById("deselectall").addEventListener("click", function () {
    const checkboxes = document.querySelectorAll("#all input[type='checkbox']");
    /*console.log("UNCHECK ALL" + checkboxes);*/
    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });

    this.setAttribute("disabled", "true");
    document.getElementById("archiveselected").setAttribute("disabled", "true");
});

document.getElementById("all").addEventListener("change", function (e) {
    if (e.target && e.target.type === "checkbox") {
        const anyChecked = document.querySelector("#all input[type='checkbox']:checked");
        /*console.log("INDIVIDUAL: ", anyChecked);*/
        if (anyChecked) {
            document.getElementById("deselectall").removeAttribute("disabled");
            document.getElementById("archiveselected").removeAttribute("disabled");
        } else {
            document.getElementById("deselectall").setAttribute("disabled", "true");
            document.getElementById("archiveselected").setAttribute("disabled", "true");
        }
    }
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

document.getElementById("unread").addEventListener("change", function (e) {
    if (e.target && e.target.type === "checkbox") {
        const anyChecked = document.querySelector("#unread input[type='checkbox']:checked");

        if (anyChecked) {
            document.getElementById("deselectunread").removeAttribute("disabled");
            document.getElementById("archiveselectedunread").removeAttribute("disabled");
        } else {
            document.getElementById("deselectunread").setAttribute("disabled", "true");
            document.getElementById("archiveselectedunread").setAttribute("disabled", "true");
        }
    }
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

document.getElementById("archive").addEventListener("change", function (e) {
    if (e.target && e.target.type === "checkbox") {
        const anyChecked = document.querySelector("#archive input[type='checkbox']:checked");

        if (anyChecked) {
            document.getElementById("deselectarchive").removeAttribute("disabled");
            document.getElementById("unarchiveselected").removeAttribute("disabled");
        } else {
            document.getElementById("deselectarchive").setAttribute("disabled", "true");
            document.getElementById("unarchiveselected").setAttribute("disabled", "true");
        }
    }
});


async function archiveNotifications(ids) {
    const response = await fetch('/Notification/ArchiveSelected', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(ids)
    });

    if (!response.ok) {
        throw new Error('Server failed to archive notifications.');
    }

    const result = await response.json();

    if (!result.success) {
        throw new Error(result.message || 'Failed to archive notifications.');
    }

    return result;
}

async function unarchiveNotifications(ids) {
    console.log("SELECTED IDs:" + ids);
    console.log("Stringified JSON body:", JSON.stringify(ids));
    const response = await fetch('/Notification/UnarchiveSelected', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(ids)
    });

    if (!response.ok) {
        throw new Error('Server failed to unarchive notifications.');
    }

    const result = await response.json();

    if (!result.success) {
        throw new Error(result.message || 'Failed to unarchive notifications.');
    }

    return result;
}

document.getElementById("archiveselected").addEventListener("click", async function () {
    const selectedIDs = Array.from(document.querySelectorAll("#all input[type='checkbox']:checked"))
        .map(cb => cb.closest("li")?.dataset.notificationid)
        .filter(id => id);

    if (selectedIDs.length === 0) return;

    try {
        await archiveNotifications(selectedIDs);

        Swal.fire({
            title: "Success!",
            text: "Selected notifications have been archived.",
            icon: "success",
            confirmButtonText: "OK",
            customClass: {
                popup: "swal2-popup",
                confirmButton: "swal2-confirm"
            }
        }).then((result) => {
            if (result.isConfirmed) {
                location.reload();
            }
        });
    } catch (error) {
        console.error("Failed to archive notifications:", error);

        Swal.fire({
            title: "Oops!",
            text: "Something went wrong. Please try again.",
            icon: "error",
            confirmButtonText: "OK",
            customClass: {
                popup: "swal2-popup",
                confirmButton: "swal2-confirm"
            }
        });
    }
});

document.getElementById("unarchiveselected").addEventListener("click", async function () {
    const selectedIDs = Array.from(document.querySelectorAll("#archive input[type='checkbox']:checked"))
        .map(cb => cb.closest("li")?.dataset.notificationid)
        .filter(id => id);

    if (selectedIDs.length === 0) return;

    try {
        await unarchiveNotifications(selectedIDs);

        Swal.fire({
            title: "Success!",
            text: "Selected notifications have been unarchived.",
            icon: "success",
            confirmButtonText: "OK",
            customClass: {
                popup: "swal2-popup",
                confirmButton: "swal2-confirm"
            }
        }).then((result) => {
            if (result.isConfirmed) {
                location.reload();
            }
        });
    } catch (error) {
        console.error("Failed to unarchive notifications:", error);

        Swal.fire({
            title: "Oops!",
            text: "Something went wrong. Please try again.",
            icon: "error",
            confirmButtonText: "OK",
            customClass: {
                popup: "swal2-popup",
                confirmButton: "swal2-confirm"
            }
        });
    }
});
