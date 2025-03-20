document.addEventListener("DOMContentLoaded", function () {
    const rowsPerPage = 5;
    let currentPage = 1;

    const table = document.querySelector("#ADuserTable tbody");
    const rows = Array.from(table.getElementsByTagName("tr"));
    let filteredRows = [...rows];
    const totalPages = () => Math.ceil(filteredRows.length / rowsPerPage);

    const searchInput = document.querySelector("#userSearch");
    const paginationDropdown = document.querySelector("#userPagination");
    const prevButton = document.querySelector("#prevPage");
    const nextButton = document.querySelector("#nextPage");

    function displayRows() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        rows.forEach(row => row.style.display = "none");
        filteredRows.slice(start, end).forEach(row => row.style.display = "table-row");

        updatePaginationControls();
    }

    function updatePaginationControls() {
        paginationDropdown.innerHTML = "";
        for (let i = 1; i <= totalPages(); i++) {
            const option = document.createElement("option");
            option.value = i;
            option.textContent = i;
            if (i === currentPage) option.selected = true;
            paginationDropdown.appendChild(option);
        }

        prevButton.style.opacity = currentPage === 1 ? "0.5" : "1";
        nextButton.style.opacity = currentPage === totalPages() ? "0.5" : "1";
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
        if (currentPage < totalPages()) {
            currentPage++;
            displayRows();
            paginationDropdown.value = currentPage;
        }
    });

    searchInput.addEventListener("input", function () {
        const query = this.value.toLowerCase();
        filteredRows = rows.filter(row =>
            Array.from(row.getElementsByTagName("td")).some(td =>
                td.textContent.toLowerCase().includes(query)
            )
        );

        currentPage = 1;
        displayRows();
    });

    displayRows();
});