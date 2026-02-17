import ProductGrid from '../components/home-components/ProductGrid';
import ImageCarousel from '../components/home-components/ImageCarousel';
import { mockProducts } from '../components/home-components/data/mockProducts';
import EmailSubscription from '../components/home-components/EmailSubscription';

const HomePage = ()=>{

    const carouselImages = [
  {
    id: 1,
    url: 'https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=1200',
    title: 'Caballos Árabes Puros'
  },
  {
    id: 2,
    url: 'https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=1200',
    title: 'Elegancia y Potencia'
  },
  {
    id: 3,
    url: 'https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=1200',
    title: 'Equipamiento Premium para tu jinete'
  },
  {
    id: 4,
    url: 'https://images.pexels.com/photos/1996333/pexels-photo-1996333.jpeg?auto=compress&cs=tinysrgb&w=1200',
    title: 'Tu Compañero Perfecto'
  }
];
    return (
        <>
            <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                <ImageCarousel images={carouselImages}/>
                <ProductGrid products={mockProducts} />
                <EmailSubscription />
            </main>
        </>
    )
}

export default HomePage;