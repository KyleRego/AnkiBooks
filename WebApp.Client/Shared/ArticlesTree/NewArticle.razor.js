export function setupDialog(domContainerId) {
    const containingElement = document.getElementById(domContainerId);
    const dialog = containingElement.querySelector("dialog");
    const showButton = containingElement.querySelector(".showDialogBtn");
    const closeButton = containingElement.querySelector(".closeDialogBtn");

    showButton.addEventListener("click", () => {
        dialog.showModal();
    });

    closeButton.addEventListener("click", () => {
        dialog.close();
    });

    // Using this setupDialog to also re-setup the dialog after a submit
    // seems to play well with Blazor better than other things I tried
    closeButton.click();
}