import React, { useState } from 'react';
import './DietPlan.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const DietPlan = () => {
    const [diabetes, setDiabetes] = useState('');
    const [cholesterol, setCholesterol] = useState('');
    const [thyroid, setThyroid] = useState('');
    const [heartDisease, setHeartDisease] = useState('');
    const [depression, setDepression] = useState('');
    const navigate = useNavigate();

    const handleDiet = async (e) => {
        e.preventDefault();
    
        try {
            const response = await axios.post('http://localhost:5274/api/DietPlan/DietPlan', {
                diabetes: diabetes, 
                cholesterol: cholesterol,
                thyroid: thyroid,
                heart: heartDisease,
                depression: depression,
            });
    
            if (response.status === 200) {
                alert("User Data Saved Successfully");
                navigate('/meal'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                alert("Please Try Again");
            } else {
                console.error('There was an error!', error);
            }
        }
    };    

    return (
        <div className='layout'>
        <Header />
         <Dashboard />
        <div style={{ marginTop: '40px' , height: '580px' }} className='wrapper'>
            <form onSubmit={handleDiet}>
                <h1 style={{ marginTop: '5px' }}>Request a Diet Plan</h1>
                <h4 style={{ color: 'black'}}>If you have bellow diseases please select the diseases' levels</h4>
                <div style={{ marginTop: '10px' }} className="input-box">
                    <label>Diabetes</label>
                    <select value={diabetes} onChange={(e) => setDiabetes(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="none">None</option>
                        <option value="low">Level 1 (70-99 mg/dL)</option>
                        <option value="medium">Level 2 (100-125 mg/dL)</option>
                        <option value="high">Level 3 (126 mg/dL or Higher)</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Cholesterol</label>
                    <select value={cholesterol} onChange={(e) => setCholesterol(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="none">None</option>
                        <option value="low">Level 1 (Less than 200 mg/dL)</option>
                        <option value="medium">Level 2 (200-239 mg/dL)</option>
                        <option value="high">Level 3 (240mg/dL and Above)</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Thyroid Disorders</label>
                    <select value={thyroid} onChange={(e) => setThyroid(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="none">None</option>
                        <option value="low">Level 1 (Thyroid-Stimulating Hormone (TSH))</option>
                        <option value="medium">Level 2 (Free Thyroxine (Free T4))</option>
                        <option value="high">Level 3 (Free Triiodothyronine (Free T3))</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Heart Disease</label>
                    <select value={heartDisease} onChange={(e) => setHeartDisease(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="none">None</option>
                        <option value="low">Level 1 (Heart Rate 60bpm-100bpm)</option>
                        <option value="medium">Level 2 (Blood Pressure 120/80 mmHg)</option>
                        <option value="high">Level 3 (Ejection Fraction 55%-70%)</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Depression</label>
                    <select value={depression} onChange={(e) => setDepression(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="none">None</option>
                        <option value="low">Level 1 (Mild Depression)</option>
                        <option value="medium">Level 2 (Moderate Depression)</option>
                        <option value="high">Level 3 (Severe Depression)</option>
                    </select>
                </div>
                {/*<button type="submit">Submit</button>*/}
                <button type="submit" onClick={() => navigate('/meals')}>Submit</button>
            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default DietPlan;