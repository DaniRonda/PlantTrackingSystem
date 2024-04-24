import { Selector } from 'testcafe';

fixture`Conversion App E2E Test`
    .page`http://localhost:4200`;

test('Conversion App should convert currencies correctly', async t => {
       await t
        .typeText('#amount', '100')
        .click('#fromCurrency')
        .click(Selector('option').withText('USD'))
        .click('#toCurrency')
        .click(Selector('option').withText('EUR'))
        .click('button');
       
    const resultText = await Selector('.conversion-result p').innerText;
    await t.expect(resultText).contains('100 USD is');

    });
