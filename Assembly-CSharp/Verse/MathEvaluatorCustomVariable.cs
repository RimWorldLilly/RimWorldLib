using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200009D RID: 157
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0002774E File Offset: 0x0002594E
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00027751 File Offset: 0x00025951
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00027754 File Offset: 0x00025954
		public XPathResultType VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00027757 File Offset: 0x00025957
		public MathEvaluatorCustomVariable(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002776D File Offset: 0x0002596D
		public object Evaluate(XsltContext xsltContext)
		{
			return ((MathEvaluatorCustomContext)xsltContext).ArgList.GetParam(this.name, this.prefix);
		}

		// Token: 0x0400025B RID: 603
		private string prefix;

		// Token: 0x0400025C RID: 604
		private string name;
	}
}
