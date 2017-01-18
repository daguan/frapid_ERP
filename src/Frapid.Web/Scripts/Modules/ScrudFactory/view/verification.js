function createVerificationButton() {
    if (!hasVerification()) {
        return;
    };

    if (!$("#VerifyButton").length) {
        var anchor = $("<a href='javascript:void(0);' id='VerifyButton' class='ui basic button' />");
        anchor.html(window.i18n.Verify);

        anchor.insertAfter(addNewButton);
    };
};
