import { useState } from "react";
import { Heart, Trash2, ChevronDown, Tag, ShoppingBag, CheckCircle } from "lucide-react";
import type { Product } from '../../types/product';
import { useCart } from '../../context/CartContext';

// ─── Types ────────────────────────────────────────────────────────────────────

interface CartItem {
  product: Product;
  size: string;
  quantity: number;
}

// ─── Constants ────────────────────────────────────────────────────────────────

const DELIVERY = 9.99;
const SIZES = ["XS", "S", "M", "L", "XL"];
const QUANTITIES = [1, 2, 3, 4, 5];
const VALID_PROMO = "SAVE10";
const PROMO_DISCOUNT = 10;

// ─── Sub-components ───────────────────────────────────────────────────────────

interface SelectFieldProps {
  value: string | number;
  options: (string | number)[];
  label: (opt: string | number) => string;
  onChange: (val: string) => void;
}

function SelectField({ value, options, label, onChange }: SelectFieldProps) {
  return (
    <div className="relative">
      <select
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="appearance-none bg-white border border-gray-300 rounded-md py-1.5 pl-3 pr-8 text-sm text-gray-700 cursor-pointer focus:outline-none focus:ring-1 focus:ring-gray-400"
      >
        {options.map((opt) => (
          <option key={opt} value={opt}>
            {label(opt)}
          </option>
        ))}
      </select>
      <ChevronDown
        size={13}
        className="absolute right-2 top-1/2 -translate-y-1/2 text-gray-500 pointer-events-none"
      />
    </div>
  );
}

interface CartItemProps {
  item: CartItem;
  isWishlisted: boolean;
  onSizeChange: (id: number, size: string) => void;
  onQtyChange: (id: number, qty: number) => void;
  onRemove: (id: number) => void;
  onWishlist: (id: number) => void;
  showDivider: boolean;
}

function CartItem({
  item,
  isWishlisted,
  onSizeChange,
  onQtyChange,
  onRemove,
  onWishlist,
  showDivider,
}: CartItemProps) {
  return (
    <div>
      <div className="flex gap-5">
        {/* Image */}
        <div className="w-28 h-28 rounded-lg overflow-hidden bg-stone-100 border border-stone-200 shrink-0">
          <img
            src={item.product.image}
            alt={item.product.name}
            className="w-full h-full object-cover"
          />
        </div>

        {/* Info */}
        <div className="flex-1 min-w-0">
          <div className="flex justify-between items-start gap-2">
            <div>
              <p className="font-bold text-sm text-gray-900">{item.product.name}</p>
              <p className="text-gray-500 text-xs">{item.product.description} </p>
            </div>
            <span className="text-blue-600 font-bold text-sm whitespace-nowrap">
              ${(item.product.price * item.quantity).toFixed(2)}
            </span>
          </div>

          {/* Selects */}
          <div className="flex gap-3 mb-3">
            <SelectField
              value={item.size}
              options={SIZES}
              label={(s) => `Size ${s}`}
              onChange={(val) => onSizeChange(item.product.id, val)}
            />
            <SelectField
              value={item.quantity}
              options={QUANTITIES}
              label={(q) => `Quantity ${q}`}
              onChange={(val) => onQtyChange(item.product.id, Number(val))}
            />
          </div>

          {/* Actions */}
          <div className="flex gap-4">
            <button
              onClick={() => onWishlist(item.product.id)}
              className="text-gray-400 hover:text-red-500 transition-colors"
              aria-label="Add to wishlist"
            >
              <Heart
                size={19}
                className={isWishlisted ? "fill-red-500 stroke-red-500" : ""}
              />
            </button>
            <button
              onClick={() => onRemove(item.product.id)}
              className="text-gray-400 hover:text-gray-700 transition-colors"
              aria-label="Remove item"
            >
              <Trash2 size={19} />
            </button>
          </div>
        </div>
      </div>

      {showDivider && <hr className="my-5 border-gray-100" />}
    </div>
  );
}

// ─── Main Component ───────────────────────────────────────────────────────────

