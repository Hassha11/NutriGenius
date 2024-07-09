import React from 'react';
import { FaSearch, FaUser } from 'react-icons/fa';
import './Header.css'; // Assuming your CSS is in Header.css

const Header = () => {
    return (
        <div className='wrapper-header'>
            <div className="left-section">
                <a href="/login" className="signup-link">Sign Up | Login</a>
            </div>
            <div className="right-section">
                <FaSearch className='header-icon search-icon'/>
                <a href="/user">
                <FaUser className='header-icon'/> 
                </a>
            </div>
        </div>
    );
};

export default Header;
