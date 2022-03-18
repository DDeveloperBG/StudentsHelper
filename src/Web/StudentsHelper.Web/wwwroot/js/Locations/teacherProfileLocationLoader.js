var isInitialLoad = null;

window.addEventListener('load', async () => {
    const activeValueAttribute = 'active-value';
    const defaultLocationNames = {
        'Input.RegionId': 'Област',
        'Input.TownshipId': 'Община',
        'Input.PopulatedAreaId': 'Населено място',
        'Input.SchoolId': 'Училище',
    };
    const hiddenClassName = 'hidden';
    const getSelect = (name) => document.querySelector(`select[name="Input.${name}"]`);
    const selects = [getSelect('RegionId'), getSelect('TownshipId'), getSelect('PopulatedAreaId'), getSelect('SchoolId')];

    isInitialLoad = true;
    await loadOptions(selects[0], null);
    await loadOptions(selects[1], selects[0].getAttribute(activeValueAttribute));
    await loadOptions(selects[2], selects[1].getAttribute(activeValueAttribute));
    await loadOptions(selects[3], selects[2].getAttribute(activeValueAttribute));
    isInitialLoad = false;

    selects.slice(0, selects.length - 1).forEach((select, index) => select.addEventListener('change', loadOptionsForNext.bind(null, index)));

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

    async function loadOptions(select, lastSelectedId) {
        if (select.children.length > 0 && lastSelectedId == null) {
            return;
        }

        await fetch(`/Locations/Get?selectName=${select.name}&lastSelectedId=${lastSelectedId}`)
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

            if (isInitialLoad) {
                select.value = select.getAttribute(activeValueAttribute);
            } else {
                select.value = 0;
            }

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
