using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using wpf_backend.Data;

namespace wpf_backend
{
    public class Back
    {
        /// <summary>
        /// ID of the current user
        /// </summary>
        private static int sessionId = 0;

        public Back() 
        { 
        }

        /// <summary>
        /// Check login data
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <returns>
        /// 0 for success;
        /// -1 for incorrect password;
        /// -2 for login incorrect;
        /// </returns>
        public int loginCheck(String login, String pass)
        {
            int status = -1;

            using (var context = new AppDbContext())
            {
                var userList = context.users.ToList();
                foreach (var user in userList)
                {
                    if (user.login == login) // User exists
                    {
                        if (user.password == pass) // Password correct
                        {
                            status = 0;
                            sessionId = Convert.ToInt32(user.id);
                            goto exitPoint;
                        } else // Password incorrect
                        {
                            status = -1;
                            goto exitPoint;
                        }
                    }
                }

                // Login not found
                status = -2;
            }
            
            exitPoint:
            return status;
        }

        /// <summary>
        /// Add a user to the db
        /// </summary>
        /// <param name="login"></param>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns>
        /// 0 for success
        /// -1 for errors
        /// </returns>
        public int signUp(String login, String name, String pass)
        {
            User new_user = new User();
            new_user.login = login;
            new_user.name = name;
            new_user.password = pass;
            new_user.date_joined = DateTime.Now.ToUniversalTime();

            using (var context = new AppDbContext())
            {
                // User insertion
                var userList = context.users.ToList();
                foreach (var user in userList)
                {
                    if (user.login == login)
                    {
                        return -1;
                    }
                }

                context.users.Add(new_user);
                context.SaveChanges();

                sessionId = context.users.OrderBy(user => user.id).Last().id;

                // User settings profile creation
                User_settings sets = new User_settings();
                sets.fk_user_id = sessionId;
                sets.lang = LangType.ua;
                sets.theme = ThemeType.light;
                sets.time_format = TimeFormatType._24;
                sets.date_format = DateFormatType.eu;

                context.user_settings.Add(sets);
                context.SaveChanges();
            }

            return 0;
        }

        /// <summary>
        /// Delete current user account
        /// </summary>
        /// <returns>
        /// 0 for success
        /// -1 for errors
        /// </returns>
        public int selfDestruct()
        {
            if(sessionId != 0)
            {
                User toDelete = new User();
                toDelete.id = sessionId;
                using (var context = new AppDbContext())
                {
                    context.users.Remove(toDelete);
                    context.SaveChanges();
                    
                    sessionId = 0;
                }
            } else
            {
                return -1;
            }

            return 0;
        }
    
        /// <summary>
        /// Change name of the current user
        /// </summary>
        /// <param name="new_name"></param>
        /// <returns>
        /// 0 for success
        /// -1 for errors
        /// </returns>
        public int changeName(String new_name)
        {
            if (new_name == null)
            {
                return -1;
            }

            using (var context = new AppDbContext())
            {
                var toUpdate = context.users.Find(sessionId);
                if (toUpdate != null)
                {
                    toUpdate.name = new_name;
                    context.SaveChanges();
                } else
                {
                    return -1;
                }
                
            }

            return 0;
        }

        /// <summary>
        /// Retrieve projects list for the current user
        /// </summary>
        /// <returns>
        /// List of current user projects or NULL
        /// </returns>
        public List<Project> getUserProjects()
        {
            List<Project> toReturn = new List<Project> { };

            // Initialize the database context
            using (var context = new AppDbContext())
            {
                // Retrieve the list of products from the database
                var projList = context.projects.ToList();
                if (projList != null)
                {
                    foreach (var project in projList)
                    {
                        if(project.fk_user_id == sessionId)
                        {
                            toReturn.Add(project);
                        }
                    }
                } else
                {
                    toReturn = null;
                }
                
            }

            return toReturn;
        }

        /// <summary>
        /// Add a project for the current user
        /// </summary>
        /// <param name="title"></param>
        /// <param name="descr"></param>
        /// <param name="deadline"></param>
        /// <param name="estimate"></param>
        /// <param name="state"></param>
        /// <param name="priority"></param>
        /// <returns>
        /// 0 for success
        /// </returns>
        public int addProject(string title, string descr, DateTime deadline, TimeSpan estimate, string state, string priority)
        {
            Project new_proj = new Project();
            new_proj.fk_user_id = sessionId;
            new_proj.title = title;
            new_proj.description = descr;
            new_proj.created_at = DateTime.UtcNow;
            new_proj.deadline = deadline.ToUniversalTime();
            new_proj.state = StateType.in_progress;

            Enum.TryParse(priority, out PriorityType priora);
            new_proj.priority = priora;

            new_proj.estimate = estimate;

            using (var context = new AppDbContext())
            {
                context.projects.Add(new_proj);
                context.SaveChanges();
            }

            return 0;
        }

