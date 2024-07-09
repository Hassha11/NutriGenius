import React from 'react';
import './ForgotPassword.css';

const ForgotPassword = () => {
    return (
        <div className='wrapper'>
            <form action="">
                <h1>Change Password</h1>
                <div className="input-box">
                    <input type="text" placeholder='Username' required/>    
                </div>
                <div className="input-box">
                    <input type="text" placeholder='New Password' required/>
                </div>
                <div className="input-box">
                    <input type="text" placeholder='Confirm Password' required/>
                </div>
                <button type="submit">Submit</button>

            </form>
        </div>
    );
};

export default ForgotPassword;