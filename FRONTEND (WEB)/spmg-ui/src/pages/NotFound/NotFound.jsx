// Img
import { Link } from 'react-router-dom';
import notfound from '../../assets/img/notfound.svg';

function NotFound() {
    return (
      <div className="notfound-background">
        <div className="notfound">
          <Link to="/">404 - NOT FOUND</Link>
          <img draggable="false" src={notfound} />
        </div>
      </div>
    );
  }

export default NotFound;