import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Size } from './enum/Size';
import { pizzaSizes } from './Constants/pizzaSize';
import { vegToppings } from './Constants/vegToppings';
import { nonVegToppings } from './Constants/nonVegToppings';
import { offers } from './Constants/offers';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  title = 'Order Pizza';
  public size = Size;
  pizzaSizes = pizzaSizes;
  vegToppings = vegToppings;
  nonVegToppings = nonVegToppings;

  offers = offers;

  mediumSizeOffer: any = null;
  largeSizeOffer: any = null;
  showMediumSizeOffer = false;
  showLargeSizeOffer = false;

  smallSizePrice: number | undefined;
  mediumSizePrice: number | undefined;
  largeSizePrice: number | undefined;
  extraLargeSizePrice: number | undefined;

  pizzaForm!: FormGroup;
  constructor(private fb: FormBuilder) { }
  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.pizzaForm = this.fb.group({
      smallSize: this.fb.group({
        Tomatoes: [false],
        Onions: [false],
        Bellpepper: [false],
        Mushrooms: [false],
        Pineapple: [false],
        Sausage: [false],
        Pepperoni: [false],
        Barbecuechicken: [false],
        numberOfPizza:[1]
      }),
      mediumSize: this.fb.group({
        Tomatoes: [false],
        Onions: [false],
        Bellpepper: [false],
        Mushrooms: [false],
        Pineapple: [false],
        Sausage: [false],
        Pepperoni: [false],
        Barbecuechicken: [false],
        numberOfPizza:[1]
      }),
      largeSize: this.fb.group({
        Tomatoes: [false],
        Onions: [false],
        Bellpepper: [false],
        Mushrooms: [false],
        Pineapple: [false],
        Sausage: [false],
        Pepperoni: [false],
        Barbecuechicken: [false],
        numberOfPizza:[1]
      }),
      extraLargeSize: this.fb.group({
        Tomatoes: [false],
        Onions: [false],
        Bellpepper: [false],
        Mushrooms: [false],
        Pineapple: [false],
        Sausage: [false],
        Pepperoni: [false],
        Barbecuechicken: [false],
        numberOfPizza:[1]
      }),
    })
  }
  getSelectedToppings(formName: string): FormArray {
    const formGroup = this.pizzaForm.get(formName) as FormGroup;
    if (formGroup) {
      return formGroup.get('selectedToppings') as FormArray;
    } else {
      throw new Error(`FormGroup with name ${formName} does not exist`);
    }
  }
  calculatePrice(size: any) {
    switch (size) {
      case Size.Small:
        const smallSizeToppings = this.pizzaForm.value.smallSize;
        if (this.checkAnyToppingSelected(smallSizeToppings)){
          this.smallSizePrice = this.pizzaSizes.find(pizza => pizza.name === size)?.price;
          this.smallSizePrice ? this.smallSizePrice += this.getPrice(smallSizeToppings) : 0;
          if (this.smallSizePrice) this.smallSizePrice = this.smallSizePrice * smallSizeToppings.numberOfPizza;
        }
        break;
      case Size.Medium:
        const mediumSizeToppings = this.pizzaForm.value.mediumSize;
        if (this.checkAnyToppingSelected(mediumSizeToppings)){
          this.mediumSizePrice = this.pizzaSizes.find(pizza => pizza.name === size)?.price;
          this.mediumSizePrice ? this.mediumSizePrice += this.getPrice(mediumSizeToppings) : 0;
          if (this.mediumSizePrice) this.mediumSizePrice = this.mediumSizePrice * mediumSizeToppings.numberOfPizza
          this.getOffer(mediumSizeToppings, Size.Medium);
        }
        break;
      case Size.Large:
        const largeSizeToppings = this.pizzaForm.value.largeSize;
        if (this.checkAnyToppingSelected(largeSizeToppings)){
          this.largeSizePrice = this.pizzaSizes.find(pizza => pizza.name === size)?.price;
          this.largeSizePrice ? this.largeSizePrice += this.getPrice(largeSizeToppings) : 0;
          if (this.largeSizePrice) this.largeSizePrice = this.largeSizePrice * largeSizeToppings.numberOfPizza
          this.getOffer(largeSizeToppings, Size.Large);
        }
        break;
      case Size.ExtraLarge:
        const extraLargeSizeToppings = this.pizzaForm.value.extraLargeSize;
        if (this.checkAnyToppingSelected(extraLargeSizeToppings)){
          this.extraLargeSizePrice = this.pizzaSizes.find(pizza => pizza.name === size)?.price;
          this.extraLargeSizePrice ? this.extraLargeSizePrice += this.getPrice(extraLargeSizeToppings) : 0;
          if (this.extraLargeSizePrice) this.extraLargeSizePrice = this.extraLargeSizePrice * extraLargeSizeToppings.numberOfPizza
        }
        break;
      default:
        console.log("Unknown size selected.");
    }
  }

  getPrice(selectedToppings: any) {
    let price = 0;
    this.vegToppings.forEach(topping => {
      if (selectedToppings[topping.name]) {
        price += topping.price;
      }
    });

    this.nonVegToppings.forEach(topping => {
      if (selectedToppings[topping.name]) {
        price += topping.price;
      }
    });
    return price;
  }

  checkAnyToppingSelected(data: any){
    if (Object.values(data).filter(value => value === true).length > 0) {
      return true;
    }
    return false;
  }

  getOffer(data: any, size: any) {
    if (size == Size.Medium) {
      if (Object.values(data).filter(value => value === true).length === 2  && data.numberOfPizza == 1) {
        this.mediumSizeOffer = this.offers.find(x => x.name === 'Offer1');
        this.showMediumSizeOffer = true;
      }
      else if(Object.values(data).filter(value => value === true).length === 4  && data.numberOfPizza == 2){
        this.mediumSizeOffer = this.offers.find(x => x.name === 'Offer2');
        this.showMediumSizeOffer = true;
      }
      else {
        this.mediumSizeOffer = null;
        this.showMediumSizeOffer = false;
      }
    }

    if (size == Size.Large) {
      let totalToppingCount = 0;
      for (const [key, value] of Object.entries(data)) {
        if (value === true) {
          if (key === 'Barbecuechicken' || key === 'Pepperoni') {
            totalToppingCount += 2;
          } else {
            totalToppingCount += 1;
          }
        }
      }
      if (totalToppingCount === 4   && data.numberOfPizza == 1) {
        this.largeSizeOffer = this.offers.find(x => x.name === 'Offer3');
        this.showLargeSizeOffer = true;
      } 
      else {
        this.largeSizeOffer = null;
        this.showLargeSizeOffer = false;
      }
    }
  }
}
