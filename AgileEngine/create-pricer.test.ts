import { createPricer } from "./create-pricer";

it('provides the latest price given the options selected so far', () => { 
   // starting a coffee order 
   const pricer = createPricer(); 
 
   // set the default options 
   pricer('size', 'small'); 
   const defaultPrice = pricer('creamer', 'none'); 
   expect(defaultPrice).toBe(1.00); 
 
   // user selects dairy creamer 
   const priceAfterFirstSelection = pricer('creamer', 'dairy'); 
   expect(priceAfterFirstSelection).toBe(1.25); 
 
   // user selects non-dairy creamer 
   const priceAfterSecondSelection = pricer('creamer', 'non-dairy'); 
   expect(priceAfterSecondSelection).toBe(1.50); 
 
   // user selects large 
   const priceAfterThirdSelection = pricer('size', 'large'); 
   expect(priceAfterThirdSelection).toBe(2.50); 
});