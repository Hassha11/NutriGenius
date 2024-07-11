//import React from 'react';
import React, { useState } from 'react';
import './ForgotPassword.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const ForgotPassword = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleForgot = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/ForgotPassword/ResetPassword', {
                userName: username,
                password: password,
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
        <div className='layout'>
        <Header />
         <Dashboard />
        <div className='wrapper'>
            <form onSubmit={handleForgot}>
                <h1>Change Password</h1>
                <div className="input-box">
                    <input type="text" placeholder='Username' value={username} onChange={(e) => setUsername(e.target.value)} required/>    
                </div>
                <div className="input-box">
                    <input type="password" placeholder='New Password' value={password} onChange={(e) => setPassword(e.target.value)} required/>
                </div>
                <div className="input-box">
                    <input type="password" placeholder='Confirm Password' required/>
                </div>
                <button type="submit">Submit</button>

            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default ForgotPassword;