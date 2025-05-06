let _hearingsData = [];
let _allmycases = [];

document.addEventListener('DOMContentLoaded', function () {
    dashboard_fetchAllCaseHearings();
    dashboard_fetchAllMyCases();
});

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
            console.log("Received data for _allmycases:", data);
            _allmycases = Array.isArray(data) ? data : [];  // Safe fallback
            render_dashboard_mycasesTable();
        })
        .catch(err => console.error('Error fetching my cases:', err));
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

    _allmycases.forEach(mycaseData => {
        const formattedDate = mycaseData.DatetimeAchieved
            ? new Date(mycaseData.DatetimeAchieved).toLocaleDateString('en-GB')
            : 'N/A';
        const fullTitle = mycaseData.Title || '';
        const truncatedTitle = fullTitle.length > 40 ? fullTitle.substring(0, 40) + '...' : fullTitle;

        mycasebod.innerHTML += `
            <li class="flex h-center space-between mycase-clickable" onclick="location.href='/CaseDetails?id=${mycaseData.ERCCaseID}'", "CaseManagement")'">
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