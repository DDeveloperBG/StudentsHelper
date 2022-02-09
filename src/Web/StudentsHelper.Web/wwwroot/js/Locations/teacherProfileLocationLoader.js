window.addEventListener('load', () => {
    const hiddenClassName = 'hidden';
    const pastValueAttribute = 'past-value';
    const getSelect = (name) => document.querySelector(`select[name="Input.${name}"]`);
    const selects = [getSelect('RegionId'), getSelect('TownshipId'), getSelect('PopulatedAreaId'), getSelect('SchoolId')];
    
    loadOptions(selects[0], null);
    loadOptions(selects[1], selects[0].getAttribute(pastValueAttribute));
    loadOptions(selects[2], selects[1].getAttribute(pastValueAttribute));
    loadOptions(selects[3], selects[2].getAttribute(pastValueAttribute));

    function loadOptions(select, lastSelectedId) {
        fetch(`/Locations/Get?selectName=${select.name}&lastSelectedId=${lastSelectedId}`)
            .then(async (res) => await res.json())
            .then(showOptions);

        function showOptions(options) {
            const container = new DocumentFragment();

            options.forEach(option => {
                container.appendChild(createOption(option));
            });

            select.innerHTML = '';
            select.appendChild(container);
            select.value = select.getAttribute(pastValueAttribute);
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
