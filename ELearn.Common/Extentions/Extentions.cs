using ELearn.Common.Pagination;
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
        public static Expression<Func<Course, bool>> ToCourseExpression(this string Filter, bool isAdmin, string userId = "")
        {
            Expression<Func<Course, bool>> exp;

            if (string.IsNullOrEmpty(Filter) || string.IsNullOrWhiteSpace(Filter))
            {
                if (isAdmin)
                {
                    exp = null;
                }
                else
                {
                    exp = p => p.TeacherId == userId;
                }
            }
            else
            {
                if (isAdmin)
                {
                    exp = p => p.Id.Contains(Filter) ||
                          p.Title.Contains(Filter) ||
                          p.Description.Contains(Filter) ||
                          p.Category.Name.Contains(Filter);
                }
                else
                {
                    exp = p => (p.Id.Contains(Filter) ||
                          p.Title.Contains(Filter) ||
                          p.Description.Contains(Filter) ||
                          p.Category.Name.Contains(Filter)) &&
                          p.TeacherId == userId;
                }
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
                if (SortHeader == "categoryName")
                {
                    return "Category.Name" + "," + SortDirection;
                }
                else if (SortHeader == "teacherName")
                {
                    return "Teacher.UserName" + "," + SortDirection;
                }
                else if (SortHeader == "prerequisitesTitle")
                {
                    return "Prerequisites.Title" + "," + SortDirection;
                }
                return SortHeader.FirstCharToUpper() + "," + SortDirection;
            }
        }
        #endregion
    }
}
