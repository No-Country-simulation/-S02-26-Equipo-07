import { useState } from 'react';
import { X } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

interface RegisterProps {
  isOpen: boolean;
  onClose: () => void;
  onOpenLogin: () => void;
}

interface RegisterResponse {
  token: string;
  username: string;
  role: string;
  expiresAt: string;
}

const Register = ({ isOpen, onClose, onOpenLogin }: RegisterProps) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    role: 'user'
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Auth/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          username: formData.username,
          email: formData.email,
          password: formData.password,
          role: formData.role
        }),
      });

      if (!response.ok) {
        throw new Error('Error al registrar usuario');
      }

      const data: RegisterResponse = await response.json();
      
      localStorage.setItem('token', data.token);
      localStorage.setItem('username', data.username);
      localStorage.setItem('role', data.role);
      
      setFormData({ username: '', email: '', password: '', role: 'user' });
      onClose();
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error al registrar');
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex overflow-hidden">
      <div className="w-full lg:w-5/12 flex items-center justify-center p-8 lg:p-16 bg-white relative">
        <button
          onClick={onClose}
          className="absolute top-4 right-4 p-2 hover:bg-zinc-100 rounded-full transition-colors"
        >
          <X size={24} className="text-zinc-600" />
        </button>

        <div className="w-full max-w-md">
          <div className="flex items-center gap-2 mb-8">
            <span className="text-xl font-semibold text-zinc-900">Register</span>
          </div>

          <h1 className="text-5xl font-semibold tracking-tighter text-zinc-900 mb-2">Regístrate</h1>
          <p className="text-zinc-600 text-lg mb-10">Inicia sesión con</p>

          <div className="grid grid-cols-3 gap-4 mb-10">
            <button className="flex items-center justify-center gap-3 border border-zinc-300 hover:border-zinc-400 rounded-lg py-4 transition-all">
              <img src="https://www.google.com/images/branding/googleg/1x/googleg_standard_color_128dp.png" alt="Google" className="w-6 h-6" />
            </button>
            <button className="flex items-center justify-center gap-3 bg-black text-white hover:bg-zinc-900 rounded-lg py-4 transition-all">
              <i className="fab fa-apple text-3xl"></i>
            </button>
            <button className="flex items-center justify-center gap-3 bg-[#1877F2] text-white hover:bg-[#1662cc] rounded-lg py-4 transition-all">
              <i className="fab fa-facebook-f text-2xl"></i>
            </button>
          </div>

          <div className="flex items-center gap-4 mb-8">
            <div className="flex-1 h-px bg-zinc-200"></div>
            <span className="text-zinc-400 font-medium text-sm">O</span>
            <div className="flex-1 h-px bg-zinc-200"></div>
          </div>

          {error && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-xl mb-6">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <div className="mb-8">
              <label className="block text-sm font-semibold text-zinc-700 mb-2">Tu Nombre</label>
              <input 
                type="text" 
                name="username"
                placeholder="Nombre de usuario" 
                value={formData.username}
                onChange={handleChange}
                className="w-full px-5 py-3 border border-zinc-300 rounded-lg focus:outline-none focus:border-zinc-900 text-base" 
                required 
              />
            </div>

            <div className="mb-8">
              <label className="block text-sm font-semibold text-zinc-700 mb-2">Detalles de inicio de sesión</label>
              <input 
                type="email" 
                name="email"
                placeholder="Correo electrónico" 
                value={formData.email}
                onChange={handleChange}
                className="w-full px-5 py-3 border border-zinc-300 rounded-lg focus:outline-none focus:border-zinc-900 mb-4 text-base" 
                required 
              />
              <input 
                type="password" 
                name="password"
                placeholder="Contraseña" 
                value={formData.password}
                onChange={handleChange}
                className="w-full px-5 py-3 border border-zinc-300 rounded-lg focus:outline-none focus:border-zinc-900 text-base" 
                required 
              />
              <p className="text-xs text-zinc-500 mt-3">
                Mínimo 8 caracteres con al menos una mayúscula, una minúscula, un carácter especial y un número
              </p>
            </div>

            <div className="space-y-5 mb-10">
              <label className="flex gap-3 cursor-pointer">
                <input type="checkbox" className="w-5 h-5 accent-black mt-0.5" required />
                <span className="text-sm leading-tight text-zinc-600">
                  Al hacer clic en 'Registrarse' aceptas los Términos y Condiciones de nuestro sitio web, Aviso de Privacidad y Términos y Condiciones.
                </span>
              </label>
              <label className="flex gap-3 cursor-pointer">
                <input type="checkbox" className="w-5 h-5 accent-black mt-0.5" />
                <span className="text-sm text-zinc-600">
                  Manténme conectado — se aplica a todas las opciones de inicio de sesión.
                </span>
              </label>
            </div>

            <button 
              type="submit"
              disabled={loading}
              className="w-full bg-black hover:bg-zinc-900 transition-colors text-white font-semibold text-lg py-4 rounded-lg flex items-center justify-center gap-3 disabled:opacity-50"
            >
              {loading ? 'CREANDO CUENTA...' : 'REGISTRARSE'}
              <span className="text-3xl leading-none">→</span>
            </button>
          </form>
        </div>
      </div>

      <div className="hidden lg:block lg:w-7/12 relative bg-zinc-900">
        <img 
          src="https://images.pexels.com/photos/8748624/pexels-photo-8748624.jpeg" 
          alt="Jinete mujer caballo arena" 
          className="absolute inset-0 w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-gradient-to-r from-black/20 to-transparent"></div>
        <div className="absolute top-12 right-12">
          <h2 className="text-white text-8xl font-black tracking-[-6px] drop-shadow-2xl">LOGO</h2>
        </div>
      </div>
    </div>
  );
};

export default Register;
