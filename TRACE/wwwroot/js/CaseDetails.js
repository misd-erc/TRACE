﻿var caseId1 = 0;
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
        GetCaseRelatedByErcID(caseId);

    } else {
        console.error('No case ID found in URL.');
    }
});

function updateBreadcrumb() {
    const breadcrumb = document.querySelector(".crumbs");
    breadcrumb.innerHTML = `<a href="/dashboard">Dashboard</a> > <a href='javascript:history.back();'>Case Management</a> > ${caseTitle} > Milestones`;
}

function showTabcase(tabName, event) {
    /*console.log("showTab triggered:", tabName, event);*/

    if (!event || !event.target) {
        /*console.warn("Missing event or event.target!");*/
        return;
    }

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
    /*console.log("Active Tab Text:", tabText);*/

    breadcrumb.innerHTML = `<a href="/dashboard">Dashboard</a> > <a href='javascript:history.back();'>Case Management</a> > ${caseTitle} > ${tabText}`;
}

let allCaseEvents = [];
let currentPage = 1;
const rowsPerPage = 10;

function fetchCaseEvent(caseId) {
    fetch(`/CaseEvent/GetCaseEventByErcID?id=${caseId}`)
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        })
        .then(result => {
            if (!result.success) {
                console.error('No events found:', result.message);
                return;
            }

            allCaseEvents = result.data || [];
            currentPage = 1;
            renderCaseEventTable();
            renderPagination();
        })
        .catch(error => {
            console.error('Error fetching case details:', error);
        });
}

function renderCaseEventTable() {
    const caseeventbody = document.getElementById('caseeventbody');
    caseeventbody.innerHTML = '';

    const start = (currentPage - 1) * rowsPerPage;
    const paginatedData = allCaseEvents.slice(start, start + rowsPerPage);

    paginatedData.forEach(event => {
        const formattedDate = event.eventDatetime
            ? new Date(event.eventDatetime).toLocaleDateString('en-GB')
            : 'N/A';

        const row = `
            <tr>
                <td data-label="BY">${event.userID || 'N/A'}</td>
                <td data-label="DESCRIPTION">${event.eventDescription || 'N/A'}</td>
                <td data-label="DATE">${formattedDate}</td>
            </tr>
        `;
        caseeventbody.innerHTML += row;
    });
}

