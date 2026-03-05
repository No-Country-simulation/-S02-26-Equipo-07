import { useState } from 'react';
import { Routes, Route } from 'react-router';
import Landing from './pages/landing';
import HomePage from './pages/home';
import Login from './components/Login';
import Register from './components/Register';
import Product from './pages/product';


function App() {
  const [loginOpen, setLoginOpen] = useState(false);
  const [registerOpen, setRegisterOpen] = useState(false);
  
  const isLoggedIn = !!localStorage.getItem('token');
  
  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('role');
    window.location.reload();
  };
  
  return (
    <div className="min-h-screen bg-linear-to-br from-amber-50 to-orange-50">
      <main>
        <Routes>
          {/* Definimos qué componente se ve en la raíz */}
          <Route path="/" element={<Landing />} />
          
          {/* Definimos la ruta para el producto con un parámetro dinámico :id */}
          <Route path="/product/:id" element={<Product />} />
        </Routes>
      </main>
      
      <Login 
        isOpen={loginOpen} 
        onClose={() => setLoginOpen(false)} 
        onOpenRegister={() => {
          setLoginOpen(false);
          setRegisterOpen(true);
        }} 
      />
      <Register 
        isOpen={registerOpen} 
        onClose={() => setRegisterOpen(false)}
        onOpenLogin={() => {
          setRegisterOpen(false);
          setLoginOpen(true);
        }}
      />
    </div>
  );
}

export default App;
