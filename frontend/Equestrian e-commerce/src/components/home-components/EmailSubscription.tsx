import { useState } from 'react';
import { Mail } from 'lucide-react';

export default function EmailSubscription() {
  const [email, setEmail] = useState('');
  const [submitted, setSubmitted] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!email) {
      setError('Por favor ingresa un email');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      setError('Por favor ingresa un email válido');
      return;
    }

    setSubmitted(true);
    setEmail('');

    setTimeout(() => {
      setSubmitted(false);
    }, 3000);
  };

  return (
    <div className="relative w-full h-80 md:h-96 rounded-lg overflow-hidden shadow-lg">
      <img
        src="https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=1200"
        alt="Suscripción a newsletter"
        className="w-full h-full object-cover"
      />

      <div className="absolute inset-0 bg-black/40 flex items-center justify-center">
        <div className="w-full max-w-md px-6">
          <div className="text-center mb-6">
            <h2 className="text-3xl md:text-4xl font-bold text-white mb-2">
              Únete a Nuestra Comunidad
            </h2>
            <p className="text-white/90">
              Recibe ofertas exclusivas y noticias sobre nuestros mejores caballos
            </p>
          </div>

          {submitted ? (
            <div className="bg-green-500 text-white rounded-lg p-4 text-center font-semibold animate-pulse">
              ¡Suscripción exitosa! Revisa tu email
            </div>
          ) : (
            <form onSubmit={handleSubmit} className="space-y-3">
              <div className="relative">
                <div className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400">
                  <Mail size={20} />
                </div>
                <input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="tu@email.com"
                  className="w-full pl-12 pr-4 py-3 rounded-lg bg-white/95 backdrop-blur-sm text-gray-900 placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-amber-500 transition-all duration-300"
                />
              </div>

              {error && (
                <p className="text-gray-300 text-sm font-medium">{error}</p>
              )}

              <button
                type="submit"
                className="w-full bg-gray-500 hover:bg-gray-600 text-white font-bold py-3 rounded-lg transition-colors duration-300 shadow-lg hover:shadow-xl"
              >
                Suscribirse
              </button>
            </form>
          )}
        </div>
      </div>
    </div>
  );
}
