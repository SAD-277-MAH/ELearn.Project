using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Routes.V1
{
    public class ApiV1Routes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Panel = "panel";

        public const string BaseSitePanel = Root + "/" + Version + "/" + Panel;

        #region AuthRoutes
        public static class Auth
        {
            // /api/v1/site/panel/auth/register/student
            // POST
            public const string RegisterStudent = BaseSitePanel + "/auth/register/student";

            // /api/v1/site/panel/auth/register/teacher
            // POST
            public const string RegisterTeacher = BaseSitePanel + "/auth/register/teacher";

            // /api/v1/site/panel/Auth/login
            // POST
            public const string Login = BaseSitePanel + "/auth/login";
        }
        #endregion

        #region StudentRoutes
        public static class Student
        {
            // /api/v1/site/panel/students/{id}
            // GET
            public const string GetStudent = BaseSitePanel + "/students/{id}";
        }
        #endregion

        #region TeacherRoutes
        public static class Teacher
        {
            // /api/v1/site/panel/teachers/{id}
            // GET
            public const string GetTeacher = BaseSitePanel + "/teachers/{id}";
        }
        #endregion
    }
}
