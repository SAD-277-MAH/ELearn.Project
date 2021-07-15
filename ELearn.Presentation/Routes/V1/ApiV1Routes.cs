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

            // /api/v1/panel/auth/login
            // POST
            public const string Login = BasePanel + "/auth/login";

            // /api/v1/panel/auth/activationcode/email
            // GET
            public const string ResendActivationEmail = BasePanel + "/auth/activationcode/email";

            // /api/v1/panel/auth/activationcode/phonenumber
            // GET
            public const string ResendActivationPhoneNumber = BasePanel + "/auth/activationcode/phonenumber";

            // /api/v1/panel/auth/activate/email
            // GET
            public const string ActivateEmail = BasePanel + "/auth/activate/email";

            // /api/v1/panel/auth/activate/phonenumber
            // GET
            public const string ActivatePhoneNumber = BasePanel + "/auth/activate/phonenumber";
        }
        #endregion

        #region SettingRoutes
        public static class Setting
        {
            // /api/v1/site/settings/site
            // GET
            public const string GetSiteSetting = BaseSite + "/settings/site";

            // /api/v1/site/settings/messagesender
            // GET
            public const string GetMessageSenderSetting = BaseSite + "/settings/messagesender";

            // /api/v1/site/settings/site
            // PUT
            public const string UpdateSiteSetting = BaseSite + "/settings/site";

            // /api/v1/site/settings/messagesender
            // PUT
            public const string UpdateMessageSenderSetting = BaseSite + "/settings/messagesender";
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

        #region CategoryRoutes
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

            // /api/v1/site/categories
            // POST
            public const string AddCategory = BaseSite + "/categories";

            // /api/v1/site/categories
            // PUT
            public const string UpdateCategory = BaseSite + "/categories/{id}";

            // /api/v1/site/categories
            // DELETE
            public const string DeleteCategory = BaseSite + "/categories/{id}";
        }
        #endregion

        #region CourseRoutes
        public static class Course
        {
            // /api/v1/site/courses
            // GET
            public const string GetCourses = BaseSite + "/courses";

            // /api/v1/site/{userId}/courses
            // GET
            public const string GetUserCourses = BaseSite + "/{userId}/courses";

            // /api/v1/site/courses/{id}
            // GET
            public const string GetCourse = BaseSite + "/courses/{id}";

            // /api/v1/site/{userId}/courses
            // POST
            public const string AddCourse = BaseSite + "/{userId}/courses";

            // /api/v1/site/{userId}/courses/{id}
            // PUT
            public const string UpdateCourse = BaseSite + "/{userId}/courses/{id}";

            // /api/v1/site/courses/{id}
            // DELETE
            public const string DeleteCourse = BaseSite + "/courses/{id}";
        }
        #endregion

        #region SessionRoutes
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

        #region OrderRoutes
        public static class Order
        {
            // /api/v1/site/{userId}/order
            // GET
            public const string GetOrder = BaseSite + "/{userId}/order";

            // /api/v1/site/{userId}/order/history
            // GET
            public const string GetOrders = BaseSite + "/{userId}/order/history";

            // /api/v1/site/{userId}/order/{courseId}
            // POST
            public const string AddToOrder = BaseSite + "/{userId}/order/{courseId}";

            // /api/v1/site/{userId}/order/{courseId}
            // DELETE
            public const string RemoveFromOrder = BaseSite + "/{userId}/order/{courseId}";

            // /api/v1/site/{userId}/order/pay
            // GET
            public const string Payment = BaseSite + "/{userId}/order/pay";
        }
        #endregion

        #region CommentRoutes
        public static class Comment
        {
            // /api/v1/site/courses/{courseId}/comments
            // GET
            public const string GetComments = BaseSite + "/courses/{courseId}/comments";

            // /api/v1/site/{userId}/courses/{courseId}/comments
            // POST
            public const string AddComment = BaseSite + "/{userId}/courses/{courseId}/comments";
        }
        #endregion
    }
}
