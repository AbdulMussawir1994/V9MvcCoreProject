
$(document).ready(function () {

    // 🔹 Format CNIC while typing (e.g., 42101-1234567-9)
    $("input[name='cnic']").on("input", function () {
        let raw = $(this).val().replace(/\D/g, ""); // remove non-digits
        if (raw.length > 13) raw = raw.substring(0, 13);

        let formatted = raw;
        if (raw.length > 5 && raw.length <= 12) {
            formatted = raw.substring(0, 5) + "-" + raw.substring(5, 12);
        } else if (raw.length > 12) {
            formatted = raw.substring(0, 5) + "-" + raw.substring(5, 12) + "-" + raw.substring(12);
        }

        $(this).val(formatted);
    });
});

function showSpinner() {
    document.getElementById("spinner").classList.add("active");
}

function hideSpinner() {
    document.getElementById("spinner").classList.remove("active");
}

function Login() {
    showSpinner();

    var data = {
        CNIC: $("input[name='cnic']").val().trim(),
        Password: $("input[name='password']").val().trim()
    };

    // 1️⃣ Remove any non-digit characters (like '-' or spaces)
    data.CNIC = data.CNIC.replace(/\D/g, '');

    // 2️⃣ Validate CNIC (must be exactly 13 digits)
    if (!/^\d{13}$/.test(data.CNIC)) {
        $("#cnic-error").text("CNIC must contain exactly 13 digits (numbers only).");
        hideSpinner();
        return;
    } else {
        $("#cnic-error").text(""); // clear error
    }

    // 3️⃣ Validate password field
    if (data.Password === "") {
        $("#password-error").text("Password is required.");
        hideSpinner();
        return;
    } else {
        $("#password-error").text(""); // clear error
    }

    // 4️⃣ Perform AJAX request
    $.ajax({
        type: "POST",
        url: "/Account/Login",
        data: data,
        success: function (response) {
            console.log(response)
            hideSpinner();

            if (response && response.Succeeded) {
                Swal.fire({
                    icon: 'success',
                    title: 'Login Successful',
                    text: 'Redirecting to Home page...',
                    showConfirmButton: false,
                    timer: 1500
                });
                setTimeout(() => {
                    window.location.href = "/Home/Index";
                }, 1500);
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    text: response.StatusMessage || "Invalid login attempt.",
                    confirmButtonColor: '#3085d6',
                });
            }
        },
        error: function () {
            hideSpinner();
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error during login request.',
                confirmButtonColor: '#d33',
            });
        }
    });
}