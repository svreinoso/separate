import axios from 'axios';
import { getCookies } from './CookiesManager';

const baseUrl = 'https://localhost:5001';

const handleError = (error, errorCallback) => {
  try {
    const message = error.response.data.message;
    errorCallback && errorCallback(message);
    console.log(message)
    console.log(error.response);
  } catch (error) {
    console.log(error);
    errorCallback && errorCallback('Error')    
  }
}

const getRequestConfig = () => {
  const currentUser = getCookies('currentUser');
  if (!currentUser) return null;
  let user = JSON.parse(currentUser);
  return {
    headers: {
      'Authorization': `Bearer ${user.access_token}`,
      'Access-Control-Allow-Origin': '*',
    }
  }
}

export const httpGet = async (path, errorCallback) => {
  try {
    let result = await axios.get(`${baseUrl}/api${path}`, getRequestConfig());
    return result.data;
  } catch (error) {
    handleError(error, errorCallback);
  }
}

export const httpPost = async (data, errorCallback) => {
  try {
    let result = await axios.post(`${baseUrl}/api/Account/Register`, data);
    return result.data;
  } catch (error) {
    handleError(error, errorCallback);
  }
}

export const requestLogin = async (data, errorCallback) => {
  try {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/x-www-form-urlencoded");

    var urlencoded = new URLSearchParams();
    urlencoded.append("username", data.email);
    urlencoded.append("password", data.password);
    urlencoded.append("grant_type", "password");

    var requestOptions = {
      method: 'POST',
      headers: myHeaders,
      body: urlencoded,
      redirect: 'follow'
    };

    let response = await fetch(`${baseUrl}/connect/token`, requestOptions);
    if(response.ok === true) {
      let result = await response.json();
      return result;
    } else {
      const error = await response.json();
      handleError(error, errorCallback);
    }
  } catch (error) {
    handleError(error, errorCallback);
  }
}