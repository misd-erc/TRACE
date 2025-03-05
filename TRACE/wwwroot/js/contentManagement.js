function showTab(tabName) {
    document.querySelectorAll('.cms-content').forEach(function (content) {
        content.classList.remove('active');
    });

    document.querySelectorAll('.cms-nav li').forEach(function (navItem) {
        navItem.classList.remove('active');
    });

    document.getElementById(tabName).classList.add('active');

    event.target.classList.add('active');
}

/*FOR CMS MODAL FIELD TEMPLATES*/
const fieldTemplates = {
    'Case Category': [
        { label: 'Category Name', type: 'text', name: 'categoryName' },
        { label: 'Description', type: 'textarea', name: 'description' },
        { label: 'Is Internal?', type: 'checkbox', name: 'isInternal' }
    ],
    'Case Event Types': [
        { label: 'Event Type Name', type: 'text', name: 'eventType' },
    ],
};


/*CMS MODAL*/
document.querySelectorAll('.content-list li').forEach(item => {
    item.addEventListener('click', function () {
        const contentName = this.innerText.trim();
        openModal(contentName);
    });
});

let currentContentName = '';

function openModal(contentName) {
    currentContentName = contentName;
    document.getElementById('cmsModal').style.display = 'flex';
    document.getElementById('modalTitle').innerText = "Customize "+contentName;

    const form = document.getElementById('modalForm');
    form.innerHTML = '';

    const fields = fieldTemplates[contentName] || [];
    fields.forEach(field => {
        const fieldWrapper = document.createElement('div');
        fieldWrapper.classList.add('form-group');

        const label = document.createElement('label');
        label.innerText = field.label;

        let input;

        if (field.type === 'textarea') {
            input = document.createElement('textarea');
            input.name = field.name;
            input.required = true;
        }
        else if (field.type === 'select') {
            input = document.createElement('select');
            input.name = field.name;
            input.required = true;
            field.options.forEach(option => {
                const opt = document.createElement('option');
                opt.value = option;
                opt.innerText = option;
                input.appendChild(opt);
            });
        }
        else if (field.type === 'checkbox') {
            input = document.createElement('input');
            input.type = 'checkbox';
            input.name = field.name;
            input.value = 'true';

            const hiddenInput = document.createElement('input');
            hiddenInput.type = 'hidden';
            hiddenInput.name = field.name;
            hiddenInput.value = 'false';
            form.appendChild(hiddenInput);

            fieldWrapper.appendChild(label);
            label.appendChild(input);
            form.appendChild(fieldWrapper);

            return;
        }
        else {
            input = document.createElement('input');
            input.type = field.type;
            input.name = field.name;
            input.required = true;
        }

        fieldWrapper.appendChild(label);
        fieldWrapper.appendChild(input);
        form.appendChild(fieldWrapper);
    });

    const submitDiv = document.createElement('div');
    submitDiv.classList.add('submit-div');

    const submitBtn = document.createElement('button');
    submitBtn.type = 'submit';
    submitBtn.innerHTML = "<i class='bx bxs-save'></i> Save";

    submitDiv.appendChild(submitBtn);
    form.appendChild(submitDiv);

    form.onsubmit = handleFormSubmit;
}


function closeModal() {
    document.getElementById('cmsModal').style.display = 'none';
}

function handleFormSubmit(event) {
    event.preventDefault();

    const formData = new FormData(event.target);
    const data = Object.fromEntries(formData);

    if (data.hasOwnProperty('isInternal')) {
        data.isInternal = data.isInternal === 'true';
    }

    console.log('Saving data for:', currentContentName);
    console.log('Form Data:', data);

    switch (currentContentName) {
        case 'Case Category':
            saveCaseCategory(data);
            break;

        case 'Case Event Types':
            saveCaseEventType(data);
            break;

        /*ADD NALANG IBA PANG CASES DITO PARA SA IBANG TABLE*/
        default:
            console.warn('No save function defined for:', currentContentName);
    }

    closeModal();
}
