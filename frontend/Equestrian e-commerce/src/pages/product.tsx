import { useNavigate, useParams } from 'react-router'; // 1. Importamos useParams
import ProductView from '../components/product-components/Product';
import { mockProducts } from '../components/home-components/data/mockProducts'; // 2. IMPORTANTE: Importa tus datos reales
import type { Product as ProductType } from '../types/product';

export default function Product() {
  // 3. Obtenemos el "id" que definimos en App.tsx (:id)
  const { id } = useParams<{ id: string }>();
  const productId = id ? parseInt(id, 10) : null;
  const navigate = useNavigate();

  // 4. Buscamos el producto en el array de datos usando ese ID
  const product = mockProducts.find((p) => p.id === productId);

  // 5. ¡Súper importante! Si no hay producto, mostramos algo para evitar el crash
  if (!product) {
    return (
      <div className="min-h-screen flex flex-col items-center justify-center bg-amber-50">
        <h2 className="text-2xl font-bold text-gray-800 mb-4">¡Ups! Producto no encontrado</h2>
        <button
          onClick={() => navigate('/')}
          className="bg-gray-600 text-white px-6 py-2 rounded-lg"
        >
          Volver a la tienda
        </button>
      </div>
    );
  }

  // 6. Si existe, lo pasamos al ProductView (ahora ya no es undefined)
  return (
    <div className="min-h-screen bg-linear-to-br from-amber-50 to-orange-50 py-8 px-4">
      <div className="max-w-7xl mx-auto">
        <button
          onClick={() => navigate('/')}
          className="inline-flex items-center text-gray-800 hover:text-gray-900 font-medium mb-8 transition-colors"
        >
          ← Volver a productos
        </button>
        <ProductView product={product} />
      </div>
    </div>
  );
}