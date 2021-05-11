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
    }
}
