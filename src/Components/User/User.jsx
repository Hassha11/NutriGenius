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
    /*const handleSubmit = async (e) => {
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

                navigate('/user'); 
            }
        } catch (error) {
            console.error('There was an error fetching user data!', error);
            setError('Invalid username or password');
        }
    };*/

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            // Make the POST request to the login API
            const response = await axios.post('http://localhost:5274/api/User/User', {
                userName: username,
                password: password
            });
    
            console.log(response); // Log the entire response to check if data is being returned correctly
    
            if (response.status === 200) {
                // Extract user data from the response
                const userData = response.data;
    
                // Log the user data
                console.log('User data:', userData);
    
                // Store the userId, username, and password in localStorage for later use
                localStorage.setItem('userId', userData.UserID);
                localStorage.setItem('username', userData.UserName);
                localStorage.setItem('password', userData.Password);
    
                // Update the component state with the user data
                setUserId(userData.UserID);
                setName(userData.Name);
                setGender(userData.Gender);
                setDOB(userData.DOB);
                setUsername(userData.UserName);
                setPassword(userData.Password);
    
                // Redirect the user to the profile page (or wherever you want to navigate)
                navigate('/user'); 
            }
        } catch (error) {
            console.error('There was an error fetching user data!', error);
            setError('Invalid username or password');
        }
    };     

    /*useEffect(() => {
        if (userId) {
            const fetchProfileData = async () => {
                try {
                    const response = await axios.get(`http://localhost:5274/api/User/Profile`);
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
    }, [userId]);*/

    useEffect(() => {
        const fetchProfileData = async () => {
            const storedUsername = localStorage.getItem('username');
            const storedPassword = localStorage.getItem('password');

            // Logging to verify localStorage values
            console.log('Stored Username:', storedUsername);
            console.log('Stored Password:', storedPassword);

            if (storedUsername && storedPassword) {
                try {
                    const response = await axios.get(`http://localhost:5274/api/User/Profile`, {
                   //const response = await axios.get(`http://localhost:5274/api/User/Profile?userName=${storedUsername}&password=${storedPassword}`, {
                        params: {
                            userName: storedUsername,
                            password: storedPassword
                        }
                    });

                    // Log the entire response for debugging
                    console.log('Profile API Response:', response);

                    if (response.status === 200) {
                        const profileData = response.data;

                        // Logging the profile data
                        console.log('Profile Data:', profileData);

                        setName(profileData.Name);
                        setGender(profileData.Gender);
                        setDOB(profileData.DOB);
                        setUsername(profileData.UserName);
                        setPassword(profileData.Password);
                    }
                } catch (error) {
                    console.error('There was an error fetching profile data!', error);
                    setError('Failed to load profile data');
                }
            } else {
                console.error('No stored user information found');
                setError('User is not logged in');
            }
        };

        fetchProfileData();
    }, []);  

    return (
        <div className='layout'>
            <Header />
            <Dashboard />
            <div style={{ marginTop: '5px', height: '460px' }} className='wrapper-user'>
                <form onSubmit={handleSubmit}>
                    <h1 style={{ marginTop: '-5px', color: 'GrayText' }}>User Profile</h1>
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
