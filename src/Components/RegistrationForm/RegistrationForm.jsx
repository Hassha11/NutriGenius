import React from 'react';
import './RegistrationForm.css';

const RegistrationForm = () => {
    return (
        <div className='wrapper'>
            <form action="">
                <h1>Registration</h1>
                <div className="input-box">
                    <input type="text" placeholder='Name' required/>    
                </div>
                <div className="input-box">
                    <input type="text" placeholder='Age' required/>
                </div>
                <div className="input-box">
                    <input type="text" placeholder='Height' required/>
                </div>
                <div className="input-box">
                    <input type="text" placeholder='Weight' required/>
                </div>
                <div className="input-box">
                    <select required>
                        <option value="" disabled selected>Gender</option>
                        <option value="male">Male</option>
                        <option value="female">Female</option>
                    </select>
                </div>
                <div className="input-box">
                      <label for="dob">Date of Birth</label>
                      <input type="date" id="dob" name="dob" required value="2000-01-01"/>
                </div>

                <button type="submit">Submit</button>

            </form>
        </div>
    );
};

export default RegistrationForm;