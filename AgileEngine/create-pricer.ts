export type Category = 'size' | 'creamer';
export type Option = 'small' | 'medium' | 'large' | 'none' | 'dairy' | 'non-dairy';
export type Price = number;

export interface Pricer {
  /** 
   * Invoked each time the user makes a selection. 
   * No need to validate arguments, the caller validates the 
arguments before this function is invoked. 
   * @returns the _total_ price of the coffee so far given all the 
selections made 
   */
  (category: Category, option: Option): Price
}

/** 
* A new pricer is created for each coffee being purchased. 
*/
export const createPricer = (): Pricer => {
  let state: { size: Option | null; creamer: Option | null } = {
    size: null,
    creamer: null
  };

  const priceOfSize = (size: Option | null): number =>
    size === "small" ? 1.0 :
    size === "medium" ? 1.5 :
    size === "large" ? 2.0 :
    0;

  const priceOfCreamer = (creamer: Option | null): number =>
    creamer === "none" ? 0 :
    creamer === "dairy" ? 0.25 :
    creamer === "non-dairy" ? 0.5 :
    0;

  return (category, option) => {
    state = { ...state, [category]: option };

    return priceOfSize(state.size) + priceOfCreamer(state.creamer);
  };
};