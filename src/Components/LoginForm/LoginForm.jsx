//import React from 'react';
import './LoginForm.css';
import axios from 'axios';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaUser, FaLock } from "react-icons/fa";

const LoginForm = () => {

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/Login/Login', {
                userName: username,
                password: password,
            });

            alert(response.data);
            if (response.data === "Data Inserted") {
                navigate('/dashboard'); 
            }
        } catch (error) {
            console.error('There was an error logging in!', error);
        }
    };

    return (
        <div className='wrapper'>
            <form onSubmit={handleLogin}>
                <h1>Login</h1>
                <div className="input-box">
                    <input type="text" placeholder='Username' value={username} onChange={(e) => setUsername(e.target.value)} required/> 
                    <FaUser className='icon'/>    
                </div>
                <div className="input-box">
                    <input type="password" placeholder='Password' value={password} onChange={(e) => setPassword(e.target.value)} required/>
                    <FaLock className='icon'/>
                </div>

                <div className="remember-forgot">
                    <label><input type="checkbox"/>Remember me</label>
                    <a href="#">Forgot password?</a>
                </div>

                <button type="submit">Login</button>

                <div className="register-link">
                    <p>Don't have an account? <a href="#">Register</a></p>
                </div>
            </form>
        </div>
    );
};

export default LoginForm;