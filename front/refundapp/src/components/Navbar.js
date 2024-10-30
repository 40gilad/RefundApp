// src/components/Navbar.js
import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

const Navbar = () => (
  <nav className="navbar">
    <ul>
      <li><Link to="/main">Main</Link></li>
      <li><Link to="/compare">Compare</Link></li>
      <li><Link to="/kaki">Kaki</Link></li>
    </ul>
  </nav>
);

export default Navbar;
