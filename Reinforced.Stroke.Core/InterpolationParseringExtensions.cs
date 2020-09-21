using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reinforced.Stroke.Core
{
	internal class InterpolationParseringExtensions
	{
		public static string RevealQuery(DbContext context, LambdaExpression expr, bool fullQualified, out object[] parameters)
		{
			const string err = "SQL Stroke must be in form of context.Stroke(x=>$\"SOME SQL WITH {x} AND {x.Field} USAGE\")";
			if (!(expr.Body is MethodCallExpression bdy)) throw new Exception(err);
			if (bdy.Method.DeclaringType != typeof(string) || bdy.Method.Name != "Format")
			{
				throw new Exception(err);
			}

			if (!(bdy.Arguments[0] is ConstantExpression fmtExpr)) throw new Exception(err);
			var format = fmtExpr.Value.ToString();

			var startingIndex = 1;
			var arguments = bdy.Arguments;
			var longFormat = false;
			if (bdy.Arguments.Count == 2)
			{
				var secondArg = bdy.Arguments[1];
				if (secondArg.NodeType == ExpressionType.NewArrayInit)
				{
					var array = (NewArrayExpression)secondArg;
					arguments = array.Expressions;
					startingIndex = 0;
					longFormat = true;
				}
			}

			var formatArgs = new List<object>();
			var sqlParams = new List<object>();
			for (var i = startingIndex; i < arguments.Count; i++)
			{
				var cArg = Unconvert(arguments[i]);
				if (!IsScopedParameterAccess(cArg))
				{
					var lex = Expression.Lambda(cArg);
					var compiled = lex.Compile();
					var result = compiled.DynamicInvoke();
					formatArgs.Add($"{{{sqlParams.Count}}}");
					sqlParams.Add(result);
					continue;
				}
				if (cArg.NodeType == ExpressionType.Parameter)
				{
					if (fullQualified)
					{
						var par = (ParameterExpression)cArg;
						var argIdx = longFormat ? i : i - 1;
						if (NeedsDec(format, argIdx))
						{
							formatArgs.Add($"[{context.GetTableName(cArg.Type)}] [{par.Name}]");
						}
						else
						{
							formatArgs.Add(par.Name);
						}
					}
					else formatArgs.Add($"[{context.GetTableName(cArg.Type)}]");

					continue;
				}

				var argProp = (MemberExpression)cArg;

				if (argProp.Expression.NodeType != ExpressionType.Parameter)
				{
					var root = GetRootMember(argProp);
					throw new Exception($"Please refer only top-level properties of {root.Type}");
				}

				var colId = $"[{context.GetColumnName(argProp.Member.DeclaringType, argProp.Member.Name)}]";
				if (fullQualified)
				{
					var root = GetRootMember(argProp);
					var parRef = root as ParameterExpression;
					colId = $"[{parRef}].{colId}";
				}
				formatArgs.Add(colId);
			}
			var sqlString = string.Format(format, formatArgs.ToArray());
			parameters = sqlParams.ToArray();
			return sqlString;
		}

		private static bool NeedsDec(string format, int argNumber)
		{
			var searchString = $"{{{argNumber}}}";
			var idx = format.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) - 1;
			if (idx <= 0)
				return false;

			while (char.IsWhiteSpace(format, idx)) idx--;
			if (idx - 4 < 0)
				return false;

			var s = format.Substring(idx - 3, 4);
			return string.Compare(s, "FROM", StringComparison.InvariantCultureIgnoreCase) == 0 ||
				   string.Compare(s, "JOIN", StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		private static Expression Unconvert(Expression ex)
		{
			if (ex.NodeType == ExpressionType.Convert)
			{
				var cex = (UnaryExpression)ex;
				ex = cex.Operand;
			}
			return ex;
		}

		private static Expression GetRootMember(MemberExpression expr)
		{
			var accessee = expr.Expression as MemberExpression;
			var current = expr.Expression;
			while (accessee != null)
			{
				accessee = accessee.Expression as MemberExpression;
				if (accessee != null) current = accessee.Expression;
			}
			return current;
		}

		private static bool IsScopedParameterAccess(Expression expr)
		{
			if (expr.NodeType == ExpressionType.Parameter)
				return true;

			if (!(expr is MemberExpression ex))
				return false;

			var root = GetRootMember(ex);
			return root?.NodeType == ExpressionType.Parameter;
		}
	}
}
