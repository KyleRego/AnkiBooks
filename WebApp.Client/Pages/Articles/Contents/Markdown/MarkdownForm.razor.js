export function setupEventHandlers(domContainerId) {
    const containingElement = document.getElementById(domContainerId);
    const textArea = containingElement.querySelector("textArea");
    const boldButton = containingElement.querySelector(".markdown-bold-button");
    const italicButton = containingElement.querySelector(".markdown-italic-button");
    const strikeButton = containingElement.querySelector(".markdown-strike-button");

    for (let i = 1; i <= 6; i++)
    {
        const headingSelector = `.markdown-heading-${i}-button`;
        containingElement.querySelector(headingSelector).addEventListener("click", () => {
            insertAroundSelection(`${"#".repeat(i)} `, "");
        })
    }

    const linkButton = containingElement.querySelector(".markdown-link-button");

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

    boldButton.addEventListener("click", () => {
        insertBold();
    });

    italicButton.addEventListener("click", () => {
        insertItalic();
    })

    strikeButton.addEventListener("click", () => {
        insertStrikethrough();
    });

    linkButton.addEventListener("click", () => {
        insertLink();
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

    function insertLink() {
        insertAroundSelection("[", "]()");
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