        /// <summary>
        /// Insert new task into the database. Both for project and non-project tasks;
        /// For project specify project_id;
        /// For non-project set null.
        /// </summary>
        /// <param name="proj_id"></param>
        /// <param name="title"></param>
        /// <param name="descr"></param>
        /// <param name="deadline"></param>
        /// <param name="estimate"></param>
        /// <param name="state"></param>
        /// <param name="priority"></param>
        /// <returns>
        /// 0 for success
        /// </returns>
        public int addTask(int? proj_id, string title, string? descr, DateTime deadline, TimeSpan? estimate, string state, string priority)
        {
            Data.Task new_task = new Data.Task();
            new_task.fk_user_id = sessionId;
            new_task.title = title;
            new_task.description = descr;
            new_task.created_at = DateTime.UtcNow;
            new_task.deadline = deadline.ToUniversalTime();
            new_task.state = StateType.in_progress;
            //Enum.TryParse(state, out StateType stt);
            //new_task.state = stt;

            new_task.estimate = estimate;

            Enum.TryParse(priority, out PriorityType priora);
            new_task.priority = priora;

            if (proj_id  == null) // Simple task insertion
            {
                new_task.fk_project_id = null;
                new_task.project_sequence_num = null;
            }
            else // Project subtask insertion
            {
                new_task.fk_project_id = proj_id;

                using (var context = new AppDbContext())
                {
                    var maxSeq = context.tasks.Where(t => t.fk_project_id.Equals(proj_id)).Max(t => t.project_sequence_num);
                    new_task.project_sequence_num = (maxSeq != null) ? (short?)(maxSeq + 1) : 1;
                }

            }

            using (var context = new AppDbContext())
            {
                context.tasks.Add(new_task);
                context.SaveChanges();
            }

            return 0;
        }

        /// <summary>
        /// Get tasks for current user, projId optional
        /// </summary>
        /// <param name="projId"></param>
        /// <returns>
        /// List of Data.Task or null
        /// </returns>
        public List<Data.Task> getUserTasks(int? projId)
        {
            List<Data.Task> toReturn = new List<Data.Task> { };

            // Initialize the database context
            using (var context = new AppDbContext())
            {
                // Retrieve the list of products from the database
                var taskList = context.tasks.ToList();
                if (taskList != null)
                {
                    if (projId == null)
                    {
                        foreach (var task in taskList)
                        {
                            if (task.fk_user_id == sessionId)
                            {
                                toReturn.Add(task);
                            }
                        }
                    } else
                    {
                        foreach (var task in taskList)
                        {
                            if (task.fk_user_id == sessionId &&
                                task.fk_project_id == projId)
                            {
                                toReturn.Add(task);
                            }
                        }
                    }
                    
                }
                else
                {
                    toReturn = null;
                }

            }

            return toReturn;
        }

        /// <summary>
        /// Retrieve settings of the current user
        /// </summary>
        /// <returns>
        /// User_settings element or null
        /// </returns>
        public User_settings getUserSettings()
        {
            User_settings toReturn = new User_settings();

            // Initialize the database context
            using (var context = new AppDbContext())
            {
                // Retrieve the list of products from the database
                toReturn = context.user_settings.Where(t => t.fk_user_id == sessionId).First();

            }

            return toReturn;
        }

        /// <summary>
        /// Function to update element of User_settings (set where_col and where_val to null)
        /// or any other table (specify additional parameters)
        /// </summary>
        /// <param name="table"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <param name="where_col"></param>
        /// <param name="where_val"></param>
        /// <returns>
        /// 0 for success
        /// </returns>
        public int updateDBElement(string table, string col, string value, string? where_col, string? where_val)
        {
            if (where_col == null || where_val == null) // Update row by sessionID
            {
                using (var context = new AppDbContext())
                {
                    context.Database.ExecuteSqlRaw(
                        $"UPDATE {table} " +
                        $"SET {col}=\'{value}\' " +
                        $"WHERE fk_user_id={sessionId};");

                    context.SaveChanges();
                }
            } else // Update by sessionID and where_col
            {
                using (var context = new AppDbContext())
                {
                    context.Database.ExecuteSqlRaw(
                        $"UPDATE {table} " +
                        $"SET {col}=\'{value}\' " +
                        $"WHERE fk_user_id={sessionId} AND {where_col}=\'{where_val}\';");

                    context.SaveChanges();
                }
            }

            return 0;
        }

        /// <summary>
        /// Get id of the current logged user
        /// </summary>
        /// <returns>
        /// int user id
        /// </returns>
        public int getSID()
        {
            return sessionId;
        }

        /// <summary>
        /// Set user id = 0 for the current session
        /// </summary>
        public void terminateSession()
        {
            sessionId = 0;
        }
    }
}
