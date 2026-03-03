import { Shield, Shirt, Heart, Package } from 'lucide-react';

const categories = [
  {
    icon: Shield,
    title: 'Equipamiento',
    description: 'Monturas, riendas, estribos y más',
    color: 'bg-amber-100 text-amber-900',
  },
  {
    icon: Shirt,
    title: 'Vestuario',
    description: 'Ropa profesional para jinetes',
    color: 'bg-orange-100 text-orange-900',
  },
  {
    icon: Heart,
    title: 'Cuidado',
    description: 'Productos de salud y bienestar',
    color: 'bg-rose-100 text-rose-900',
  },
  {
    icon: Package,
    title: 'Accesorios',
    description: 'Complementos y herramientas',
    color: 'bg-yellow-100 text-yellow-900',
  },
];

export default function Categories() {
  return (
    <section id="categorias" className="py-20 px-4 sm:px-6 lg:px-8 bg-white">
      <div className="max-w-7xl mx-auto">
        <div className="text-center mb-16">
          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Nuestras Categorías
          </h2>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            Encuentra todo lo que necesitas en un solo lugar
          </p>
        </div>

        <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-8">
          {categories.map((category) => {
            const Icon = category.icon;
            return (
              <div
                key={category.title}
                className="group p-8 rounded-2xl border-2 border-gray-100 hover:border-amber-200 hover:shadow-xl transition-all cursor-pointer"
              >
                <div className={`w-16 h-16 ${category.color} rounded-xl flex items-center justify-center mb-6 group-hover:scale-110 transition-transform`}>
                  <Icon size={32} />
                </div>
                <h3 className="text-2xl font-bold text-gray-900 mb-2">
                  {category.title}
                </h3>
                <p className="text-gray-600">{category.description}</p>
              </div>
            );
          })}
        </div>
      </div>
    </section>
  );
}
