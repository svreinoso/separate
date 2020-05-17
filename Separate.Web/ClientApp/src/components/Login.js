import React, { useState, useEffect } from 'react'
import { Button, Form, FormGroup, Label, Input, Container, Row, Col } from 'reactstrap';
import { requestLogin, httpGet } from '../HttpManager';
import { setCookies, getCookies } from '../CookiesManager';

function Login() {
	const [email, setEmail] = useState('')
	const [password, setPassword] = useState('')

	useEffect(() => {
		const currentUser = getCookies('currentUser');
		if (currentUser) window.location.href = "/home";
	}, [])


	const doLogin = async (e) => {
		e.preventDefault();
		console.log(email, password);
		let singInResult = await requestLogin({ email, password });
		if (singInResult) {
			setCookies('currentUser', JSON.stringify(singInResult), 1);
			let user = await httpGet('/Account/me');
			user = { ...user, ...singInResult };
			setCookies('currentUser', JSON.stringify(user), 1);
			window.location.href = "/home";
		}
	}

	return (
		<Container>
			<h1 className="text-center">Login</h1>
			<Row className="justify-content-center">
				<Col xs={12} sm={6} md={4} lg={3} xl={2}>
					<Form onSubmit={e => doLogin(e)}>
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

		</Container>
	)
}

export default Login
