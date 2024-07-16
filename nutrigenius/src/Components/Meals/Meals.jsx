import React, { useState, useEffect } from 'react';
import './Meals.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Header from '../Header/Header';
import Dashboard from '../Dashboard/Dashboard';
import Footer from '../Footer/Footer';

const Meals = () => {
    const [diabetes, setDiabetes] = useState('');
    const [cholesterol, setCholesterol] = useState('');
    const [thyroid, setThyroid] = useState('');
    const [heartDisease, setHeartDisease] = useState('');
    const [depression, setDepression] = useState('');
    const [points, setPoints] = useState('');
    const navigate = useNavigate();

    // Fetch data from the database when the component mounts
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get('http://localhost:5274/api/DietPlan/GetDietPlan');
                if (response.data) {
                    const { diabetes, cholesterol, thyroid, heartDisease, depression, points } = response.data;
                    setDiabetes(diabetes);
                    setCholesterol(cholesterol);
                    setThyroid(thyroid);
                    setHeartDisease(heartDisease);
                    setDepression(depression);
                    setPoints(points);
                }
            } catch (error) {
                console.error('Error fetching data', error);
            }
        };

        fetchData();
    }, []);

    const handleMeal = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post('http://localhost:5274/api/Meals/Diet', {
                diabetes,
                cholesterol,
                thyroid,
                heartDisease,
                depression,
                points,
            });

            if (response.status === 200) {
                navigate('/'); 
            }
           
        } catch (error) {
            if (error.response && error.response.status === 401) {
                console.error('Registration Unsuccessful');
            } else {
                console.error('There was an error!', error);
            }
        }
    };

    return (
        <div className='layout-meals'>
            <Header />
            <Dashboard />
            <div className='wrapper-meals'>
                <form onSubmit={handleMeal}>
                    <div className="form-group">
                        <label htmlFor="diabetes">Diabetes</label>
                        <input
                            type="text"
                            id="diabetes"
                            value={diabetes}
                            onChange={(e) => setDiabetes(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="cholesterol">Cholesterol</label>
                        <input
                            type="text"
                            id="cholesterol"
                            value={cholesterol}
                            onChange={(e) => setCholesterol(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="thyroid">Thyroid</label>
                        <input
                            type="text"
                            id="thyroid"
                            value={thyroid}
                            onChange={(e) => setThyroid(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="heartDisease">Heart Disease</label>
                        <input
                            type="text"
                            id="heartDisease"
                            value={heartDisease}
                            onChange={(e) => setHeartDisease(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="depression">Depression</label>
                        <input
                            type="text"
                            id="depression"
                            value={depression}
                            onChange={(e) => setDepression(e.target.value)}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="points">Points</label>
                        <input
                            type="text"
                            id="points"
                            value={points}
                            onChange={(e) => setPoints(e.target.value)}
                        />
                    </div>
                    <button type="submit">Submit</button>
                </form>
            </div>
            <Footer />
        </div>
    );
};

export default Meals;
