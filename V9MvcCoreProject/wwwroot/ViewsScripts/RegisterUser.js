
    $(document).ready(function () {

        loadPermissionTemplates();

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

        // 🔹 Live Password Match Check
        $("input[name='confirmpassword'], input[name='password']").on("input", function () {
            let pass = $("input[name='password']").val();
            let confirm = $("input[name='confirmpassword']").val();

            if (confirm.length > 0) {
                if (pass === confirm) {
                    $("#confirmpassword-error").text("✅ Passwords match").css("color", "lightgreen");
                } else {
                    $("#confirmpassword-error").text("❌ Passwords do not match").css("color", "red");
                }
            } else {
                $("#confirmpassword-error").text("");
            }
        });
    });

function showSpinner() {
    document.getElementById("spinner").classList.add("active");
}

function hideSpinner() {
    document.getElementById("spinner").classList.remove("active");
}


function loadPermissionTemplates() {

    $.ajax({
        type: "GET",
        url: "/Permission/GetPermissionTemplates",
        dataType: "json",
        success: function (response) {
            debugger
            console.log("Templates loaded:", response);
            const $dropdown = $("#templateId");
            $dropdown.empty().append('<option value="">Select Template Name</option>');

            if (response && response.length > 0) {
                $.each(response, function (i, item) {
                    $dropdown.append(
                        $("<option></option>")
                            .val(item.Id)
                            .text(item.TemplateName)
                    );
                });
            } else {
                $dropdown.append('<option value="">No templates available</option>');
            }
        },
        error: function (xhr) {
            console.error("Error loading templates:", xhr.responseText);
            alert("Failed to load permission templates.");
        }
    });
}


// 🔹 On Submit
function Register() {
    debugger
    showSpinner();

    let data = {
        CNIC: $("input[name='cnic']").val().trim(),
        Username: $("input[name='username']").val().trim(),
        Email: $("input[name='email']").val().trim(),
        Password: $("input[name='password']").val().trim(),
        ConfirmPassword: $("input[name='confirmpassword']").val().trim(),
        TemplateId: $("select[name='templateId']").val().trim()
    };

    let hasError = false;

    // Clear previous errors
    $(".form-error").text("");

    // ✅ CNIC Validation
    let cnicDigits = data.CNIC.replace(/\D/g, '');
    if (cnicDigits.length !== 13) {
        $("#cnic-error").text("CNIC must contain exactly 13 digits.");
        hideSpinner();
        hasError = true;
    }

    // ✅ Username Validation
    if (!data.Username) {
        $("#username-error").text("Username is required.");
        hideSpinner();
        hasError = true;
    }

    // ✅ Email Validation
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!data.Email) {
        $("#email-error").text("Email is required.");
        hideSpinner();
        hasError = true;
    } else if (!emailPattern.test(data.Email)) {
        $("#email-error").text("Invalid email format.");
        hideSpinner();
        hasError = true;
    }

    // ✅ Password Validation
    if (!data.Password) {
        $("#password-error").text("Password is required.");
        hideSpinner();
        hasError = true;
    }

    // ✅ Confirm Password Validation
    if (!data.ConfirmPassword) {
        $("#confirmpassword-error").text("Confirm password is required.");
        hideSpinner();
        hasError = true;
    } else if (data.Password !== data.ConfirmPassword) {
        $("#confirmpassword-error").text("Passwords do not match.");
        hideSpinner();
        hasError = true;
    }

    if (!data.TemplateId) {
        $("#templateId-error").text("Please select template name.");
        hideSpinner();
        hasError = true;
    }

    if (hasError) return;

    // ✅ Prepare data for backend (clean CNIC)
    data.CNIC = cnicDigits;

    // 🔹 Example: Ajax POST request
    $.ajax({
        type: "POST",
        url: "/Account/Register",
        data: data,
        success: function (response) {
            hideSpinner();
            console.log("Register log", response)
            if (response && response.IsSuccess) {
                Swal.fire({
                    icon: 'success',
                    title: 'Registration Successful!',
                    text: 'Redirecting to login page...',
                    showConfirmButton: false,
                    timer: 2000
                });
                setTimeout(() => {
                    window.location.href = "/Account/Login";
                }, 2000);
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Registration Failed',
                    text: response.Message || 'Please try again later.'
                });
            }
        },
        error: function () {
            hideSpinner();
            Swal.fire({
                icon: 'error',
                title: 'Server Error',
                text: 'Unable to process registration right now.'
            });
        }
    });
}