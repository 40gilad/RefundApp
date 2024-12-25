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
      processed: false, // Added to track if the row has been processed
      reason: '', // Added reason field
    },
  ]);

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

  const handleOkClick = async (index) => {
    const authToken = localStorage.getItem('authToken');
    const uEmail = localStorage.getItem('userEmail');
    
    if (!authToken) {
      alert('Authorization token is missing');
      return;
    }

    console.log(rows[index].errorSource)
    // Prepare the data to be sent
    const data = {
      uEmail: uEmail,
      orderId: rows[index].orderId,
      customerName: `${rows[index].firstName} ${rows[index].lastInitial}`,
      refundDate: rows[index].date,
      amount: rows[index].amount,
      reason: rows[index].reason, // Added reason to the sent data
      isResturantFault: rows[index].errorSource === "restaurant",
    };

    try {
      const response = await axios.post(
        'https://localhost:7017/Gateway/ProcessRequest?route=add-refund',
        data,  // Send the data directly, without wrapping it in an array
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

    // Mark the row as processed
    const updatedRows = rows.map((row, i) =>
      i === index ? { ...row, processed: true } : row
    );
    setRows(updatedRows);

    // Add a new row after sending the current row
    const newRow = {
      date: new Date().toISOString().slice(0, 10),
      firstName: '',
      lastInitial: '',
      orderId: '',
      amount: '',
      errorSource: 'restaurant',
      processed: false, // New row is not processed
      reason: '', // New row has empty reason
    };

    setRows([...updatedRows, newRow]);
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
            <th>סיבה</th> {/* Added column for reason */}
            <th>אישור</th>
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
              <td>
                <input
                  type="text"
                  value={row.reason}
                  placeholder="סיבה"
                  onChange={(e) => handleChange(index, 'reason', e.target.value)} // Handle reason changes
                />
              </td>
              <td>
                {!row.processed && (
                  <button
                    onClick={() => handleOkClick(index)}
                    style={{
                      backgroundColor: 'blue',
                      color: 'white',
                      border: 'none',
                      padding: '5px 10px',
                      cursor: 'pointer',
                    }}
                  >
                    OK
                  </button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Compare;
