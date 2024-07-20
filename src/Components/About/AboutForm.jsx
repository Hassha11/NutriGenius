import React from 'react';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const AboutForm = () => {
    return (
        <div className='layout'>
        <Header />
         <Dashboard />
        <div>
            <h1 style={{ color: 'GrayText', marginTop: '-220px', marginLeft: '-400px' }}>About Us</h1>
            <p1><h3 style={{ marginTop: '30px', color: 'black', marginLeft: '60px', fontFamily: 'cursive'}}>We always try to give you a very healthy personalized Diet Plan</h3></p1>
            <p2><h3 style={{ marginTop: '30px', color: 'black', marginLeft: '150px', fontFamily: 'cursive'}}>Contact Us</h3></p2>
            <p3><h4 style={{ marginTop: '10px', color: 'black', marginLeft: '150px', fontFamily: 'cursive'}}>Email - nutrigenius@gmail.com</h4></p3>
            <p3><h4 style={{ marginTop: '10px', color: 'black', marginLeft: '150px', fontFamily: 'cursive'}}>Tel - 011-2234323</h4></p3>
        </div>
        <Footer/>
        </div>
    );
};

export default AboutForm;
