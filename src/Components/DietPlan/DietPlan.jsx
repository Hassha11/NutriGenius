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
                diebetes: diabetes,
                cholesterol: cholesterol,
                thyroid: thyroid,
                heart: heartDisease,
                depression: depression,
            });

            if (response.status === 200) {
                //alert("Registration Success");
                navigate('/'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                //alert("Registration Unsuccessful");
            } else {
                console.error('There was an error!', error);
            }
        }
    };


    return (
        <div className='layout'>
        <Header />
         <Dashboard />
        <div className='wrapper'>
            <form onSubmit={handleDiet}>
                <h2>Request a Diet Plan</h2>
                <h4>If you have bellow diseases please select the diseases' levels</h4>
                <div className="input-box">
                    <label>Diabetes</label>
                    <select value={diabetes} onChange={(e) => setDiabetes(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="low">Low</option>
                        <option value="medium">Medium</option>
                        <option value="high">High</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Cholesterol</label>
                    <select value={cholesterol} onChange={(e) => setCholesterol(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="low">Low</option>
                        <option value="medium">Medium</option>
                        <option value="high">High</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Thyroid Disorders</label>
                    <select value={thyroid} onChange={(e) => setThyroid(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="low">Low</option>
                        <option value="medium">Medium</option>
                        <option value="high">High</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Heart Disease</label>
                    <select value={heartDisease} onChange={(e) => setHeartDisease(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="low">Low</option>
                        <option value="medium">Medium</option>
                        <option value="high">High</option>
                    </select>
                </div>
                
                <div className="input-box">
                    <label>Depression</label>
                    <select value={depression} onChange={(e) => setDepression(e.target.value)} required>
                        <option value="">Select Level</option>
                        <option value="low">Low</option>
                        <option value="medium">Medium</option>
                        <option value="high">High</option>
                    </select>
                </div>

                <button type="submit">Submit</button>
            </form>
        </div>
        <Footer/>
        </div>
    );
};

export default DietPlan;