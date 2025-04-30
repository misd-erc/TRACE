document.addEventListener("DOMContentLoaded", function () {
    fetch('/users/get-notification-preferences')
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                document.getElementById("emailnotifToggle").checked = result.data.isEmailNotif;
                document.getElementById("systemnotifToggle").checked = result.data.isSystemNotif;
            } else {
                Swal.fire({
                    title: "Error!",
                    text: result.message,
                    icon: "error",
                    confirmButtonText: "OK"
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: "Oops!",
                text: "Failed to load notification settings.",
                icon: "error",
                confirmButtonText: "OK"
            });
        });
});

document.getElementById("saveNotifSettingsBtn").addEventListener("click", function () {
    const isEmailNotif = document.getElementById("emailnotifToggle").checked;
    const isSystemNotif = document.getElementById("systemnotifToggle").checked;

    fetch('/users/update-notification-preferences', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            isEmailNotif: isEmailNotif,
            isSystemNotif: isSystemNotif
        })
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire({
                    title: "Success!",
                    text: result.message,
                    icon: "success",
                    confirmButtonText: "OK",
                    customClass: {
                        popup: "swal2-popup",
                        confirmButton: "swal2-confirm"
                    }
                });
            } else {
                Swal.fire({
                    title: "Error!",
                    text: result.message,
                    icon: "error",
                    confirmButtonText: "Try Again"
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: "Oops!",
                text: "Something went wrong. Please try again.",
                icon: "error",
                confirmButtonText: "OK",
                customClass: {
                    popup: "swal2-popup",
                    confirmButton: "swal2-confirm"
                }
            });
        });
});

document.getElementById("revertNotifSettingsBtn").addEventListener("click", function () {
    Swal.fire({
        title: "Revert Settings?",
        text: "This will reset your notification preferences to default values.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Revert",
        cancelButtonText: "Cancel"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch('/users/revert-notification-preferences', {
                method: 'POST'
            })
                .then(response => response.json())
                .then(result => {
                    if (result.success) {
                        document.getElementById("emailnotifToggle").checked = true;
                        document.getElementById("systemnotifToggle").checked = true;

                        Swal.fire({
                            title: "Reverted!",
                            text: result.message,
                            icon: "success",
                            confirmButtonText: "OK",
                            customClass: {
                                popup: "swal2-popup",
                                confirmButton: "swal2-confirm"
                            }
                        });
                    } else {
                        Swal.fire({
                            title: "Error!",
                            text: result.message,
                            icon: "error",
                            confirmButtonText: "OK"
                        });
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    Swal.fire({
                        title: "Oops!",
                        text: "An error occurred while reverting settings.",
                        icon: "error",
                        confirmButtonText: "OK"
                    });
                });
        }
    });
});