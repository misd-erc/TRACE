var caseId1 = 0;
let caseTitle = "";

let tableDataMap = {};
const itemsPerPage = 5;

; document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const caseId = urlParams.get('id');
    caseid1 = caseId;
    if (caseId) {
        fetchCaseDetails(caseId);
        fetchCaseEvent(caseId);
        fetchCaseAssignmentWithErcId(caseId);
        fetchCaseHearingWithErcId(caseId);

    } else {
        console.error('No case ID found in URL.');
    }
});

function updateBreadcrumb() {
    const breadcrumb = document.querySelector(".crumbs");
    breadcrumb.innerHTML = `<a href="/dashboard">Dashboard</a> > <a href='javascript:history.back();'>Case Management</a> > ${caseTitle}`;
}

function showTab(tabName) {
    document.querySelectorAll('.cms-content').forEach(function (content) {
        content.classList.remove('active');
    });

    document.querySelectorAll('.cms-nav li').forEach(function (navItem) {
        navItem.classList.remove('active');
    });

    document.getElementById(tabName).classList.add('active');
    event.target.classList.add('active');


    const breadcrumb = document.querySelector(".crumbs");
    const tabText = event.target.innerText.trim();
    breadcrumb.innerHTML = `<a href="/dashboard">Dashboard</a> > <a href='javascript:history.back();'>Case Management</a> > ${caseTitle} > ${tabText}`;
}

