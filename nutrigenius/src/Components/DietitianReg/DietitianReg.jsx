import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './DietitianReg.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const DietitianReg = () => {
    const [name, setName] = useState('');
    const [gender, setGender] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmpass, setConfirmPass] = useState('');
    const [qualifications, setQualifications] = useState('');
    const navigate = useNavigate();

    const handleDietitianReg = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/Registration/Registration', {
                name: name,
                gender: gender,
                userName: username,
                password: password,
                confirmPass: confirmpass,
                qualifications: qualifications
            });

            if (response.status === 200) {
                alert("Registration Success");
                navigate('/'); 
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
        <div className='layout-dietitian'>
          <Header />
           <Dashboard />
        <div style={{ marginTop: '40px' , height: '580px' }} className='wrapper-dietitian'>
            <form onSubmit={handleDietitianReg}>
                <h1>Dietitian Registration</h1>
                <div style={{ marginTop: '10px' }} className="input-box">
                    <input
                        type="text"
                        placeholder='Name'
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        required
                    />    
                </div>
                <div style={{ marginTop: '-8px' }} className="input-box">
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
                <div style={{ marginTop: '10px' }} className="input-box">
                    <input
                        type="text"
                        placeholder='Username'
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div style={{ marginTop: '-6px' }} className="input-box">
                    <input
                        type="password"
                        placeholder='Password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <div style={{ marginTop: '-6px' }} className="input-box">
                    <input
                        type="password"
                        placeholder='Confirm Password'
                        value={confirmpass}
                        onChange={(e) => setConfirmPass(e.target.value)}
                        required
                    />
                </div>
                <div style={{ marginTop: '-6px' }} className="input-box">
                    <input
                        type="text"
                        placeholder='Qualifications'
                        value={qualifications}
                        onChange={(e) => setQualifications(e.target.value)}
                        required
                    />
                </div>
                <button style={{ marginTop: '-2px' }} type="submit">Register</button>
            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default DietitianReg;
