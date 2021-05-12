﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Routes.V1
{
    public class ApiV1Routes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Site = "site";

        public const string Panel = "panel";

        public const string BaseSite = Root + "/" + Version + "/" + Site;

        public const string BasePanel = Root + "/" + Version + "/" + Panel;

        #region AuthRoutes
        public static class Auth
        {
            // /api/v1/panel/auth/register/student
            // POST
            public const string RegisterStudent = BasePanel + "/auth/register/student";

            // /api/v1/panel/auth/register/teacher
            // POST
            public const string RegisterTeacher = BasePanel + "/auth/register/teacher";

            // /api/v1/panel/Auth/login
            // POST
            public const string Login = BasePanel + "/auth/login";
        }
        #endregion

        #region StudentRoutes
        public static class Student
        {
            // /api/v1/panel/students/{id}
            // GET
            public const string GetStudent = BasePanel + "/students/{id}";
        }
        #endregion

        #region TeacherRoutes
        public static class Teacher
        {
            // /api/v1/panel/teachers/{id}
            // GET
            public const string GetTeacher = BasePanel + "/teachers/{id}";
        }
        #endregion

        #region Category
        public static class Category
        {
            // /api/v1/site/categories
            // GET
            public const string GetCategories = BaseSite + "/categories";

            // /api/v1/site/categories/parents
            // GET
            public const string GetParentCategories = BaseSite + "/categories/parents";

            // /api/v1/site/categories/childs
            // GET
            public const string GetChildCategories = BaseSite + "/categories/childs";

            // /api/v1/site/categories/{id}
            // GET
            public const string GetCategory = BaseSite + "/categories/{id}";

            // /api/v1/site/{userId}/categories
            // POST
            public const string AddCategory = BaseSite + "/{userId}/categories";

            // /api/v1/site/{userId}/categories
            // PUT
            public const string UpdateCategory = BaseSite + "/{userId}/categories/{id}";

            // /api/v1/site/{userId}/categories
            // DELETE
            public const string DeleteCategory = BaseSite + "/{userId}/categories/{id}";
        }
        #endregion

        #region Course
        public static class Course
        {
            // /api/v1/site/{userId}/courses
            // GET
            public const string GetCourses = BaseSite + "/{userId}/courses";

            // /api/v1/site/{userId}/courses/{id}
            // GET
            public const string GetCourse = BaseSite + "/{userId}/courses/{id}";

            // /api/v1/site/{userId}/courses
            // POST
            public const string AddCourse = BaseSite + "/{userId}/courses";

            // /api/v1/site/{userId}/courses/{id}
            // PUT
            public const string UpdateCourse = BaseSite + "/{userId}/courses/{id}";

            // /api/v1/site/{userId}/courses/{id}
            // DELETE
            public const string DeleteCourse = BaseSite + "/{userId}/courses/{id}";
        }
        #endregion

        #region Session
        public static class Session
        {
            // /api/v1/site/{userId}/courses/{courseId}/sessions
            // GET
            public const string GetSessions = BaseSite + "/{userId}/courses/{courseId}/sessions";

            // /api/v1/site/{userId}/courses/{courseId}/sessions/{id}
            // GET
            public const string GetSession = BaseSite + "/{userId}/courses/{courseId}/sessions/{id}";

            // /api/v1/site/{userId}/courses/{courseId}/sessions
            // POST
            public const string AddSession = BaseSite + "/{userId}/courses/{courseId}/sessions";

            // /api/v1/site/{userId}/courses/{courseId}/sessions/{id}
            // PUT
            public const string UpdateSession = BaseSite + "/{userId}/courses/{courseId}/sessions/{id}";

            // /api/v1/site/{userId}/courses/{courseId}/sessions/{id}
            // DELETE
            public const string DeleteSession = BaseSite + "/{userId}/courses/{courseId}/sessions/{id}";
        }
        #endregion
    }
}
