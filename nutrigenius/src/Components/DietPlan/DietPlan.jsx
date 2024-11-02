import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';
import Meals from '../Meals/Meals';

const DietPlanForm = () => {
    const navigate = useNavigate();
    
    // State variables for form inputs
    const [age, setAge] = useState('');
    const [bmi, setBmi] = useState('');
    const [diabetes, setDiabetes] = useState(0);
    const [cholesterol, setCholesterol] = useState(0);
    const [thyroidDiseases, setThyroidDiseases] = useState(0);
    const [heartDiseases, setHeartDiseases] = useState(0);
    const [depression, setDepression] = useState(0);

    const handleSubmit = async (e) => {
        e.preventDefault(); // Prevent the default form submission

        try {
            const response = await axios.post('http://localhost:5274/api/GetDietPlan/GetDietPlan', {
                age: parseInt(age), // Convert to integer
                bmi: parseFloat(bmi), // Convert to float
                diabetes: parseInt(diabetes),
                cholesterol: parseInt(cholesterol),
                thyroidDiseases: parseInt(thyroidDiseases),
                heartDiseases: parseInt(heartDiseases),
                depression: parseInt(depression),
                dietPlan: "Diet plan string here" // Modify as needed
            });

            if (response.status === 200) {
                console.log(response.data);
                alert("Diet plan saved successfully!");
                //navigate('/meals'); 
                navigate('/meals', { state: { age, bmi, diabetes, cholesterol, thyroidDiseases, heartDiseases, depression } });
            }
        } catch (error) {
            console.error('Error occurred while saving:', error);
            if (error.response) {
                alert("Failed to save diet plan: " + error.response.data);
            } else {
                alert("An unexpected error occurred. Please try again.");
            }
        }
    };

    const inputStyle = {
        width: '260px', 
        marginBottom: '4px', 
        padding: '12px',
        marginTop: '10px'
    };

    return (
        <div className="diet-plan-form">
        <Header />
        <Dashboard />
        <div className="wrapper">
            <h2>Create Your Diet Plan</h2>
            <form onSubmit={handleSubmit}>
                <div>
                <div className="input-box" style={{ display: 'flex', alignItems: 'center', marginTop: '35px', marginBottom: '8px' }}>
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
                    </div>
                <div>
                <div className="input-box reduced-gap" style={{ display: 'flex', alignItems: 'center', marginTop: '10px', marginBottom: '8px' }}>
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
                    </div>
                <div>
                <div className="input-box reduced-gap" style={{ display: 'flex', alignItems: 'center', marginTop: '10px', marginBottom: '8px' }}>
                <label htmlFor="diabetes">Diabetes</label>
                    <select id="diabetes" value={diabetes} onChange={(e) => setDiabetes(e.target.value)} required style={inputStyle}>
                    <option value="">Select Level</option>
                    <option value="0">None</option>
                    <option value="1">Level 1 (70-99 mg/dL)</option>
                    <option value="2">Level 2 (100-125 mg/dL)</option>
                    <option value="3">Level 3 (126 mg/dL or Higher)</option>
                    </select>
                    </div>
                    </div>
                <div>
                <div className="input-box reduced-gap" style={{ display: 'flex', alignItems: 'center', marginTop: '10px', marginBottom: '8px' }}>
                        <label>Cholesterol</label>
                        <select value={cholesterol} onChange={(e) => setCholesterol(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Less than 200 mg/dL)</option>
                            <option value="2">Level 2 (200-239 mg/dL)</option>
                            <option value="3">Level 3 (240 mg/dL and Above)</option>
                        </select>
                    </div>
                    </div>
                <div>
                <div className="input-box reduced-gap" style={{ display: 'flex', alignItems: 'center', marginTop: '10px', marginBottom: '8px' }}>
                <label>Thyroid Disorders</label>
                        <select value={thyroidDiseases} onChange={(e) => setThyroidDiseases(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Thyroid-Stimulating Hormone (TSH))</option>
                            <option value="2">Level 2 (Free Thyroxine (Free T4))</option>
                            <option value="3">Level 3 (Free Triiodothyronine (Free T3))</option>
                        </select>
                    </div>
                    </div>
                <div>
                <div className="input-box reduced-gap" style={{ display: 'flex', alignItems: 'center', marginTop: '10px', marginBottom: '8px' }}>
                <label>Heart Disease</label>
                        <select value={heartDiseases} onChange={(e) => setHeartDiseases(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Heart Rate 60bpm-100bpm)</option>
                            <option value="2">Level 2 (Blood Pressure 120/80 mmHg)</option>
                            <option value="3">Level 3 (Ejection Fraction 55%-70%)</option>
                        </select>
                    </div>
                    </div>
                <div>
                <div className="input-box reduced-gap" style={{ display: 'flex', alignItems: 'center', marginTop: '10px', marginBottom: '8px' }}>
                <label>Depression</label>
                        <select value={depression} onChange={(e) => setDepression(e.target.value)} required style={inputStyle}>
                            <option value="">Select Level</option>
                            <option value="0">None</option>
                            <option value="1">Level 1 (Mild Depression)</option>
                            <option value="2">Level 2 (Moderate Depression)</option>
                            <option value="3">Level 3 (Severe Depression)</option>
                        </select>
                    </div>
                    </div>
                <button type="submit">Submit Diet Plan</button>
            </form>
            </div>
            <Footer />
        </div>
    );
};

export default DietPlanForm;
