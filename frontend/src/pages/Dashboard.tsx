import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./Dashboard.css";

function Dashboard() {
  const navigate = useNavigate();
  const [projectsCount, setProjectsCount] = useState(0);
  const [projects, setProjects] = useState<any[]>([]);
  const [tasksCount, setTasksCount] = useState(0);
  const [pendingTasksCount, setPendingTasksCount] = useState(0);
  const [completedTasksCount, setCompletedTasksCount] = useState(0);

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

      if (response.status === 401) {
  localStorage.removeItem("token");
  navigate("/");
  return;
}

if (!response.ok) {
  return;
}

      const data = await response.json();

      setProjectsCount(data.length);
      setProjects(data);
      const tasksResponse = await fetch("http://localhost:5025/api/tasks", {
  headers: {
    Authorization: `Bearer ${token}`,
  },
});
if (tasksResponse.status === 401) {
  localStorage.removeItem("token");
  navigate("/");
  return;
}
if (tasksResponse.ok) {
  const tasksData = await tasksResponse.json();
  setTasksCount(tasksData.length);

  const pendingTasks = tasksData.filter(
    (task: { status: string }) => task.status.toLowerCase() === "in progress"
  );

  setPendingTasksCount(pendingTasks.length);

  const completedTasks = tasksData.filter(
  (task: { status: string }) => task.status.toLowerCase() === "completed"
);

setCompletedTasksCount(completedTasks.length);
}
    } catch (error) {
      console.error(error);
    }
  };

  loadProjects();
}, []);

return (
  <div className="dashboard-page">
    <div className="dashboard-header">
      <div>
        <h1>Dashboard</h1>
        <p>Bem-vindo ao TaskFlow.</p>
      </div>

      <button className="logout-button" onClick={handleLogout}>
        Sair
      </button>
    </div>

    <div className="dashboard-cards">
      <div className="dashboard-card">
        <span>Projetos</span>
        <strong>{projectsCount}</strong>
      </div>

      <div className="dashboard-card">
        <span>Tarefas</span>
        <strong>{tasksCount}</strong>
      </div>

      <div className="dashboard-card">
        <span>Em andamento</span>
        <strong>{pendingTasksCount}</strong>
      </div>

      <div className="dashboard-card">
        <span>Concluídas</span>
        <strong>{completedTasksCount}</strong>
      </div>
    </div>
  
  <div className="projects-section">
  <h2>Meus Projetos</h2>

  <div className="projects-list">
    {projects.map((project) => (
      <div
  className="project-item"
  key={project.id}
  onClick={() => navigate(`/projects/${project.id}`)}
>
        <h3>{project.name}</h3>
        <p>{project.description}</p>
        <span>Status: {project.status}</span>
      </div>
    ))}
 </div>
</div>
</div>
);
}

export default Dashboard;