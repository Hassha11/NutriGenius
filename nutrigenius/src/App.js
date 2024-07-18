import React from 'react';
import LoginForm from './Components/LoginForm/LoginForm'
import RegistrationForm from './Components/RegistrationForm/RegistrationForm';
import BMIForm from './Components/BMIForm/BMIForm';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import ForgotPassword from './Components/ForgotPassword/ForgotPassword';
import AboutForm from './Components/About/AboutForm';
import User from './Components/User/User';
import Dashboard from './Components/Dashboard/Dashboard';
import DietPlan from './Components/DietPlan/DietPlan';
import Header from './Components/Header/Header';
import Home from './Components/Home/Home';
import Meals from './Components/Meals/Meals';
import Template from './Components/Template/Template';
import DietitianReg from './Components/DietitianReg/DietitianReg';

function App() {
  return (
    <Router>
    <div className="App">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/bmi" element={<BMIForm />} />
        <Route path="/reg" element={<RegistrationForm />} />
        <Route path="/forgot" element={<ForgotPassword/>} />
        <Route path="/about" element={<AboutForm/>} />
        <Route path="/user" element={<User/>} />
        <Route path="/dashboard" element={<Dashboard/>} />
        <Route path="/diet" element={<DietPlan/>} />
        <Route path="/header" element={<Header/>} />
        <Route path="/meals" element={<Meals />} />
        <Route path="/template" element={<Template />} />
        <Route path="/DiatetianReg" element={<DietitianReg />} />
      </Routes>
    </div>
    </Router>
  );
}

export default App;
