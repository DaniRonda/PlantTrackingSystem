import { Selector } from 'testcafe';

fixture`Plant Management App E2E Test`
    .page`http://localhost:4200`;

test('Add and view a new plant', async t => {
    // Open the "Add Plant" popup
    await t.click(Selector('.round-btn.add-btn'));

    // Fill in the plant details
    const plantNameInput = Selector('input[name="plantName"]');
    const plantTypeInput = Selector('input[name="plantType"]');
    const locationInput = Selector('input[name="location"]');

    await t
        .typeText(plantNameInput, 'Test Plant')
        .typeText(plantTypeInput, 'Test Type')
        .typeText(locationInput, 'Test Location');

    // Save the new plant
    const saveButton = Selector('button').withText('Save');
    await t.click(saveButton);

    // Verify the new plant is in the list
    const newPlantButton = Selector('.plant-item .plant-btn').withText('Test Plant');
    await t.expect(newPlantButton.exists).ok();

    // Select the new plant
    await t.click(newPlantButton);

    // Verify the plant details are displayed correctly
    const plantName = Selector('#plantName').innerText;
    await t.expect(plantName).eql('Test Plant');

    // Verify that the plant data table is initially empty
    const plantDataRows = Selector('#plantData tr');
    await t.expect(plantDataRows.count).eql(0);
});

test('Filter plants by name', async t => {
    // Filter the plants list
    const filterInput = Selector('#sidebar input[type="text"]');
    await t.typeText(filterInput, 'Test Plant');

    // Verify that only the filtered plant is visible
    const visiblePlants = Selector('.plant-item .plant-btn');
    await t.expect(visiblePlants.count).eql(1);
    await t.expect(visiblePlants.innerText).eql('Test Plant');
});

test('Delete a plant', async t => {
    // Select the plant to be deleted
    const plantButton = Selector('.plant-item .plant-btn').withText('Test Plant');
    await t.click(plantButton);

    // Click the delete button
    const deleteButton = Selector('.plant-item .delete-btn');
    await t.click(deleteButton);

    // Confirm the deletion
    const confirmButton = Selector('button').withText('Confirm');
    await t.click(confirmButton);

    // Verify the plant is removed from the list
    await t.expect(plantButton.exists).notOk();
});
