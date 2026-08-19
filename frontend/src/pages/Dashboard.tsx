import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

function Dashboard() {
  const navigate = useNavigate();
  const [projectsCount, setProjectsCount] = useState(0);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
  };
  useEffect(() => {
  const loadProjects = async () => {
    const token = localStorage.getItem("token");

    try {
      const response = await fetch("http://localhost:5025/api/projects", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        return;
      }

      const data = await response.json();

      setProjectsCount(data.length);
    } catch (error) {
      console.error(error);
    }
  };

  loadProjects();
}, []);

  return (
    <div>
      <h1>Dashboard</h1>
      <p>Bem-vindo ao TaskFlow.</p>
      <p>Projetos: {projectsCount}</p>

      <button onClick={handleLogout}>Sair</button>
    </div>
  );
}

export default Dashboard;