function renderPagination() {
    const totalPages = Math.ceil(allCaseEvents.length / rowsPerPage);
    const paginationSelect = document.querySelector('#caseEventpagination select');
    const prev = document.querySelector('#caseEventpagination .prev');
    const next = document.querySelector('#caseEventpagination .next');

    paginationSelect.innerHTML = '';

    for (let i = 1; i <= totalPages; i++) {
        const option = document.createElement('option');
        option.value = i;
        option.textContent = i;
        if (i === currentPage) option.selected = true;
        paginationSelect.appendChild(option);
    }

    paginationSelect.onchange = (e) => {
        currentPage = parseInt(e.target.value);
        renderCaseEventTable();
    };

    prev.onclick = () => {
        if (currentPage > 1) {
            currentPage--;
            renderCaseEventTable();
            renderPagination();
        }
    };

    next.onclick = () => {
        if (currentPage < totalPages) {
            currentPage++;
            renderCaseEventTable();
            renderPagination();
        }
    };
}
function completeTask(id) {
    console.log("asdsad")
    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to mark this task as completed?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, complete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/CaseTask/CompleteTask/${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire(
                            'Completed!',
                            data.message,
                            'success'
                        );
                        // Optionally refresh or update UI here
                    } else {
                        Swal.fire(
                            'Error!',
                            'Task not found or cannot be completed.',
                            'error'
                        );
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    Swal.fire(
                        'Error!',
                        'Something went wrong while updating the task.',
                        'error'
                    );
                });
        }
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
            //console.log("data", result.data);

            result.data.forEach(event => {
                // Check if TargetCompletionDate exists
                if (event.actualCompletionDate) {
                    const formattedDate = event.actualCompletionDate
                        ? new Date(event.actualCompletionDate).toLocaleDateString('en-GB')
                        : 'N/A';
                    const row = `
                        <tr>
                            <td data-label="TASKED TO">${event.userId || 'N/A'}</td>
                            <td data-label="DETAILS">${event.task || 'N/A'}</td>
                            <td data-label="TARGET DATE">${formattedDate}</td>
                            <td data-label="ACTION" class="actions">
                              
                                <i class='bx bxs-x-circle' title="Pin task" ></i>
                            </td>
                        </tr>
                    `;
                    caseassignment1.innerHTML += row; // Completed Task
                } else {
                    const formattedDate = event.targetCompletionDate
                        ? new Date(event.targetCompletionDate).toLocaleDateString('en-GB')
                        : 'N/A';
                    const row = `
                        <tr>
                            <td data-label="TASKED TO">${event.userId || 'N/A'}</td>
                            <td data-label="DETAILS">${event.task || 'N/A'}</td>
                            <td data-label="TARGET DATE">${formattedDate}</td>
                            <td data-label="ACTION" class="actions">
                                <i class='bx bxs-check-circle' title="Mark as complete" onclick="completeTask(${event.caseTaskId})"></i>
                             
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
                    const formattedDate = event.DateAssigned
                        ? new Date(event.DateAssigned).toLocaleDateString('en-GB')
                        : 'N/A';
                    caseassignment.innerHTML += `
                               <tr>
                                <td data-label="OFFICER TYPE">${caseData.OfficerType}</td>
                                <td data-label="ASSIGNED PERSONEL">${caseData.UserID}</td>
                                <td data-label="DATE ASSIGNED">${formattedDate}</td>
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
                    const formattedDate = caseData.HearingDate
                        ? new Date(caseData.HearingDate).toLocaleDateString('en-GB')
                        : 'N/A';
                    casehearing.innerHTML += `
                    <tr>
                        <td data-label="HEARING CATEGORY">${caseData.HearingCategory}</td>
                        <td data-label="DATE AND TIME">${formattedDate} ${caseData.Time}</td>
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
            if (event.length > 0) {
              
                event.forEach(event => {
                    const caseData = event; 
                    const formattedDate = caseData.datetimeCreated
                        ? new Date(caseData.datetimeCreated).toLocaleDateString('en-GB')
                        : 'N/A';
                    casehearing.innerHTML += `
                            <tr >
                                <td data-label="NotedBy">${caseData.notedBy}</td>
                                <td data-label="DESCRIPTION">${caseData.notes}</td>
                                <td data-label="DATE">${formattedDate}</td>
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
function deleteCaseAssignment(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/CaseAssignment/Delete/${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        // Optionally remove the row or refresh
                    } else {
                        Swal.fire(
                            'Error!',
                            'Something went wrong.',
                            'error'
                        );
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    Swal.fire(
                        'Error!',
                        'Something went wrong while deleting.',
                        'error'
                    );
                });
        }
    });
}


