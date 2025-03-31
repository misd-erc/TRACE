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
        fetchCaseTaskWithErcId(caseId);
        fetchCaseNoteWithErcId(caseId);

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

function fetchCaseTaskWithErcId(caseId) {
    const caseassignment = document.getElementById('pendingtask');
    const caseassignment1 = document.getElementById('completedTask');

    // Clear the existing content
    caseassignment.innerHTML = '';
    caseassignment1.innerHTML = '';

    fetch(`/CaseTask/GetCaseTaskByErcID?id=${caseId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            return response.json();
        })
        .then(result => {
            if (!result.success || !result.data || result.data.length === 0) {
                caseassignment.innerHTML = `<tr><td colspan="4">No Pending Tasks Found</td></tr>`;
                caseassignment1.innerHTML = `<tr><td colspan="4">No Completed Tasks Found</td></tr>`;
                return;
            }
            console.log("data", result.data);

            result.data.forEach(event => {
             
               

                // Check if TargetCompletionDate exists
                if (event.actualCompletionDate) {
                    const row = `
                        <tr>
                            <td data-label="TASKED TO">${event.userId || 'N/A'}</td>
                            <td data-label="DETAILS">${event.task || 'N/A'}</td>
                            <td data-label="TARGET DATE">${event.targetCompletionDate || 'N/A'}</td>
                            <td data-label="ACTION" class="actions">
                              
                                <i class='bx bxs-x-circle' title="Pin task"></i>
                            </td>
                        </tr>
                    `;
                    caseassignment1.innerHTML += row; // Completed Task
                } else {
                    const row = `
                        <tr>
                            <td data-label="TASKED TO">${event.userId || 'N/A'}</td>
                            <td data-label="DETAILS">${event.task || 'N/A'}</td>
                            <td data-label="TARGET DATE">${event.targetCompletionDate || 'N/A'}</td>
                            <td data-label="ACTION" class="actions">
                                <i class='bx bxs-check-circle' title="Mark as complete"></i>
                             
                            </td>
                        </tr>
                    `;
                    caseassignment.innerHTML += row; // Pending Task
                }
            });
        })
        .catch(error => {
            console.error('Error fetching case details:', error);
            caseassignment.innerHTML = `<tr><td colspan="4">Error fetching data</td></tr>`;
            caseassignment1.innerHTML = `<tr><td colspan="4">Error fetching data</td></tr>`;
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
            const caseassignment = document.getElementById('caseassignment');
            caseassignment.innerHTML = '';
            if (data.length > 0) {
                const caseData = data[0];
                data.forEach(event => {
                    caseassignment.innerHTML += `
                               <tr>
                                <td data-label="OFFICER TYPE">${caseData.OfficerType}</td>
                                <td data-label="ASSIGNED PERSONEL">${caseData.UserID}</td>
                                <td data-label="DATE ASSIGNED">${caseData.DateAssigned}</td>
                                <td data-label="ASSIGNED BY">${caseData.AssignedBy}</td>
                                <td data-label="ACTION" class="actions">
                                <i class='bx bxs-x-circle' title="Archive"></i>
                            </td>
                        </tr>
                        `;
                })
                // Update values dynamically
               
               
            } else {
                caseassignment.innerHTML = `
                            <tr>
                                  <td colspan="5">No Case Assignment</td>
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
            const casehearing = document.getElementById('casehearing');
            casehearing.innerHTML = '';
            if (data.length > 0) {
                data.forEach(event => {
                    const caseData = event;
                    casehearing.innerHTML += `
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
                })
             

                // Update values dynamically
              
            
            } else {
                casehearing.innerHTML = `
                                <tr>
                                    <td colspan="6">No Hearings Found</td>
                                </tr>
                            `;
            }

        })
        .catch(error => {
            console.error('Error fetching case details:', error);
        });
}
function fetchCaseNoteWithErcId(caseId) {
    fetch(`/CaseNote/GetCaseNoteByErcID?id=${caseId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(event => {
            const casehearing = document.getElementById('casenote');
            casehearing.innerHTML = '';
            if (event.data.length > 0) {
                event.data.forEach(event => {
                    const caseData = event; 
               
                    casehearing.innerHTML += `
                                       <tr >
                                            <td data-label="NotedBy">${caseData.notedBy}</td>
                                            <td data-label="DESCRIPTION">${caseData.notes}</td>
                                            <td data-label="DATE">${caseData.datetimeCreated}</td>
                                            <td data-label="ACTION" class="actions">
                                                <i class='bx bxs-edit' title="Edit" onclick="CasenoteEdit(${caseData.caseNoteId})"></i>
                                                <i class='bx bxs-x-circle' title="Archive"></i>
                                            </td>
                                        </tr>
                            `;
                })
               

                // Update values dynamically
              
            } else {
                casehearing.innerHTML = `
                                <tr>
                                    <td colspan="6">No Data Found</td>
                                </tr>
                            `;
            }

        })
        .catch(error => {
            console.error('Error fetching case details:', error);
        });
}
function CasenoteEdit(caseNoteID) {
    window.location.href = `/casenote/edit?id=${caseNoteID}`;
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
                const caseage = document.getElementById('caseage');
                caseage.innerHTML = calculateDays(caseData.DateFiled) + "Days";
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
        const currentmilestone = document.getElementById('currentmilestoneage');
        if (data.length > 0) {
            currentmilestone.innerHTML = calculateDays1(data[0].DatetimeAchieved) + "Days";
        }
       
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

function calculateDays(dateFiled) {
    const filedDate = new Date(dateFiled);
    const currentDate = new Date();

    // Calculate the difference in milliseconds
    const timeDifference = currentDate - filedDate;

    // Convert milliseconds to days
    const daysDifference = Math.floor(timeDifference / (1000 * 60 * 60 * 24));

    return daysDifference;
}

function calculateDays1(dateFiled) {
    console.log(dateFiled)
    const filedDate = new Date(dateFiled);

    if (isNaN(filedDate.getTime())) {
        console.error('Invalid date format');
        return;
    }

    const currentDate = new Date();

    // Calculate the difference in milliseconds
    const timeDifference = currentDate.getTime() - filedDate.getTime();

    // Convert milliseconds to days
    const daysDifference = Math.floor(timeDifference / (1000 * 60 * 60 * 24));

    console.log(`Days since filed: ${daysDifference} days`);
    return daysDifference;
}