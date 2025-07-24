
/*FOR CASE FILTER*/
document.addEventListener("DOMContentLoaded", function () {
    const filterButton = document.querySelector(".case-filter");
    const dropdown = document.querySelector(".case-filter-dropdown");

    filterButton.addEventListener("click", function () {
        if (dropdown.classList.contains("active")) {
            dropdown.style.opacity = "0";
            dropdown.style.transform = "translateY(-10px)";
            setTimeout(() => {
                dropdown.classList.remove("active");
                dropdown.style.display = "none";
            }, 300);
        } else {
            dropdown.style.display = "block";
            setTimeout(() => {
                dropdown.classList.add("active");
                dropdown.style.opacity = "1";
                dropdown.style.transform = "translateY(0)";
            }, 10);
        }
    });


    document.addEventListener("click", function (event) {
        if (!filterButton.contains(event.target) && !dropdown.contains(event.target)) {
            dropdown.style.opacity = "0";
            dropdown.style.transform = "translateY(-10px)";
            setTimeout(() => {
                dropdown.classList.remove("active");
                dropdown.style.display = "none";
            }, 300);
        }
    });
});
