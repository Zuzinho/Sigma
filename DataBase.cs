using System;
using System.Collections.Generic;
using Sigma.Models;
using Microsoft.Data.SqlClient;

namespace Sigma
{
    public class DataBase
    {
        private static string connection = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=" +
            "|DataDirectory|\\SigmaDB.mdf;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        private static Project _initProject(SqlDataReader reader)
        {
            int id = (int)reader.GetValue(0);
            string title = (string)reader.GetValue(1);
            string technology = (string)reader.GetValue(2);
            string photoUrl = (string)reader.GetValue(3);
            string url = (string)reader.GetValue(4);
            string about = (string)reader.GetValue(5);
            int user_id = (int)reader.GetValue(6);
            bool selected = (bool)reader.GetValue(7);
            return new Project(id, title, about, user_id , technology , selected,photoUrl, url);
        }
        private static Link _initLink(SqlDataReader reader)
        {
            int id = (int)reader.GetValue(0);
            string provider = (string)reader.GetValue(1);
            string url = (string)reader.GetValue(2);
            int userId = (int)reader.GetValue(3);
            string ProviderAvatarUrl = (string)reader.GetValue(4);
            return new Link(id, provider,url,userId,ProviderAvatarUrl);
        }

        public static void CreateProjectsTable(int userId)
        {
            string name = "ProjectsUser" + userId;
            string sql = "if OBJECT_ID('" + name + "') is null CREATE TABLE " + name + " (Id INT NOT NULL,Title NVARCHAR(128),Technology NVARCHAR(64),PhotoUrl NVARCHAR (256),Link NVARCHAR (256),About NVARCHAR (MAX),UserId INT,Selected BIT,PRIMARY KEY (Id));";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void CreateLinksTable(int userId)
        {
            string name = "LinksUser" + userId;
            string sql = "if OBJECT_ID('" + name + "') is null CREATE TABLE " + name + " (Id INT NOT NULL,Provider NVARCHAR(20),Url NVARCHAR(256),UserId INT,ProviderAvatarUrl NVARCHAR (256),PRIMARY KEY (Id));";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void AddProject(int userId, Project project)
        {
            string name = "ProjectsUser" + userId;
            string sql = "INSERT INTO " + name + " (Id,Title,Technology,PhotoUrl,Link,About,UserId,Selected" +
                ") VALUES (" + project.Id + ",'" + project.Title + "','" + project.Technology + "','" + project.PhotoUrl + "'," +
                "'" + project.Url + "','" + project.About + "'," + project.UserId + ", " + Convert.ToByte(project.Selected) + ");";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void AddLink(int userId, Link link)
        {
            string name = "LinksUser" + userId;
            string sql = "INSERT INTO " + name + " (Id,Provider,Url,UserId,ProviderAvatarUrl" +
                ") VALUES (" + link.Id + ",'" + link.Provider + "','" + link.Url + "'," + link.UserId + "," +
                "'" + link.ProviderAvatarUrl + "');";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static List<Project> GetProjects(int userId)
        {
            string name = "ProjectsUser" + userId;
            string sql = "SELECT * FROM " + name;
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            var reader = command.ExecuteReader();
            List<Project> list = new List<Project>();
            while (reader.Read())
            {
                list.Add(_initProject(reader));
            }
            con.Close();
            return list;
        }
        public static List<Link> GetLinks(int userId)
        {
            string name = "LinksUser" + userId;
            string sql = "SELECT * FROM " + name;
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            var reader = command.ExecuteReader();
            List<Link> list = new List<Link>();
            while (reader.Read())
            {
                list.Add(_initLink(reader));
            }
            con.Close();
            return list;
        }
        public static void ChangeData(int userId,Project project)
        {
            string name = "ProjectsUser" + userId;
            string sql = "UPDATE " + name + " SET Title = '" + project.Title + "', Technology = '" + project.Technology + "',PhotoUrl = '" + project.PhotoUrl + "',Link = '" + project.Url + "', About = '" + project.About + "'  WHERE Id = " + project.Id + "; ";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void ChangeData(int userId,Link  link)
        {
            string name = "LinksUser" + userId;
            string sql = "UPDATE " + name + " SET Provider = '" + link.Provider + "', Url = '" + link.Url + "',ProviderAvatarUrl = '" + link.ProviderAvatarUrl + "'  WHERE Id = " + link.Id + "; ";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void DeleteProject(int userId,int projectId)
        {
            string name = "ProjectsUser" + userId;
            string sql = "DELETE " + name + " WHERE Id = " + projectId.ToString();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void DeleteLink(int userId, int linkId)
        {
            string name = "LinksUser" + userId;
            string sql = "DELETE " + name + " WHERE Id = " + linkId.ToString();
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        private static void _deleteProjects(int userId)
        {
            string name = "ProjectsUser" + userId;
            string sql = "DROP TABLE " + name;
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        private static void _deleteLinks(int userId)
        {
            string name = "LinksUser" + userId;
            string sql = "DROP TABLE " + name;
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
            con.Close();
        }
        public static void DeleteData(int userId)
        {
            _deleteProjects(userId);
            _deleteLinks(userId);
        }
    }
}