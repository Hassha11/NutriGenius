//import React from 'react';
import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './BMIForm.css';

 
const BMIForm = () => {
    //const [name, setUserID] = useState('');
    const [age, setAge] = useState('');
    const [gender, setGender] = useState('');
    const [height, setHeight] = useState('');
    const [weight, setWeight] = useState('');
    const navigate = useNavigate();

    const handleBMI = async (event) => {
        event.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/BMI/BMI', {
                age: age,
                gender: gender,
                height: height,
                weight: weight,
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
        <div className='wrapper'>

            <nav>

            </nav>
            <form onSubmit={handleBMI}>
                <h1>BMI Calculation</h1>
                <div className="input-box">
                    <select value={gender} onChange={(e) => setGender(e.target.value)}required>
                        <option value="" disabled selected>Gender</option>
                        <option value="male">Male</option>
                        <option value="female">Female</option>
                    </select>
                </div>
                <div className="input-box">
                <input type="text" placeholder='Age' value={age} onChange={(e) => setAge(e.target.value)} required/> 
                </div>
                <div className="input-box">
                <input type="text" placeholder='Height' value={height} onChange={(e) => setHeight(e.target.value)} required/> 
                </div>
                <div className="input-box">
                <input type="text" placeholder='Weight' value={weight} onChange={(e) => setWeight(e.target.value)} required/> 
                </div>
                <div class="flex-container">
                <button type="submit">Calculate</button>
                <div className="input-box">
                    <input type="text" placeholder='BMI'/>
                    </div><br></br>
                <div className="input-box">
                    <input type="text" placeholder='Status'/>
                </div>
                </div>
                <div class="flex-container">
                <h4>Do you want Diet Plan?</h4>
                    <button type="submit">Yes</button>
                    <button type="submit">No</button>
                </div>
            </form>
        </div>
    );
};

export default BMIForm;