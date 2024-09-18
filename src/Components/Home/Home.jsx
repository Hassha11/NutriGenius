import React from 'react';
import { Link, Outlet } from 'react-router-dom';
import './Home.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const Home = () => {
    return (
        <div className="layout-home">
        <Dashboard/>
        <Header/>
            <h1 style={{ marginTop: '-60px', marginLeft: '-180px', color:'black' }}>Welcome to NutriGenius...</h1><br></br>
            <p1><h3 style={{ top: '100px', marginLeft: '170px' }}>NutriGenius will give you the guidance you need to live a very healthy life.</h3></p1>
            <p2><h3 style={{ top: '100px', marginLeft: '170px' }}> A personalized diet plan is provoded here according to your BMI value.</h3></p2>
            <p3><h3 style={{ top: '100px', marginLeft: '170px' }}>So you can get your favourite foods very healthy regularly withoud any fear.</h3></p3>
            <Footer/>
        </div>
    );
};

export default Home;