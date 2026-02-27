import { useState } from 'react';
import { Routes, Route, Link } from 'react-router';
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
      <header className="bg-white shadow-sm border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex justify-between items-center">
          <Link to="/" className="flex flex-col">
            <h1 className="text-2xl font-bold text-gray-900">
              Equestrian Gear
            </h1>
            <p className="text-gray-600 text-sm">
              Productos para jinetes y caballos
            </p>
          </Link>
          
          <nav className="flex gap-4">
            {isLoggedIn ? (
              <>
                <span className="text-gray-700 font-medium">
                  Hola, {localStorage.getItem('username')}
                </span>
                <button
                  onClick={handleLogout}
                  className="text-gray-700 hover:text-gray-900 font-medium"
                >
                  Cerrar sesión
                </button>
              </>
            ) : (
              <>
                <button
                  onClick={() => setRegisterOpen(true)}
                  className="text-gray-700 hover:text-gray-900 font-medium"
                >
                  Regístrate
                </button>
                <button
                  onClick={() => setLoginOpen(true)}
                  className="bg-gray-700 hover:bg-gray-800 text-white px-4 py-2 rounded-lg font-medium"
                >
                  Iniciar Sesión
                </button>
              </>
            )}
          </nav>
        </div>
      </header>
      <main>
        <Routes>
          <Route path="/" element={<HomePage />} />
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
