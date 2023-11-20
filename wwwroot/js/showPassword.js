document.addEventListener("DOMContentLoaded", function () {
    
    const passwordInput = document.getElementById("login-password");
    const togglePasswordButton = document.getElementById("toggle-password");
    const passwordIcon = document.getElementById("password-icon");

    let isMouseDown = false;

    function togglePasswordVisibility() {
        if (passwordInput.type === "password") {
            passwordInput.type = "text";
            passwordIcon.classList.replace("bi-eye-slash-fill", "bi-eye-fill");
        } else {
            passwordInput.type = "password";
            passwordIcon.classList.replace("bi-eye-fill", "bi-eye-slash-fill");
        }
    }

    function togglePasswordVisibilityOnMouseLeave() {
        if (passwordInput.type === "text" && !isMouseDown) {
            passwordInput.type = "password";
            passwordIcon.classList.replace("bi-eye-fill", "bi-eye-slash-fill");
        }
    }

    function handleMouseDown() {
        isMouseDown = true;
        togglePasswordVisibility();
    }

    function handleMouseUp() {
        isMouseDown = false;
        togglePasswordVisibility();
    }

    function handleMouseLeave() {
        isMouseDown = false;
        togglePasswordVisibilityOnMouseLeave();
    }

    togglePasswordButton.addEventListener("mousedown", handleMouseDown);
    togglePasswordButton.addEventListener("mouseup", handleMouseUp);
    togglePasswordButton.addEventListener("mouseleave", handleMouseLeave);
});