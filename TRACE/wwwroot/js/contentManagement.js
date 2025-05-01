function showTab(tabName) {
    document.querySelectorAll('.cms-content').forEach(function (content) {
        content.classList.remove('active');
    });

    document.querySelectorAll('.cms-nav li').forEach(function (navItem) {
        navItem.classList.remove('active');
    });

    document.getElementById(tabName).classList.add('active');

    event.target.classList.add('active');
}


document.querySelectorAll('.content-list li').forEach(item => {
    item.addEventListener('click', function () {
        const contentName = this.innerText.trim();
        openModal(contentName);
    });
});

let currentContentName = '';

function openModal(contentName) {
    const modal = document.getElementById('cmsModal');
    modal.style.display = 'block';

    document.querySelector('.mini-loader-overlay').style.display = 'flex';

    setTimeout(() => {
        document.querySelector('.mini-loader-overlay').style.display = 'none';
        
    }, 2000);
    currentContentName = contentName;
    document.getElementById('cmsModal').style.display = 'flex';
    document.getElementById('modalTitle').innerText = "Customize " + contentName;
}


function closeModal() {
    document.getElementById('cmsModal').style.display = 'none';
    $(".cms-modal .modal-content .modal-btn")
        .html("")
        .removeAttr("onclick");
}

function loadCaseCategories() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Category")
        .attr("onclick", "window.location.href='/CaseCategories/Create'");
    $.ajax({
        url: "/CaseCategories/GetCaseCategories",
        type: "GET",
        dataType: "json",
        success: function (response) {
            /*console.log(response);*/
            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
            <tr>
                <td>${item.category}</td>
                <td>${item.description}</td>
                <td><a href="/CaseCategories/Edit/${item.caseCategoryId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },

        error: function () {
            alert("Error loading case categories.");
        }
    });
}

function loadCaseCompanies() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Company")
        .attr("onclick", "window.location.href='/Companies/Create'");
    $.ajax({
        url: "/Companies/GetCompanies",
        type: "GET",
        dataType: "json",
        success: function (response) {
            /*console.log(response);*/
            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
            <tr>
                <td>${item.companyName}</td>
                <td>${item.shortName}</td>
                <td><a href="/CaseCategories/Edit/${item.CompanyId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },

        error: function () {
            alert("Error loading case categories.");
        }
    });
}
function loadCaseEventTypes() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Case Event Type")
        .attr("onclick", "window.location.href='/CaseEventType/Create'");
    $.ajax({
        url: "/CaseEventType/GetCaseEventTypes",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.eventType}</td>
                                <td>No Description Needed</td>
                                <td><a href="/CaseEventType/Edit/${item.caseEventTypeId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
                            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },
        error: function () {
            alert("Error loading case event types.");
        }
    });
}

function loadCaseStatuses() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Case Status")
        .attr("onclick", "window.location.href='/CaseStatus/Create'");
    $.ajax({
        url: "/CaseStatus/GetCaseStatus",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.status}</td>
                                <td>${item.description ? item.description : "No Data Entry"}</td>
                                <td><a href="/CaseStatus/Edit/${item.caseStatusId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
                            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },
        error: function () {
            alert("Error loading case status.");
        }
    });
}

function loadCaseMilestones() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Case Milestone")
        .attr("onclick", "window.location.href='/CaseMilestone/Create'");
    $.ajax({
        url: "/CaseMilestone/GetCaseMilestones",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.milestone}</td>
                                <td>${item.description ? item.description : "No Data Entry"}</td>
                                <td><a href="/CaseMilestone/Edit/${item.caseMilestoneId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
                            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },
        error: function () {
            alert("Error loading case milestone.");
        }
    });
}

function loadHandlingOfficerTypes() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Handling Officer Type")
        .attr("onclick", "window.location.href='/HandlingOfficerType/Create'");
    $.ajax({
        url: "/HandlingOfficerType/GetHearingOfficerTypes",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.officerType}</td>
                                <td>No Description Needed</td>
                                <td><a href="/HandlingOfficerType/Edit/${item.hearingOfficerTypeId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
                            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },
        error: function () {
            alert("Error loading handling officer.");
        }
    });
}
function loadHearingCategories() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Hearing Category")
        .attr("onclick", "window.location.href='/HearingCategory/Create'");
    $.ajax({
        url: "/HearingCategory/GetHearingCategories",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.category}</td>
                                <td>${item.description ? item.description : "No Data Entry"}</td>
                                <td><a href="/HearingCategory/Edit/${item.hearingCategoryId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
                            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },
        error: function () {
            alert("Error loading hearing category.");
        }
    });
}

function loadHearingTypes() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Hearing Type")
        .attr("onclick", "window.location.href='/HearingType/Create'");
    $.ajax({
        url: "/HearingType/GetHearingTypes",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.typeOfHearing}</td>
                                <td>${item.description}</td>
                                <td><a href="/HearingType/Edit/${item.hearingTypeId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
                            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },
        error: function () {
            alert("Error loading hearing type.");
        }
    });
}

function loadHearingVenues() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Hearing Venue")
        .attr("onclick", "window.location.href='/HearingVenue/Create'");
    $.ajax({
        url: "/HearingVenue/GetHearingVenues",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.venueName}</td>
                                <td>No Description Needed</td>
                                <td><a href="/HearingVenue/Edit/${item.hearingVenueId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
                            </tr>`;
                });
                $("#DynamicTable tbody").html(rows);
            } else {
                $("#DynamicTable tbody").html("<tr><td colspan='3'>No Data available.</td></tr>");
            }
        },
        error: function () {
            alert("Error loading hearing venue.");
        }
    });
}



/*SEARCH FILTER*/
$(document).ready(function () {
    $("#searchFilter").on("input", function () {
        const searchText = $(this).val().toLowerCase();

        $("#DynamicTable tbody tr").each(function () {
            const rowText = $(this).text().toLowerCase();

            if (rowText.includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});
