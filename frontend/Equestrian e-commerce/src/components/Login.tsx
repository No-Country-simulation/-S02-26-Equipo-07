import { useState } from 'react';
import { X, Mail, Lock } from 'lucide-react';

interface LoginProps {
  isOpen: boolean;
  onClose: () => void;
}

const Login = ({ isOpen, onClose }: LoginProps) => {
  const [formData, setFormData] = useState({
    usuario: '',
    contraseña: ''
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log('Login:', formData);
    setFormData({ usuario: '', contraseña: '' });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-4xl w-full max-h-[90vh] overflow-hidden">
        <button
          onClick={onClose}
          className="absolute top-4 right-4 p-2 hover:bg-gray-100 rounded-lg transition-colors z-10"
        >
          <X size={24} className="text-gray-600" />
        </button>

        <div className="grid grid-cols-1 md:grid-cols-2 h-full">
          <div className="md:block bg-linear-to-br from-amber-400 to-orange-500 p-8 flex flex-col justify-center">
            <img
              src="https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=500"
              alt="Caballo"
              className="rounded-lg object-cover w-full h-64 shadow-lg"
            />
            <h2 className="text-white text-2xl font-bold mt-6">Bienvenido</h2>
            <p className="text-white mt-2 opacity-90">
              Accede a tu cuenta para explorar nuestros mejores caballos.
            </p>
          </div>

          <div className="p-8 flex flex-col justify-center">
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
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 transition-all"
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
                    name="contraseña"
                    value={formData.contraseña}
                    onChange={handleChange}
                    placeholder="Tu contraseña"
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-amber-500 transition-all"
                    required
                  />
                </div>
              </div>

              <div className="text-right">
                <a href="#" className="text-sm text-amber-600 hover:text-amber-700 font-medium">
                  ¿Olvidaste tu contraseña?
                </a>
              </div>

              <button
                type="submit"
                className="w-full bg-amber-500 hover:bg-amber-600 text-white font-medium py-2 rounded-lg transition-colors mt-6"
              >
                Iniciar Sesión
              </button>
            </form>

            <p className="text-center text-gray-600 text-sm mt-4">
              ¿No tienes cuenta? <a href="#" className="text-amber-600 hover:text-amber-700 font-medium">Regístrate aquí</a>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
export default Login