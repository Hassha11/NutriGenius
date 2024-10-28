import React from 'react';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';
import Analysis from '../Analysis/Analysis';
import aboutImage from '../Assets/BMI.JPG';

const AboutForm = () => {
    return (
        <div className='layout'>
        <Header />
         <Dashboard />
         <div style={{ textAlign: 'center', marginTop: '20px' }}>
                <img 
                    src={aboutImage} 
                    alt="About" 
                    style={{ width: '80%', maxWidth: '900px', borderRadius: '8px' }} 
                />
            </div>
        <Footer/>
        </div>
    );
};

export default AboutForm;
