window.addEventListener('load', () => {
    const defaultLocationNames = {
        regionId: 'Област',
        townshipId: 'Община',
        populatedAreaId: 'Населено място',
        schoolId: 'Училище',
    };
    const hiddenClassName = 'hidden';
    const getSelect = (name) => document.querySelector(`select[name="${name}"]`);
    const selects = [getSelect('regionId'), getSelect('townshipId'), getSelect('populatedAreaId'), getSelect('schoolId')];

    selects.slice(0, selects.length - 1).forEach((select, index) => select.addEventListener('change', loadOptionsForNext.bind(null, index)));
    loadOptions(selects[0], null);

    async function loadOptionsForNext(index) {
        if (index + 1 === selects.length) {
            return;
        }

        hideNext(index + 1);

        loadOptions(selects[index + 1], selects[index].value);
    }

    function hideNext(currentIndex) {
        if (currentIndex == selects.length) return;

        selects[currentIndex].parentNode.classList.add(hiddenClassName);
        selects[currentIndex].value = 0;

        hideNext(currentIndex + 1);
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
});