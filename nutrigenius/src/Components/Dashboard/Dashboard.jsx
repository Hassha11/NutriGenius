import React from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import './Dashboard.css';


const Layout = () => {
    const navigate = useNavigate();


    const handleLogout = () => {
        // Clear user details from localStorage
        localStorage.removeItem('userId');
        localStorage.removeItem('username');
        localStorage.removeItem('password');
    
        // Navigate to the home page
        navigate('/');
    };

    return (
        <div className="layout">
            <nav>
                <ul>
                <h1>NutriGenius</h1><br></br>
                    <li>
                        <Link to="/bmi">BMI Calculating</Link>
                    </li>
                    <li>
                        <Link to="/analysis">Analysis</Link>
                    </li>
                    <li>
                        <Link to="/diet">Dietitian Recommendations</Link>
                    </li>
                    <li>
                        <Link to="/about">About</Link>
                    </li>
                    <li><br></br>
                        <button
                            style={{ marginTop: '200px', alignItems: 'center', width: '150px', height: '40px', marginLeft: '25px' }}
                            onClick={handleLogout}
                        >
                            Log Out
                        </button>
                    </li>
                </ul>
            </nav>
            <div className="container">
            <Outlet />
            </div>
        </div>
    );
};

export default Layout;
