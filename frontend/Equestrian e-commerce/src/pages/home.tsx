import ProductGrid from '../components/home-components/ProductGrid';
import { mockProducts } from '../components/home-components/data/mockProducts';


const HomePage = ()=>{
    return (
        <>
            <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                <ProductGrid products={mockProducts} />
            </main>
        </>
    )
}

export default HomePage;