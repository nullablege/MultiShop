(() => {
    const card = document.getElementById("payment-card-preview");
    const cardNumberInput = document.getElementById("payment-card-number-input");
    const cardHolderInput = document.getElementById("payment-card-holder-input");
    const monthInput = document.getElementById("payment-card-month-input");
    const yearInput = document.getElementById("payment-card-year-input");
    const cvvInput = document.getElementById("payment-card-cvv-input");
    const form = document.getElementById("payment-card-form");

    if (!card || !cardNumberInput || !cardHolderInput || !monthInput || !yearInput || !cvvInput) {
        return;
    }

    const cardNumberPreview = document.getElementById("payment-card-number-preview");
    const cardHolderPreview = document.getElementById("payment-card-holder-preview");
    const monthPreview = document.getElementById("payment-card-month-preview");
    const yearPreview = document.getElementById("payment-card-year-preview");
    const cvvPreview = document.getElementById("payment-card-cvv-preview");

    cardNumberInput.addEventListener("input", () => {
        const digits = cardNumberInput.value.replace(/\D/g, "").slice(0, 16);
        const formatted = digits.replace(/(.{4})/g, "$1 ").trim();
        cardNumberInput.value = formatted;
        cardNumberPreview.textContent = formatted || "•••• •••• •••• ••••";
    });

    cardHolderInput.addEventListener("input", () => {
        cardHolderPreview.textContent = cardHolderInput.value.trim().toLocaleUpperCase("tr-TR") || "AD SOYAD";
    });

    monthInput.addEventListener("change", () => {
        monthPreview.textContent = monthInput.value || "AA";
    });

    yearInput.addEventListener("change", () => {
        yearPreview.textContent = yearInput.value ? yearInput.value.slice(-2) : "YY";
    });

    const showCardBack = () => card.classList.add("is-flipped");
    const showCardFront = () => card.classList.remove("is-flipped");

    cvvInput.addEventListener("focus", showCardBack);
    cvvInput.addEventListener("blur", showCardFront);
    cvvInput.addEventListener("input", () => {
        const digits = cvvInput.value.replace(/\D/g, "").slice(0, 3);
        cvvInput.value = digits;
        cvvPreview.textContent = digits;
    });

    form?.addEventListener("submit", event => event.preventDefault());
})();
