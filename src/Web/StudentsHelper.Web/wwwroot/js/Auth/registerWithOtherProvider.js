window.addEventListener('DOMContentLoaded', runRegisterWithOtherProviderEngine);

function runRegisterWithOtherProviderEngine() {
    try {
        registerWithOtherProviderEngine();
    }
    catch (error) {
        console.error(error);
    }
}

function registerWithOtherProviderEngine() {
    let hiddenClassName = 'hidden';
    let clickEventName = 'click';
    let chooseOptionEventName = 'change';
    let defaultLocationNames = {
        regions: 'Област',
        townships: 'Община',
        populatedAreas: 'Населено място',
        "TeacherModel.SchoolId": 'Училище',
    };
    const partsController = ((currentPartId = 1) => {
        const methodsActivatingParts = [null, activateFirstPart, activateSecondPart];
        const getCurrentPart = () => document.getElementById(`part-${currentPartId}`);

        const changeActivePart = (event, currentPartChanger) => {
            if (!isValidCurrentPart(getCurrentPart())) return;

            if (currentPartId == 1) {
                const schoolId = document.getElementById('SchoolId');
                if (schoolId.value == 0 || Array.from(schoolId.parentNode.classList).some(x => x == hiddenClassName)) {
                    return;
                }
            }

            getCurrentPart().classList.add(hiddenClassName);
            currentPartId = currentPartChanger(currentPartId);
            methodsActivatingParts[currentPartId](event);
            getCurrentPart().classList.remove(hiddenClassName);
        };

        return {
            getCurrentPart,
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

    activateFirstPart();

    document.getElementById('past-part').addEventListener(clickEventName, partsController.showLastPart);

    document.getElementById('continue').addEventListener(clickEventName, partsController.showNextPart);

    document.getElementById('register-submit').addEventListener(clickEventName, registerUser.bind(null, partsController));

    function isValidCurrentPart(currentPart) {
        const inputFields = Array.from(currentPart.querySelectorAll('[name]'));

        return inputFields.every(x => $(x).valid());
    }

    function registerUser(partsController, e) {
        if (!isValidCurrentPart(partsController.getCurrentPart())) { e.preventDefault(); return; }
    }

    function activateFirstPart() {
        if (Array.from(document.getElementById('SchoolId').parentNode.classList).some(x => x == hiddenClassName)) {
            document.getElementById('continue').classList.add(hiddenClassName);
        } else {
            document.getElementById('continue').classList.remove(hiddenClassName);
        }

        document.getElementById('past-part').classList.add(hiddenClassName);
        document.getElementById('register-submit').classList.add(hiddenClassName);
        const getSelect = (name) => document.querySelector(`select[name="${name}"]`);
        const selects = [getSelect('regions'), getSelect('townships'), getSelect('populatedAreas'), document.getElementById('SchoolId')];

        selects.slice(0, selects.length).forEach((select, index) => select.addEventListener(chooseOptionEventName, loadOptionsForNext.bind(null, index)));
        loadOptions(selects[0], null);

        async function loadOptionsForNext(index) {
            if (index + 1 === selects.length) {
                document.getElementById('continue').classList.remove(hiddenClassName);
                return;
            }

            document.getElementById('continue').classList.add(hiddenClassName);

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
                .then(async (res) => await res.json())
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

    function activateSecondPart() {
        document.getElementById('past-part').classList.remove(hiddenClassName);
        document.getElementById('register-submit').classList.remove(hiddenClassName);
        document.getElementById('continue').classList.add(hiddenClassName);
    }
}