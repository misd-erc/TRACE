document.addEventListener("DOMContentLoaded", function () {
    const themeToggle = document.getElementById("themeToggle");
    const menuIcons = document.querySelectorAll(".main-menu li img");
    const colorPicker = document.getElementById("colorPicker");
    const colorPreview = document.querySelector(".color-preview");

    const lightTheme = {
        "--theme": "#FFF",
        "--secondary-color": "#F6B37E",
        "--body-color": "#F2F4F8",
        "--active-color": "#D3D3D3",
        "--font-color": "#444",
        "--heading-color": "#5E60CE",
    };

    const darkTheme = {
        "--theme": "#1E1E1E",
        "--secondary-color": "#F6B37E",
        "--body-color": "#121212",
        "--active-color": "#333",
        "--font-color": "#EAEAEA",
        "--heading-color": "#8D99FF",
    };

    function applyTheme(theme, isDark) {
        Object.keys(theme).forEach(key => {
            document.documentElement.style.setProperty(key, theme[key]);
        });

        menuIcons.forEach(img => {
            if (img) {
                let src = img.src;
                if (isDark) {
                    img.src = src.replace(".png", "-white.png");
                } else {
                    img.src = src.replace("-white.png", ".png");
                }
            }
        });
    }

    const savedTheme = localStorage.getItem("theme") || "light";
    const savedColor = localStorage.getItem("primaryColor") || "#4A89DC";

    applyTheme(savedTheme === "dark" ? darkTheme : lightTheme, savedTheme === "dark");

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

    document.documentElement.style.setProperty("--primary-color", savedColor);

    if (colorPicker && colorPreview) {
        colorPicker.value = savedColor;
        colorPreview.style.background = savedColor;

        colorPicker.addEventListener("input", (e) => updatePrimaryColor(e.target.value));
        colorPreview.addEventListener("click", () => colorPicker.click()); // Open picker on preview click
    } else {
        console.warn("Color picker elements not found.");
    }

    function updatePrimaryColor(color) {
        document.documentElement.style.setProperty("--primary-color", color);
        if (colorPreview) colorPreview.style.background = color;
        localStorage.setItem("primaryColor", color);
    }
});
