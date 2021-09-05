import React from 'react';
import ReactDOM from 'react-dom';


const portalRoot = document.getElementById('root');

export default function Modal({ children, isOpen}) {  
  if (!isOpen) {
    return null;
  }

  return ReactDOM.createPortal(
    <>
      {children}
    </>,
    portalRoot
  );
};