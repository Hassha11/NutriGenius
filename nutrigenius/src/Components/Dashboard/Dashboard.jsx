import React from 'react';
import { Link, Outlet } from 'react-router-dom';
import './Dashboard.css';


const Layout = () => {
    return (
        <div className="layout">
            <nav>
                <ul>
                <h1>NutriGenius</h1><br></br>
                    <li>
                        <Link to="/bmi">BMI Calculating</Link>
                    </li>
                    <li>
                        <Link to="/">Analysis</Link>
                    </li>
                    <li>
                        <Link to="/diet">Dietitian Recommendations</Link>
                    </li>
                    <li>
                        <Link to="/about">About</Link>
                    </li>
                    <li><br></br>
                        <Link style={{ marginTop: '250px', alignItems:'center' }} to="/">Log Out</Link>
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
