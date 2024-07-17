import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './User.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const User = () => {
    const [name, setName] = useState('');
    const [gender, setGender] = useState('');
    const [dob, setDOB] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [age, setAge] = useState('');
    const [height, setHeight] = useState('');
    const [weight, setWeight] = useState('');
    const [bmi, setBMI] = useState('');
    const [status, setStatus] = useState('');

    const navigate = useNavigate();

    useEffect(() => {
        // Fetch user data after component mounts
        const fetchUserData = async () => {
            try {
                const response = await axios.post('http://localhost:5274/api/User/User', {
                    // Assume you send username and password here, or use a token if you have one
                    username: username,
                    password: password
                });

                if (response.status === 200) {
                    const userData = response.data;
                    setName(userData.Name);
                    setGender(userData.Gender);
                    setDOB(userData.DOB);
                    setUsername(userData.UserName);
                    setPassword(userData.Password);
                    setAge(userData.Age);
                    setHeight(userData.Height);
                    setWeight(userData.Weight);
                    setBMI(userData.BMI);
                    setStatus(userData.Status);
                }
            } catch (error) {
                console.error('There was an error fetching user data!', error);
            }
        };

        fetchUserData();
    }, []); // Empty dependency array means this useEffect runs once when the component mounts

    return (
        <div className='layout'>
            <Header />
            <Dashboard />
            <div className='wrapper-user'>
                <form>
                    <h2>User Profile</h2>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='Name' 
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        /><br></br>
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='Gender' 
                            value={gender}
                            onChange={(e) => setGender(e.target.value)}
                        />
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='DOB' 
                            value={dob}
                            onChange={(e) => setDOB(e.target.value)}
                        />
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='Username' 
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                        />
                    </div>
                    <div className="input-box">
                        <input 
                            type="password" 
                            placeholder='Password' 
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='Age' 
                            value={age}
                            onChange={(e) => setAge(e.target.value)}
                        /> 
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='Height' 
                            value={height}
                            onChange={(e) => setHeight(e.target.value)}
                        /> 
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='Weight' 
                            value={weight}
                            onChange={(e) => setWeight(e.target.value)}
                        /> 
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='BMI' 
                            value={bmi}
                            onChange={(e) => setBMI(e.target.value)}
                        />
                    </div>
                    <div className="input-box">
                        <input 
                            type="text" 
                            placeholder='Status' 
                            value={status}
                            onChange={(e) => setStatus(e.target.value)}
                        />
                    </div>
                    <div className="button-user">
                        <button type="submit">Update</button>
                        <button type="submit">Delete</button>
                    </div>
                </form>
            </div>
            <Footer/>
        </div>
    );
};

export default User;
