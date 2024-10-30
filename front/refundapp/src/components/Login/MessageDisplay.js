// src/components/Login/MessageDisplay.js
import React from 'react';

const MessageDisplay = ({ successMessage, error }) => {
  return (
    <>
      {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}
    </>
  );
};

export default MessageDisplay;
