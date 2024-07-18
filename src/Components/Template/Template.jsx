import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './Template.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const Template = () => {
    //const [username, setUsername] = useState('');
    //const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleTemplate = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/ForgotPassword/ResetPassword', {
                //userName: username,
                //password: password,
            });

            if (response.status === 200) {
                alert("Password change Success");
                navigate('/login'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                alert("Password change Unsuccess");
            } else {
            // setError('There was an error logging in!');
                console.error('There was an error changing password!', error);
            }
        }
    };

    return (
        <div className='layout-template'>
        <Header />
         <Dashboard />
        <div style={{ marginTop: '20px', height: '150px', width: '300px', alignItems: 'center' }} className='wrapper-template'>
            <form onSubmit={handleTemplate}>
                <h1>Login As</h1>
                <div style={{ marginTop: '-10px' }} className="flex-container-template">
                        <button style={{ marginTop: '40px' }} type="button" onClick={() => navigate('/reg')}>User</button>
                        <button style={{ marginTop: '40px' }} type="button" onClick={() => navigate('/DiatetianReg')}>Dietitian</button>
               </div>
            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default Template;