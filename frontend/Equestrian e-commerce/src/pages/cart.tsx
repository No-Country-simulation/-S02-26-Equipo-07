import Navbar from '../components/Navbar';
import Footer from '../components/Footer';
import CartSection from '../components/cart-components/CartSection'

const Cart = () =>{

    const handleLogin = () => {
        console.log("Abriendo login o redirigiendo...");
        // Aquí podrías usar navigate('/login') o abrir un modal
    };

    return(
        <div className="max-w-full mx-auto  gap-8">
            <Navbar onLoginClick={handleLogin}/>
            <CartSection />
            <Footer />
        </div>
    );
}

export default Cart