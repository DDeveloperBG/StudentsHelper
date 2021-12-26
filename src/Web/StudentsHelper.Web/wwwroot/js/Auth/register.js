window.addEventListener('load', engine);
var hiddenClassName = 'hidden';
var clickEventName = 'click';
var chooseOptionEventName = 'change';
var defaultLocationNames = {
    regions: 'Област',
    townships: 'Община',
    populatedAreas: 'Населено място',
    'Input.TeacherModel.SchoolId': 'Училище',
};

function engine() {
    const partsController = ((currentPartId = 1) => {
        const methodsActivatingParts = [null, activateFirstPart, activateSecondPart, activateThirdPart, activateFourthPart];
        const getCurrentPart = () => document.getElementById(`part-${currentPartId}`);

        const changeActivePart = (event, currentPartChanger) => {
            getCurrentPart().classList.add(hiddenClassName);
            currentPartId = currentPartChanger(currentPartId);
            methodsActivatingParts[currentPartId](event);
            getCurrentPart().classList.remove(hiddenClassName);
        };

        return {
            showLastPart(e) {
                e.preventDefault();
                changeActivePart(e, (currentPartId) => --currentPartId);
            },
            showNextPart(e) {
                e.preventDefault();
                changeActivePart(e, (currentPartId) => ++currentPartId);
            }
        };
    })();

    //set up page
    let roles = Array.from(document.querySelectorAll('img.role'));

    roles.forEach(role => {
        role.addEventListener(clickEventName, partsController.showNextPart);
    });

    document.getElementById('past-part').addEventListener(clickEventName, partsController.showLastPart);

    document.getElementById('continue').addEventListener(clickEventName, partsController.showNextPart);
}

function activateFirstPart(e) {
    document.getElementById('register-submit').classList.add(hiddenClassName);
    document.getElementById('continue').classList.add(hiddenClassName);
    document.getElementById('past-part').classList.add(hiddenClassName);
    document.getElementById('second-auth-option').classList.add(hiddenClassName);
    document.querySelector('.more-options').classList.remove(hiddenClassName);
}

function activateSecondPart(e) {
    const roleInput = document.getElementById('role');
    roleInput.value = e.target.getAttribute('role');

    document.getElementById('past-part').classList.remove(hiddenClassName);
    const registerBtn = document.getElementById('register-submit');
    const continueBtn = document.getElementById('continue');
    document.getElementById('second-auth-option').classList.remove(hiddenClassName);
    document.querySelector('.more-options').classList.add(hiddenClassName);

    if (roleInput.value == 'student') {
        registerBtn.classList.remove(hiddenClassName);
        continueBtn.classList.add(hiddenClassName);
    } else {
        continueBtn.classList.remove(hiddenClassName);
        registerBtn.classList.add(hiddenClassName);
    }
}

function activateThirdPart(e) {
    document.getElementById('second-auth-option').classList.add(hiddenClassName);
    const getSelect = (name) => document.querySelector(`select[name="${name}"]`);
    const selects = [getSelect('regions'), getSelect('townships'), getSelect('populatedAreas'), getSelect('Input.TeacherModel.SchoolId')];

    selects.slice(0, selects.length - 1).forEach((select, index) => select.addEventListener(chooseOptionEventName, loadOptionsForNext.bind(null, index)));
    loadOptions(selects[0], null);

    async function loadOptionsForNext(index) {
        if (index + 1 === selects.length) {
            return;
        }

        for (let i = index + 1; i < selects.length; i++) {
            selects[i].parentNode.classList.add(hiddenClassName);
        }

        loadOptions(selects[index + 1], selects[index].value);
    }

    function loadOptions(select, lastSelectedId) {
        if (select.children.length > 0 && lastSelectedId == null) {
            return;
        }

        fetch(`/Locations/Get?selectName=${select.name}&lastSelectedId=${lastSelectedId}`)
            .then(async(res) => await res.json())
            .then(showOptions);

        function showOptions(options) {
            const container = new DocumentFragment();

            let defaultOption = defaultLocationNames[select.name];

            container.appendChild(createOption({ id: 0, name: defaultOption }));

            options.forEach(option => {
                container.appendChild(createOption(option));
            });

            select.innerHTML = '';
            select.appendChild(container);
            select.parentNode.classList.remove(hiddenClassName);

            function createOption(option) {
                let optionEl = document.createElement('option');
                optionEl.value = option.id;
                optionEl.textContent = option.name;
                return optionEl;
            }
        }
    }
}

function activateFourthPart(e) {
    document.getElementById('register-submit').classList.remove(hiddenClassName);
    document.getElementById('continue').classList.add(hiddenClassName);
}