using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200009A RID: 154
	public class MathEvaluatorCustomContext : XsltContext
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000271D9 File Offset: 0x000253D9
		public override bool Whitespace
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x000271DC File Offset: 0x000253DC
		public XsltArgumentList ArgList
		{
			get
			{
				return this.argList;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000271E4 File Offset: 0x000253E4
		public MathEvaluatorCustomContext()
		{
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000271EC File Offset: 0x000253EC
		public MathEvaluatorCustomContext(NameTable nt, XsltArgumentList args) : base(nt)
		{
			this.argList = args;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000271FC File Offset: 0x000253FC
		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
		{
			MathEvaluatorCustomFunctions.FunctionType[] functionTypes = MathEvaluatorCustomFunctions.FunctionTypes;
			for (int i = 0; i < functionTypes.Length; i++)
			{
				if (functionTypes[i].name == name)
				{
					return new MathEvaluatorCustomFunction(functionTypes[i], argTypes);
				}
			}
			return null;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00027238 File Offset: 0x00025438
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			if (this.ArgList.GetParam(name, prefix) != null)
			{
				return new MathEvaluatorCustomVariable(prefix, name);
			}
			return null;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00027252 File Offset: 0x00025452
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return false;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00027255 File Offset: 0x00025455
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return 0;
		}

		// Token: 0x04000257 RID: 599
		private XsltArgumentList argList;
	}
}
