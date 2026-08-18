import { useNavigate } from "react-router-dom";

function Dashboard() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
  };

  return (
    <div>
      <h1>Dashboard</h1>
      <p>Bem-vindo ao TaskFlow.</p>

      <button onClick={handleLogout}>Sair</button>
    </div>
  );
}

export default Dashboard;