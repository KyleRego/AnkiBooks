export function setupEventHandlers(domContainerId) {
    const containingElement = document.getElementById(domContainerId);
    const textArea = containingElement.querySelector("textArea");
    const boldButton = containingElement.querySelector(".markdown-bold-button");
    const italicButton = containingElement.querySelector(".markdown-italic-button");
    const strikeButton = containingElement.querySelector(".markdown-strike-button");
    const linkButton = containingElement.querySelector(".markdown-link-button");
    console.log(textArea);

    boldButton.addEventListener("click", () => {
        insertAroundSelection("**", "**");
    });

    italicButton.addEventListener("click", () => {
        insertAroundSelection("*", "*");
    })

    strikeButton.addEventListener("click", () => {
        insertAroundSelection("~~", "~~");
    });

    linkButton.addEventListener("click", () => {
        insertAroundSelection("[", "]()");
    })

    function insertAroundSelection(before, after) {
        const text = textArea.value;

        const selectionStart = textArea.selectionStart;
        const selectionEnd = textArea.selectionEnd;
        
        const beforeSelectionText = text.substring(0, selectionStart);
        const afterSelectionText = text.substring(selectionEnd);

        const selectionWithInserts = `${before}${text.substring(selectionStart, selectionEnd)}${after}`;

        textArea.value = `${beforeSelectionText}${selectionWithInserts}${afterSelectionText}`;

        const newSelectionStart = selectionStart + before.length;
        const newSelectionEnd = selectionEnd + before.length;

        textArea.setSelectionRange(newSelectionStart, newSelectionEnd);

        textArea.focus();
    }
}