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
            const linotif = document.createElement('li');
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

            allnotifList.appendChild(linotif.cloneNode(true));

            if (notif.isArchived) {
                archivednotifList.appendChild(linotif.cloneNode(true));
                archivedCount++;
            } else if (!notif.isRead) {
                unreadnotifList.appendChild(linotif.cloneNode(true));
                unreadCount++;
            }
        });

        allCounter.textContent = notifications.length;
        unreadCounter.textContent = unreadCount;
        archivedCounter.textContent = archivedCount;

        if (unreadCount === 0) unreadnotifList.innerHTML = `<li><span class="ndesc nothing">No unread notifications.</span></li>`;
        if (archivedCount === 0) archivednotifList.innerHTML = `<li><span class="ndesc nothing">No archived notifications.</span></li>`;

    } catch (error) {
        console.error('Error loading notifications:', error);
    }
}

function formatDate(dateString) {
    const date = new Date(dateString);
    if (isNaN(date)) return dateString;
    return date.toLocaleString('en-US', {
        day: '2-digit', month: 'short', year: 'numeric',
        hour: '2-digit', minute: '2-digit'
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
