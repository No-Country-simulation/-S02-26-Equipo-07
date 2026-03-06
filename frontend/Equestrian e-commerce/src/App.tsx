import { useState } from 'react';
import { Routes, Route } from 'react-router';
import Landing from './pages/landing';
import HomePage from './pages/home' ;
import CartPage from './pages/cart' ;
import ShopPage from './pages/shop';
import CheckoutPage from './pages/checkout'
import Login from './components/Login';
import Register from './components/Register';
import Product from './pages/product';
import { CartProvider } from './context/CartContext';


function App() {
  const [loginOpen, setLoginOpen] = useState(false);
  const [registerOpen, setRegisterOpen] = useState(true);
  return (
    <CartProvider>
      <div className="min-h-screen bg-linear-to-br from-amber-50 to-orange-50">
        <main>
          <Routes>
            <Route path="/"            element={<Landing />} />
            <Route path="/home"        element={<HomePage />} />
            <Route path="/product"     element={<ShopPage />} />
            <Route path="/product/:id" element={<Product />} />
            <Route path="/cart"        element={<CartPage />} />
            <Route path="/checkout"    element={<CheckoutPage />} />
          </Routes>
        </main>
      </div>
    </CartProvider>
  );
}

export default App;
