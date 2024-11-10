import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './BMIForm.css';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const BMIForm = () => {
    const [age, setAge] = useState('');
    const [gender, setGender] = useState('');
    const [height, setHeight] = useState('');
    const [weight, setWeight] = useState('');
    const [bmi, setBmi] = useState('');
    const [status, setStatus] = useState('');
    const navigate = useNavigate();

    const isUserLoggedIn = localStorage.getItem('authToken'); 

    const handleBMI = async (event) => {
        event.preventDefault();

        if (!isUserLoggedIn) {
            alert("Please log in to the system");
            navigate('/login'); 
            return;
        }

        try {
            const response = await axios.post('http://localhost:5274/api/BMI/BMI', {
                height: parseFloat(height),
                weight: parseFloat(weight),
                age:age,
                gender: gender,
                //bmi:bmi,
                status: status, 
            });

            if (response.status === 200) {
                console.log(response.data);
                const { bmi, status } = response.data; // Assuming the response includes BMI and status
                setBmi(bmi);
                setStatus(status);
                alert("BMI Calculation Success");
            }

        } catch (error) {
            if (error.response && error.response.status === 400) {
                alert("BMI Calculation Unsuccessful");
            } else {
                console.error('There was an error calculating BMI!', error);
            }
        }
    };

    return (
        <div className='layout'>
            <Header />
            <Dashboard />
            <div style={{ marginTop: '40px' , height: '560px' }} className="wrapper">
                <form onSubmit={handleBMI}>
                    <h1>BMI Calculation</h1>
                    <div className="input-box">
                        <select value={gender} onChange={(e) => setGender(e.target.value)} required>
                            <option value="" disabled>Gender</option>
                            <option value="male">Male</option>
                            <option value="female">Female</option>
                        </select>
                    </div>
                    <div style={{ marginTop: '10px' }} className="input-box">
                        <input type="text" placeholder='Age' value={age} onChange={(e) => setAge(e.target.value)} required />
                    </div>
                    <div style={{ marginTop: '10px' }} className="input-box">
                        <input type="text" placeholder='Height (cm)' value={height} onChange={(e) => setHeight(e.target.value)} required />
                    </div>
                    <div style={{ marginTop: '10px' }} className="input-box">
                        <input type="text" placeholder='Weight (kg)' value={weight} onChange={(e) => setWeight(e.target.value)} required />
                    </div>
                    <div style={{ marginTop: '10px' }} className="flex-container">
                        <button style={{ marginTop: '-25px' }} type="submit">Calculate</button>
                        <div  style={{ marginTop: '10px' }} className="input-box">
                            <input style={{ width: '120px' }} type="text" placeholder='BMI' value={bmi} readOnly />
                        </div>
                        <div style={{ marginTop: '10px' }} className="input-box">
                            <input style={{ width: '120px' }}  type="text" placeholder='Status' value={status} readOnly />
                        </div>
                    </div>
                    <h4 style={{ marginTop: '-30px', color: 'GrayText' }}>Do you want a Diet Plan?</h4>
                    <div style={{ marginTop: '-10px' }} className="flex-container-dietplan">
                        <button  style={{ marginTop: '10px', width: '100px', height: '35px', marginRight: '90px'}} type="button" onClick={() => navigate('/diet?age=${age}&bmi=${bmi}')}>Yes</button>
                        <button style={{ marginTop: '10px', width: '100px', height: '35px', marginRight: '30px' }} type="button">No</button>
                    </div>
                </form>
            </div>
            <Footer />
        </div>
    );
};

export default BMIForm;
