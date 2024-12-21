import React, { useState } from 'react';
import axios from 'axios';

const Compare = () => {
  const [rows, setRows] = useState([
    {
      date: new Date().toISOString().slice(0, 10), // Today's date in YYYY-MM-DD format
      firstName: '',
      lastInitial: '',
      orderId: '',
      amount: '',
      errorSource: 'restaurant', // 'restaurant' or 'wolt'
    },
  ]);

  const handleAddRow = () => {
    setRows([
      ...rows,
      {
        date: new Date().toISOString().slice(0, 10),
        firstName: '',
        lastInitial: '',
        orderId: '',
        amount: '',
        errorSource: 'restaurant',
      },
    ]);
  };

  const handleChange = (index, field, value) => {
    const updatedRows = rows.map((row, i) =>
      i === index ? { ...row, [field]: value } : row
    );
    setRows(updatedRows);
  };

  const handleErrorSourceClick = (index) => {
    const updatedRows = rows.map((row, i) =>
      i === index
        ? {
            ...row,
            errorSource: row.errorSource === 'restaurant' ? 'wolt' : 'restaurant',
          }
        : row
    );
    setRows(updatedRows);
  };

  const handleApply = async () => {
    // Retrieve the stored auth token
    const authToken = localStorage.getItem('authToken');
    
    if (!authToken) {
      alert('Authorization token is missing');
      return;
    }

    // Prepare the data to be sent
    const data = rows.map(row => ({
      date: row.date,
      firstName: row.firstName,
      lastInitial: row.lastInitial,
      orderId: row.orderId,
      amount: row.amount,
      errorSource: row.errorSource,
    }));

    try {
      const response = await axios.post(
        'https://localhost:7017/Gateway/ProcessRequest?route=jsons-compare',
        { data },
        {
          headers: {
            Authorization: `Bearer ${authToken}`,
            'Content-Type': 'application/json',
          },
        }
      );

      if (response.status === 200) {
        console.log('Data successfully sent to the server:', response.data);
        alert('Data submitted successfully');
      }
    } catch (error) {
      console.error('Error submitting data:', error);
      alert('Failed to submit data');
    }
  };

  return (
    <div
      style={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        padding: '20px',
      }}
    >
      <h1>Compare Page</h1>
      <table
        border="1"
        style={{
          width: '200%',
          maxWidth: '1000px', // Ensures the table doesn't stretch too wide
          textAlign: 'center',
          tableLayout: 'fixed', // Ensures uniform column widths
          marginTop: '20px',
        }}
      >
        <thead>
          <tr>
            <th>תאריך</th>
            <th>שם פרטי</th>
            <th>אות של שם המשפחה</th>
            <th>מס' הזמנה</th>
            <th>סכום</th>
            <th>טעות שלנו / של וולט</th>
          </tr>
        </thead>
        <tbody>
          {rows.map((row, index) => (
            <tr key={index}>
              <td>
                <input
                  type="date"
                  value={row.date}
                  onChange={(e) => handleChange(index, 'date', e.target.value)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={row.firstName}
                  placeholder="שם פרטי"
                  onChange={(e) => handleChange(index, 'firstName', e.target.value)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={row.lastInitial}
                  placeholder="אות משפחה"
                  maxLength="1"
                  onChange={(e) => handleChange(index, 'lastInitial', e.target.value)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={row.orderId}
                  placeholder="מס' הזמנה"
                  onChange={(e) => handleChange(index, 'orderId', e.target.value)}
                />
              </td>
              <td>
                <input
                  type="number"
                  value={row.amount}
                  placeholder="סכום"
                  onChange={(e) => handleChange(index, 'amount', e.target.value)}
                />
              </td>
              <td>
                <button
                  onClick={() => handleErrorSourceClick(index)}
                  style={{
                    backgroundColor:
                      row.errorSource === 'restaurant' ? 'green' : 'blue',
                    color: 'white',
                    border: 'none',
                    padding: '5px 10px',
                    cursor: 'pointer',
                  }}
                >
                  {row.errorSource === 'restaurant' ? 'טעות שלנו' : 'טעות של וולט'}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <button
        onClick={handleAddRow}
        style={{ marginTop: '10px', padding: '10px 20px' }}
      >
        Add Row
      </button>
      <button
        onClick={handleApply}
        style={{
          marginTop: '20px',
          padding: '10px 20px',
          backgroundColor: 'blue',
          color: 'white',
          border: 'none',
          cursor: 'pointer',
        }}
      >
        Apply
      </button>
    </div>
  );
};

export default Compare;
