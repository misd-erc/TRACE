﻿const ctx = document.getElementById('categoryLineChart').getContext('2d');
const primaryColor = localStorage.getItem('primaryColor') || '#007bff';
const selectedTheme = localStorage.getItem('theme') || 'light';
let fontColor = (selectedTheme === "dark") ? "#fff" : "#444";

let categoryChart;

function loadChartData(filter = '') {
    const filterToUse = (filter === 'Alltime') ? '' : filter;

    fetch(`/ERCCase/GetCountForEachCategory?filter=${encodeURIComponent(filterToUse)}`)
        .then(response => response.json())
        .then(data => {
            const labels = data.map(item => item.Category);
            const values = data.map(item => item.TotalCases);

            const chartData = {
                labels: labels,
                datasets: [{
                    label: 'Cases Filed',
                    data: values,
                    backgroundColor: primaryColor,
                    borderColor: primaryColor,
                    borderWidth: 1
                }]
            };

            const config = {
                type: 'bar',
                data: chartData,
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false,
                            labels: {
                                color: fontColor
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
                            beginAtZero: true,
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

            if (categoryChart) {
                categoryChart.data = chartData;
                categoryChart.update();
            } else {
                categoryChart = new Chart(ctx, config);
            }
        })
        .catch(error => {
            console.error("Error fetching chart data:", error);
        });
}


loadChartData();


document.addEventListener('DOMContentLoaded', function () {
    const filterSelect = document.querySelector('#dateFilter');
    const resetButton = document.getElementById("resetFilter");

    filterSelect.addEventListener('change', function () {
        const selectedFilter = this.value;
        loadChartData(selectedFilter);
    });

    resetButton.addEventListener('click', function () {
        filterSelect.selectedIndex = 0;

        loadChartData('');
    });
});