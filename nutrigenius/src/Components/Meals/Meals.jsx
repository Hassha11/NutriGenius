import React, { useState } from 'react';
import './Meals.css';
import axios from 'axios';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

function DietPlanForm() {
    const [age, setAge] = useState('');
    const [bmi, setBmi] = useState('');
    const [diabetes, setDiabetes] = useState('');
    const [cholesterol, setCholesterol] = useState('');
    const [thyroidDiseases, setThyroidDiseases] = useState('');
    const [heartDiseases, setHeartDiseases] = useState('');
    const [depression, setDepression] = useState('');
    const [dietPlan, setDietPlan] = useState('');

    // Handle form submission to send data to the backend
    const handleMeal = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/Diet/get-diet-plan', {
                age: age,
                bmi: bmi,
                diabetes,
                cholesterol,
                thyroidDiseases,
                heartDiseases,
                depression,
                dietPlan
            });

            if (response.status === 200) {
                setDietPlan(response.data.dietPlan); // Store the returned diet plan
            }
        } catch (error) {
            console.error('Error generating diet plan', error);
        }
    };

    // Function to generate a downloadable file
    const downloadDietPlan = () => {
        const element = document.createElement("a");
        const file = new Blob([dietPlan], { type: 'text/plain' });
        element.href = URL.createObjectURL(file);
        element.download = "DietPlan.txt";
        document.body.appendChild(element); // Required for this to work in FireFox
        element.click();
    };

    // Function to clear the form fields
    const resetForm = () => {
        setAge('');
        setBmi('');
        setDiabetes('');
        setCholesterol('');
        setThyroidDiseases('');
        setHeartDiseases('');
        setDepression('');
        setDietPlan('');
    };

    return (
        <div className='layout-meals'>
            <Header />
            <Dashboard />
            <div className='wrapper-meals'>
                <form onSubmit={handleMeal}>
                    <div className="form-group">
                        <label htmlFor="age">Age</label>
                        <textarea
                            id="age"
                            value={age}
                            onChange={(e) => setAge(e.target.value)}
                            rows={1}
                            style={{ width: '100%' }}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="bmi">BMI</label>
                        <textarea
                            id="bmi"
                            value={bmi}
                            onChange={(e) => setBmi(e.target.value)}
                            rows={1}
                            style={{ width: '100%' }}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="diabetes">Diabetes</label>
                        <select
                            id="diabetes"
                            value={diabetes}
                            onChange={(e) => setDiabetes(parseInt(e.target.value))}
                        >
                            <option value={0}>None</option>
                            <option value={1}>Level 1 (70-99 mg/dL)</option>
                            <option value={2}>Level 2 (100-125 mg/dL)</option>
                            <option value={3}>Level 3 (126 mg/dL or Higher)</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="cholesterol">Cholesterol</label>
                        <select
                            id="cholesterol"
                            value={cholesterol}
                            onChange={(e) => setCholesterol(parseInt(e.target.value))}
                        >
                            <option value={0}>None</option>
                            <option value={1}>Level 1 (Less than 200 mg/dL)</option>
                            <option value={2}>Level 2 (200-239 mg/dL)</option>
                            <option value={3}>Level 3 (240mg/dL and Above)</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="thyroidDiseases">Thyroid Diseases</label>
                        <select
                            id="thyroidDiseases"
                            value={thyroidDiseases}
                            onChange={(e) => setThyroidDiseases(parseInt(e.target.value))}
                        >
                            <option value={0}>None</option>
                            <option value={1}>Level 1 (Thyroid-Stimulating Hormone (TSH))</option>
                            <option value={2}>Level 2 (Free Thyroxine (Free T4))</option>
                            <option value={3}>Level 3 (Free Triiodothyronine (Free T3))</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="heartDiseases">Heart Diseases</label>
                        <select
                            id="heartDiseases"
                            value={heartDiseases}
                            onChange={(e) => setHeartDiseases(parseInt(e.target.value))}
                        >
                            <option value={0}>None</option>
                            <option value={1}>Level 1 (Heart Rate 60bpm-100bpm)</option>
                            <option value={2}>Level 2 (Blood Pressure 120/80 mmHg)</option>
                            <option value={3}>Level 3 (Ejection Fraction 55%-70%)</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="depression">Depression</label>
                        <select
                            id="depression"
                            value={depression}
                            onChange={(e) => setDepression(parseInt(e.target.value))}
                        >
                            <option value={0}>None</option>
                            <option value={1}>Level 1 (Mild Depression)</option>
                            <option value={2}>Level 2 (Moderate Depression)</option>
                            <option value={3}>Level 3 (Severe Depression)</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="dietPlan">Diet Plan</label>
                        <textarea
                            id="dietPlan"
                            value={dietPlan}
                            readOnly
                            rows={5}
                            style={{ width: '100%' }}
                        />
                    </div>
                    
                    {/* Buttons for Download and Close */}
                    <div className="button-container">
                        <button type="button" onClick={downloadDietPlan}>Download Diet Plan</button>
                        <button type="button" onClick={resetForm}>Close</button>
                    </div>
                </form>
                
                {dietPlan && (
                    <div className="diet-plan">
                        <h3>Your Diet Plan</h3>
                        <p>{dietPlan}</p>
                    </div>
                )}
            </div>
            <Footer />
        </div>
    );    
}

export default DietPlanForm;
