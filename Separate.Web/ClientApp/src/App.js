import React from 'react';
import './App.css';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { ChatHub } from './components/ChatHub'
import Login from './components/Login';
import Register from './components/Register';
import Logout from './components/Logout';

function App() {
  return (
    <Layout>
      <Route exact path='/' component={Home} />
      <Route exact path='/home' component={Home} />
      <Route path='/counter' component={Counter} />
      <Route path='/fetch-data' component={FetchData} />
      <Route path='/chat-hub' component={ChatHub} />
      <Route path='/logout' component={Logout} />
      <Route path='/login' component={Login} />
      <Route path='/register' component={Register} />
    </Layout>
  );
}

export default App;
