export function setupEventHandlers(domContainerId) {
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
}