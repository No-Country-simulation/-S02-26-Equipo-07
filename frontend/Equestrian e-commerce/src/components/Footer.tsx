import { Facebook, Instagram, Twitter, Mail } from 'lucide-react';

export default function Footer() {
  return (
    <footer className="bg-gray-900 text-gray-300 mt-20">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-8">
          <div className="flex flex-col items-start">
            <div className="text-3xl font-bold text-amber-500 mb-2">🐴</div>
            <h3 className="text-lg font-bold text-white mb-2">Premium Horses</h3>
            <p className="text-sm text-gray-400">
              Encuentra los mejores caballos de raza pura para tu vida
            </p>
          </div>

          <div>
            <h4 className="text-white font-semibold mb-4">Compañía</h4>
            <ul className="space-y-2">
              <li>
                <button className="hover:text-amber-500 transition-colors text-sm">
                  Quiénes Somos
                </button>
              </li>
              <li>
                <button className="hover:text-amber-500 transition-colors text-sm">
                  Categorías
                </button>
              </li>
              <li>
                <button className="hover:text-amber-500 transition-colors text-sm">
                  Términos de Servicio
                </button>
              </li>
              <li>
                <button className="hover:text-amber-500 transition-colors text-sm">
                  Política de Privacidad
                </button>
              </li>
            </ul>
          </div>

          <div>
            <h4 className="text-white font-semibold mb-4">Nuestras Redes</h4>
            <div className="flex gap-4">
              <button
                className="p-2 bg-gray-800 rounded-lg hover:bg-amber-500 transition-colors"
                aria-label="Facebook"
              >
                <Facebook size={20} />
              </button>
              <button
                className="p-2 bg-gray-800 rounded-lg hover:bg-amber-500 transition-colors"
                aria-label="Instagram"
              >
                <Instagram size={20} />
              </button>
              <button
                className="p-2 bg-gray-800 rounded-lg hover:bg-amber-500 transition-colors"
                aria-label="Twitter"
              >
                <Twitter size={20} />
              </button>
              <button
                className="p-2 bg-gray-800 rounded-lg hover:bg-amber-500 transition-colors"
                aria-label="Email"
              >
                <Mail size={20} />
              </button>
            </div>
          </div>

          <div>
            <h4 className="text-white font-semibold mb-4">Contacto</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <span className="text-gray-400">Email:</span>
                <br />
                <a href="mailto:info@premiumhorses.com" className="hover:text-amber-500 transition-colors">
                  info@premiumhorses.com
                </a>
              </li>
              <li>
                <span className="text-gray-400">Teléfono:</span>
                <br />
                <a href="tel:+1234567890" className="hover:text-amber-500 transition-colors">
                  +1 (234) 567-890
                </a>
              </li>
            </ul>
          </div>
        </div>

        <div className="border-t border-gray-800 pt-8">
          <div className="flex flex-col md:flex-row items-center justify-between">
            <p className="text-sm text-gray-400">
              &copy; 2024 Premium Horses. Todos los derechos reservados.
            </p>
            <div className="flex gap-6 mt-4 md:mt-0">
              <button className="text-sm text-gray-400 hover:text-amber-500 transition-colors">
                Términos
              </button>
              <button className="text-sm text-gray-400 hover:text-amber-500 transition-colors">
                Privacidad
              </button>
              <button className="text-sm text-gray-400 hover:text-amber-500 transition-colors">
                Cookies
              </button>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
}
