let _hearingsData = [];
let currentPage = 1;
const rowsPerPage = 5;

document.addEventListener('DOMContentLoaded', function () {
    fetchAllCaseHearings();

    document.getElementById('searcherinoInput').addEventListener('input', () => {
        currentPage = 1;
        renderTable();
        updatePagination();
    });

    document.getElementById('paginationSelect').addEventListener('change', (e) => {
        currentPage = parseInt(e.target.value);
        renderTable();
    });

    document.getElementById('prevPage').addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            renderTable();
            updatePagination();
        }
    });

    document.getElementById('nextPage').addEventListener('click', () => {
        const maxPages = Math.ceil(filteredHearings().length / rowsPerPage);
        if (currentPage < maxPages) {
            currentPage++;
            renderTable();
            updatePagination();
        }
    });
});

function fetchAllCaseHearings() {
    fetch(`/Hearings/GetHearing`)
        .then(res => res.json())
        .then(data => {
            _hearingsData = data;
            renderTable();
            updatePagination();
        })
        .catch(err => console.error('Error:', err));
}

function filteredHearings() {
    const searchTerm = document.getElementById('searcherinoInput').value.toLowerCase();
    return _hearingsData.filter(h =>
        (h.HearingCategory || '').toLowerCase().includes(searchTerm) ||
        (h.HearingVenue || '').toLowerCase().includes(searchTerm) ||
        (h.HearingTypes || '').toLowerCase().includes(searchTerm) ||
        (h.HearingTypeDescriptions || '').toLowerCase().includes(searchTerm)
    );
}

function renderTable() {
    const casehearingbod = document.getElementById('casehearingbod');
    casehearingbod.innerHTML = '';

    const filtered = filteredHearings();
    const start = (currentPage - 1) * rowsPerPage;
    const paginatedData = filtered.slice(start, start + rowsPerPage);

    if (paginatedData.length === 0) {
        casehearingbod.innerHTML = `<tr><td colspan="6">No Hearings Found</td></tr>`;
        return;
    }

    paginatedData.forEach(caseData => {
        const formattedDate = caseData.HearingDate
            ? new Date(caseData.HearingDate).toLocaleDateString('en-GB')
            : 'N/A';
        const link = caseData.HearingLinks
            ? (caseData.HearingLinks.startsWith('http') ? caseData.HearingLinks : 'https://' + caseData.HearingLinks)
            : null;

        casehearingbod.innerHTML += `
            <tr>
                <td>${caseData.HearingCategory}</td>
                <td>${formattedDate} ${caseData.Time}</td>
                <td>${caseData.HearingVenue}</td>
                <td>${link ? `<a href="${link}" target="_blank"><button>Watch</button></a>` : 'N/A'}</td>
                <td>${caseData.HearingTypeDescriptions} (${caseData.HearingTypes})</td>
                <td>${caseData.Remarks}</td>
            </tr>
        `;
    });
}

function updatePagination() {
    const totalPages = Math.ceil(filteredHearings().length / rowsPerPage);
    const paginationSelect = document.getElementById('paginationSelect');
    paginationSelect.innerHTML = '';

    for (let i = 1; i <= totalPages; i++) {
        paginationSelect.innerHTML += `<option value="${i}" ${i === currentPage ? 'selected' : ''}>${i}</option>`;
    }
}
