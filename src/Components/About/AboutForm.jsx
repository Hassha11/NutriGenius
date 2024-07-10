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
            <h2>About Form</h2>
            <p>Place your about form fields here.</p>
        </div>
        <Footer/>
        </div>
    );
};

export default AboutForm;
