let _hearingsData = [];
let _allmycases = [];
let _totalcases = [];
let currentPagedmc = 1;
const casesPerPagedmc = 4;

document.addEventListener('DOMContentLoaded', function () {
    dashboard_fetchcountofunreadnotifhehe();
    dashboard_fetchAllMyCases();
    dashboard_fetchAllCaseHearings();
    dashboard_fetchTotalCountCases();
    updateDateTime();
    setInterval(updateDateTime, 60000);

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

    document.getElementById('dbmcSearchInput').addEventListener('input', function () {
        currentPagedmc = 1;
        render_dashboard_mycasesTable();
    });
});


function dashboard_fetchcountofunreadnotifhehe() {
    fetch('/Notification/GetUnreadNotificationCount')
        .then(response => response.json())
        .then(data => {
            const count = data.count;
            const unreadLink = document.getElementById('unreadCountLink');
            if (unreadLink) {
                unreadLink.textContent = `${count} unread`;
            } else {
                console.error("Element with ID 'unreadCountLink' not found.");
            }
        })
        .catch(error => {
            console.error("Failed to load unread notifications count:", error);
        });
}

function dashboard_fetchAllCaseHearings() {
    fetch(`/Hearings/GetHearing`)
        .then(res => res.json())
        .then(data => {
            _hearingsData = data;
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
function dashboard_fetchTotalCountCases() {
    fetch(`/Erccase/GetTotalCasesForEachCard`)
        .then(res => {
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            return res.json();
        })
        .then(data => {
            _totalcases = Array.isArray(data) ? data : [];
            render_dashboard_totalcards(_totalcases);
        })
        .catch(err => console.error('Error fetching my cases:', err));
}
function setupPagination(totalItems = _allmycases.length) {
    const totalPages = Math.ceil(totalItems / casesPerPagedmc);
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
function render_dashboard_hearingTable(data = _hearingsData) {
    const casehearingbod = document.getElementById('dashboardhearings');
    casehearingbod.innerHTML = '';

    if (!data || data.length === 0) {
        casehearingbod.innerHTML = `<i>No Hearings Found in Selected Date Range</i>`;
        return;
    }

    data.slice(0, 4).forEach(caseData => {
        const formattedDate = caseData.HearingDate
            ? new Date(caseData.HearingDate).toLocaleDateString('en-GB')
            : 'N/A';

        const link = caseData.HearingLinks
            ? (caseData.HearingLinks.startsWith('http') ? caseData.HearingLinks : 'https://' + caseData.HearingLinks)
            : null;

        const streamIcon = link
            ? `<a href="${link}"><i class='bx bx-show-alt' title='Watch virtual hearing'></i></a>`
            : '';

        casehearingbod.innerHTML += `
                <li class="flex space-between">
                    <div>
                        <strong>${caseData.CaseNo} | ${caseData.HearingTypes} ${streamIcon}</strong><br />
                        <span class="venue">${caseData.HearingVenue}</span><br />
                        <i class="helper">${caseData.Remarks}</i>
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

    const start = (currentPagedmc - 1) * casesPerPagedmc;
    const end = start + casesPerPagedmc;
    const searchInput = document.getElementById('dbmcSearchInput').value.toLowerCase();

    const filteredCases = _allmycases.filter(mycase => {
        const caseNo = mycase.CaseNo?.toLowerCase() || '';
        const title = mycase.Title?.toLowerCase() || '';
        return caseNo.includes(searchInput) || title.includes(searchInput);
    });

    const totalPages = Math.ceil(filteredCases.length / casesPerPagedmc);
    const paginatedCases = filteredCases.slice((currentPagedmc - 1) * casesPerPagedmc, currentPagedmc * casesPerPagedmc);
    setupPagination(filteredCases.length);

    if (filteredCases.length === 0) {
        mycasebod.innerHTML = `<i>No Cases assigned to you found.</i>`;
        return;
    }

    paginatedCases.forEach(mycaseData => {
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
}
function render_dashboard_totalcards(data) {
    let tooltip = document.querySelector('.tooltip');
    if (!tooltip) {
        tooltip = document.createElement('div');
        tooltip.classList.add('tooltip');
        document.body.appendChild(tooltip);
    }

    data.forEach(item => {
        const formattedTotalCases = item.TotalCases;
        let shortVersion = formattedTotalCases.toLocaleString();

        if (formattedTotalCases >= 1000 && formattedTotalCases < 1000000) {
            shortVersion = (formattedTotalCases / 1000).toFixed(1) + 'K';
        } else if (formattedTotalCases >= 1000000) {
            shortVersion = (formattedTotalCases / 1000000).toFixed(1) + 'M';
        }

        let selector = '';

        switch (item.GroupedStatus) {
            case 'Pending / Ongoing':
                selector = '.purple-bg .count';
                break;
            case 'Submitted for Resolution':
                selector = '.orange-bg .count';
                break;
            case 'Promulgated / Decided':
                selector = '.blue-bg .count';
                break;
            case 'Decided with MR':
                selector = '.green-bg .count';
                break;
            case 'Closed / Dismissed':
                selector = '.red-bg .count';
                break;
        }

        const cardElement = document.querySelector(selector);

        if (cardElement) {
            cardElement.textContent = shortVersion;

            cardElement.addEventListener('mouseenter', function () {
                tooltip.textContent = formattedTotalCases.toLocaleString();
                tooltip.style.display = 'block';
            });

            cardElement.addEventListener('mousemove', function (e) {
                tooltip.style.left = (e.pageX + 10) + 'px';
                tooltip.style.top = (e.pageY + 10) + 'px';
            });

            cardElement.addEventListener('mouseleave', function () {
                tooltip.style.display = 'none';
            });
        }
    });
}

function updateDateTime() {
    const dateElement = document.querySelector('.date-today');
    const now = new Date();

    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    const dayName = days[now.getDay()];

    const options = { day: '2-digit', month: 'long', year: 'numeric' };
    const formattedDate = now.toLocaleDateString('en-GB', options);

    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    const militaryTime = `${hours}:${minutes}`;

    dateElement.textContent = `${dayName}, ${formattedDate} ${militaryTime}`;
}

document.querySelector('[name="hearingfilterino"]').addEventListener('click', function () {
    const startDate = document.querySelector('[name="db_hearingfrom"]').value;
    const endDate = document.querySelector('[name="db_hearingto"]').value;

    let filteredData = _hearingsData;

    if (startDate && endDate) {
        const start = new Date(startDate);
        const end = new Date(endDate);

        end.setHours(23, 59, 59, 999);

        filteredData = _hearingsData.filter(item => {
            const hearingDate = new Date(item.HearingDate);
            return hearingDate >= start && hearingDate <= end;
        });
    }

    render_dashboard_hearingTable(filteredData);
});
