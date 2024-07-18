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
    const [userId, setUserId] = useState('');
    const [error, setError] = useState(null);

    const navigate = useNavigate();

    // Handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post('http://localhost:5274/api/User/User', {
                userName: username,
                password: password
            });

            if (response.status === 200) {
                const userData = response.data;
                setUserId(userData.UserID);
                setName(userData.Name);
                setGender(userData.Gender);
                setDOB(userData.DOB);
                setUsername(userData.UserName);
                setPassword(userData.Password);
            }
        } catch (error) {
            console.error('There was an error fetching user data!', error);
            setError('Invalid username or password');
        }
    };

    useEffect(() => {
        if (userId) {
            const fetchProfileData = async () => {
                try {
                    const response = await axios.get(`http://localhost:5274/api/User/Profile/10`);
                    if (response.status === 200) {
                        const profileData = response.data;
                        setName(profileData.Name);
                        setGender(profileData.Gender);
                        setDOB(profileData.DOB);
                        setUsername(profileData.UserName);
                    }
                } catch (error) {
                    console.error('There was an error fetching profile data!', error);
                }
            };

            fetchProfileData();
        }
    }, [userId]);

    return (
        <div className='layout'>
            <Header />
            <Dashboard />
            <div style={{ marginTop: '5px', height: '460px' }} className='wrapper-user'>
                <form onSubmit={handleSubmit}>
                    <h2 style={{ marginTop: '-5px' }}>User Profile</h2>
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
                    <div style={{ marginTop: '-15px'}}className="button-user">
                        <button type="submit">Update</button>
                        <button type="submit">Delete</button>
                    </div>
                </form>
                {error && <p className="error">{error}</p>}
            </div>
            <Footer />
        </div>
    );
};

export default User;
