import React, { useState } from 'react';
import axios from 'axios';
import { Link, Outlet } from 'react-router-dom';
import './Footer.css';

const Footer = () => {

return (
    <div className='wrapper-footer'>
                <ul>
                <h1>NutriGenius</h1><br></br>
                <div className='flex-container'>
                    <li>
                        <Link to="/bmi">BMI Calculating</Link>
                    </li>
                    <li>
                        <Link to="/">Analysis</Link>
                    </li>
                    <li>
                        <Link to="/user">User Profile</Link>
                    </li>
                    <li>
                        <Link to="/diet">Dietitian Recommendations</Link>
                    </li>
                    <li>
                        <Link to="/about">About</Link>
                    </li>
                    </div>
                    </ul>
    </div>
);
};

export default Footer;