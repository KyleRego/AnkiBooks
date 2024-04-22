export function setupDialog(domContainerId) {
    const containingElement = document.getElementById(domContainerId);
    const dialog = containingElement.querySelector("dialog");
    const showButton = containingElement.querySelector(".show-dialog-button");
    const closeButton = containingElement.querySelector(".close-dialog-button");

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