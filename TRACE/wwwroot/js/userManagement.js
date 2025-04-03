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
document.addEventListener("DOMContentLoaded", function () {
    fetchUsers(); // Fetch users when the page loads
});

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

            if (result.success && Array.isArray(result.data) && result.data.length > 0) {
                tableBody.innerHTML = '';

                result.data.forEach(user => {
                    tableBody.innerHTML += `
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
                    `;
                });

                initializeTableFunctions(); // 🔥 Initialize search & pagination AFTER data is loaded
            } else {
                tableBody.innerHTML = `<tr><td colspan="4">No Data Found</td></tr>`;
            }
        })
        .catch(error => {
            console.error('Error fetching user data:', error);
            document.getElementById("userdata").innerHTML = `<tr><td colspan="4">Failed to fetch data. Please try again later.</td></tr>`;
        });
}

function initializeTableFunctions() {
    const tableBody = document.querySelector("#userdata");
    const searchInput = document.querySelector("#userSearch");
    const paginationDropdown = document.querySelector("#userPagination");
    const prevButton = document.querySelector("#prevPage");
    const nextButton = document.querySelector("#nextPage");

    const rowsPerPage = 5;
    let currentPage = 1;

    function getAllRows() {
        return Array.from(tableBody.getElementsByTagName("tr"));
    }

    function totalPages(rows) {
        return Math.ceil(rows.length / rowsPerPage);
    }

    function displayRows(rows) {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        tableBody.innerHTML = "";
        rows.slice(start, end).forEach(row => tableBody.appendChild(row));

        updatePaginationControls(rows);
    }

    function updatePaginationControls(rows) {
        paginationDropdown.innerHTML = "";
        for (let i = 1; i <= totalPages(rows); i++) {
            const option = document.createElement("option");
            option.value = i;
            option.textContent = i;
            if (i === currentPage) option.selected = true;
            paginationDropdown.appendChild(option);
        }

        prevButton.style.opacity = currentPage === 1 ? "0.5" : "1";
        nextButton.style.opacity = currentPage === totalPages(rows) ? "0.5" : "1";
    }

    paginationDropdown.addEventListener("change", function () {
        currentPage = parseInt(this.value);
        displayRows(getAllRows());
    });

    prevButton.addEventListener("click", function () {
        if (currentPage > 1) {
            currentPage--;
            displayRows(getAllRows());
            paginationDropdown.value = currentPage;
        }
    });

    nextButton.addEventListener("click", function () {
        if (currentPage < totalPages(getAllRows())) {
            currentPage++;
            displayRows(getAllRows());
            paginationDropdown.value = currentPage;
        }
    });

    searchInput.addEventListener("input", function () {
        const query = this.value.toLowerCase();
        const allRows = getAllRows();
        const filteredRows = allRows.filter(row =>
            Array.from(row.getElementsByTagName("td")).some(td =>
                td.textContent.toLowerCase().includes(query)
            )
        );

        currentPage = 1;
        displayRows(filteredRows);
    });

    displayRows(getAllRows());
}
