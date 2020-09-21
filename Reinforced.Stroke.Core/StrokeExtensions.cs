using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Reinforced.Stroke.Core
{
	/// <summary>
	/// Extensions for SQL Stroke (type-safe raw SQL execution)
	/// </summary>
	public static class StrokeExtensions
	{
		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T>(this DbContext s, Expression<Func<T, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static Task StrokeAsync<T>(this DbContext s, Expression<Func<T, string>> stroke, bool fullQualified = false)
		{
			return InnerStrokeAsync(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T1, T2>(this DbContext s, Expression<Func<T1, T2, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T1, T2, T3>(this DbContext s, Expression<Func<T1, T2, T3, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T1, T2, T3, T4>(this DbContext s, Expression<Func<T1, T2, T3, T4, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T1, T2, T3, T4, T5>(this DbContext s, Expression<Func<T1, T2, T3, T4, T5, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T1, T2, T3, T4, T5, T6>(this DbContext s, Expression<Func<T1, T2, T3, T4, T5, T6, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T1, T2, T3, T4, T5, T6, T7>(this DbContext s, Expression<Func<T1, T2, T3, T4, T5, T6, T7, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		/// <summary>
		/// Executes SQL stroke on EF's DB context
		/// </summary>
		/// <param name="s">Data Context</param>
		/// <param name="stroke">Stroke query</param>
		/// <param name="fullQualified">True to automatically use table aliases</param>
		public static void Stroke<T1, T2, T3, T4, T5, T6, T7, T8>(this DbContext s, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, string>> stroke, bool fullQualified = false)
		{
			InnerStroke(s, stroke, fullQualified);
		}

		private static void InnerStroke(DbContext context, LambdaExpression expr, bool fullQualified)
		{
			var sql = InterpolationParseringExtensions.RevealQuery(context, expr, fullQualified, out var pars);
			context.Database.ExecuteSqlRaw(sql, pars);
		}

		private static Task InnerStrokeAsync(DbContext context, LambdaExpression expr, bool fullQualified)
		{
			var sql = InterpolationParseringExtensions.RevealQuery(context, expr, fullQualified, out var pars);
			return context.Database.ExecuteSqlRawAsync(sql, pars);
		}
	}
}
