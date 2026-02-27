export interface Product {
  id: number;
  name: string;
  price: number;
  stars: number;
  image?: string;
  description?: string;
  images?: string[];
  availableSizes?: ('XS' | 'S' | 'M' | 'L' | 'XL')[];
}