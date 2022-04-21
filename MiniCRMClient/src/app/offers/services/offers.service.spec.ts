import { OffersService } from "./offers.service";

describe('test', () => {
    let offersService: OffersService;
    beforeEach(() => { offersService = new OffersService(); });

    it('returnsPath', () => {
        expect(offersService.getPathByName('Фотографии и описание товара')).toBe('assets/static/NewspaperClipping.svg');
    })
});