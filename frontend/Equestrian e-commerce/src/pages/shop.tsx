import { useState } from 'react';
import Navbar from '../components/Navbar';
import Footer from '../components/Footer';
import Products from '../components/home-components/ProductGrid';
import CategoryFilterBar, { type CategoryFilter } from '../components/home-components/CategoryFilterBar';
import { mockProducts } from '../components/home-components/data/mockProducts';

const Shop = () => {
    const handleLogin = () => {
        console.log("Abriendo login o redirigiendo...");
        // Aquí podrías usar navigate('/login') o abrir un modal
    };
    const [activeFilter, setActiveFilter] = useState<CategoryFilter>('all');
    const [searchQuery, setSearchQuery] = useState('');

    const filteredProducts = mockProducts.filter((p) => {
    const matchesCategory =
        activeFilter === 'all' || p.category === activeFilter;
    const matchesSearch =
        p.name.toLowerCase().includes(searchQuery.toLowerCase());
    return matchesCategory && matchesSearch;
    });


    return(
        <div>
            <Navbar onLoginClick={handleLogin} />
            <CategoryFilterBar
            active={activeFilter}
            onChange={setActiveFilter}
            searchQuery={searchQuery}
            onSearchChange={setSearchQuery}
            />
            <Products products={filteredProducts} />
            <Footer />
        </div>
    )
}

export default Shop