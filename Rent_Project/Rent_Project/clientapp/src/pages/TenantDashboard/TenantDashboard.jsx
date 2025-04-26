import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { FaHeart, FaEnvelope, FaFileAlt, FaTrash, FaEye } from 'react-icons/fa';
import './TenantDashboard.css';

const TenantDashboard = () => {
  const [activeTab, setActiveTab] = useState('applications');
  const [savedProperties, setSavedProperties] = useState([]);
  const [applications, setApplications] = useState([]);
  const [messages, setMessages] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    // Mock data - replace with API calls
    const mockSavedProperties = [
      {
        id: 1,
        title: 'Luxury Apartment',
        landlord: 'Elite Residences',
        price: 2000,
        location: 'Manhattan',
        image: 'property1.jfif'
      }
    ];

    const mockApplications = [
      {
        id: 1,
        propertyId: 1,
        propertyTitle: 'Modern Downtown Apartment',
        status: 'pending',
        date: '2023-06-15',
        documents: ['id_proof.pdf', 'income_verification.pdf'],
        landlord: 'Prime Properties',
        message: 'Looking forward to your review'
      },
      {
        id: 2,
        propertyId: 2,
        propertyTitle: 'Cozy Studio',
        status: 'approved',
        date: '2023-06-10',
        documents: ['id_proof.pdf'],
        landlord: 'Urban Living',
        message: 'Application approved! Contact us to proceed.'
      }
    ];

    const mockMessages = [
      {
        id: 1,
        from: 'Prime Properties',
        subject: 'Application Update',
        date: '2023-06-16',
        read: false
      }
    ];

    setSavedProperties(mockSavedProperties);
    setApplications(mockApplications);
    setMessages(mockMessages);
  }, []);

  const withdrawApplication = (id) => {
    setApplications(applications.filter(app => app.id !== id));
  };

  const viewApplicationDetails = (application) => {
    navigate(`/application/${application.id}`, {
      state: { application }
    });
  };

  const viewSavedProperty = (propertyId) => {
    navigate(`/property/${propertyId}`);
  };

  return (
    <div className="tenant-dashboard">
      <div className="sidebar">
        <button 
          className={`nav-item ${activeTab === 'applications' ? 'active' : ''}`}
          onClick={() => setActiveTab('applications')}
        >
          <FaFileAlt /> My Applications ({applications.length})
        </button>
        <button 
          className={`nav-item ${activeTab === 'saved' ? 'active' : ''}`}
          onClick={() => setActiveTab('saved')}
        >
          <FaHeart /> Saved Properties ({savedProperties.length})
        </button>
        {/* Commenting out the messages button
        <button 
          className={`nav-item ${activeTab === 'messages' ? 'active' : ''}`}
          onClick={() => setActiveTab('messages')}
        >
          <FaEnvelope /> Messages ({messages.filter(m => !m.read).length})
        </button>
        */}
      </div>

      <div className="main-content">
        {activeTab === 'applications' && (
          <div className="applications-section">
            <h2>My Rental Applications</h2>
            {applications.length === 0 ? (
              <p>You haven't applied to any properties yet.</p>
            ) : (
              <div className="applications-list">
                {applications.map(app => (
                  <div key={app.id} className="application-card">
                    <div className="application-header">
                      <h3>{app.propertyTitle}</h3>
                      <span className={`status-badge ${app.status}`}>
                        {app.status}
                      </span>
                    </div>
                    <div className="application-details">
                      <p><strong>Landlord:</strong> {app.landlord}</p>
                      <p><strong>Applied on:</strong> {app.date}</p>
                      <p><strong>Documents:</strong> {app.documents.join(', ')}</p>
                    </div>
                    <div className="application-actions">
                      <button 
                        className="view-btn"
                        onClick={() => viewApplicationDetails(app)}
                      >
                        <FaEye /> View Details
                      </button>
                      {app.status === 'pending' && (
                        <button 
                          className="withdraw-btn"
                          onClick={() => withdrawApplication(app.id)}
                        >
                          <FaTrash /> Withdraw
                        </button>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        )}

        {activeTab === 'saved' && (
          <div className="saved-section">
            <h2>Saved Properties</h2>
            {savedProperties.length === 0 ? (
              <p>You haven't saved any properties yet.</p>
            ) : (
              <div className="saved-list">
                {savedProperties.map(property => (
                  <div key={property.id} className="saved-card">
                    <img 
                      src={`${process.env.PUBLIC_URL}/assets/properties/${property.image}`}
                      alt={property.title}
                      className="saved-property-image"
                      onError={(e) => {
                        e.target.onerror = null;
                        e.target.src = `${process.env.PUBLIC_URL}/assets/properties/default.jfif`;
                      }}
                    />
                    <div className="saved-card-content">
                      <h3>{property.title}</h3>
                      <p className="location">{property.location}</p>
                      <p className="price">${property.price}/month</p>
                      <div className="property-actions">
                        <button 
                          className="view-btn"
                          onClick={() => viewSavedProperty(property.id)}
                        >
                          View Details
                        </button>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        )}

        {/* Commenting out the messages section
        {activeTab === 'messages' && (
          <div className="messages-section">
            <h2>Messages</h2>
            {messages.length === 0 ? (
              <p>You have no messages.</p>
            ) : (
              <div className="messages-list">
                {messages.map(message => (
                  <div 
                    key={message.id} 
                    className={`message-card ${message.read ? '' : 'unread'}`}
                  >
                    <h3>{message.from}</h3>
                    <p>{message.subject}</p>
                    <div className="message-footer">
                      <span>{message.date}</span>
                      <button className="view-btn">View</button>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        )}
        */}
      </div>
    </div>
  );
};

export default TenantDashboard;