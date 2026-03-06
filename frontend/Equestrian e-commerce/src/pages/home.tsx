import ProductGrid from '../components/home-components/ProductGrid';
import ImageCarousel from '../components/home-components/ImageCarousel';
import { mockProducts } from '../components/home-components/data/mockProducts';
import EmailSubscription from '../components/home-components/EmailSubscription';
import Navbar from '../components/Navbar';
import Footer from '../components/Footer'

const HomePage = ()=>{

  const handleLogin = () => {
        console.log("Abriendo login o redirigiendo...");
        // Aquí podrías usar navigate('/login') o abrir un modal
    };

    const carouselImages = [
  {
    id: 1,
    url: 'https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=1200',
    title: 'Caballos Árabes Puros'
  },
  {
    id: 2,
    url: 'https://cdn0.ecologiaverde.com/es/posts/4/8/7/caballo_5784_orig.jpg',
    title: 'Elegancia y Potencia'
  },
  {
    id: 3,
    url: 'https://st3.depositphotos.com/3713385/15654/i/950/depositphotos_156540448-stock-photo-side-view-of-colorful-jockeys.jpg',
    title: 'Equipamiento Premium para tu jinete'
  },
  {
    id: 4,
    url: 'https://img.freepik.com/foto-gratis/deporte-montar-caballo-caballos-tierra_23-2150995475.jpg?semt=ais_hybrid&w=740&q=80',
    title: 'Tu Compañero Perfecto'
  }
];
    return (
        <>
            <main className="max-w-full mx-auto gap-8">
                <div className='mt-8'>
                  <Navbar onLoginClick={handleLogin}  />
                </div>
                <div className='mt-8'>
                  <ImageCarousel images={carouselImages}/>
                </div>
                <div className='mt-8'>
                  <ProductGrid products={mockProducts}  />
                </div>
                <div className='mt-8'>
                  <EmailSubscription />
                </div>
                <Footer />
            </main>
        </>
    )
}

export default HomePage;
