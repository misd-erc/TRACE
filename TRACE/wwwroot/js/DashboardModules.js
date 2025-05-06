let _hearingsData = [];
let _allmycases = [];
let currentPagedmc = 1;
const casesPerPagedmc = 4;

document.addEventListener('DOMContentLoaded', function () {
    dashboard_fetchAllMyCases();
    dashboard_fetchAllCaseHearings();

    document.getElementById('paginationSelectmyCases').addEventListener('change', function () {
        currentPagedmc = parseInt(this.value);
        render_dashboard_mycasesTable();
    });

    document.getElementById('mcprevPage').addEventListener('click', function () {
        if (currentPagedmc > 1) {
            currentPagedmc--;
            render_dashboard_mycasesTable();
        }
    });

    document.getElementById('mcnextPage').addEventListener('click', function () {
        const totalPages = Math.ceil(_allmycases.length / casesPerPagedmc);
        if (currentPagedmc < totalPages) {
            currentPagedmc++;
            render_dashboard_mycasesTable();
        }
    });
});

function dashboard_fetchAllCaseHearings() {
    fetch(`/Hearings/GetHearing`)
        .then(res => res.json())
        .then(data => {
            _hearingsData = data;
            console.log("Hearing data:", data);
            render_dashboard_hearingTable();
        })
        .catch(err => console.error('Error:', err));
}
function dashboard_fetchAllMyCases() {
    fetch(`/Erccase/GetAllMyCasesDashboard`)
        .then(res => {
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            return res.json();
        })
        .then(data => {
            _allmycases = Array.isArray(data) ? data : [];
            setupPagination();
            render_dashboard_mycasesTable();
        })
        .catch(err => console.error('Error fetching my cases:', err));
}

function setupPagination() {
    const totalPages = Math.ceil(_allmycases.length / casesPerPagedmc);
    const paginationSelect = document.getElementById('paginationSelectmyCases');
    paginationSelect.innerHTML = '';

    for (let i = 1; i <= totalPages; i++) {
        const option = document.createElement('option');
        option.value = i;
        option.textContent = i;
        paginationSelect.appendChild(option);
    }

    paginationSelect.value = currentPagedmc;
}
function render_dashboard_hearingTable() {
    const casehearingbod = document.getElementById('dashboardhearings');
    casehearingbod.innerHTML = '';

    if (_hearingsData.length === 0) {
        casehearingbod.innerHTML = `<i>No Upcomming Hearings Found</i>`;
        return;
    }

    _hearingsData.forEach(caseData => {
        const formattedDate = caseData.HearingDate
            ? new Date(caseData.HearingDate).toLocaleDateString('en-GB')
            : 'N/A';

        const link = caseData.HearingLinks
            ? (caseData.HearingLinks.startsWith('http') ? caseData.HearingLinks : 'https://' + caseData.HearingLinks)
            : null;

        // Conditionally render the icon if a valid link exists
        const streamIcon = link
            ? `<a href="${link}"><i class='bx bx-show-alt' title='Watch virtual hearing'></i></a>`
            : '';

        casehearingbod.innerHTML += `
            <li class="flex h-center space-between">
                <div>
                    <strong>${caseData.CaseNo} | ${caseData.HearingTypes} ${streamIcon}</strong><br />
                    <span class="venue">${caseData.HearingVenue}</span>
                </div>
                <div>
                    <span class="date">${formattedDate}</span><br />
                    <span class="time">${caseData.Time}</span>
                </div>
            </li>
        `;
    });
}
function render_dashboard_mycasesTable() {
    const mycasebod = document.getElementById('mydashbboardcases');
    mycasebod.innerHTML = '';

    if (_allmycases.length === 0) {
        mycasebod.innerHTML = `<i>No Cases assigned to you found.</i>`;
        return;
    }

    const start = (currentPagedmc - 1) * casesPerPagedmc;
    const end = start + casesPerPagedmc;
    const casesToRender = _allmycases.slice(start, end);

    casesToRender.forEach(mycaseData => {
        const formattedDate = mycaseData.DatetimeAchieved
            ? new Date(mycaseData.DatetimeAchieved).toLocaleDateString('en-GB')
            : 'N/A';
        const fullTitle = mycaseData.Title || '';
        const truncatedTitle = fullTitle.length > 40 ? fullTitle.substring(0, 40) + '...' : fullTitle;

        mycasebod.innerHTML += `
            <li class="flex h-center space-between mycase-clickable" onclick="location.href='/CaseDetails?id=${mycaseData.ERCCaseID}'">
                <div>
                    <strong>${mycaseData.CaseNo}</strong><br />
                    <span class="type">${mycaseData.Nature}</span>
                </div>
                <div>
                    <span class="date" title="${fullTitle}">${truncatedTitle}</span><br />
                    <span class="status">${mycaseData.CaseStatus}</span>
                </div>
                <div>
                    <span class="milestone">${mycaseData.MilestoneDescription}</span><br />
                    <span class="milestonedate">${formattedDate}</span>
                </div>
            </li>
        `;
    });

    setupPagination(); // Update the dropdown in case of page change
}