export default function CartSection() {
  const { cart, removeFromCart, updateQty, updateSize } = useCart();
  const [wishlist, setWishlist]     = useState<Record<number, boolean>>({});
  const [promoCode, setPromoCode]   = useState<string>("");
  const [discount, setDiscount]     = useState<number | null>(null);
  const [promoApplied, setPromoApplied] = useState<boolean>(false);
  const [promoError, setPromoError] = useState<boolean>(false);
  const toggleWishlist = (id: number): void =>
  setWishlist((prev) => ({ ...prev, [id]: !prev[id] }));

  const subtotal = cart.reduce((sum, i) => sum + i.product.price * i.quantity, 0);
  const total = subtotal + DELIVERY - (discount ?? 0);

  const applyPromo = (): void => {
    if (promoCode.toUpperCase() === VALID_PROMO) {
      setDiscount(PROMO_DISCOUNT);
      setPromoApplied(true);
      setPromoError(false);
    } else {
      setPromoError(true);
      setPromoApplied(false);
    }
  };

  return (
    <section className="bg-stone-100 pt-12 pb-0 min-h-screen">
      {/* ── Sale Banner ── */}
      <div className="max-w-4xl mx-auto px-6 pb-8">
        <h2 className="text-2xl font-bold text-gray-900 mb-1">
          Descuentos para celebrar la apertura de la tienda!
        </h2>
        <p className="text-gray-600 text-m">
          Recuerda suscribirte a nuestros boletines quincenales para poder obtener codigos de descuento de hasta 60%!
        </p>
        <p className="text-gray-600 text-sm mt-0.5">
          <a href="#" className="text-gray-900 underline underline-offset-2 hover:text-blue-600 transition-colors">
            Registrate 
          </a>{" "}
        </p>
      </div>

      {/* ── Cart + Summary Grid ── */}
      <div className="max-w-4xl mx-auto px-6 pb-16 grid grid-cols-1 md:grid-cols-[1fr_300px] gap-8 items-start">

        {/* ── Your Bag ── */}
        <div className="bg-white rounded-xl p-7 shadow-sm">
          <h3 className="text-xl font-bold text-gray-900 mb-1">Tu Carrito</h3>

          {cart.length === 0 ? (
            <div className="flex flex-col items-center justify-center py-14 text-gray-300 gap-3">
              <ShoppingBag size={40} />
              <p className="text-sm">Your bag is empty.</p>
            </div>
          ) : (
            cart.map((item, i) => (
              <CartItem
                key={item.product.id}
                item={item} 
                isWishlisted={!!wishlist[item.product.id]}
                onSizeChange={updateSize}
                onQtyChange={updateQty}
                onRemove={removeFromCart}
                onWishlist={toggleWishlist}
                showDivider={i < cart.length - 1}
              />
            ))
          )}
        </div>

        {/* ── Order Summary ── */}
        <div>
          <h3 className="text-2xl font-bold text-gray-900 mb-5">
            Resumen de compra
          </h3>

          {/* Line items */}
          <div className="flex flex-col gap-3 mb-5">
            <div className="flex justify-between text-sm text-gray-700">
              <span>
                {cart.length} Articulos{cart.length !== 1 ? "" : ""}
              </span>
              <span>${subtotal.toFixed(2)}</span>
            </div>

            <div className="flex justify-between text-sm text-gray-700">
              <span>Envio</span>
              <span>${DELIVERY.toFixed(2)}</span>
            </div>

            <div className="flex justify-between text-sm text-gray-700">
              <span>Descuento</span>
              <span>{discount ? `-$${discount.toFixed(2)}` : "-"}</span>
            </div>

            <hr className="border-gray-200" />

            <div className="flex justify-between text-base font-bold text-gray-900">
              <span>Total</span>
              <span>${total.toFixed(2)}</span>
            </div>
          </div>

          {/* Checkout button */}
          <button className="w-full bg-gray-900 hover:bg-gray-700 active:bg-gray-800 text-white font-bold text-sm tracking-widest rounded-lg py-3.5 mb-5 transition-colors cursor-pointer">
            Comprar
          </button>

          {/* Promo code */}
          <p className="text-sm text-gray-800 font-medium mb-2">
            Ingrese su codigo de descuento
          </p>
          <div className="flex gap-2">
            <input
              type="text"
              placeholder="Ingrese su codigo de descuento"
              value={promoCode}
              onChange={(e) => {
                setPromoCode(e.target.value);
                setPromoError(false);
              }}
              onKeyDown={(e) => e.key === "Enter" && applyPromo()}
              disabled={promoApplied}
              className="flex-1 border border-gray-300 rounded-lg px-3 py-2 text-sm text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-1 focus:ring-gray-400 disabled:bg-gray-50 disabled:text-gray-400"
            />
            <button
              onClick={applyPromo}
              disabled={promoApplied}
              className={`px-4 py-2 rounded-lg text-sm font-bold tracking-wide transition-colors cursor-pointer
                ${promoApplied
                  ? "bg-green-600 text-white cursor-default"
                  : "bg-gray-900 hover:bg-gray-700 text-white"
                }`}
            >
              {promoApplied ? (
                <CheckCircle size={16} />
              ) : (
                "Activar"
              )}
            </button>
          </div>

          {/* Feedback messages */}
          {promoApplied && (
            <p className="text-green-600 text-xs mt-2 flex items-center gap-1">
              <Tag size={12} /> Descuento de 10% valido, disfrute su descuento!
            </p>
          )}
          {promoError && (
            <p className="text-red-500 text-xs mt-2">
              Codigo no valido ingrese otro por favor <span className="font-semibold"></span>.
            </p>
          )}
        </div>
      </div>
    </section>
  );
}
