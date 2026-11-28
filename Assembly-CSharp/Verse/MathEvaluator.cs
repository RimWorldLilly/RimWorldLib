using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000099 RID: 153
	public static class MathEvaluator
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0002706F File Offset: 0x0002526F
		private static XPathNavigator Navigator
		{
			get
			{
				if (MathEvaluator.doc == null)
				{
					MathEvaluator.doc = new XPathDocument(new StringReader("<root />"));
				}
				if (MathEvaluator.navigator == null)
				{
					MathEvaluator.navigator = MathEvaluator.doc.CreateNavigator();
				}
				return MathEvaluator.navigator;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000270A8 File Offset: 0x000252A8
		public static double Evaluate(string expr)
		{
			if (expr.NullOrEmpty())
			{
				return 0.0;
			}
			expr = MathEvaluator.AddSpacesRegex.Replace(expr, " ${1} ");
			expr = expr.Replace("/", " div ");
			expr = expr.Replace("%", " mod ");
			double result;
			try
			{
				XPathExpression xpathExpression = XPathExpression.Compile("number(" + expr + ")");
				xpathExpression.SetContext(MathEvaluator.Context);
				double num = (double)MathEvaluator.Navigator.Evaluate(xpathExpression);
				if (double.IsNaN(num))
				{
					Log.ErrorOnce("Expression \"" + expr + "\" evaluated to NaN.", expr.GetHashCode() ^ 48337162);
					num = 0.0;
				}
				result = num;
			}
			catch (XPathException ex)
			{
				string str = "Could not evaluate expression \"";
				string str2 = expr;
				string str3 = "\". Error: ";
				XPathException ex2 = ex;
				Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), expr.GetHashCode() ^ 980986121);
				result = 0.0;
			}
			return result;
		}

		// Token: 0x04000253 RID: 595
		private static XPathDocument doc;

		// Token: 0x04000254 RID: 596
		private static XPathNavigator navigator;

		// Token: 0x04000255 RID: 597
		private static readonly Regex AddSpacesRegex = new Regex("([\\+\\-\\*])");

		// Token: 0x04000256 RID: 598
		private static readonly MathEvaluatorCustomContext Context = new MathEvaluatorCustomContext(new NameTable(), new XsltArgumentList());
	}
}
