const themeTogglenow = document.getElementById("themeToggle");

const lightTheme = {
    "--theme": "#FFF",
    "--primary-color": "#5E60CE",
    "--secondary-color": "#F6B37E",
    "--body-color": "#F2F4F8",
    "--active-color": "#D3D3D3",
    "--font-color": "#444",
    "--heading-color": "#5E60CE",
};

const darkTheme = {
    "--theme": "#1E1E1E",
    "--primary-color": "#8D99FF",
    "--secondary-color": "#F6B37E",
    "--body-color": "#121212",
    "--active-color": "#333",
    "--font-color": "#EAEAEA",
    "--heading-color": "#8D99FF",
};

function applyTheme(theme) {
    Object.keys(theme).forEach(key => {
        document.documentElement.style.setProperty(key, theme[key]);
    });
}

const savedTheme = localStorage.getItem("theme") || "light";
applyTheme(savedTheme === "dark" ? darkTheme : lightTheme);
themeToggle.checked = savedTheme === "dark";

themeTogglenow.addEventListener("change", () => {
    if (themeToggle.checked) {
        applyTheme(darkTheme);
        localStorage.setItem("theme", "dark");
    } else {
        applyTheme(lightTheme);
        localStorage.setItem("theme", "light");
    }
});