function GetCaseRelatedByErcID(caseId) {
    fetch(`/RelatedCase/GetCaseRelatedByErcID?id=${caseId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(event => {
            const casehearing = document.getElementById('caserelated');
            casehearing.innerHTML = '';
            //console.log("asd",event)
            if (event.length > 0) {
                event.forEach(event => {
                    const caseData = event; 
               
                    casehearing.innerHTML += `
                                <tr>
                                    <td data-label="CASE NO.">${caseData.RelatedCaseNo}</td>
                                    <td data-label="CASE TITLE"><a href='/CaseDetails?id=${caseData.RelatedCaseID}'>${caseData.RelatedCaseTitle}</a></td>
                                    <td data-label="ACTION" class="actions">
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

function formatDate(dateStr) {
    return dateStr ? new Date(dateStr).toLocaleDateString('en-GB') : 'N/A';
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
                const othercaseDetailsDiv = document.getElementById('othercaseDetails');
                const caseage = document.getElementById('caseage');
                caseage.innerHTML = calculateDays(caseData.DateFiled) + " Days";

                // Format all date fields
                const formattedDateFiled = formatDate(caseData.DateFiled);
                const formattedDateDocketed = formatDate(caseData.DateDocketed);
                const formattedDateApproved = formatDate(caseData.DatetimeApproved);
                const formattedPADeliberation = formatDate(caseData.PADeliberationDate);
                const formattedFADeliberation = formatDate(caseData.FADeliberationDate);
                const formattedPATargetOrder = formatDate(caseData.PATargetOrder);
                const formattedFATargetOrder = formatDate(caseData.FATargetOrder);
                const formattedSubmittedResolution = formatDate(caseData.SubmittedForResolution);
                const formattedMeterSIN = formatDate(caseData.MeterSIN);
                const formattedTargetFAIssuance = formatDate(caseData.TargetFAIssuance);
                const formattedTargetPAIssuance = formatDate(caseData.TargetPAIssuance);

                caseDetailsDiv.innerHTML = `
            <div>
                <span><strong>ERC Case No.: </strong> <i>${caseData.CaseNo}</i></span>
                <span><strong>Case Title: </strong> <i>${caseData.Title}</i></span>
                <span><strong>Case Category: </strong> <i>${caseData.CaseCategory}</i></span>
            </div>
            <div>
                <span><strong>Case Nature: </strong> <i>${caseData.CaseNature}</i></span>
                <span><strong>Date Filed: </strong> <i>${formattedDateFiled}</i></span>
                <span><strong>Date Docketed: </strong> <i>${formattedDateDocketed}</i></span>
            </div>
            <div>
                <span><strong>Docketed By: </strong> <i>${caseData.DocketedBy ?? 'N/A'}</i></span>
                <span><strong>Applicant: </strong> <i>${caseData.CompanyName ?? 'N/A'}</i></span>
                <span><strong>Respondent: </strong> <i>${caseData.CorrespondentName ?? 'No Data Yet'}</i></span>
            </div>
            <div>
                <span><strong>No. of Folders: </strong> <i>${caseData.NoOfFolders ?? 'N/A'}</i></span>
                <span><strong>Date Approved: </strong> <i>${formattedDateApproved}</i></span>
                <span><strong class="green-txt">${caseData.CaseStatus}</strong></span>
            </div>
        `;

                othercaseDetailsDiv.innerHTML = `
            <div>
                <span><strong>Amount Claimed: </strong> <i>${caseData.AmountClaimed ?? 'N/A'}</i></span>
                <span><strong>Amount Settled: </strong> <i>${caseData.AmountSettled ?? 'N/A'}</i></span>
                <span><strong>Target PA Issuance: </strong> <i>${formattedTargetPAIssuance ?? 'N/A'}</i></span>
            </div>
            <div>
                <span><strong>Target FA Issuance: </strong> <i>${formattedTargetFAIssuance ?? 'N/A'}</i></span>
                <span><strong>PA Deliberation Date: </strong> <i>${formattedPADeliberation}</i></span>
                <span><strong>FA Deliberation Date: </strong> <i>${formattedFADeliberation}</i></span>
            </div>
            <div>
                <span><strong>PA Target Order: </strong> <i>${formattedPATargetOrder}</i></span>
                <span><strong>FA Target Order: </strong> <i>${formattedFATargetOrder}</i></span>
                <span><strong>Synopsis: </strong> <i>${caseData.Synopsis ?? 'No Synopsis Yet'}</i></span>
            </div>
            <div>
                <span><strong>Submitted for Resolution: </strong> <i>${formattedSubmittedResolution}</i></span>
                <span><strong>Meter SIN: </strong> <i>${caseData.MeterSIN ?? 'N/A'}</i></span>
                <span><strong>Remarks: </strong> <i>${caseData.Remarks ?? 'None'}</i></span>
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
            //console.log(isAchieved);

            const isFirst = index === 0;
            const isLast = index === data.length - 1;

            const row = isAchieved
                ? `
            <div class="step completed">
                ${!isFirst ? '<div class="progress-line active"></div>' : ''} 
                <div class="circle">✔</div>
                <div class="label">${item.Milestone.replace("/", "<wbr>/")}</div>
                ${!isLast ? '<div class="progress-line active"></div>' : ''}
            </div>
        `
                        : `
            <div class="step">
                ${!isFirst ? '<div class="progress-line"></div>' : ''}
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
    //console.log(dateFiled)
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

    //console.log(`Days since filed: ${daysDifference} days`);
    return daysDifference;
}

function toggleCaseDetails() {
    const detailsDiv = document.getElementById("othercaseDetails");
    const arrowIcon = document.getElementById("arrowIcon");

    if (detailsDiv.style.display === "none" || detailsDiv.style.display === "") {
        detailsDiv.style.display = "grid";
        arrowIcon.style.transform = "rotate(180deg)";
    } else {
        detailsDiv.style.display = "none";
        arrowIcon.style.transform = "rotate(0deg)";
    }
}