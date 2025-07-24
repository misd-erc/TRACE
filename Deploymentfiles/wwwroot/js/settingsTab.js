document.addEventListener('DOMContentLoaded', function () {
    const tabs = document.querySelectorAll('.settings-tab .tab');
    const sections = document.querySelectorAll('.setting-card');

    function showSection(targetId) {
        sections.forEach(section => {
            section.style.display = 'none';
        });

        const targetSection = document.getElementById(targetId);
        if (targetSection) {
            targetSection.style.display = 'block';
        } else {
            console.error(`Section with ID '${targetId}' not found.`);
        }
    }

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('active'));

            tab.classList.add('active');

            const targetId = tab.getAttribute('data-tab');
            showSection(targetId);
        });
    });

    const defaultTab = document.querySelector('.settings-tab .tab[data-tab="_appearance"]');
    if (defaultTab) {
        defaultTab.classList.add('active');
        showSection('_appearance');
    }
});
