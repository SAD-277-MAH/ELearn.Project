using ELearn.Common.Pagination;
using ELearn.Common.Utilities;
using ELearn.Data.Enums;
using ELearn.Data.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ELearn.Common.Extentions
{
    public static class Extentions
    {
        public static void AddAppError(this HttpResponse response, string message)
        {
            response.Headers.Add("App-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "App-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static bool IsImage(this IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    var image = Image.FromStream(file.OpenReadStream());
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static int ToAge(this DateTime dateOfBirth)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth.AddYears(age) > DateTime.Today)
            {
                age--;
            }

            return age;
        }

        public static string ToShamsiDateTime(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)} {dateTime.Hour}:{dateTime.Minute}";
        }

        public static string ToShamsiDate(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)}";
        }

        public static string FirstCharToUpper(this string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormater = new JsonSerializerSettings();
            camelCaseFormater.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormater));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        #region IQueryableExtensions
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderBy", propertyName, comparer);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "OrderByDescending", propertyName, comparer);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenBy", propertyName, comparer);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName, IComparer<object> comparer = null)
        {
            return CallOrderedQueryable(query, "ThenByDescending", propertyName, comparer);
        }

        /// <summary>
        /// Builds the Queryable functions using a TSource property name.
        /// </summary>
        public static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName, IComparer<object> comparer = null)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            return comparer != null
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param),
                        Expression.Constant(comparer)
                    )
                )
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                    Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new[] { typeof(T), body.Type },
                        query.Expression,
                        Expression.Lambda(body, param)
                    )
                );
        }
        #endregion

        #region Course Paging
        public static Expression<Func<Course, bool>> ToCourseExpression(this string Filter, StatusType statusType)
        {
            Expression<Func<Course, bool>> exp = p => true;

            if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrWhiteSpace(Filter))
            {
                Expression<Func<Course, bool>> tempExp = (p =>
                                                          p.Title.Contains(Filter) ||
                                                          p.Description.Contains(Filter) ||
                                                          p.Category.Name.Contains(Filter));

                exp = CombineExpressions.CombiningExpressions<Course>(exp, tempExp, ExpressionsType.And);
            }

            switch (statusType)
            {
                case StatusType.All:
                    break;
                case StatusType.Approved:
                    Expression<Func<Course, bool>> tempExpApproved = p => p.Status == 1;
                    exp = CombineExpressions.CombiningExpressions<Course>(exp, tempExpApproved, ExpressionsType.And);
                    break;
                case StatusType.Pending:
                    Expression<Func<Course, bool>> tempExpPending = p => p.Status == 0;
                    exp = CombineExpressions.CombiningExpressions<Course>(exp, tempExpPending, ExpressionsType.And);
                    break;
                case StatusType.Reject:
                    Expression<Func<Course, bool>> tempExpReject = p => p.Status == -1;
                    exp = CombineExpressions.CombiningExpressions<Course>(exp, tempExpReject, ExpressionsType.And);
                    break;
                default:
                    break;
            }

            return exp;
        }

        public static string ToCourseOrderBy(this string SortHeader, string SortDirection)
        {
            if (string.IsNullOrEmpty(SortHeader) || string.IsNullOrWhiteSpace(SortHeader))
            {
                return "";
            }
            else
            {
                if (SortHeader.ToLower() == "categoryname")
                {
                    return "Category.Name" + "," + SortDirection;
                }
                else if (SortHeader.ToLower() == "teachername")
                {
                    return "Teacher.UserName" + "," + SortDirection;
                }
                return SortHeader.FirstCharToUpper() + "," + SortDirection;
            }
        }
        #endregion

        #region Comment Paging
        public static Expression<Func<Comment, bool>> ToCommentExpression(this string Filter, StatusType statusType)
        {
            Expression<Func<Comment, bool>> exp = p => true;

            if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrWhiteSpace(Filter))
            {
                Expression<Func<Comment, bool>> tempExp = (p => p.Text.Contains(Filter));

                exp = CombineExpressions.CombiningExpressions<Comment>(exp, tempExp, ExpressionsType.And);
            }

            switch (statusType)
            {
                case StatusType.All:
                    break;
                case StatusType.Approved:
                    Expression<Func<Comment, bool>> tempExpApproved = p => p.Status == 1;
                    exp = CombineExpressions.CombiningExpressions<Comment>(exp, tempExpApproved, ExpressionsType.And);
                    break;
                case StatusType.Pending:
                    Expression<Func<Comment, bool>> tempExpPending = p => p.Status == 0;
                    exp = CombineExpressions.CombiningExpressions<Comment>(exp, tempExpPending, ExpressionsType.And);
                    break;
                case StatusType.Reject:
                    Expression<Func<Comment, bool>> tempExpReject = p => p.Status == -1;
                    exp = CombineExpressions.CombiningExpressions<Comment>(exp, tempExpReject, ExpressionsType.And);
                    break;
                default:
                    break;
            }

            return exp;
        }

        public static string ToCommentOrderBy(this string SortHeader, string SortDirection)
        {
            if (string.IsNullOrEmpty(SortHeader) || string.IsNullOrWhiteSpace(SortHeader))
            {
                return "";
            }
            else
            {
                return SortHeader.FirstCharToUpper() + "," + SortDirection;
            }
        }
        #endregion

        //#region Document Paging
        //public static Expression<Func<Document, bool>> ToDocumentExpression(this string Filter, StatusType statusType)
        //{
        //    Expression<Func<Document, bool>> exp = p => true;

        //    if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrWhiteSpace(Filter))
        //    {
        //        Expression<Func<Document, bool>> tempExp = (p =>    p.Teacher.UserName.ToLower().Contains(Filter.ToLower())
        //                                                         || p.Teacher.LastName.Contains(Filter));

        //        exp = CombineExpressions.CombiningExpressions<Document>(exp, tempExp, ExpressionsType.And);
        //    }

        //    switch (statusType)
        //    {
        //        case StatusType.All:
        //            break;
        //        case StatusType.Approved:
        //            Expression<Func<Document, bool>> tempExpApproved = p => p.Status == 1;
        //            exp = CombineExpressions.CombiningExpressions<Document>(exp, tempExpApproved, ExpressionsType.And);
        //            break;
        //        case StatusType.Pending:
        //            Expression<Func<Document, bool>> tempExpPending = p => p.Status == 0;
        //            exp = CombineExpressions.CombiningExpressions<Document>(exp, tempExpPending, ExpressionsType.And);
        //            break;
        //        case StatusType.Reject:
        //            Expression<Func<Document, bool>> tempExpReject = p => p.Status == -1;
        //            exp = CombineExpressions.CombiningExpressions<Document>(exp, tempExpReject, ExpressionsType.And);
        //            break;
        //        default:
        //            break;
        //    }

        //    return exp;
        //}

        //public static string ToDocumentOrderBy(this string SortHeader, string SortDirection)
        //{
        //    if (string.IsNullOrEmpty(SortHeader) || string.IsNullOrWhiteSpace(SortHeader))
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        if (SortHeader.ToLower() == "username")
        //        {
        //            return "Teacher.UserName" + "," + SortDirection;
        //        }
        //        else if (SortHeader.ToLower() == "lastname")
        //        {
        //            return "Teacher.LastName" + "," + SortDirection;
        //        }
        //        return SortHeader.FirstCharToUpper() + "," + SortDirection;
        //    }
        //}
        //#endregion

        #region Teacher Paging
        public static Expression<Func<Teacher, bool>> ToTeacherExpression(this string Filter, StatusType statusType)
        {
            Expression<Func<Teacher, bool>> exp = p => true;

            if (!string.IsNullOrEmpty(Filter) && !string.IsNullOrWhiteSpace(Filter))
            {
                Expression<Func<Teacher, bool>> tempExp = (p => p.User.UserName.Contains(Filter)
                                                                || p.User.FirstName.Contains(Filter)
                                                                || p.User.LastName.Contains(Filter));

                exp = CombineExpressions.CombiningExpressions<Teacher>(exp, tempExp, ExpressionsType.And);
            }

            switch (statusType)
            {
                case StatusType.All:
                    break;
                case StatusType.Approved:
                    Expression<Func<Teacher, bool>> tempExpApproved = p => p.Status == 1;
                    exp = CombineExpressions.CombiningExpressions<Teacher>(exp, tempExpApproved, ExpressionsType.And);
                    break;
                case StatusType.Pending:
                    Expression<Func<Teacher, bool>> tempExpPending = p => p.Status == 0;
                    exp = CombineExpressions.CombiningExpressions<Teacher>(exp, tempExpPending, ExpressionsType.And);
                    break;
                case StatusType.Reject:
                    Expression<Func<Teacher, bool>> tempExpReject = p => p.Status == -1;
                    exp = CombineExpressions.CombiningExpressions<Teacher>(exp, tempExpReject, ExpressionsType.And);
                    break;
                default:
                    break;
            }

            return exp;
        }

        public static string ToTeacherOrderBy(this string SortHeader, string SortDirection)
        {
            if (string.IsNullOrEmpty(SortHeader) || string.IsNullOrWhiteSpace(SortHeader))
            {
                return "";
            }
            else
            {
                if (SortHeader.ToLower() == "username")
                {
                    return "User.UserName" + "," + SortDirection;
                }
                else if (SortHeader.ToLower() == "phonenumber")
                {
                    return "User.PhoneNumber" + "," + SortDirection;
                }
                else if (SortHeader.ToLower() == "firstname")
                {
                    return "User.FirstName" + "," + SortDirection;
                }
                else if (SortHeader.ToLower() == "lastname")
                {
                    return "User.LastName" + "," + SortDirection;
                }
                return SortHeader.FirstCharToUpper() + "," + SortDirection;
            }
        }
        #endregion
    }
}
