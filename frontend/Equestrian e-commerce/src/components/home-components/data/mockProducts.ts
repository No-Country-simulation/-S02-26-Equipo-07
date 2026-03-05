import type { Product } from '../../../types/product';
import Image1 from '../casco.jpg';

export const mockProducts: Product[] = [
  {
    id: 1,
    name: 'Casco',
    Category: "rider", //Esto puede ser unicamente rider y horse
    price: 15000,
    amount: 3,
    stars: 5,
    description: 'Caballo árabe puro de excelente pedigrí. Ideal para doma clásica y competiciones. Temperamento dócil y elegante.',
    image: Image1,
    images: [
      Image1,
      'https://images.pexels.com/photos/2317904/pexels-photo-2317904.jpeg?auto=compress&cs=tinysrgb&w=500',
      Image1
    ],
    availableSizes: ['S', 'M', 'L']
  },
  {
    id: 2,
    name: 'Casco',
    price: 12000,
    stars: 4,
    description: 'Yegua Quarter Horse con excelente conformación. Perfecta para trabajo en hacienda y competiciones.',
    image: Image1,
    images: [
      Image1,
      'https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=500'
    ],
    availableSizes: ['M', 'L', 'XL']
  },
  {
    id: 3,
    name: 'Casco',
    price: 18000,
    stars: 5,
    description: 'Potro andaluz de tres años con características excepcionales. Ideal para iniciación en doma.',
    image: Image1,
    images: [
      Image1,
      Image1,
      Image1
    ],
    availableSizes: ['XS', 'S', 'M']
  },
  { id: 4, name: 'Casco', image: Image1, price: 25000, stars: 5 },
  { id: 5, name: 'Casco', image: Image1, price: 9500, stars: 4 },
  { id: 6, name: 'Casco', image: Image1, price: 8000, stars: 4 },
  { id: 7, name: 'Casco', image: Image1, price: 3500, stars: 5 },
  { id: 8, name: 'Casco', image: Image1, price: 11000, stars: 4 },
  { id: 9, name: 'Casco', image: Image1, price: 14000, stars: 5 },
  { id: 10, name: 'Casco', image: Image1, price: 13500, stars: 4 },
  { id: 11, name: 'Casco', image: Image1, price: 7500, stars: 3 },
  { id: 12, name: 'Casco', image: Image1, price: 16000, stars: 4 },
  { id: 13, name: 'Casco', image: Image1, price: 19000, stars: 5 },
  { id: 14, name: 'Casco', image: Image1, price: 10500, stars: 4 },
  { id: 15, name: 'Casco', image: Image1, price: 22000, stars: 5 },
  { id: 16, name: 'Casco', image: Image1, price: 8500, stars: 4 },
  { id: 17, name: 'Casco', image: Image1, price: 13000, stars: 4 },
  { id: 18, name: 'Casco', image: Image1, price: 20000, stars: 5 },
  { id: 19, name: 'Casco', image: Image1, price: 17500, stars: 5 },
  { id: 20, name: 'Casco', image: Image1, price: 24000, stars: 5 },
  { id: 21, name: 'Casco', image: Image1, price: 9000, stars: 3 },
  { id: 22, name: 'Casco', image: Image1, price: 21000, stars: 5 },
  { id: 23, name: 'Casco', image: Image1, price: 4500, stars: 4 },
  { id: 24, name: 'Casco', image: Image1, price: 28000, stars: 5 },
  { id: 25, name: 'Yegua Marwari', price: 15500, stars: 4 },
];

