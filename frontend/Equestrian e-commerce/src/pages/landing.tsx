import Header from '../components/landing-components/Header';
import Hero from '../components/landing-components/Hero';
import Categories from '../components/landing-components/Categories';
import Features from '../components/landing-components/Features';
import Footer from '../components/Footer';

function Landing() {
  const handleLogin = () => {
    console.log('Login clicked');
  };

  const handleRegister = () => {
    console.log('Register clicked');
  };

  return (
    <div className="min-h-screen">
      <Header onLoginClick={handleLogin} onRegisterClick={handleRegister} />
      <Hero />
      <Categories />
      <Footer />
    </div>
  );
}

export default Landing;