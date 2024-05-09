export function setupEventHandlers(domContainerId) {
    const containingElement = document.getElementById(domContainerId);
    const textArea = containingElement.querySelector("textArea");

    textArea.addEventListener("keydown", (event) => {
        if (event.ctrlKey && ["b", "i", "s"].includes(event.key)) {
            event.preventDefault();

            const keyName = event.key;

            switch (keyName)
            {
                case "b":
                    insertBold();
                    break;
                case "i":
                    insertItalic();
                    break;
                case "s":
                    insertStrikethrough();
                    break;
            }
        }
    })

    function insertBold() {
        insertAroundSelection("**", "**");
    }

    function insertItalic() {
        insertAroundSelection("*", "*");
    }

    function insertStrikethrough() {
        insertAroundSelection("~~", "~~")
    }

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