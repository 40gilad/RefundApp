// src/components/Login/LoginForm.js
import React from 'react';

const LoginForm = ({ email, setEmail, password, setPassword, showPassword, setShowPassword, handleSubmit, loading }) => {
  return (
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
      <button type="submit" disabled={loading}>
        {loading ? 'Loading...' : 'Submit'}
      </button>
    </form>
  );
};

export default LoginForm;
