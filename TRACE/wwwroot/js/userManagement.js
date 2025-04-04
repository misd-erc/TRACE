﻿function showTab(tabName) {
    document.querySelectorAll('.cms-content').forEach(function (content) {
        content.classList.remove('active');
    });

    document.querySelectorAll('.cms-nav li').forEach(function (navItem) {
        navItem.classList.remove('active');
    });

    document.getElementById(tabName).classList.add('active');

    event.target.classList.add('active');
}

document.addEventListener("DOMContentLoaded", function () {
    fetchUsers();
});

// Prevent redeclaration of allUsers
window.allUsers = window.allUsers || [];

function fetchUsers() {
    fetch(`/Users/GetAllUsers`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(result => {
            const tableBody = document.getElementById("userdata");

            // Check if the table exists before modifying it
            if (!tableBody) {
                //console.warn("Skipping fetchUsers: userdata table not found.");
                return;
            }

            if (result && Array.isArray(result.data) && result.data.length > 0) {
                window.allUsers = result.data; // Store data globally
                initializeTableFunctions();
            } else {
                tableBody.innerHTML = `<tr><td colspan="4">No Data Found</td></tr>`;
            }
        })
        .catch(error => {
            console.error('Error fetching user data:', error);
            const tableBody = document.getElementById("userdata");
            if (tableBody) {
                tableBody.innerHTML = `<tr><td colspan="4">Failed to fetch data. Please try again later.</td></tr>`;
            }
        });
}


function initializeTableFunctions() {
    const tableBody = document.querySelector("#userdata");
    const searchInput = document.querySelector("#userSearch");
    const paginationDropdown = document.querySelector("#userPagination");
    const prevButton = document.querySelector("#prevPage");
    const nextButton = document.querySelector("#nextPage");

    // Stop execution if table elements do not exist
    if (!tableBody || !searchInput || !paginationDropdown || !prevButton || !nextButton) {
        console.warn("Skipping initializeTableFunctions: Some elements are missing.");
        return;
    }

    const rowsPerPage = 5;
    let currentPage = 1;
    let filteredUsers = [...window.allUsers];

    function renderTable(data) {
        tableBody.innerHTML = data.map(user => `
            <tr>
                <td>${user.fullname || 'N/A'}</td>
                <td>${user.email || 'N/A'}</td>
                <td>${user.department || 'N/A'}</td>
                <td>
                    <a href="/Users/Edit/${user.id}">
                        <button title="Manage role for this user">Manage</button>
                    </a>
                </td>
            </tr>
        `).join('');
    }

    function displayRows() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;
        renderTable(filteredUsers.slice(start, end));
        updatePaginationControls();
    }

    function updatePaginationControls() {
        paginationDropdown.innerHTML = "";
        const total = Math.ceil(filteredUsers.length / rowsPerPage);

        for (let i = 1; i <= total; i++) {
            const option = document.createElement("option");
            option.value = i;
            option.textContent = i;
            if (i === currentPage) option.selected = true;
            paginationDropdown.appendChild(option);
        }

        prevButton.disabled = currentPage === 1;
        nextButton.disabled = currentPage === total;
    }

    paginationDropdown.addEventListener("change", function () {
        currentPage = parseInt(this.value);
        displayRows();
    });

    prevButton.addEventListener("click", function () {
        if (currentPage > 1) {
            currentPage--;
            displayRows();
            paginationDropdown.value = currentPage;
        }
    });

    nextButton.addEventListener("click", function () {
        if (currentPage < Math.ceil(filteredUsers.length / rowsPerPage)) {
            currentPage++;
            displayRows();
            paginationDropdown.value = currentPage;
        }
    });

    searchInput.addEventListener("input", function () {
        const query = this.value.toLowerCase();
        filteredUsers = window.allUsers.filter(user =>
            Object.values(user).some(val =>
                val && val.toString().toLowerCase().includes(query)
            )
        );
        currentPage = 1;
        displayRows();
    });

    displayRows();
}
