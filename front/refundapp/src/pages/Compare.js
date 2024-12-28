import React, { useState } from 'react';
import axios from 'axios';

const Compare = () => {
  const [rows, setRows] = useState([
    {
      date: new Date().toISOString().slice(0, 10),
      firstName: '',
      lastInitial: '',
      orderId: '',
      amount: '',
      errorSource: 'restaurant',
      processed: false,
      reason: '',
    },
  ]);
  const [showFileUpload, setShowFileUpload] = useState(false);
  const [file, setFile] = useState(null);

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

    const data = {
      uEmail: uEmail,
      orderId: rows[index].orderId,
      customerName: `${rows[index].firstName} ${rows[index].lastInitial}`,
      refundDate: rows[index].date,
      amount: rows[index].amount,
      reason: rows[index].reason,
      isResturantFault: rows[index].errorSource === 'restaurant',
    };

    try {
      const response = await axios.post(
        'https://localhost:7017/Gateway/ProcessRequest?route=add-refund',
        data,
        {
          headers: {
            Authorization: `Bearer ${authToken}`,
            'Content-Type': 'application/json',
          },
        }
      );

    } catch (error) {
      console.error('Error submitting data:', error);
      alert('Failed to submit data');
    }

    const updatedRows = rows.map((row, i) =>
      i === index ? { ...row, processed: true } : row
    );
    setRows(updatedRows);

    const newRow = {
      date: new Date().toISOString().slice(0, 10),
      firstName: '',
      lastInitial: '',
      orderId: '',
      amount: '',
      errorSource: 'restaurant',
      processed: false,
      reason: '',
    };

    setRows([...updatedRows, newRow]);
  };

  const handleFileUpload = (event) => {
    setFile(event.target.files[0]);
  };

  const handleSubmitFile = async () => {
    const authToken = localStorage.getItem('authToken');
    const uEmail = localStorage.getItem('userEmail');
    
    if (!authToken) {
      alert('Authorization token is missing');
      return;
    }
  
    if (!file) {
      alert('Please select a file to upload');
      return;
    }
  
    const formData = new FormData();
    formData.append('file', file);
    formData.append('uEmail', uEmail);  // Include uEmail in formData
  
    try {
      const response = await axios.post(
        'https://localhost:7017/Gateway/UploadFile',
        formData,  // Send only formData
        {
          headers: {
            Authorization: `Bearer ${authToken}`,
          },
        }
      );
  
      if (response.status === 200) {
        alert('File uploaded successfully');
        // Optionally clear the file input here if needed
      }
    } catch (error) {
      console.error('Error uploading file:', error.response || error);
      alert('Failed to upload file');
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
          maxWidth: '1000px',
          textAlign: 'center',
          tableLayout: 'fixed',
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
            <th>סיבה</th>
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
                  onChange={(e) => handleChange(index, 'reason', e.target.value)}
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
      <button
        onClick={() => setShowFileUpload(!showFileUpload)}
        style={{
          marginTop: '20px',
          backgroundColor: 'blue',
          color: 'white',
          border: 'none',
          padding: '10px 20px',
          cursor: 'pointer',
        }}
      >
        Compare
      </button>
      {showFileUpload && (
        <div style={{ marginTop: '20px' }}>
          <input type="file" onChange={handleFileUpload} />
          <button
            onClick={handleSubmitFile}
            style={{
              marginLeft: '10px',
              backgroundColor: 'green',
              color: 'white',
              border: 'none',
              padding: '10px 20px',
              cursor: 'pointer',
            }}
          >
            Submit
          </button>
        </div>
      )}
    </div>
  );
};

export default Compare;
