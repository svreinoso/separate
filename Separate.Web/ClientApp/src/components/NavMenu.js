import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { getCookies } from '../CookiesManager';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  getMenu() {
    const currentUser = getCookies('currentUser');
    return currentUser ? [
      {
        path: '/',
        name: 'Home'
      }, {
        path: '/counter',
        name: 'Counter'
      }, {
        path: '/fetch-data',
        name: 'Fetch Data'
      }, {
        path: '/chat-hub',
        name: 'Chat Hub'
      },
      {
        path: '/logout',
        name: 'Logout'
      }
    ] : [
        {
          path: '/login',
          name: 'Login'
        }, {
          path: '/register',
          name: 'Register'
        },
      ]
  }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">Separate.Web</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                {
                  this.getMenu().map((m, index) => {
                    return <NavItem key={index}>
                      <NavLink tag={Link} className="text-dark" to={m.path}>{m.name}</NavLink>
                    </NavItem>
                  })
                }
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
