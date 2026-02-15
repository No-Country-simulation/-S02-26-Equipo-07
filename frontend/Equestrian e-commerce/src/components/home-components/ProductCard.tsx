import { Star } from 'lucide-react';
import type { Product } from './types/product';

interface ProductCardProps {
  product: Product;
}

export default function ProductCard({ product }: ProductCardProps) {
  const renderStars = () => {
    return Array.from({ length: 5 }, (_, index) => (
      <Star
        key={index}
        size={16}
        className={` ${
          index < product.stars
            ? 'fill-yellow-400 text-yellow-400'
            : 'fill-gray-200 text-gray-200'
        }`}
      />
    ));
  };

  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-xl transition-shadow duration-300">
      <div className="aspect-square bg-linear-to-br from-amber-50 to-orange-100 flex items-center justify-center">
        <img
          src={product.image || `./casco.jpg`}
          alt={product.name}
          className="w-full h-full object-cover"
        />
      </div>
      <div className="p-4">
        <div className="flex justify-center items-center gap-1 mb-3">
          {renderStars()}
        </div>
        <h3 className="font-semibold text-center text-gray-800 text-lg mb-2 line-clamp-2 min-h-14">
          {product.name}
        </h3>
        <p className="text-2xl text-center font-bold text-gray-800">
          ${product.price.toLocaleString()}
        </p>
      </div>
    </div>
  );
}