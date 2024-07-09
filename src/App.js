import LoginForm from './Components/LoginForm/LoginForm'
import RegistrationForm from './Components/RegistrationForm/RegistrationForm';
import BMIForm from './Components/BMIForm/BMIForm';
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import ForgotPassword from './Components/ForgotPassword/ForgotPassword';


function App() {
  return (
    <Router>
    <div className="App">
      <Routes>
        <Route path="/" element={<LoginForm />} />
        <Route path="/bmi" element={<BMIForm />} />
        <Route path="/reg" element={<RegistrationForm />} />
        <Route path="/forgot" element={<ForgotPassword/>} />
      </Routes>
    </div>
    </Router>
  );
}

export default App;
