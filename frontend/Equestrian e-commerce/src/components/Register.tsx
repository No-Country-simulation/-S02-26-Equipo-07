import { useState } from 'react';
import { X, User, Mail, Lock } from 'lucide-react';

interface RegisterProps {
  isOpen: boolean;
  onClose: () => void;
}

const Register = ({ isOpen, onClose }: RegisterProps) => {
  const [formData, setFormData] = useState({
    nombre: '',
    apellido: '',
    email: '',
    contraseña: '',
    terminosAceptados: false,
    recibirNovedades: false
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.terminosAceptados) {
      alert('Debes aceptar los términos y condiciones');
      return;
    }
    console.log('Register:', formData);
    setFormData({
      nombre: '',
      apellido: '',
      email: '',
      contraseña: '',
      terminosAceptados: false,
      recibirNovedades: false
    });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        <div className="sticky top-0 bg-white flex items-center justify-between p-6 border-b border-gray-200">
          <h1 className="text-3xl font-bold text-gray-900">Crear Cuenta</h1>
          <button
            onClick={onClose}
            className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
          >
            <X size={24} className="text-gray-600" />
          </button>
        </div>

        <div className="p-8">
          <p className="text-gray-600 mb-6">Completa los siguientes datos para registrarte</p>

          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Nombre *
                </label>
                <div className="relative">
                  <User className="absolute left-3 top-3 text-gray-400" size={20} />
                  <input
                    type="text"
                    name="nombre"
                    value={formData.nombre}
                    onChange={handleChange}
                    placeholder="Tu nombre"
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500 transition-all"
                    required
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Apellido *
                </label>
                <div className="relative">
                  <User className="absolute left-3 top-3 text-gray-400" size={20} />
                  <input
                    type="text"
                    name="apellido"
                    value={formData.apellido}
                    onChange={handleChange}
                    placeholder="Tu apellido"
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500 transition-all"
                    required
                  />
                </div>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Correo Electrónico *
              </label>
              <div className="relative">
                <Mail className="absolute left-3 top-3 text-gray-400" size={20} />
                <input
                  type="email"
                  name="email"
                  value={formData.email}
                  onChange={handleChange}
                  placeholder="tu@email.com"
                  className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500 transition-all"
                  required
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Contraseña *
              </label>
              <div className="relative">
                <Lock className="absolute left-3 top-3 text-gray-400" size={20} />
                <input
                  type="password"
                  name="contraseña"
                  value={formData.contraseña}
                  onChange={handleChange}
                  placeholder="Mínimo 8 caracteres"
                  className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500 transition-all"
                  required
                />
              </div>
            </div>

            <div className="space-y-3 mt-6 pt-6 border-t border-gray-200">
              <label className="flex items-start gap-3 cursor-pointer">
                <input
                  type="checkbox"
                  name="terminosAceptados"
                  checked={formData.terminosAceptados}
                  onChange={handleChange}
                  className="mt-1 w-4 h-4 text-gray-500 border-gray-300 rounded focus:ring-gray-500 cursor-pointer"
                  required
                />
                <span className="text-sm text-gray-700">
                  He leído y aceptado todos los términos y condiciones <span className="text-red-500">*</span>
                </span>
              </label>

              <label className="flex items-start gap-3 cursor-pointer">
                <input
                  type="checkbox"
                  name="recibirNovedades"
                  checked={formData.recibirNovedades}
                  onChange={handleChange}
                  className="mt-1 w-4 h-4 text-gray-500 border-gray-300 rounded focus:ring-gray-500 cursor-pointer"
                />
                <span className="text-sm text-gray-700">
                  Quiero recibir todas las novedades sobre sus productos por email
                </span>
              </label>
            </div>

            <button
              type="submit"
              className="w-full bg-gray-700 hover:bg-gray-800 text-white font-medium py-2 rounded-lg transition-colors mt-6"
            >
              Crear Cuenta
            </button>
          </form>

          <p className="text-center text-gray-600 text-sm mt-4">
            ¿Ya tienes cuenta? <a href="#" className="text-gray-700 hover:text-gray-800 font-medium">Inicia sesión aquí</a>
          </p>
        </div>
      </div>
    </div>
  );
}
export default Register;