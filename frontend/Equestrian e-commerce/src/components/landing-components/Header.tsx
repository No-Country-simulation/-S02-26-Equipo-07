import { Menu, X } from 'lucide-react';
import { useState } from 'react';

interface HeaderProps {
  onLoginClick: () => void;
  onRegisterClick: () => void;
}

export default function Header({ onLoginClick, onRegisterClick }: HeaderProps) {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  return (
    <header className="block w-full bg-white/95 backdrop-blur-sm shadow-sm z-50">
      <nav className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <div className="flex items-center">
            <span className="text-2xl font-bold text-gray-900">EquiStore</span>
          </div>

          <div className="hidden md:flex items-center space-x-8">
            <a href="#productos" className="text-gray-700 hover:text-gray-900 transition-colors duration-300">
              Productos
            </a>
            <a href="#categorias" className="text-gray-700 hover:text-gray-900 transition-colors duration-300">
              Categorías
            </a>
            <a href="#nosotros" className="text-gray-700 hover:text-gray-900 transition-colors duration-300">
              Nosotros
            </a>
            <a href="#contacto" className="text-gray-700 hover:text-gray-900 transition-colors duration-300">
              Contacto
            </a>
          </div>

          <div className="hidden md:flex items-center space-x-4">
            <button
              onClick={onLoginClick}
              className="px-4 py-2 text-gray-900 hover:text-gray-500 cursor-pointer transition-colors font-medium duration-300"
            >
              Login
            </button>
            <button
              onClick={onRegisterClick}
              className="px-6 py-2 bg-gray-900 text-white rounded-lg hover:bg-gray-700 cursor-pointer transition-colors font-medium duration-300"
            >
              Register
            </button>
          </div>

          <button
            className="md:hidden"
            onClick={() => setIsMenuOpen(!isMenuOpen)}
          >
            {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
          </button>
        </div>

        {isMenuOpen && (
          <div className="md:hidden py-4 space-y-4">
            <a
              href="#productos"
              className="block text-gray-700 hover:text-gray-900 transition-colors duration-300"
            >
              Productos
            </a>
            <a
              href="#categorias"
              className="block text-gray-700 hover:text-gray-900 transition-colors duration-300"
            >
              Categorías
            </a>
            <a
              href="#nosotros"
              className="block text-gray-700 hover:text-gray-900 transition-colors duration-300"
            >
              Nosotros
            </a>
            <a
              href="#contacto"
              className="block text-gray-700 hover:text-gray-900 transition-colors duration-300"
            >
              Contacto
            </a>
            <div className="flex flex-col space-y-2 pt-4 border-t">
              <button
                onClick={onLoginClick}
                className="px-4 py-2 text-gray-900 border border-gray-900 rounded-lg hover:bg-amber-50 transition-colors font-medium"
              >
                Login
              </button>
              <button
                onClick={onRegisterClick}
                className="px-4 py-2 bg-gray-900 text-white rounded-lg hover:bg-gray-800 transition-colors font-medium"
              >
                Register
              </button>
            </div>
          </div>
        )}
      </nav>
    </header>
  );
}