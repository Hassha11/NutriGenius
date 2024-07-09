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
            <form action="">
                <h1>BMI Calculation</h1>
                <div className="input-box">
                    <select required>
                        <option value="" disabled selected>Gender</option>
                        <option value="male">Male</option>
                        <option value="female">Female</option>
                    </select>
                </div>
                <div className="input-box">
                    <input type="text" placeholder='Age' required/>
                </div>
                <div className="input-box">
                    <input type="text" placeholder='Height' required/>
                </div>
                <div className="input-box">
                    <input type="text" placeholder='Weight' required/>
                </div>
                <div class="flex-container">
                <button type="submit">Calculate</button>
                <div className="input-box">
                    <input type="text" placeholder='BMI' required/>
                    </div>    
                </div>

            </form>
        </div>
    );
};

export default BMIForm;