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

    const handleBMI = async (event) => {
        event.preventDefault();

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
            <div className='wrapper'>
                <form onSubmit={handleBMI}>
                    <h1>BMI Calculation</h1>
                    <div className="input-box">
                        <select value={gender} onChange={(e) => setGender(e.target.value)} required>
                            <option value="" disabled>Gender</option>
                            <option value="male">Male</option>
                            <option value="female">Female</option>
                        </select>
                    </div>
                    <div className="input-box">
                        <input type="text" placeholder='Age' value={age} onChange={(e) => setAge(e.target.value)} required />
                    </div>
                    <div className="input-box">
                        <input type="text" placeholder='Height' value={height} onChange={(e) => setHeight(e.target.value)} required />
                    </div>
                    <div className="input-box">
                        <input type="text" placeholder='Weight' value={weight} onChange={(e) => setWeight(e.target.value)} required />
                    </div>
                    <div className="flex-container">
                        <button type="submit">Calculate</button>
                        <div className="input-box">
                            <input type="text" placeholder='BMI' value={bmi} readOnly />
                        </div>
                        <div className="input-box">
                            <input type="text" placeholder='Status' value={status} readOnly />
                        </div>
                    </div>
                    <h4>Do you want a Diet Plan?</h4>
                    <div className="flex-container-dietplan">
                        <button type="button" onClick={() => navigate('/diet')}>Yes</button>
                        <button type="button">No</button>
                    </div>
                </form>
            </div>
            <Footer />
        </div>
    );
};

export default BMIForm;
