document.addEventListener('DOMContentLoaded', function () {
    fetchAllCaseHearings();
});

function fetchAllCaseHearings() {
    fetch(`/Hearings/GetHearing`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const casehearingbod = document.getElementById('casehearingbod');
            casehearingbod.innerHTML = '';
            if (data.length > 0) {
                data.forEach(event => {
                    const caseData = event;
                    const formattedDate = caseData.HearingDate
                        ? new Date(caseData.HearingDate).toLocaleDateString('en-GB')
                        : 'N/A';

                    const hearingTypes = caseData.HearingTypes || 'N/A';
                    const hearingTypeDescriptions = caseData.HearingTypeDescriptions || 'N/A';

                    casehearingbod.innerHTML += `
                    <tr>
                        <td data-label="HEARING CATEGORY">${caseData.HearingCategory}</td>
                        <td data-label="DATE AND TIME">${formattedDate} ${caseData.Time}</td>
                        <td data-label="VENUE">${caseData.HearingVenue}</td>
                        <td data-label="LINK">
                              ${caseData.HearingLinks
                            ? (() => {
                                const link = caseData.HearingLinks.startsWith('http://') || caseData.HearingLinks.startsWith('https://')
                                    ? caseData.HearingLinks
                                    : 'https://' + caseData.HearingLinks;
                                return `<a href="${link}" target="_blank" rel="noopener noreferrer"><button>Watch</button></a>`;
                            })()
                            : 'N/A'
                        }
                            </td>

                        <td data-label="TYPE">${hearingTypeDescriptions} (${hearingTypes})</td>
                        <td data-label="REMARKS">${caseData.Remarks}</td>
                    </tr>
                    `;
                });
            } else {
                casehearingbod.innerHTML = `
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