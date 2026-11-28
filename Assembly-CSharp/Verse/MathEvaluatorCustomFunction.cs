using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200009C RID: 156
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00027700 File Offset: 0x00025900
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00027708 File Offset: 0x00025908
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00027715 File Offset: 0x00025915
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00027722 File Offset: 0x00025922
		public XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00027725 File Offset: 0x00025925
		public MathEvaluatorCustomFunction(MathEvaluatorCustomFunctions.FunctionType functionType, XPathResultType[] argTypes)
		{
			this.functionType = functionType;
			this.argTypes = argTypes;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002773B File Offset: 0x0002593B
		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.functionType.func(args);
		}

		// Token: 0x04000259 RID: 601
		private XPathResultType[] argTypes;

		// Token: 0x0400025A RID: 602
		private MathEvaluatorCustomFunctions.FunctionType functionType;
	}
}
