// src/components/Login/ToggleButton.js
import React from 'react';

const ToggleButton = ({ isRegister, handleToggle }) => {
  return (
    <button onClick={handleToggle}>
      {isRegister ? 'Already have an account? Login' : 'Need an account? Register'}
    </button>
  );
};

export default ToggleButton;
