import React from 'react';
import { Link, Outlet } from 'react-router-dom';
import './Home.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const Home = () => {
    return (
        <div className="layout">
        <Dashboard/>
        <Header/>
        <Footer/>
            <h1>Welcome to NutriGenius...</h1>
        </div>
    );
};

export default Home;