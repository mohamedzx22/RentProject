import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaHome, FaPlus, FaEnvelope, FaChartLine, FaFileAlt } from 'react-icons/fa';
import './LandlordDashboard.css';

const LandlordDashboard = () => {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState('properties');
  const [properties, setProperties] = useState([]);
  const [applications, setApplications] = useState([]);
  const [messages, setMessages] = useState([]);

  useEffect(() => {
    // Mock data - replace with API calls
    const mockProperties = [
      {
        id: 1,
        title: 'Downtown Apartment',
        price: 1800,
        location: 'New York',
        status: 'available',
        views: 124,
        image: 'property1.jfif'
      },
      {
        id: 2,
        title: 'Suburban House',
        price: 2200,
        location: 'Chicago',
        status: 'rented',
        views: 89,
        image: 'property2.jfif'
      }
    ];

    const mockApplications = [
      {
        id: 1,
        propertyId: 1,
        tenantName: 'John Doe',
        date: '2023-06-15',
        status: 'pending'
      }
    ];

    const mockMessages = [
      {
        id: 1,
        from: 'John Doe',
        subject: 'Question about the apartment',
        date: '2023-06-16',
        read: false
      }
    ];

    setProperties(mockProperties);
    setApplications(mockApplications);
    setMessages(mockMessages);
  }, []);

  const handleAddProperty = () => {
    navigate('/add-property');
  };

  const handleStatusChange = (propertyId, newStatus) => {
    setProperties(properties.map(prop => 
      prop.id === propertyId ? { ...prop, status: newStatus } : prop
    ));
  };

  const handleEdit = (propertyId) => {
    navigate(`/edit-property/${propertyId}`);
  };

  const handleDelete = (propertyId) => {
    const confirmDelete = window.confirm('Are you sure you want to delete this property?');
    if (confirmDelete) {
      setProperties(properties.filter(prop => prop.id !== propertyId));
    }
  };

  const handleApprove = (appId) => {
    setApplications(applications.map(app =>
      app.id === appId ? { ...app, status: 'approved' } : app
    ));
  };

  const handleReject = (appId) => {
    setApplications(applications.map(app =>
      app.id === appId ? { ...app, status: 'rejected' } : app
    ));
  };

  const handleViewApplication = (appId) => {
    navigate(`/application/${appId}`);
  };

  return (
    <div className="landlord-dashboard">
      <div className="sidebar">
        <button 
          className={`nav-item ${activeTab === 'properties' ? 'active' : ''}`}
          onClick={() => setActiveTab('properties')}
        >
          <FaHome /> My Properties
        </button>
        <button 
          className={`nav-item ${activeTab === 'applications' ? 'active' : ''}`}
          onClick={() => setActiveTab('applications')}
        >
          <FaFileAlt /> Applications
        </button>
        <button 
          className={`nav-item ${activeTab === 'analytics' ? 'active' : ''}`}
          onClick={() => setActiveTab('analytics')}
        >
          <FaChartLine /> Analytics
        </button>
      </div>

      <div className="main-content">
        {activeTab === 'properties' && (
          <div className="properties-section">
            <div className="section-header">
              <h2>My Properties</h2>
              <button className="add-property" onClick={handleAddProperty}>
                <FaPlus /> Add Property
              </button>
            </div>
            {properties.length === 0 ? (
              <p>You haven't listed any properties yet.</p>
            ) : (
              <table>
                <thead>
                  <tr>
                    <th>Image</th>
                    <th>Property</th>
                    <th>Location</th>
                    <th>Price</th>
                    <th>Views</th>
                    <th>Status</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {properties.map(property => (
                    <tr key={property.id}>
                      <td>
                        <img 
                          src={`${process.env.PUBLIC_URL}/assets/properties/${property.image}`} 
                          alt={property.title}
                          className="property-thumbnail"
                          onError={(e) => {
                            e.target.onerror = null; 
                            e.target.src = `${process.env.PUBLIC_URL}/assets/properties/default.jfif`;
                          }}
                        />
                      </td>
                      <td>{property.title}</td>
                      <td>{property.location}</td>
                      <td>${property.price}</td>
                      <td>{property.views}</td>
                      <td>
                        <select
                          value={property.status}
                          onChange={(e) => handleStatusChange(property.id, e.target.value)}
                        >
                          <option value="available">Available</option>
                          <option value="rented">Rented</option>
                          <option value="maintenance">Maintenance</option>
                        </select>
                      </td>
                      <td>
                        <button className="edit-btn" onClick={() => handleEdit(property.id)}>Edit</button>
                        <button className="delete-btn" onClick={() => handleDelete(property.id)}>Delete</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        )}

        {activeTab === 'applications' && (
          <div className="applications-section">
            <h2>Rental Applications</h2>
            {applications.length === 0 ? (
              <p>No applications received yet.</p>
            ) : (
              <table>
                <thead>
                  <tr>
                    <th>Property</th>
                    <th>Applicant</th>
                    <th>Date</th>
                    <th>Status</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {applications.map(app => (
                    <tr key={app.id}>
                      <td>{properties.find(p => p.id === app.propertyId)?.title || 'Property'}</td>
                      <td>{app.tenantName}</td>
                      <td>{app.date}</td>
                      <td>
                        <span className={`status-badge ${app.status}`}>
                          {app.status}
                        </span>
                      </td>
                      <td>
                        <button className="approve-btn" onClick={() => handleApprove(app.id)}>Approve</button>
                        <button className="reject-btn" onClick={() => handleReject(app.id)}>Reject</button>
                        <button className="view-btn" onClick={() => handleViewApplication(app.id)}>View</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        )}

        {activeTab === 'analytics' && (
          <div className="analytics-section">
            <h2>Property Analytics</h2>
            <div className="analytics-grid">
              <div className="stat-card">
                <h3>Total Properties</h3>
                <p>{properties.length}</p>
              </div>
              <div className="stat-card">
                <h3>Available Properties</h3>
                <p>{properties.filter(p => p.status === 'available').length}</p>
              </div>
              <div className="stat-card">
                <h3>Total Views</h3>
                <p>{properties.reduce((sum, prop) => sum + prop.views, 0)}</p>
              </div>
              <div className="stat-card">
                <h3>Pending Applications</h3>
                <p>{applications.filter(app => app.status === 'pending').length}</p>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default LandlordDashboard;
