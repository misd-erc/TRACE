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
    breadcrumb.innerHTML = `<a href="/dashboard">Dashboard</a> > Case Management (My Cases) > <a href="/letterofcomplaints">Letter Of Complaints</a> > Case Title > ${tabText}`;
}