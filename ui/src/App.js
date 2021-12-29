import logo from './logo.svg';
import './App.css';
import {Home} from './Home';
import {Artist} from './Artist';
import {Album} from './Album';
import {Song} from './Song';
import {BrowserRouter, Route, Routes, NavLink} from 'react-router-dom';


function App() {
  return (
    <BrowserRouter>
      <div className="App container">
        <h3 className="d-flex jusify-content-center m-3">
          Welcome to Music Library
        </h3>

        {/*
      Navigation Menu, Stacks vertically on small screens with a 
      white background and black text
      Creates unordered list in the navbar
      Items are buttons that have basic styling and a blue outline(primary)
      */}
        <nav className="navbar navbar-expand-sm bg-light navbar-dark">
          <ul className="navbar-nav">
            <li className="nav-item- m-1">
              <NavLink className="btn btn-light btn-outline-primary" to="/home">
                Home
              </NavLink>
            </li>
            <li className="nav-item- m-1">
              <NavLink className="btn btn-light btn-outline-primary" to="/artist">
                Artist
              </NavLink>
            </li>
            <li className="nav-item- m-1">
              <NavLink className="btn btn-light btn-outline-primary" to="/album">
                Album
              </NavLink>
            </li>
            <li className="nav-item- m-1">
              <NavLink className="btn btn-light btn-outline-primary" to="/song">
                Song
              </NavLink>
            </li>
          </ul>
        </nav>
        {/*Using the Routes tag will only allow one matching route to be rendered*/}
        <Routes>
          <Route path='/home' element={<Home/>} />
          <Route path='/artist' element={<Artist/>} />
          <Route path='/album' element={<Album/>} />
          <Route path='/song' element={<Song/>} />
        </Routes>

      </div>
    </BrowserRouter>
  );
}

export default App;
