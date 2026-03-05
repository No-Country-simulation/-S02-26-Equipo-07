import { useState } from 'react';
import { X } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

interface LoginProps {
  isOpen: boolean;
  onClose: () => void;
  onOpenRegister: () => void;
}

interface LoginResponse {
  token: string;
  username: string;
  role: string;
  expiresAt: string;
}

const Login = ({ isOpen, onClose, onOpenRegister }: LoginProps) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    username: '',
    password: ''
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
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/Auth/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          username: formData.username,
          password: formData.password
        }),
      });

      if (!response.ok) {
        throw new Error('Usuario o contraseña incorrectos');
      }

      const data: LoginResponse = await response.json();
      
      localStorage.setItem('token', data.token);
      localStorage.setItem('username', data.username);
      localStorage.setItem('role', data.role);
      
      setFormData({ username: '', password: '' });
      onClose();
      navigate('/');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error al iniciar sesión');
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 flex bg-gray-950/60 items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-4xl w-full max-h-[90vh] overflow-hidden">

        <div className="w-full max-w-md">
          <div className="flex items-center gap-2 mb-8">
            <span className="text-xl font-semibold text-zinc-900">Login</span>
          </div>

          <div className="p-8 flex flex-col justify-center">
            <button
              onClick={onClose}
              className="absolute top-24 right-60 p-2 hover:bg-gray-100 rounded-lg transition-colors z-10"
            >
              <X size={24} className="text-gray-600" />
            </button>
            <h1 className="text-3xl font-bold text-gray-900 mb-2">Iniciar Sesión</h1>
            <p className="text-gray-600 mb-6">Ingresa tus datos para continuar</p>

            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Usuario
                </label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 text-gray-400" size={20} />
                  <input
                    type="text"
                    name="usuario"
                    value={formData.usuario}
                    onChange={handleChange}
                    placeholder="Tu usuario o email"
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500 transition-all"
                    required
                  />
                </div>
              </div>

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

          <div className="flex items-center gap-4 mb-10">
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
            <div className="mb-6">
              <input 
                type="text" 
                name="username"
                placeholder="Usuario" 
                value={formData.username}
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
              <div className="mt-3 text-right">
                <a href="#" className="text-sm text-zinc-600 hover:text-zinc-900 hover:underline">
                  ¿Olvidaste tu contraseña?
                </a>
              </div>
            </div>

            <div className="mb-8">
              <label className="flex gap-3 cursor-pointer items-start">
                <input type="checkbox" className="w-5 h-5 accent-black mt-0.5" />
                <span className="text-sm text-zinc-600">
                  Manténme conectado — se aplica a todas las opciones de inicio de sesión.<br />
                  <a href="#" className="text-zinc-500 hover:underline">Más info</a>
                </span>
              </label>
            </div>

            <button 
              type="submit"
              disabled={loading}
              className="w-full bg-black hover:bg-zinc-900 transition-colors text-white font-semibold text-lg py-4 rounded-lg flex items-center justify-center gap-3 disabled:opacity-50"
            >
              {loading ? 'INICIANDO SESIÓN...' : 'INICIAR SESIÓN'}
              <span className="text-3xl leading-none">→</span>
            </button>
          </form>

          <p className="mt-6 text-center text-sm text-zinc-500">
            Al hacer clic en "Iniciar sesión" aceptas nuestros 
            <a href="#" className="text-zinc-700 hover:underline">Términos y Condiciones</a>, 
            <a href="#" className="text-zinc-700 hover:underline">Aviso de Privacidad</a> y 
            <a href="#" className="text-zinc-700 hover:underline">Términos y Condiciones</a>.
          </p>

          <p className="text-center text-zinc-600 text-sm mt-6">
            ¿No tienes cuenta?{' '}
            <button onClick={() => { onClose(); onOpenRegister(); }} className="text-zinc-900 hover:underline font-semibold">
              Regístrate aquí
            </button>
          </p>
        </div>
      </div>

      <div className="hidden lg:block lg:w-7/12 relative bg-zinc-900">
        <img 
          src="https://thumbs.dreamstime.com/b/equestrian-rider-jumps-over-hurdle-horse-competition-outdoors-equestrian-rider-horse-jumping-over-hurdle-332311558.jpg" 
          alt="Jinete mujer saltando obstáculo caballo arena"
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

export default Login;
