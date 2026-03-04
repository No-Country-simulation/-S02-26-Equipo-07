import { useState } from 'react';
import Header from '../components/landing-components/Header';
import Hero from '../components/landing-components/Hero';
import Categories from '../components/landing-components/Categories';
import Features from '../components/landing-components/Features';
import Footer from '../components/Footer';
import Login from '../components/Login'
import Register from '../components/Register'

function Landing() {
  

  const [isLoginOpen, setIsLoginOpen] = useState(false);
  const [isRegisterOpen, setIsRegisterOpen] = useState(false);

  return (
    <div className="min-h-screen">
      <Header onLoginClick={() => setIsLoginOpen(true)}
        onRegisterClick={() => setIsRegisterOpen(true)} />
        <Login isOpen={isLoginOpen} onClose={() => setIsLoginOpen(false)} />
        <Register isOpen={isRegisterOpen} onClose={() => setIsRegisterOpen(false)} />
      <Hero />
      <Categories />
      <Footer />
    </div>
  );
}

export default Landing;