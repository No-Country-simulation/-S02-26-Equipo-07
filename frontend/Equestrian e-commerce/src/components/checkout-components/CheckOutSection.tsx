import { useState } from "react";
import { CreditCard, Store, ChevronDown, CheckCircle2, Package } from "lucide-react";
import { useCart } from "../../context/CartContext";

// ─── Types ────────────────────────────────────────────────────────────────────

interface ShippingForm {
  firstName: string;
  lastName: string;
  country: string;
  address: string;
  city: string;
  postalCode: string;
  phone: string;
  saveInfo: boolean;
}

interface PaymentForm {
  cardNumber: string;
  expirationDate: string;
  securityCode: string;
  cardHolderName: string;
  savePayment: boolean;
  sameBilling: boolean;
  ageConfirm: boolean;
  newsletter: boolean;
}

type DeliveryOption = "standard" | "pickup";

// ─── Constants ────────────────────────────────────────────────────────────────

const DELIVERY_COST = 9.99;

// ─── Reusable Field ───────────────────────────────────────────────────────────

interface FieldProps {
  placeholder: string;
  value: string;
  onChange: (val: string) => void;
  type?: string;
  className?: string;
}

function Field({ placeholder, value, onChange, type = "text", className = "" }: FieldProps) {
  return (
    <input
      type={type}
      placeholder={placeholder}
      value={value}
      onChange={(e) => onChange(e.target.value)}
      className={`w-full border border-gray-300 rounded-lg px-4 py-2.5 text-sm text-gray-700 placeholder-gray-400 bg-white focus:outline-none focus:ring-1 focus:ring-gray-400 transition ${className}`}
    />
  );
}

// ─── Reusable Checkbox ────────────────────────────────────────────────────────

interface CheckboxProps {
  checked: boolean;
  onChange: (val: boolean) => void;
  label: string;
}

function Checkbox({ checked, onChange, label }: CheckboxProps) {
  return (
    <label className="flex items-center gap-2 cursor-pointer select-none">
      <input
        type="checkbox"
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
        className="w-4 h-4 accent-gray-800 cursor-pointer"
      />
      <span className="text-sm text-gray-700">{label}</span>
    </label>
  );
}

// ─── Success Modal ────────────────────────────────────────────────────────────

function SuccessModal({ onClose }: { onClose: () => void }) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm">
      <div className="bg-white rounded-2xl shadow-2xl p-10 max-w-sm w-full mx-4 flex flex-col items-center text-center gap-5">
        <div className="w-20 h-20 rounded-full bg-green-50 flex items-center justify-center">
          <CheckCircle2 size={48} className="text-green-500" />
        </div>
        <div>
          <h2 className="text-xl font-bold text-gray-900 mb-2">¡Compra realizada!</h2>
          <p className="text-gray-500 text-sm leading-relaxed">
            Su compra ha sido aceptada.<br />
            La recibirá en los siguientes días hábiles.
          </p>
        </div>
        <div className="flex items-center gap-2 bg-stone-100 rounded-lg px-4 py-3 w-full justify-center">
          <Package size={18} className="text-gray-500" />
          <span className="text-sm text-gray-600">Recibirá un email con el seguimiento</span>
        </div>
        <button
          onClick={onClose}
          className="w-full bg-gray-900 hover:bg-gray-700 text-white font-bold text-sm tracking-widest rounded-lg py-3 transition-colors cursor-pointer"
        >
          VOLVER A LA TIENDA
        </button>
      </div>
    </div>
  );
}

// ─── Main Component ───────────────────────────────────────────────────────────

