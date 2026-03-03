import { ArrowRight } from 'lucide-react';

export default function Hero() {
  return (
    <section className="pt-24 pb-16 px-4 sm:px-6 lg:px-8 bg-linear-to-br from-amber-50 to-orange-50">
      <div className="max-w-7xl mx-auto">
        <div className="grid md:grid-cols-2 gap-12 items-center">
          <div className="space-y-6">
            <h1 className="text-5xl md:text-6xl font-bold text-gray-900 leading-tight">
              Todo para tu{' '}
              <span className="text-amber-900">Caballo</span> y{' '}
              <span className="text-amber-900">Jinete</span>
            </h1>
            <p className="text-xl text-gray-600 leading-relaxed">
              Descubre la mejor selección de equipamiento ecuestre, accesorios y productos
              de calidad profesional para ti y tu compañero.
            </p>
            <div className="flex flex-col sm:flex-row gap-4">
              <button className="px-8 py-4 bg-amber-900 text-white rounded-lg hover:bg-amber-800 transition-all transform hover:scale-105 font-semibold flex items-center justify-center gap-2">
                Ver Productos
                <ArrowRight size={20} />
              </button>
              <button className="px-8 py-4 border-2 border-amber-900 text-amber-900 rounded-lg hover:bg-amber-50 transition-colors font-semibold">
                Nuestro Catálogo
              </button>
            </div>
          </div>

          <div className="relative">
            <div className="aspect-square rounded-2xl bg-linear-to-br from-amber-800 to-amber-600 shadow-2xl overflow-hidden">
              <div className="absolute inset-0 flex items-center justify-center text-white text-6xl opacity-20">
                🐴
              </div>
            </div>
            <div className="absolute -bottom-6 -right-6 w-32 h-32 bg-orange-400 rounded-full blur-3xl opacity-50"></div>
            <div className="absolute -top-6 -left-6 w-32 h-32 bg-amber-400 rounded-full blur-3xl opacity-50"></div>
          </div>
        </div>
      </div>
    </section>
  );
}
