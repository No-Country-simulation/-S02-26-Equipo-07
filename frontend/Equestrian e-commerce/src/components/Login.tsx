import { useState } from 'react';
import { X, Mail, Lock } from 'lucide-react';
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
    <div className="fixed inset-0 bg-black flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-4xl w-full max-h-[90vh] overflow-hidden">
        <button
          onClick={onClose}
          className="absolute top-4 right-4 p-2 hover:bg-gray-100 rounded-lg transition-colors z-10"
        >
          <X size={24} className="text-gray-600" />
        </button>

        <div className="grid grid-cols-1 md:grid-cols-2 h-full">
          <div className="md:block bg-linear-to-br from-gray-700 to-gray-800 p-8 flex flex-col justify-center">
            <img
              src="https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=500"
              alt="Caballo"
              className="rounded-lg object-cover w-full h-64 shadow-lg"
            />
            <h2 className="text-white text-2xl font-bold mt-6">Bienvenido</h2>
            <p className="text-white mt-2 opacity-90">
              Accede a tu cuenta para disfrutar de las mejores ofertas.
            </p>
          </div>

          <div className="p-8 flex flex-col justify-center">
            <h1 className="text-3xl font-bold text-gray-900 mb-2">Iniciar Sesión</h1>
            <p className="text-gray-600 mb-6">Ingresa tus datos para continuar</p>

            <form onSubmit={handleSubmit} className="space-y-4">
              {error && (
                <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
                  {error}
                </div>
              )}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Usuario
                </label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 text-gray-400" size={20} />
                  <input
                    type="text"
                    name="username"
                    value={formData.username}
                    onChange={handleChange}
                    placeholder="Tu usuario"
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500 transition-all"
                    required
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Contraseña
                </label>
                <div className="relative">
                  <Lock className="absolute left-3 top-3 text-gray-400" size={20} />
                  <input
                    type="password"
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    placeholder="Tu contraseña"
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500 transition-all"
                    required
                  />
                </div>
              </div>

              <div className="text-right">
                <a href="#" className="text-sm text-gray-700 hover:text-gray-800 font-medium">
                  ¿Olvidaste tu contraseña?
                </a>
              </div>

              <button
                type="submit"
                disabled={loading}
                className="w-full bg-gray-600 hover:bg-gray-700 text-white font-medium py-2 rounded-lg transition-colors mt-6 disabled:opacity-50"
              >
                {loading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
              </button>
            </form>

            <p className="text-center text-gray-600 text-sm mt-4">
              ¿No tienes cuenta? <button onClick={onOpenRegister} className="text-gray-700 hover:text-gray-800 font-medium">Regístrate aquí</button>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
export default Login