export default function CheckOut() {
  const { cart, clearCart } = useCart();

  const [email, setEmail] = useState<string>("");
  const [shipping, setShipping] = useState<ShippingForm>({
    firstName: "", lastName: "", country: "", address: "",
    city: "", postalCode: "", phone: "", saveInfo: true,
  });
  const [payment, setPayment] = useState<PaymentForm>({
    cardNumber: "", expirationDate: "", securityCode: "",
    cardHolderName: "", savePayment: true, sameBilling: true,
    ageConfirm: true, newsletter: true,
  });
  const [delivery, setDelivery] = useState<DeliveryOption>("standard");
  const [showSuccess, setShowSuccess] = useState<boolean>(false);

  const deliveryCost = delivery === "standard" ? DELIVERY_COST : 0;
  const subtotal = cart.reduce((sum, i) => sum + i.product.price * i.quantity, 0);
  const total = subtotal + deliveryCost;

  const handleSubmit = (): void => {
    setShowSuccess(true);
  };

  const handleClose = (): void => {
    setShowSuccess(false);
    clearCart?.();
  };

  return (
    <section className="bg-stone-100 min-h-screen pt-10 pb-16">
      {showSuccess && <SuccessModal onClose={handleClose} />}

      <div className="max-w-4xl mx-auto px-6 grid grid-cols-1 md:grid-cols-[1fr_300px] gap-8 items-start">

        {/* ══════════════ LEFT COLUMN ══════════════ */}
        <div className="flex flex-col gap-6">

          {/* Contact Details */}
          <div className="bg-white rounded-xl p-7 shadow-sm">
            <h2 className="text-xl font-bold text-gray-900 mb-1">Detalles de contacto</h2>
            <p className="text-gray-500 text-xs mb-4">
              Usaremos la informacion proporcionada para mantenerte en informado sobre el envio.
            </p>
            <Field
              placeholder="Email"
              value={email}
              onChange={setEmail}
              type="email"
            />
          </div>

          {/* Shipping Address */}
          <div className="bg-white rounded-xl p-7 shadow-sm">
            <h2 className="text-xl font-bold text-gray-900 mb-4">Informacion del comprador</h2>
            <div className="flex flex-col gap-3">
              <div className="grid grid-cols-2 gap-3">
                <Field placeholder="Nombre*" value={shipping.firstName}
                  onChange={(v) => setShipping((s) => ({ ...s, firstName: v }))} />
                <Field placeholder="Apellido*" value={shipping.lastName}
                  onChange={(v) => setShipping((s) => ({ ...s, lastName: v }))} />
              </div>

              <Field placeholder="Pais y Provincia/estado*" value={shipping.country}
                onChange={(v) => setShipping((s) => ({ ...s, country: v }))} />

              <div>
                <Field placeholder="Direccion*" value={shipping.address}
                  onChange={(v) => setShipping((s) => ({ ...s, address: v }))} />
                <p className="text-gray-400 text-xs mt-1 ml-1">
                  Escribe tu dirección.
                </p>
              </div>

              <div className="grid grid-cols-2 gap-3">
                <Field placeholder="Ciudad*" value={shipping.city}
                  onChange={(v) => setShipping((s) => ({ ...s, city: v }))} />
                <Field placeholder="Codigo Postal*" value={shipping.postalCode}
                  onChange={(v) => setShipping((s) => ({ ...s, postalCode: v }))} />
              </div>

              <div>
                <Field placeholder="Numero de telefono*" value={shipping.phone}
                  onChange={(v) => setShipping((s) => ({ ...s, phone: v }))} type="tel" />
                <p className="text-gray-400 text-xs mt-1 ml-1">E.g. (123) 456-7890</p>
              </div>

              <Checkbox
                checked={shipping.saveInfo}
                onChange={(v) => setShipping((s) => ({ ...s, saveInfo: v }))}
                label="Guarda esta informacion para el futuro"
              />
            </div>
          </div>

          {/* Delivery Options */}
          <div className="bg-white rounded-xl p-7 shadow-sm">
            <h2 className="text-xl font-bold text-gray-900 mb-4">Opciones de Envio</h2>
            <div className="flex flex-col gap-3">

              <button
                onClick={() => setDelivery("standard")}
                className={`w-full text-left rounded-xl border-2 px-5 py-4 transition-all cursor-pointer ${
                  delivery === "standard"
                    ? "border-gray-800 bg-gray-50"
                    : "border-gray-200 hover:border-gray-300 bg-white"
                }`}
              >
                <div className="flex justify-between items-center">
                  <div>
                    <p className="font-semibold text-sm text-gray-900">Envio a domicilio</p>
                    <p className="text-gray-500 text-xs mt-0.5">
                      Recuerda ingresar tu direccion para que te informemos cuando llega tu envio
                    </p>
                  </div>
                  <span className="text-blue-600 font-bold text-sm">${DELIVERY_COST.toFixed(2)}</span>
                </div>
              </button>

              <button
                onClick={() => setDelivery("pickup")}
                className={`w-full text-left rounded-xl border-2 px-5 py-4 transition-all cursor-pointer ${
                  delivery === "pickup"
                    ? "border-gray-800 bg-gray-50"
                    : "border-gray-200 hover:border-gray-300 bg-white"
                }`}
              >
                <div className="flex justify-between items-center">
                  <div>
                    <p className="font-semibold text-sm text-gray-900 flex items-center gap-1.5">
                      <Store size={14} /> Recoger en la tienda
                    </p>
                    <p className="text-gray-500 text-xs mt-0.5">Paga ahora y luego retira en tienda</p>
                  </div>
                  <span className="font-bold text-sm text-gray-900">Gratis</span>
                </div>
              </button>
            </div>
          </div>

          {/* Payment */}
          <div className="bg-white rounded-xl p-7 shadow-sm">
            <h2 className="text-xl font-bold text-gray-900 mb-4">Payment</h2>
            <div className="flex flex-col gap-3">

              {/* Method selector (visual only) */}
              <div className="flex items-center justify-between border border-gray-300 rounded-lg px-4 py-2.5 bg-white">
                <div className="flex items-center gap-2">
                  <CreditCard size={17} className="text-gray-600" />
                  <span className="text-sm text-gray-700 font-medium">Tarjeta de Credito</span>
                  <span className="text-base leading-none">🔴🟡</span>
                </div>
                <ChevronDown size={16} className="text-gray-500" />
              </div>

              <Field placeholder="Numero de Tarjeta*" value={payment.cardNumber}
                onChange={(v) => setPayment((p) => ({ ...p, cardNumber: v }))} />

              <div className="grid grid-cols-2 gap-3">
                <Field placeholder="Fecha de caducidad*" value={payment.expirationDate}
                  onChange={(v) => setPayment((p) => ({ ...p, expirationDate: v }))} />
                <Field placeholder="Codigo de seguridad*" value={payment.securityCode}
                  onChange={(v) => setPayment((p) => ({ ...p, securityCode: v }))} type="password" />
              </div>

              <Field placeholder="Nombre del titular*" value={payment.cardHolderName}
                onChange={(v) => setPayment((p) => ({ ...p, cardHolderName: v }))} />

              <div className="flex flex-col gap-2 pt-1">
                <Checkbox checked={payment.savePayment}
                  onChange={(v) => setPayment((p) => ({ ...p, savePayment: v }))}
                  label="Recuerdar mis datos" />
                <Checkbox checked={payment.ageConfirm}
                  onChange={(v) => setPayment((p) => ({ ...p, ageConfirm: v }))}
                  label="Soy mayor de edad" />
              </div>

              <div className="pt-3 border-t border-gray-100">
                <p className="text-sm font-semibold text-gray-900 mb-2">
                  Quieres recibir informacion de ofertas de parte de nuestro boletin?
                </p>
                <Checkbox checked={payment.newsletter}
                  onChange={(v) => setPayment((p) => ({ ...p, newsletter: v }))}
                  label="Si, me gustaria recibir informacion de descuentos y ofertas!" />
              </div>

              <button
                onClick={handleSubmit}
                className="mt-1 w-full bg-blue-600 hover:bg-blue-700 active:bg-blue-800 text-white font-bold text-sm tracking-widest rounded-lg py-3.5 transition-colors cursor-pointer"
              >
                Comprar
              </button>
            </div>
          </div>
        </div>

        {/* ══════════════ RIGHT COLUMN ══════════════ */}
        <div className="flex flex-col gap-4 md:sticky md:top-6">

          {/* Order Summary */}
          <div className="bg-white rounded-xl p-6 shadow-sm">
            <h3 className="text-lg font-bold text-gray-900 mb-4">Resumen de Compra</h3>
            <div className="flex flex-col gap-2.5 text-sm text-gray-700">
              <div className="flex justify-between">
                <span>{cart.length} Articulo{cart.length !== 1 ? "s" : ""}</span>
                <span>${subtotal.toFixed(2)}</span>
              </div>
              <div className="flex justify-between">
                <span>Gasto de Envio</span>
                <span>{delivery === "pickup" ? "Free" : `$${DELIVERY_COST.toFixed(2)}`}</span>
              </div>
              <div className="flex justify-between">
                <span>Descuento</span>
                <span>-</span>
              </div>
              <hr className="border-gray-200 my-1" />
              <div className="flex justify-between font-bold text-base text-gray-900">
                <span>Total</span>
                <span>${total.toFixed(2)}</span>
              </div>
            </div>
          </div>

          {/* Order Details */}
          <div className="bg-white rounded-xl p-6 shadow-sm">
            <h3 className="text-base font-bold text-gray-900 mb-4">Detalles de compra</h3>
            {cart.length === 0 ? (
              <p className="text-gray-400 text-sm text-center py-4">
                No hay items en el carrito
              </p>
            ) : (
              <div className="flex flex-col gap-4">
                {cart.map((item) => (
                  <div key={item.product.id} className="flex gap-3">
                    <div className="w-16 h-16 rounded-lg overflow-hidden bg-stone-100 border border-stone-200 shrink-0">
                      <img
                        src={
                          item.product.image ||
                          "https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=200"
                        }
                        alt={item.product.name}
                        className="w-full h-full object-cover"
                      />
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className="font-bold text-xs text-gray-900">{item.product.name}</p>
                      {item.product.description && (
                        <p className="text-gray-500 text-xs line-clamp-2">
                          {item.product.description}
                        </p>
                      )}
                      <p className="text-gray-400 text-xs mt-1">
                        Size {item.size}&nbsp;&nbsp;Quantity {item.quantity}
                      </p>
                      <p className="text-blue-600 font-bold text-sm mt-0.5">
                        ${(item.product.price * item.quantity).toFixed(2)}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

      </div>
    </section>
  );
}
