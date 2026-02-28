import { useNavigate } from 'react-router-dom';
import { Star, User, Menu, X } from 'lucide-react';
import { useState } from 'react';

interface NavbarProps {
  onLoginClick: () => void;
}

export default function Navbar({ onLoginClick }: NavbarProps) {
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');

  const handleSearch = () => {
    if (searchQuery.trim()) {
      console.log('Búsqueda especial:', searchQuery);
      setSearchQuery('');
    }
  };

  return (
    <nav className="bg-white shadow-md border-b border-gray-200 sticky top-0 z-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          <button
            onClick={() => navigate('/')}
            className="flex items-center gap-2 hover:opacity-80 transition-opacity"
          >
            <div className="text-2xl font-bold text-amber-600">🐴</div>
            <span className="hidden sm:inline text-lg font-bold text-gray-900">
              Premium Horses
            </span>
          </button>

          <div className="hidden md:flex items-center gap-8">
            <button
              onClick={() => navigate('/')}
              className="text-gray-700 hover:text-amber-600 font-medium transition-colors"
            >
              Home
            </button>
            <button
              onClick={() => navigate('/')}
              className="text-gray-700 hover:text-amber-600 font-medium transition-colors"
            >
              Productos
            </button>

            <div className="flex items-center gap-2 bg-gray-100 rounded-lg px-3 py-2">
              <input
                type="text"
                placeholder="Búsqueda especial..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
                className="bg-transparent outline-none text-sm flex-1 placeholder-gray-500"
              />
              <button
                onClick={handleSearch}
                className="text-amber-600 hover:text-amber-700 transition-colors"
              >
                <Star size={18} />
              </button>
            </div>
          </div>

          <div className="flex items-center gap-4">
            <button
              onClick={onLoginClick}
              className="hidden sm:flex items-center gap-2 text-gray-700 hover:text-amber-600 transition-colors"
            >
              <User size={20} />
              <span className="hidden md:inline text-sm font-medium">Perfil</span>
            </button>

            <button
              onClick={() => setIsMenuOpen(!isMenuOpen)}
              className="md:hidden text-gray-700 hover:text-amber-600"
            >
              {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
            </button>
          </div>
        </div>

        {isMenuOpen && (
          <div className="md:hidden pb-4 space-y-3">
            <button
              onClick={() => {
                navigate('/');
                setIsMenuOpen(false);
              }}
              className="block w-full text-left px-4 py-2 text-gray-700 hover:bg-amber-50 rounded-lg"
            >
              Home
            </button>
            <button
              onClick={() => {
                navigate('/');
                setIsMenuOpen(false);
              }}
              className="block w-full text-left px-4 py-2 text-gray-700 hover:bg-amber-50 rounded-lg"
            >
              Productos
            </button>
            <div className="px-4 py-2">
              <div className="flex items-center gap-2 bg-gray-100 rounded-lg px-3 py-2">
                <input
                  type="text"
                  placeholder="Búsqueda..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
                  className="bg-transparent outline-none text-sm flex-1"
                />
                <button
                  onClick={handleSearch}
                  className="text-amber-600"
                >
                  <Star size={18} />
                </button>
              </div>
            </div>
            <button
              onClick={() => {
                onLoginClick();
                setIsMenuOpen(false);
              }}
              className="block w-full text-left px-4 py-2 text-gray-700 hover:bg-amber-50 rounded-lg flex items-center gap-2"
            >
              <User size={18} />
              Perfil
            </button>
          </div>
        )}
      </div>
    </nav>
  );
}
