import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './RegistrationForm.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const RegistrationForm = () => {
    const [name, setName] = useState('');
    const [gender, setGender] = useState('');
    const [DOB, setDOB] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmpass, setConfirmPass] = useState('');
    const navigate = useNavigate();

    const handleRegistration = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/Registration/Registration', {
                name: name,
                gender: gender,
                dob: DOB,
                userName: username,
                password: password,
                confirmPass: confirmpass,
            });

            if (response.status === 200) {
                alert("Registration Success");
                navigate('/login'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                alert("Registration Unsuccessful");
            } else {
                console.error('There was an error registering!', error);
            }
        }
    };

    return (
        <div className='layout'>
          <Header />
           <Dashboard />
        <div style={{ marginTop: '30px' , height: '580px' }} className='wrapper'>
            <form onSubmit={handleRegistration}>
            <div className="header-container">
              <h1 style={{ marginTop: '-10px' }}>Registration</h1> {/* Adjust the margin as needed */}
            </div>
                <div style={{ marginTop: '10px' }} className="input-box">
                    <input
                        type="text"
                        placeholder='Name'
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                    />    
                </div>
                <div style={{ marginTop: '10px' }} className="input-box">
                    <select
                        value={gender}
                        onChange={(e) => setGender(e.target.value)}
                        required
                    >
                        <option value="" disabled>Gender</option>
                        <option value="male">Male</option>
                        <option value="female">Female</option>
                    </select>
                </div>
                <div style={{ marginTop: '5px' }} className="input-box">
                    <label style={{ color: 'GrayText' }} htmlFor="dob">Date of Birth</label>
                    <input
                        type="date"
                        id="dob"
                        value={DOB}
                        onChange={(e) => setDOB(e.target.value)}
                        required
                    />
                </div>
                <div style={{ marginTop: '5px' }} className="input-box">
                    <input
                        type="text"
                        placeholder='Username'
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div style={{ marginTop: '-12px' }} className="input-box">
                    <input
                        type="password"
                        placeholder='Password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <div style={{ marginTop: '-10px' }} className="input-box">
                    <input
                        type="password"
                        placeholder='Confirm Password'
                        value={confirmpass}
                        onChange={(e) => setConfirmPass(e.target.value)}
                        required
                    />
                </div>
                <button style={{ marginTop: '-12px' }} type="submit">Register</button>
            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default RegistrationForm;
