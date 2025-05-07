const ctx = document.getElementById('categoryLineChart').getContext('2d');

const primaryColor = localStorage.getItem('primaryColor') || '#007bff';
const selectedTheme = localStorage.getItem('theme') || 'light';

let fontColor = (selectedTheme === "dark") ? "#fff" : "#444";

fetch('/ERCCase/GetCountForEachCategory')
    .then(response => response.json())
    .then(data => {
        const labels = data.map(item => item.Category);
        const values = data.map(item => item.TotalCases);

        const chartData = {
            labels: labels,
            datasets: [{
                label: 'Cases Filed',
                data: values,
                borderColor: primaryColor,
                backgroundColor: 'rgba(255, 51, 102, 0.2)',
                pointBackgroundColor: '#ffffff',
                pointBorderColor: primaryColor,
                pointRadius: 5,
                pointHoverRadius: 7,
                tension: 0
            }]
        };

        const config = {
            type: 'line',
            data: chartData,
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        display: false,
                        labels: {
                            color: '#fff'
                        }
                    },
                    tooltip: {
                        titleColor: '#fff',
                        bodyColor: '#fff',
                        callbacks: {
                            label: function (context) {
                                return `Cases: ${context.raw}`;
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        ticks: {
                            color: fontColor
                        },
                        grid: {
                            color: selectedTheme === "dark" ? "#444" : "#ddd"
                        }
                    },
                    y: {
                        ticks: {
                            color: fontColor
                        },
                        grid: {
                            color: selectedTheme === "dark" ? "#444" : "#ddd"
                        }
                    }
                }
            }
        };

        new Chart(ctx, config);
    })
    .catch(error => {
        console.error("Error fetching chart data:", error);
    });
