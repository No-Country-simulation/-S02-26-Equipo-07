import { Search, User, ChessKnight } from "lucide-react";

// ─── Types ────────────────────────────────────────────────────────────────────

export type CategoryFilter = "all" | "rider" | "horse";

interface CategoryFilterBarProps {
  active: CategoryFilter;
  onChange: (category: CategoryFilter) => void;
  searchQuery: string;
  onSearchChange: (query: string) => void;
}

// ─── Filter Buttons ───────────────────────────────────────────────────────────

const FILTERS: { label: string; value: CategoryFilter; icon: React.ReactNode }[] = [
  { label: "Jinetes",           value: "rider",  icon: <User size={17} /> },
  { label: "Búsqueda especial", value: "all",    icon: <Search size={17} /> },
  { label: "Caballos",          value: "horse",  icon: <ChessKnight size={17} /> },
];

// ─── Main Component ───────────────────────────────────────────────────────────

export default function CategoryFilterBar({
  active,
  onChange,
  searchQuery,
  onSearchChange,
}: CategoryFilterBarProps) {
  return (
    <div className="w-full bg-white border-b border-gray-200 shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-3 flex flex-col sm:flex-row items-center gap-3">

        {/* Category buttons */}
        <div className="flex w-full sm:w-auto gap-2">
          {FILTERS.map((f) => (
            <button
              key={f.value}
              onClick={() => onChange(f.value)}
              className={`flex-1 sm:flex-none flex items-center justify-center gap-2 px-5 py-2.5 rounded-lg text-sm font-semibold transition-all cursor-pointer border
                ${active === f.value
                  ? "bg-gray-900 text-white border-gray-900"
                  : "bg-white text-gray-600 border-gray-300 hover:border-gray-500 hover:text-gray-900"
                }`}
            >
              {f.icon}
              <span className="hidden sm:inline">{f.label}</span>
              <span className="sm:hidden">{f.label.split(" ")[0]}</span>
            </button>
          ))}
        </div>

        {/* Search input — visible when "Búsqueda especial" is active */}
        <div
          className={`w-full sm:flex-1 transition-all duration-200 overflow-hidden ${
            active === "all" ? "max-h-12 opacity-100" : "max-h-0 opacity-0 pointer-events-none"
          }`}
        >
          <div className="relative">
            <Search size={16} className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 pointer-events-none" />
            <input
              type="text"
              placeholder="Buscar producto..."
              value={searchQuery}
              onChange={(e) => onSearchChange(e.target.value)}
              className="w-full pl-9 pr-4 py-2.5 border border-gray-300 rounded-lg text-sm text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-1 focus:ring-gray-400 bg-white"
            />
          </div>
        </div>

      </div>
    </div>
  );
}
