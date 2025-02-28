document.addEventListener("DOMContentLoaded", function () {
    const themeToggle = document.getElementById("themeToggle");
    const menuIcons = document.querySelectorAll(".main-menu li img");
    const colorOptions = document.querySelectorAll(".color-option");

    const lightTheme = {
        "--theme": "#FFF",
        "--secondary-color": "#F6B37E",
        "--body-color": "#F2F4F8",
        "--active-color": "#ffffff31",
        "--font-color": "#444",
        "--heading-color": "#5E60CE",
    };

    const darkTheme = {
        "--theme": "#242424",
        "--secondary-color": "#F6B37E",
        "--body-color": "#121212",
        "--active-color": "#ffffff31",
        "--font-color": "#EAEAEA",
        "--heading-color": "#8D99FF",
    };

    function applyTheme(theme, isDark) {
        Object.keys(theme).forEach(key => {
            document.documentElement.style.setProperty(key, theme[key]);
        });

        // Optional: Handle menu icon switching if needed
        // menuIcons.forEach(img => {
        //     if (img) {
        //         let src = img.src;
        //         if (isDark) {
        //             img.src = src.replace(".png", "-white.png");
        //         } else {
        //             img.src = src.replace("-white.png", ".png");
        //         }
        //     }
        // });
    }

    // Load saved theme and primary color
    const savedTheme = localStorage.getItem("theme") || "light";
    const savedColor = localStorage.getItem("primaryColor") || "#1A33B6";

    applyTheme(savedTheme === "dark" ? darkTheme : lightTheme, savedTheme === "dark");
    document.documentElement.style.setProperty("--primary-color", savedColor);

    // Preselect saved color
    colorOptions.forEach(option => {
        if (option.getAttribute("data-color") === savedColor) {
            option.classList.add("selected");
        }
    });

    if (themeToggle) {
        themeToggle.checked = savedTheme === "dark";

        themeToggle.addEventListener("change", () => {
            if (themeToggle.checked) {
                applyTheme(darkTheme, true);
                localStorage.setItem("theme", "dark");
            } else {
                applyTheme(lightTheme, false);
                localStorage.setItem("theme", "light");
            }
        });
    } else {
        console.warn("Theme toggle element not found.");
    }

    colorOptions.forEach(option => {
        option.addEventListener("click", () => {
            const selectedColor = option.getAttribute("data-color");

            updatePrimaryColor(selectedColor);

            colorOptions.forEach(opt => opt.classList.remove("selected"));
            option.classList.add("selected");
        });
    });

    function updatePrimaryColor(color) {
        document.documentElement.style.setProperty("--primary-color", color);
        localStorage.setItem("primaryColor", color);
    }
});
