document.addEventListener("DOMContentLoaded", function () {

    const passwordInput = document.getElementById("login-password");
    const togglePasswordButton = document.getElementById("toggle-password");
    const passwordIcon = document.getElementById("password-icon");

    let isMouseDownOrTouching = false;

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
        if (passwordInput.type === "text" && !isMouseDownOrTouching) {
            passwordInput.type = "password";
            passwordIcon.classList.replace("bi-eye-fill", "bi-eye-slash-fill");
        }
    }

    function handleMouseDownOrTouchStart() {
        isMouseDownOrTouching = true;
        togglePasswordVisibility();
    }

    function handleMouseUpOrTouchEnd() {
        isMouseDownOrTouching = false;
        togglePasswordVisibility();
    }

    function handleMouseLeaveOrTouchCancel() {
        isMouseDownOrTouching = false;
        togglePasswordVisibilityOnMouseLeave();
    }

    // Desktop events
    togglePasswordButton.addEventListener("mousedown", handleMouseDownOrTouchStart);
    togglePasswordButton.addEventListener("mouseup", handleMouseUpOrTouchEnd);
    togglePasswordButton.addEventListener("mouseleave", handleMouseLeaveOrTouchCancel);

    // Mobile events
    togglePasswordButton.addEventListener("touchstart", handleMouseDownOrTouchStart);
    togglePasswordButton.addEventListener("touchend", handleMouseUpOrTouchEnd);
    togglePasswordButton.addEventListener("touchcancel", handleMouseLeaveOrTouchCancel);

});