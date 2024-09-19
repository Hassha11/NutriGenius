import React, { useState } from 'react';
import './LoginForm.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { FaUser, FaLock } from "react-icons/fa";
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';


const LoginForm = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    /*const handleLogin = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/Login/Login', {
                userName: username,
                password: password,
            });

            if (response.status === 200) {
                alert("Login Success");
                navigate('/bmi'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                alert("Login Unsuccess");
            } else {
            // setError('There was an error logging in!');
                console.error('There was an error logging in!', error);
            }
        }
    };*/

    const handleLogin = async (event) => {
        event.preventDefault();
    
        try {
            const response = await axios.post('http://localhost:5274/api/Login/Login', {
                userName: username,
                password: password,
            });
    
            if (response.status === 200) { //17
                // Store user information in localStorage
                localStorage.setItem('username', username);
                localStorage.setItem('password', password);
    
                //alert("Login Success");
                //navigate('/bmi'); 
            } //

            if (response.data.userType === 'User') {
                alert("Login Success");
                navigate('/bmi'); 

            } else if (response.data.userType === 'Dietitian') {
                alert("Login Success");
                navigate('/'); 
            }
    
        } catch (error) {
            if (error.response && error.response.status === 401) {
                alert("Login Unsuccess");
            } else {
                console.error('There was an error logging in!', error);
            }
        }
    };

    return (
        <div className='layout'>
          <Header />
           <Dashboard />
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
                    <label style={{ color: 'GrayText' }}><input type="checkbox"/>Remember me</label>
                    <a href="/forgot">Forgot password?</a>
                </div>

                <button type="submit">Login</button>

                <div className="register-link">
                    <p style={{ color: 'GrayText' }}>Don't have an account? <a href="/template">Register</a></p>
                </div>
            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default LoginForm;
