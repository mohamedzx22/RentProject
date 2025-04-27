import React, { useState, useEffect } from 'react';
import { Container, Row, Navbar, Offcanvas, Nav } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { NavLink } from 'react-router-dom';
import '../Header/header.css';

const Header = () => {
  const [show, setShow] = useState(false);  // To control the offcanvas menu visibility
  const [isSticky, setIsSticky] = useState(false);  // To handle sticky header

  const toggleMenu = () => {
    setShow(!show);  // Toggle the 'show' state when the button is clicked
  };

  // Function to handle sticky header on scroll
  const handleScroll = () => {
    const scrollTop = window.scrollY;
    if (scrollTop >= 120) {
      setIsSticky(true); // Add sticky effect when scrolling past 120px
    } else {
      setIsSticky(false); // Remove sticky effect when scrolling back to top
    }
  };

  useEffect(() => {
    window.addEventListener('scroll', handleScroll); // Attach scroll event listener
    return () => {
      window.removeEventListener('scroll', handleScroll); // Clean up the event listener
    };
  }, []);

  return (
    <section className={`header-section ${isSticky ? 'is-sticky' : ''}`}>
      <Container>
        <Row>
          <Navbar expand="lg" className="p-0">
            {/* Logo Section */}
            

            {/* Desktop Menu */}
            <Navbar.Collapse id="offcanvasNavbar">
              <Nav className="ml-auto">
                <NavLink to="/" className="nav-link">Home</NavLink>
                <NavLink to="/about" className="nav-link about">About Us</NavLink> {/* إضافة class 'about' */}
                <NavLink to="/listings" className="nav-link listings">Latest Listing</NavLink> {/* إضافة class 'listings' */}
                <NavLink to="/contact" className="nav-link">Contact Us</NavLink>
              </Nav>
            </Navbar.Collapse>

            {/* Register and Login Buttons */}
            <div className="ms-md-4 ms-2">
              <NavLink to="/register" className="primaryBtn d-none d-sm-inline-block">Register</NavLink>
            </div>
            <div className="ms-md-4 ms-2">
              <NavLink to="/login" className="primaryBtn d-none d-sm-inline-block">Login</NavLink>
            </div>

            {/* Offcanvas Button for Mobile View */}
            <Navbar.Toggle aria-controls="offcanvasNavbar" onClick={toggleMenu} />

            {/* Offcanvas Menu (For Mobile View) */}
            <Offcanvas show={show} onHide={toggleMenu} placement="end">
              <Offcanvas.Header closeButton>
                <Offcanvas.Title>Menu</Offcanvas.Title>
              </Offcanvas.Header>
              <Offcanvas.Body>
                <Nav className="flex-column">
                  <NavLink to="/" className="nav-link">Home</NavLink>
                  <NavLink to="/about" className="nav-link about">About Us</NavLink>
                  <NavLink to="/listings" className="nav-link listings">Latest Listing</NavLink>
                  <NavLink to="/contact" className="nav-link">Contact Us</NavLink>
                  <NavLink to="/register" className="nav-link">Register</NavLink>
                  <NavLink to="/login" className="nav-link">Login</NavLink>
                </Nav>
              </Offcanvas.Body>
            </Offcanvas>
          </Navbar>
        </Row>
      </Container>
    </section>
  );
};

export default Header;
