import React, { useState, useEffect } from 'react';
import './Meals.css';
import axios from 'axios';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

function DietPlanForm() {
    const [age, setAge] = useState('');
    const [bmi, setBmi] = useState('');
    const [diabetes, setDiabetes] = useState(0);
    const [cholesterol, setCholesterol] = useState(0);
    const [thyroidDiseases, setThyroidDiseases] = useState(0);
    const [heartDiseases, setHeartDiseases] = useState(0);
    const [depression, setDepression] = useState(0);
    const [dietPlan, setDietPlan] = useState('');
    const [loading, setLoading] = useState(true);
    const [dietID, setDietID] = useState(0); // Initialize to 0
    const [error, setError] = useState('');

    // Function to handle meal (fetching diet plan)
    const handleMeal = async () => {
        setLoading(true); // Start loading state
        setError(''); // Clear previous errors

        try {   
            const response = await axios.get('http://localhost:5274/api/GetDietPlan/GetDietPlan', {
                params: {
                    age,
                    bmi,
                    diabetes,
                    cholesterol,
                    thyroidDiseases,
                    heartDiseases,
                    depression,
                    dietID: 0 // Send initial dietID as needed for testing
                }
            });

            if (response.status === 200 && response.data.userHealthData) {
                console.log("API Response Data:", response.data);
                setAge(response.data.userHealthData.age);
                setBmi(response.data.userHealthData.bmi);
                setDiabetes(response.data.userHealthData.diabetes);
                setCholesterol(response.data.userHealthData.cholesterol);
                setThyroidDiseases(response.data.userHealthData.thyroidDiseases);
                setHeartDiseases(response.data.userHealthData.heartDiseases);
                setDepression(response.data.userHealthData.depression);
                setDietPlan(response.data.userHealthData.dietPlan); // Access with lowercase 'd'
                setDietID(response.data.userHealthData.dietID); // Update dietID if needed
            } else {
                console.error('No data received for diet plan');
                setError('No data received for diet plan');
            }
        } catch (error) {
            console.error('Error generating diet plan', error);
            setError('Error generating diet plan');
        } finally {
            setLoading(false); 
        }
    };

    useEffect(() => {
        handleMeal();
    }, [age, bmi, diabetes, cholesterol, thyroidDiseases, heartDiseases, depression]);

    // Function to generate a downloadable file
    const downloadDietPlan = () => {
        const element = document.createElement("a");
        const file = new Blob([dietPlan], { type: 'text/plain' });
        element.href = URL.createObjectURL(file);
        element.download = "DietPlan.txt";
        document.body.appendChild(element);
        element.click();
    };

    // Render loading state while data is being fetched
    if (loading) {
        return <p>Loading...</p>;
    }

    return (
        <div className='layout-meals'>
            <Header />
            <Dashboard />
            <div className='wrapper-meals'>
                <form onSubmit={(e) => e.preventDefault()}>
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
                            <option value={1}>Level 1 (TSH)</option>
                            <option value={2}>Level 2 (Free T4)</option>
                            <option value={3}>Level 3 (Free T3)</option>
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
                            <option value={1}>Level 1 (Heart Rate 60-100 bpm)</option>
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
                    <button type="button" onClick={downloadDietPlan} style={{ width: '100%', padding: '10px', marginTop: '10px'}}>Download Diet Plan</button>
                </form>

                {error && <p style={{ color: 'red' }}>{error}</p>}

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
