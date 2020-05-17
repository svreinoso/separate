import React, { useState } from 'react'
import NotificationSystem from 'react-notification-system';
import { Button, Form, FormGroup, Label, Input, Container, Row, Col } from 'reactstrap';
import { httpPost } from '../HttpManager';

function Register() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')

  const notificationSystem = React.createRef();

  const submitRegister = async (e) => {
    e.preventDefault();
    let result = await httpPost({
      email, password, firstName, lastName
    }, (error) => {
      const notification = notificationSystem.current;
      notification.addNotification({
        message: error,
        level: 'error'
      });
    });

    if (result) {
      window.location.href = '/login'
    }
  }

  return (
    <Container>
      <h1 className="text-center">Register</h1>
      <Row className="justify-content-center">
        <Col xs={12} sm={6} md={4} lg={3} xl={2}>
          <Form onSubmit={e => submitRegister(e)}>
            <FormGroup>
              <Label for="firstName">First name </Label>
              <Input type="text" required value={firstName} onChange={e => setFirstName(e.target.value)}
                name="firstName" id="firstName" placeholder="First name" />
            </FormGroup>
            <FormGroup>
              <Label for="lastName">Last name</Label>
              <Input type="text" required value={lastName} onChange={e => setLastName(e.target.value)}
                name="lastName" id="lastName" placeholder="Last name" />
            </FormGroup>
            <FormGroup>
              <Label for="exampleEmail">Email</Label>
              <Input type="email" required value={email} onChange={e => setEmail(e.target.value)}
                name="email" id="exampleEmail" placeholder="Email" />
            </FormGroup>
            <FormGroup>
              <Label for="examplePassword">Password</Label>
              <Input type="password" required value={password} onChange={e => setPassword(e.target.value)}
                name="password" id="examplePassword" placeholder="Password" />
            </FormGroup>
            <Button type="submit">Submit</Button>
          </Form>
        </Col>
      </Row>
      <NotificationSystem ref={notificationSystem} />
    </Container>
  )
}

export default Register
