import React from 'react';
import './BMIForm.css';

const BMIForm = () => {
    return (
        <div className='wrapper'>
            <form action="">
                <h1>BMI Calculation</h1>
                <div className="input-box">
                    <select required>
                        <option value="" disabled selected>Gender</option>
                        <option value="male">Male</option>
                        <option value="female">Female</option>
                    </select>
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
                <div class="flex-container">
                <button type="submit">Calculate</button>
                <div className="input-box">
                    <input type="text" placeholder='BMI' required/>
                    </div>    
                </div>

            </form>
        </div>
    );
};

export default BMIForm;