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
            // POST
            public const string ResendActivationEmail = BasePanel + "/auth/activationcode/email";

            // /api/v1/panel/auth/activationcode/phonenumber
            // POST
            public const string ResendActivationPhoneNumber = BasePanel + "/auth/activationcode/phonenumber";

            // /api/v1/panel/auth/activate/email
            // GET
            public const string ActivateEmail = BasePanel + "/auth/activate/email";

            // /api/v1/panel/auth/activate/phonenumber
            // POST
            public const string ActivatePhoneNumber = BasePanel + "/auth/activate/phonenumber";

            // /api/v1/panel/auth/forgetpassword
            // POST
            public const string ForgetPassword = BasePanel + "/auth/forgetpassword";

            // /api/v1/panel/auth/resetpassword
            // POST
            public const string ResetPassword = BasePanel + "/auth/resetpassword";
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
            // /api/v1/panel/teachers/admin
            // GET
            public const string GetTeachersForAdmin = BasePanel + "/teachers/admin";

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

            // /api/v1/site/courses/admin
            // GET
            public const string GetCoursesForAdmin = BaseSite + "/courses/admin";

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

            // /api/v1/site/courses/{id}/status
            // PATCH
            public const string UpdateCourseStatus = BaseSite + "/courses/{id}/status";

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

            // /api/v1/site/comments/admin
            // GET
            public const string GetCommentsForAdmin = BaseSite + "/comments/admin";

            // /api/v1/site/{userId}/courses/{courseId}/comments
            // POST
            public const string AddComment = BaseSite + "/{userId}/courses/{courseId}/comments";

            // /api/v1/site/comments/{id}/status
            // PATCH
            public const string UpdateCommentStatus = BaseSite + "/comments/{id}/status";
        }
        #endregion

        #region DocumentRoutes
        public static class Document
        {
            // /api/v1/panel/{userId}/documents
            // GET
            public const string GetDocuments = BasePanel + "/{userId}/documents";

            // /api/v1/panel/{userId}/documents/admin
            // GET
            public const string GetDocumentsForAdmin = BasePanel + "/{userId}/documents/admin";

            // /api/v1/panel/{userId}/documents/{id}
            // GET
            public const string GetDocument = BasePanel + "/{userId}/documents/{id}";

            // /api/v1/panel/{userId}/documents
            // POST
            public const string AddDocument = BasePanel + "/{userId}/documents";

            // /api/v1/panel/{userId}/documents/status
            // PATCH
            public const string UpdateDocumentStatus = BasePanel + "/{userId}/documents/status";
        }
        #endregion

        #region ExamRoutes
        public static class Exam
        {
            // /api/v1/site/{userId}/exams/courses/{courseId}
            // GET
            public const string GetExams = BaseSite + "/{userId}/exams/courses/{courseId}";

            // /api/v1/site/{userId}/exams/{id}
            // GET
            public const string GetExam = BaseSite + "/{userId}/exams/{id}";

            // /api/v1/site/{userId}/exams/sessions/{sessionId}
            // GET
            public const string GetExamForSession = BaseSite + "/{userId}/exams/sessions/{sessionId}";
            // /api/v1/site/{userId}/exams
            // POST
            public const string AddExam = BaseSite + "/{userId}/exams";

            // /api/v1/site/{userId}/exams/{id}
            // PUT
            public const string UpdateExam = BaseSite + "/{userId}/exams/{id}";

            // /api/v1/site/{userId}/exams/{id}/setgrade
            // PATCH
            public const string UpdatePassingGrade = BaseSite + "/{userId}/exams/{id}/setgrade";

            // /api/v1/site/{userId}/exams/{id}
            // DELETE
            public const string DeleteExam = BaseSite + "/{userId}/exams/{id}";
        }
        #endregion

        #region ExamQuestionRoutes
        public static class ExamQuestion
        {
            // /api/v1/site/{userId}/exams/{examId}/examquestions
            // GET
            public const string GetExamQuestions = BaseSite + "/{userId}/exams/{examId}/examquestions";

            // /api/v1/site/{userId}/exams/{examId}/examquestions/{id}
            // GET
            public const string GetExamQuestion = BaseSite + "/{userId}/exams/{examId}/examquestions/{id}";

            // /api/v1/site/{userId}/exams/{examId}/examquestions
            // POST
            public const string AddExamQuestion = BaseSite + "/{userId}/exams/{examId}/examquestions";

            // /api/v1/site/{userId}/exams/{examId}/examquestions/{id}
            // PUT
            public const string UpdateExamQuestion = BaseSite + "/{userId}/exams/{examId}/examquestions/{id}";

            // /api/v1/site/{userId}/exams/{examId}/examquestions/{id}
            // DELETE
            public const string DeleteExamQuestion = BaseSite + "/{userId}/exams/{examId}/examquestions/{id}";
        }
        #endregion
    }
}
