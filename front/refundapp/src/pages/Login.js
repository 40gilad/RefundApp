// src/pages/Login.js
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Login.css';

const Login = () => {
  const [isRegister, setIsRegister] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  
  const handleToggle = () => {
    setIsRegister(!isRegister);
    setError('');
  };

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    const url = `https://localhost:7017/Gateway/ProcessRequest?route=${isRegister ? 'register' : 'login'}`;
    
    // Extract the username from the email
    const username = email.split('@')[0]; // Get the part before the '@'
    
    const data = {
      uName: username,      // Use the extracted username
      uEmail: isRegister ? email : '',
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
        // Navigate to the login page after registration
        if (isRegister) {
          setIsRegister(false); // Reset to login mode
          setEmail('');        // Clear email field
          setPassword('');     // Clear password field
        }
        handleToggle();
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
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email:</label>
          <input
            type="text"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            aria-label="Email"
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type={showPassword ? 'text' : 'password'}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            aria-label="Password"
          />
          <button type="button" onClick={() => setShowPassword(!showPassword)}>
            {showPassword ? 'Hide' : 'Show'}
          </button>
        </div>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit" disabled={loading}>
          {loading ? 'Loading...' : isRegister ? 'Register' : 'Login'}
        </button>
      </form>
      <button onClick={handleToggle}>
        {isRegister ? 'Already have an account? Login' : 'Need an account? Register'}
      </button>
    </div>
  );
};

export default Login;