function fetchCaseEvent(caseId) {
    fetch(`/CaseEvent/GetCaseEventByErcID?id=${caseId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(result => {
            if (!result.success) {
                console.error('No events found:', result.message);
                return;
            }

            const data = result.data;
            const caseeventbody = document.getElementById('caseeventbody');
            if (!caseeventbody) {
                console.error('Table body not found');
                return;
            }

            caseeventbody.innerHTML = '';

            data.forEach(event => {

                const row = `
                        <tr>
                            <td data-label="BY">${event.userID || 'N/A'}</td>
                            <td data-label="DESCRIPTION">${event.eventDescription || 'N/A'}</td>
                            <td data-label="DATE">${event.eventDatetime || 'N/A'}</td>
                        </tr>
                    `;
                caseeventbody.innerHTML += row;
            });
        })
        .catch(error => {
            console.error('Error fetching case details:', error);
        });
}

function fetchCaseAssignmentWithErcId(caseId) {
    fetch(`/CaseAssignment/GetCaseAssignmentByErcID?id=${caseId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {

            if (data.length > 0) {
                const caseData = data[0];

                // Update values dynamically
                const caseassignment = document.getElementById('caseassignment');
                caseassignment.innerHTML = `
                               <tr>
                                <td data-label="OFFICER TYPE">${caseData.OfficerType}</td>
                            <td data-label="ASSIGNED PERSONEL">amqcadiente</td>
                            <td data-label="DATE ASSIGNED">03/09/2016</td>
                            <td data-label="ASSIGNED BY">agacosta</td>
                            <td data-label="ACTION" class="actions">
                                <i class='bx bxs-x-circle' title="Archive"></i>
                            </td>
                        </tr>
                        `;
            } else {
                caseassignment.innerHTML = `
                            <tr>
                                  <td >Data Not Found</td>
                            </tr>
                        `;
            }

        })
        .catch(error => {
            console.error('Error fetching case details:', error);
        });
}

function fetchCaseHearingWithErcId(caseId) {
    fetch(`/Hearing/GetHearingByErcID?id=${caseId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {

            if (data.length > 0) {
                const caseData = data[0];

                // Update values dynamically
                const casehearing = document.getElementById('casehearing');
                casehearing.innerHTML = `
                                        <tr>
                                                <td data-label="HEARING CATEGORY">${caseData.HearingCategory}</td>
                                                        <td data-label="DATE AND TIME">${caseData.HearingDate} ${caseData.Time}</td>
                                                    <td data-label="VENUE">${caseData.HearingVenue}</td>
                                                            <td data-label="TYPE">${caseData.HearingTypeDescription} (${caseData.HearingType})</td>
                                                    <td data-label="REMARKS">${caseData.Remarks}</td>
                                            <td data-label="ACTION" class="actions">
                                                <i class='bx bxs-edit' title="Edit"></i>
                                                <i class='bx bxs-x-circle' title="Archive"></i>
                                            </td>
                                        </tr>
                            `;
            } else {
                casehearing.innerHTML = `
                                <tr>
                                      <td >Data Not Found</td>
                                </tr>
                            `;
            }

        })
        .catch(error => {
            console.error('Error fetching case details:', error);
        });
}

function fetchCaseDetails(caseId) {
    fetch(`/CaseDetails/GetCaseDetails?id=${caseId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (data.length > 0) {
                const caseData = data[0];
                caseTitle = caseData.Title;
                updateBreadcrumb();
                displayCaseMilestone(caseData.CaseCategoryID);
                const caseDetailsDiv = document.getElementById('caseDetails');
                caseDetailsDiv.innerHTML = `
                      <div>
                          <span><strong>ERC Case No.: </strong> <i>${caseData.CaseNo}</i></span>
                          <span><strong>Case Title: </strong> <i>${caseData.Title}</i></span>
                          <span><strong>Case Category: </strong> <i>${caseData.Category}</i></span>
                      </div>
                      <div>
                          <span><strong>Case Nature: </strong> <i>${caseData.CaseNature}</i></span>
                          <span><strong>Date Filed: </strong> <i>${caseData.DateFiled}</i></span>
                          <span><strong>Date Docketed: </strong> <i>${caseData.DateDocketed}</i></span>
                      </div>
                      <div>
                              <span><strong>Docketed By: </strong> <i>${caseData.DocketedBy ?? 'N/A'}</i></span>
                                  <span><strong>Applicant: </strong> <i>${caseData.CompanyName ?? 'N/A'}</i></span>
                              <span><strong>Respondent: </strong> <i>${caseData.CorrespondentName ?? 'No Data Yet'}</i></span>
                      </div>
                      <div>
                          <span><strong>No. of Folders: </strong> <i>${caseData.NoOfFolders ?? 'N/A'}</i></span>
                          <span><strong>Date Approved: </strong> <i>${caseData.DatetimeApproved ?? 'N/A'}</i></span>
                          <span><strong class="green-txt">${caseData.CaseStatus}</strong></span>
                      </div>
                    `;
            } else {
                alert('Case not found!');
            }
        })
        .catch(error => {
            console.error('Error fetching case details:', error);
        });
}

async function milestoneIsAchieved(milestoneId) {
    const urlParams = new URLSearchParams(window.location.search);
    const caseId = urlParams.get('id');
    try {
        const response = await fetch(`/CaseMilestone/CheckMilestoneIsAchieved?erccaseId=${caseId}&casemilestoneId=${milestoneId}`);

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        return data.length > 0;
    } catch (error) {
        console.error('Error fetching milestone data:', error);
        return false;
    }
}

async function displayCaseMilestone(caseId) {
    try {
        const response = await fetch(`/CaseMilestone/GetMilestoneOfCases?id=${caseId}`);

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();
        const tableBody = document.getElementById('casemilestone');
        if (!tableBody) {
            console.error('Table body not found.');
            return;
        }

        tableBody.innerHTML = '';

        for (const [index, item] of data.entries()) {
            const isAchieved = await milestoneIsAchieved(item.CaseMilestoneID);
            console.log(isAchieved);

            const isLast = index === data.length - 1;

            const row = isAchieved
                ? `
            <div class="step completed">
                <div class="circle">✔</div>
                <div class="label">${item.Milestone.replace("/", "<wbr>/")}</div>
                ${!isLast ? '<div class="progress-line active"></div>' : ''}
            </div>
        `
                : `
            <div class="step">
                <div class="progress-line"></div>
                <div class="circle"></div>
                <div class="label">${item.Milestone.replace("/", "<wbr>/")}</div>
                ${!isLast ? '<div class="progress-line"></div>' : ''}
            </div>
        `;

            tableBody.innerHTML += row;
        }
    } catch (error) {
        console.error('Error fetching case details:', error);
    }
}