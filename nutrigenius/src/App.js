import LoginForm from './Components/LoginForm/LoginForm'
import RegistrationForm from './Components/RegistrationForm/RegistrationForm';
import BMIForm from './Components/BMIForm/BMIForm';
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import ForgotPassword from './Components/ForgotPassword/ForgotPassword';
import HomeForm from './Components/Home/HomeForm';
import AboutForm from './Components/About/AboutForm';
import User from './Components/User/User';
import Dashboard from './Components/Dashboard/Dashboard';

function App() {
  return (
    <Router>
    <div className="App">
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/bmi" element={<BMIForm />} />
        <Route path="/reg" element={<RegistrationForm />} />
        <Route path="/forgot" element={<ForgotPassword/>} />
        <Route path="/home" element={<HomeForm/>} />
        <Route path="/about" element={<AboutForm/>} />
        <Route path="/user" element={<User/>} />
        <Route path="/dashboard" element={<Dashboard/>} />
      </Routes>
    </div>
    </Router>
  );
}

export default App;
