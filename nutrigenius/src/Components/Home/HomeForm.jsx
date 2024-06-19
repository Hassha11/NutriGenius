import React, { useState } from 'react';
import Modal from '../Modal/Modal';
import './Home.css';

const Home = () => {
    const [showModal, setShowModal] = useState(false);
    const [modalContent, setModalContent] = useState(null);

    const handleModalClose = () => {
        setShowModal(false);
    };

    const handleModalOpen = (content) => {
        setModalContent(content);
        setShowModal(true);
    };

    return (
        <div className="home-container">
            <h2>Home Page</h2>
            <button onClick={() => handleModalOpen(<About />)}>About</button>
            <button onClick={() => handleModalOpen(<User />)}>User</button>
            <button onClick={() => handleModalOpen(<Login />)}>Login</button>
            <button onClick={() => handleModalOpen(<BMI />)}>BMI</button>

            <Modal show={showModal} onClose={handleModalClose} title="Modal">
                {modalContent}
            </Modal>
        </div>
    );
};

const About = () => (
    <div>
        <h3>About Page</h3>
        <p>Learn more about us on this page.</p>
    </div>
);

const User = () => (
    <div>
        <h3>User Page</h3>
        <p>Manage your user information here.</p>
    </div>
);

const Login = () => (
    <div>
        <h3>Login Page</h3>
        <p>Please login to access more features.</p>
    </div>
);

const BMI = () => (
    <div>
        <h3>BMI Page</h3>
        <p>Calculate your Body Mass Index (BMI) here.</p>
    </div>
);

export default Home;
