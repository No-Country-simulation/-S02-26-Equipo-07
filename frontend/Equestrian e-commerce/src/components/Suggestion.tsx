import { useState } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import ProductCard from "./home-components/ProductCard";
import type { Product } from "../types/product";

// ─── Types ────────────────────────────────────────────────────────────────────

interface SuggestionsProps {
  products: Product[];
  title?: string;
}

const VISIBLE = 4;
const MAX_ITEMS = 8;

// ─── Main Component ───────────────────────────────────────────────────────────

export default function Suggestions({ products, title = "You may also like" }: SuggestionsProps) {
  const [startIndex, setStartIndex] = useState(0);

  const items = products.slice(0, MAX_ITEMS);
  const canPrev = startIndex > 0;
  const canNext = startIndex + VISIBLE < items.length;

  const prev = (): void => {
    if (canPrev) setStartIndex((i) => i - 1);
  };

  const next = (): void => {
    if (canNext) setStartIndex((i) => i + 1);
  };

  const visible = items.slice(startIndex, startIndex + VISIBLE);

  if (items.length === 0) return null;

  return (
    <section className="w-full bg-stone-100 py-10 px-4 sm:px-6 lg:px-8">
      <div className="max-w-7xl mx-auto">

        {/* Header */}
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-2xl font-bold text-gray-900">{title}</h2>
          <div className="flex gap-2">
            <button
              onClick={prev}
              disabled={!canPrev}
              className={`p-2 rounded-lg border transition-colors ${
                canPrev
                  ? "border-gray-400 text-gray-700 hover:bg-gray-200"
                  : "border-gray-200 text-gray-300 cursor-not-allowed"
              }`}
              aria-label="Anterior"
            >
              <ChevronLeft size={20} />
            </button>
            <button
              onClick={next}
              disabled={!canNext}
              className={`p-2 rounded-lg border transition-colors ${
                canNext
                  ? "border-gray-400 text-gray-700 hover:bg-gray-200"
                  : "border-gray-200 text-gray-300 cursor-not-allowed"
              }`}
              aria-label="Siguiente"
            >
              <ChevronRight size={20} />
            </button>
          </div>
        </div>

        {/* Cards */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          {visible.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>

        {/* Dot indicators */}
        <div className="flex justify-center gap-2 mt-6">
          {Array.from({ length: items.length - VISIBLE + 1 }).map((_, i) => (
            <button
              key={i}
              onClick={() => setStartIndex(i)}
              className={`h-2 rounded-full transition-all ${
                startIndex === i ? "w-6 bg-gray-800" : "w-2 bg-gray-300 hover:bg-gray-400"
              }`}
              aria-label={`Ir a posición ${i + 1}`}
            />
          ))}
        </div>

      </div>
    </section>
  );
}
