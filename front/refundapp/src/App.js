// src/App.js
import './App.css';
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './pages/Login';
import Main from './pages/Main';
import Compare from './pages/Compare';
import Kaki from './pages/Kaki';
import Navbar from './components/Navbar';

const App = () => (
  <Router>
    <Routes>
      <Route path="/" element={<Login />} />
      <Route path="/main" element={<><Navbar /><Main /></>} />
      <Route path="/compare" element={<><Navbar /><Compare /></>} />
      <Route path="/kaki" element={<><Navbar /><Kaki /></>} />
    </Routes>
  </Router>
);

export default App;
