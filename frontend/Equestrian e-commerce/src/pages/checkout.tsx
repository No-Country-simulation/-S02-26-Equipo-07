import CheckOut from '../components/checkout-components/CheckOutSection';
import Navbar from '../components/Navbar';
import Footer from '../components/Footer'

const CheckOutPage = () =>{
    const handleLogin = () => {
        console.log("Abriendo login o redirigiendo...");
        // Aquí podrías usar navigate('/login') o abrir un modal
    };
    return(
        <div>
            <Navbar onLoginClick={handleLogin}  /> 
            <CheckOut />
            <Footer />
        </div>
    )
}

export default CheckOutPage