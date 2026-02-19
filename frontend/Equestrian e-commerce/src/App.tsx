import { useState } from 'react';
import HomePage from './pages/home';
import Login from './components/Login';
import Register from './components/Register';


function App() {
  const [loginOpen, setLoginOpen] = useState(false);
  const [registerOpen, setRegisterOpen] = useState(true);
  return (
    <div className="min-h-screen bg-linear-to-br from-amber-50 to-orange-50">
      <header className="bg-white shadow-sm border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <h1 className="text-3xl font-bold text-gray-900">
            Productos para jinetes y caballos
          </h1>
          <p className="text-gray-600 mt-2">
            Encuentra el caballo perfecto para ti
          </p>
        </div>
      </header>

      <HomePage/>
      <Login isOpen={loginOpen} onClose={() => setLoginOpen(false)} />
      <Register isOpen={registerOpen} onClose={() => setRegisterOpen(false)}/>
    </div>
  );
}

export default App;
