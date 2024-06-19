import React from 'react';
import { Link, Outlet } from 'react-router-dom';
import './Dashboard.css';

const Layout = () => {
    return (
        <div className="layout">
            <nav>
                <ul>
                    <li>
                        <Link to="/home">Home</Link>
                    </li>
                    <li>
                        <Link to="/reg">Registration</Link>
                    </li>
                    <li>
                        <Link to="/login">Login</Link>
                    </li>
                    <li>
                        <Link to="/bmi">BMI</Link>
                    </li>
                    <li>
                        <Link to="/about">About</Link>
                    </li>
                    <li>
                        <Link to="/user">User</Link>
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
