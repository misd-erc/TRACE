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
document.addEventListener('DOMContentLoaded', function () {
    fetchUsers();
  
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
            const casehearing = document.getElementById('userdata');
           

            // Check for success and data availability
            if (result.success && Array.isArray(result.data) && result.data.length > 0) {
                casehearing.innerHTML = '';
                result.data.forEach(user => {
                    casehearing.innerHTML += `
                    <tr>
                        <td data-label="Full Name">${user.fullname || 'N/A'}</td>
                        <td data-label="Email Address">${user.email || 'N/A'}</td>
                        <td data-label="Department">${user.department || 'N/A'}</td>
                        <td data-label="Action">
                            <a href="/Users/Edit/${user.id}">
                                <button title="Manage role for this user">Manage</button>
                            </a>
                        </td>
                    </tr>
                `;
                });
            } else {
                casehearing.innerHTML = `
                <tr>
                    <td colspan="5">No Data Found</td>
                </tr>
            `;
            }
        })
        .catch(error => {
            console.error('Error fetching user data:', error);
            document.getElementById('userdata').innerHTML = `
            <tr>
                <td colspan="5">Failed to fetch data. Please try again later.</td>
            </tr>
        `;
        });


}
