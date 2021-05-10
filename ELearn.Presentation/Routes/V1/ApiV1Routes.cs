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
            // /api/v1/site/panel/auth/registerstudent
            // POST
            public const string RegisterStudent = BaseSitePanel + "/auth/registerstudent";

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
    }
}
