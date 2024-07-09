//import React from 'react';
import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './User.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const User = () => {
    //const [name, setUserID] = useState('');
    const [name, setName] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [age, setAge] = useState('');
    const [height, setHeight] = useState('');
    const [weight, setWeight] = useState('');
    const [bmi, setBMI] = useState('');
    const [gender, setGender] = useState('');
    const navigate = useNavigate();

    const handleUser = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/BMI/BMI', {
                name: name,
                username: username,
                password: password,
                age: age,
                height: height,
                weight: weight,
                bmi: bmi,
                gender: gender
            });

            if (response.status === 200) {
                alert("BMI Calculation Success");
                navigate('/'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                alert("BMI Calculation Unsuccessful");
            } else {
                console.error('There was an error registering!', error);
            }
        }
    };

    return (
        <div className='layout'>
        <Header />
         <Dashboard />
        <div className='wrapper'>
            <form onSubmit={handleUser}>
                <h2>User Profile</h2>
                <div className="input-box">
                <input type="text" placeholder='Name'/>
                </div>
                <div className="input-box">
                <input type="text" placeholder='Username'/>
                </div>
                <div className="input-box">
                <input type="text" placeholder='Password'/>
                </div>
                <div className="input-box">
                <input type="text" placeholder='Age'/> 
                </div>
                <div className="input-box">
                <input type="text" placeholder='Height'/> 
                </div>
                <div className="input-box">
                <input type="text" placeholder='Weight'/> 
                </div>
                <div className="input-box">
                <input type="text" placeholder='BMI'/>
                </div>
                <div className="input-box">
                    <select value={gender} onChange={(e) => setGender(e.target.value)}required>
                        <option value="" disabled selected>Gender</option>
                        <option value="male">Male</option>
                        <option value="female">Female</option>
                    </select>
                </div>
            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default User;
