import React, { useState, useEffect } from 'react';
import './DietPlan.css';
import axios from 'axios';
import { useNavigate, useLocation  } from 'react-router-dom';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';
//import { FaAlignRight } from 'react-icons/fa';

const DietPlan = () => {
    const location = useLocation();
    const [age, setAge] = useState('');
    const [bmi, setBmi] = useState('');
    const [diabetes, setDiabetes] = useState('');
    const [cholesterol, setCholesterol] = useState('');
    const [thyroid, setThyroid] = useState('');
    const [heartDisease, setHeartDisease] = useState('');
    const [depression, setDepression] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        const params = new URLSearchParams(location.search);
        const ageParam = params.get('age') || '';
        const bmiParam = params.get('bmi') || '';
        
        setAge(ageParam);
        setBmi(bmiParam);
    }, [location.search]);

    const handleDiet = async (e) => {
        e.preventDefault();
    
        try {
            const response = await axios.post('http://localhost:5274/api/Diet/GetDietPlan', {
                Age: age,
                BMI: bmi,
                Diabetes: diabetes, 
                Cholesterol: cholesterol,
                ThyroidDiseases: thyroid,
                HeartDiseases: heartDisease,
                Depression: depression,
            });
    
            if (response.status === 200) {
                alert("User Data Saved Successfully");
                navigate('/meals'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                alert("Please Try Again");
            } else {
                console.error('There was an error!', error);
            }
        }
    };    

    const inputStyle = {
        width: '260px', // Adjust the width as needed
        marginBottom: '4px', // Reduce spacing between fields
        padding: '12px',
        marginTop: '-40px'
    };

    return (
        <div className='layout'>
            <Header />
            <Dashboard />
            <div style={{ marginTop: '40px', height: '580px' }} className='wrapper'>
                <form onSubmit={handleDiet}>
                    <h1 style={{ marginTop: '5px' }}>Request a Diet Plan</h1>
                    <h4 style={{ color: 'black' }}>If you have the below conditions, please select the levels</h4>
                    
                    <div className="input-box" style={{ marginTop: '35px', marginBottom: '8px' }}>
                    <label htmlFor="age">Age</label>
                    <input 
                     type="number" 
                     id="age" 
                     value={age} 
                     onChange={(e) => setAge(e.target.value)} 
                     required 
                     style={inputStyle}
                     />
                    </div>

                    <div className="input-box reduced-gap" style={{ marginTop: '10px', marginBottom: '8px' }}>
                    <label htmlFor="bmi">BMI</label>
                    <input 
                     type="number" 
                     id="bmi"  
                     value={bmi} 
                     onChange={(e) => setBmi(e.target.value)} 
                     required 
                     style={inputStyle}
                     />
                    </div>

                    <div className="input-box reduced-gap" style={{ marginTop: '10px', marginBottom: '8px' }}>
                    <label htmlFor="diabetes">Diabetes</label>
                    <select id="diabetes" value={diabetes} onChange={(e) => setDiabetes(e.target.value)} required style={inputStyle}>
                    <option value="">Select Level</option>
                    <option value="0">None</option>
                    <option value="1">Level 1 (70-99 mg/dL)</option>
                    <option value="2">Level 2 (100-125 mg/dL)</option>
                    <option value="3">Level 3 (126 mg/dL or Higher)</option>
                    </select>
                    </div>


                    <div className="input-box reduced-gap" style={{ marginTop: '10px', marginBottom: '8px' }}>
                        <label>Cholesterol</label>
                        <select value={cholesterol} onChange={(e) => setCholesterol(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Less than 200 mg/dL)</option>
                            <option value="2">Level 2 (200-239 mg/dL)</option>
                            <option value="3">Level 3 (240 mg/dL and Above)</option>
                        </select>
                    </div>
                    
                    <div className="input-box reduced-gap" style={{ marginTop: '10px', marginBottom: '8px' }}>
                        <label>Thyroid Disorders</label>
                        <select value={thyroid} onChange={(e) => setThyroid(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Thyroid-Stimulating Hormone (TSH))</option>
                            <option value="2">Level 2 (Free Thyroxine (Free T4))</option>
                            <option value="3">Level 3 (Free Triiodothyronine (Free T3))</option>
                        </select>
                    </div>
                    
                    <div className="input-box reduced-gap" style={{ marginTop: '10px', marginBottom: '8px' }}>
                        <label>Heart Disease</label>
                        <select value={heartDisease} onChange={(e) => setHeartDisease(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Heart Rate 60bpm-100bpm)</option>
                            <option value="2">Level 2 (Blood Pressure 120/80 mmHg)</option>
                            <option value="3">Level 3 (Ejection Fraction 55%-70%)</option>
                        </select>
                    </div>
                    
                    <div className="input-box reduced-gap" style={{ marginTop: '10px', marginBottom: '8px' }}>
                        <label>Depression</label>
                        <select value={depression} onChange={(e) => setDepression(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Mild Depression)</option>
                            <option value="2">Level 2 (Moderate Depression)</option>
                            <option value="3">Level 3 (Severe Depression)</option>
                        </select>
                    </div>
                    <button type="submit" onClick={() => navigate('/meals')} style={{ width: '380px', padding: '8px', marginTop: '-20px', float: 'left'}}>Submit</button>
                </form>
            </div>
            <Footer />
        </div>
    );
};

export default DietPlan;
