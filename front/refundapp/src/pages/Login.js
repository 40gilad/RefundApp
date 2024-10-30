// src/pages/Login.js
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Login.css';
import LoginForm from '../components/Login/LoginForm';
import ToggleButton from '../components/Login/ToggleButton';
import MessageDisplay from '../components/Login/MessageDisplay';

const Login = () => {
  const [isRegister, setIsRegister] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  
  const navigate = useNavigate();

  const handleToggle = () => {
    setIsRegister(!isRegister);
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    const url = `https://localhost:7017/Gateway/ProcessRequest?route=${isRegister ? 'register' : 'login'}`;
    const username = email.split('@')[0];

    const data = {
      uName: username,
      uEmail: email,
      uPassword: password,
      sessionId: 0,
    };
  
    try {
      const response = await axios.post(url, data, {
        headers: {
          Authorization: '0',
          'Content-Type': 'application/json',
        },
      });

      if (response.status === 200) {
        if (isRegister) {
          setSuccessMessage('Registration successful! Redirecting to login...');
          setTimeout(() => {
            setIsRegister(false);
            setSuccessMessage('');
            setEmail('');
            setPassword('');
          }, 2000);
        } else {
          const token = response.data.Token;
          localStorage.setItem('authToken', token);
          navigate('/main');
        }
      }
    } catch (error) {
      if (error.response && error.response.data && error.response.data.message) {
        setError(error.response.data.message);
      } else {
        setError(isRegister ? 'Registration failed.' : 'Login failed.');
      }
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>{isRegister ? 'Register' : 'Login'}</h2>
      <MessageDisplay successMessage={successMessage} error={error} />
      <LoginForm 
        email={email}
        setEmail={setEmail}
        password={password}
        setPassword={setPassword}
        showPassword={showPassword}
        setShowPassword={setShowPassword}
        handleSubmit={handleSubmit}
        loading={loading}
      />
      <ToggleButton isRegister={isRegister} handleToggle={handleToggle} />
    </div>
  );
};

export default Login;
