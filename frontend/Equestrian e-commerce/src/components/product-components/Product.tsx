import { useState } from 'react';
import { Star, Minus, Plus, ShoppingCart } from 'lucide-react';
import type { Product } from '../../types/product';
import { useCart } from '../../context/CartContext';

interface ProductViewProps {
  product: Product;
}

const SIZES = ['XS', 'S', 'M', 'L', 'XL'] as const;

export default function ProductView({ product }: ProductViewProps) {
  const [quantity, setQuantity] = useState(1);
  const [selectedSize, setSelectedSize] = useState<string | null>(null);
  const [mainImage, setMainImage] = useState(0);

  const images = product.images || [
    'https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=600'
  ];

  const { addToCart } = useCart();

  const handleAddToCart = () => {
    if (!selectedSize) {
      alert('Por favor selecciona un tamaño');
      return;
    }
    addToCart(product, selectedSize!, quantity);
    alert(`Producto agregado: ${quantity} x ${product.name} (Tamaño: ${selectedSize})`);
  };

  const handleQuantityChange = (delta: number) => {
    setQuantity(prev => Math.max(1, prev + delta));
  };

  const isSizeAvailable = (size: string) => {
    return product.availableSizes?.includes(size as any) ?? true;
  };

  const renderStars = (rating: number) => {
    return (
      <div className="flex items-center gap-1">
        {Array.from({ length: 5 }).map((_, i) => (
          <Star
            key={i}
            size={20}
            className={i < rating ? 'fill-yellow-400 text-amber-400' : 'text-gray-300'}
          />
        ))}
        <span className="ml-2 text-sm text-gray-600">({rating}/5)</span>
      </div>
    );
  };

  return (
    <div className="bg-white rounded-lg shadow-lg overflow-hidden">
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 p-6 lg:p-10">
        <div className="flex gap-4">
          <div className="flex flex-col gap-3 order-2 lg:order-1">
            {images.map((img, idx) => (
              <button
                key={idx}
                onClick={() => setMainImage(idx)}
                className={`w-16 h-16 rounded-lg overflow-hidden border-2 transition-all ${
                  mainImage === idx
                    ? 'border-gray-800'
                    : 'border-gray-200 hover:border-gray-300'
                }`}
              >
                <img
                  src={img}
                  alt={`Thumbnail ${idx + 1}`}
                  className="w-full h-full object-cover"
                />
              </button>
            ))}
          </div>

          <div className="flex-1 order-1 lg:order-2">
            <div className="aspect-square bg-gray-100 rounded-lg overflow-hidden">
              <img
                src={images[mainImage]}
                alt={product.name}
                className="w-full h-full object-cover"
              />
            </div>
          </div>
        </div>

        <div className="flex flex-col justify-between">
          <div>
            <h2 className="text-3xl font-bold text-gray-900 mb-4">
              {product.name}
            </h2>

            <h3 className="text-2xl font-bold text-gray-800 mb-4">
              ${product.price.toLocaleString()}
            </h3>

            <div className="mb-6">
              {renderStars(product.stars)}
            </div>

            <div className="mb-6">
              <h4 className="text-sm font-semibold text-gray-900 mb-3">
                Tamaño
              </h4>
              <div className="flex flex-wrap gap-2">
                {SIZES.map(size => {
                  const available = isSizeAvailable(size);
                  return (
                    <button
                      key={size}
                      onClick={() => available && setSelectedSize(size)}
                      disabled={!available}
                      className={`px-4 py-2 rounded-lg font-medium transition-all ${
                        !available
                          ? 'bg-gray-100 text-gray-400 cursor-not-allowed'
                          : selectedSize === size
                          ? 'bg-gray-800 text-white'
                          : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                      }`}
                    >
                      {size}
                    </button>
                  );
                })}
              </div>
            </div>

            <div className="mb-6">
              <h4 className="text-sm font-semibold text-gray-900 mb-3">
                Cantidad
              </h4>
              <div className="flex items-center gap-3">
                <button
                  onClick={() => handleQuantityChange(-1)}
                  className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
                >
                  <Minus size={20} className="text-gray-600" />
                </button>
                <span className="text-lg font-semibold min-w-12 text-center">
                  {quantity}
                </span>
                <button
                  onClick={() => handleQuantityChange(1)}
                  className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
                >
                  <Plus size={20} className="text-gray-600" />
                </button>
              </div>
            </div>

            <button
              onClick={handleAddToCart}
              className="w-full bg-gray-700 hover:bg-gray-600 text-white font-semibold py-3 rounded-lg transition-colors flex items-center justify-center gap-2 mb-4"
            >
              <ShoppingCart size={20} />
              Agregar al Carrito
            </button>
          </div>

          {product.description && (
            <div className="pt-6 border-t border-gray-200">
              <h4 className="text-sm font-semibold text-gray-900 mb-2">
                Descripción
              </h4>
              <p className="text-gray-600 leading-relaxed">
                {product.description}
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
