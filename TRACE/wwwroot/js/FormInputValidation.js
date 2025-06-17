function addSearchFilter(
    elementId,
    apiEndpoint,
    searchCategory,
    placeholderValue = "Search"
) {
    const element = document.getElementById(elementId);

    const choices = new Choices(element, {
        searchEnabled: true,
        searchChoices: false,
        placeholder: true,
        placeholderValue: placeholderValue,
        shouldSort: false
    });

    element.addEventListener('search', function (event) {
        const searchTerm = event.detail.value;
        if (searchTerm.length < 2) return;
        fetch(`${apiEndpoint}?category=${encodeURIComponent(searchCategory)}&term=${encodeURIComponent(searchTerm)}`)
            .then(response => response.json())
            .then(data => {
                choices.clearChoices();
                choices.setChoices(data.map(item => ({
                    value: item.id,
                    label: item.text,
                    selected: false,
                    disabled: false
                })), 'value', 'label', true);
            })
            .catch(error => {
                console.error('Error fetching search results:', error);
            });
    });
}

// Input Filtering
function _setInputAlphabet(elementId) {
    $(elementId).on('paste', function (e) {
        const clipboardData = (e.originalEvent || e).clipboardData;
        const pastedText = clipboardData.getData('text');
        const isValid = /^[A-Za-z ]+$/.test(pastedText);
        if (!isValid) {
            e.preventDefault();
        }
    });
    $(elementId).on('input', function () {
        const sanitized = $(this).val().replace(/[^A-Za-z ]/g, '');
        $(this).val(sanitized);
    });
}

function _setInputDescription(elementId) {
    $(elementId).on('paste', function (e) {
        const clipboardData = (e.originalEvent || e).clipboardData;
        const pastedText = clipboardData.getData('text');

        const isValid = /^[A-Za-z0-9 .,!?'"():;\-\n\r]+$/.test(pastedText);
        if (!isValid) {
            e.preventDefault();
        }
    });

    $(elementId).on('input', function () {
        const sanitized = $(this).val().replace(/[^A-Za-z0-9 .,!?'"():;\-\n\r]/g, '');
        $(this).val(sanitized);
    });
}
function _setInputAddress(elementId) {
    $(elementId).on('paste', function (e) {
        const clipboardData = (e.originalEvent || e).clipboardData;
        const pastedText = clipboardData.getData('text');
        const isValid = /^[A-Za-z0-9\s.,#\/\-\\\n\r]+$/.test(pastedText);
        if (!isValid) {
            e.preventDefault();
        }
    });

    $(elementId).on('input', function () {
        const sanitized = $(this).val().replace(/[^A-Za-z0-9\s.,#\/\-\\\n\r]/g, '');
        $(this).val(sanitized);
    });
}

function _setInputNumeric(selector) {
    var input = $(selector);
    input.on("input", function () {
        let value = this.value.replace(/[^0-9.]/g, '');
        const parts = value.split(".");
        if (parts.length >= 2) {
            const wholeNum = parts[0] || 0
            let decimal = parts.slice(1).join('')
            decimal = decimal.substring(0, 2)
            value = `${parseInt(wholeNum)}.${decimal}`;
        } else {
            value = parseInt(value || 0);
        }
        this.value = value;
    })
    input.on("paste", function (e) {
        const clipboardData = (e.originalEvent || e).clipboardData;
        const pastedText = clipboardData.getData('text');
        const isValid = /^\d*\.?d*$/.test(pastedText)
        if (!isValid) {
            e.preventDefault();
        }
    })
}
function _setInputInteger(selector) {
    var input = $(selector);
    input.on("input", function () {
        let value = this.value.replace(/[^0-9]/g, '');
        this.value = value;
    })
    input.on("paste", function (e) {
        const clipboardData = (e.originalEvent || e).clipboardData;
        const pastedText = clipboardData.getData('text');
        const isValid = /^\d+$/.test(pastedText)
        if (!isValid) {
            e.preventDefault();
        }
    })
}
// Input Validation

function _showErrorAlert(title) {
    Swal.fire({
        title: title,
        icon: "error",
        confirmButtonText: "OK",
        customClass: {
            popup: "swal2-popup",
            confirmButton: "swal2-confirm"
        }
    });
}

function _isDateInputValid(selector, keyName, isRequired = false) {
    const date = $(selector).val();
    if (!date) {
        if (isRequired) _showErrorAlert(`"${keyName}" is Required.`);
        return false;
    }
    const isValidFormat = /^\d{4}-\d{2}-\d{2}$/.test(date);
    const isValidValue = !isNaN(Date.parse(date));
    if (!isValidFormat || !isValidValue) {
        _showErrorAlert(`"${keyName}" is Invalid.`);
        return false;
    }

    return true;
}
function _inputHasValidNumber(selector, keyName, isRequired = false) {
    const value = $(selector).val();
    if (!value) {
        if (isRequired) _showErrorAlert(`"${keyName}" is Required.`);
        return false;
    }
    if (isNaN(value)) {
        _showErrorAlert(`"${keyName}" is Invalid.`);
        return false;
    }
    return true;
}

function _inputHasValue(selector, keyName) {
    const value = $(selector).val();
    if (!value) {
        _showErrorAlert(`"${keyName}" is Required.`);
        return false;
    }
    return true;
}