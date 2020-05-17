import React, { useEffect } from 'react'
import { deleteCookies } from '../CookiesManager'

function Logout() {

  useEffect(() => {
    deleteCookies('currentUser');
    window.location.href = "/login";
  }, [])

  return (
    <div>
      Logout
    </div>
  )
}

export default Logout
