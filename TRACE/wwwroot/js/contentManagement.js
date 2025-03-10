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
function loadCaseEventTypes() {
    $(".cms-modal .modal-content .modal-btn")
        .html("<i class='bx bx-plus'></i> Add New Category")
        .attr("onclick", "window.location.href='/CaseEventTypes/Create'");
    $.ajax({
        url: "/CaseEventTypes/GetCaseEventTypes",
        type: "GET",
        dataType: "json",
        success: function (response) {
            console.log(response); // Debugging

            if (response && response.data && response.data.length > 0) {
                let rows = "";
                response.data.forEach(item => {
                    rows += `
                            <tr>
                                <td>${item.caseEventTypeId}</td>
                                <td>${item.eventType}</td>
                                <td><a href="/CaseEventTypes/Edit/${item.caseEventTypeId}"><button><i class='bx bxs-edit-alt'></i> Edit</button></a></td>
                               
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
