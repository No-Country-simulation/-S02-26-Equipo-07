import { createContext, useContext, useState } from 'react';
import type { Product } from '../types/product';

interface CartItem { product: Product; size: string; quantity: number; }
interface CartContextType {
  cart: CartItem[];
  addToCart: (product: Product, size: string, qty: number) => void;
  removeFromCart: (id: number) => void;
  updateQty: (id: number, qty: number) => void;
  updateSize: (id: number, size: string) => void;
  clearCart: () => void;
}

const CartContext = createContext<CartContextType | null>(null);
export const useCart = () => useContext(CartContext)!;

export function CartProvider({ children }: { children: React.ReactNode }) {
  const [cart, setCart] = useState<CartItem[]>([]);

  const addToCart = (product: Product, size: string, quantity: number) =>
    setCart(prev => {
      const exists = prev.find(i => i.product.id === product.id && i.size === size);
      if (exists) return prev.map(i =>
        i.product.id === product.id && i.size === size
          ? { ...i, quantity: i.quantity + quantity } : i);
      return [...prev, { product, size, quantity }];
    });

  const removeFromCart = (id: number) =>
    setCart(prev => prev.filter(i => i.product.id !== id));

  const updateQty = (id: number, qty: number) =>
    setCart(prev => prev.map(i => i.product.id === id ? { ...i, quantity: qty } : i));

  const updateSize = (id: number, size: string) =>
    setCart(prev => prev.map(i => i.product.id === id ? { ...i, size } : i));

  const clearCart = (): void => setCart([]);

  return (
    <CartContext.Provider value={{ cart, addToCart, removeFromCart, updateQty, updateSize, clearCart }}>
      {children}
    </CartContext.Provider>
  );
}
