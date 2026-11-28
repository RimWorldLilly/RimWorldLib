using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D5 RID: 5845
	public class PawnExpectationsQualityOffset
	{
		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x06009241 RID: 37441 RVA: 0x00356A4E File Offset: 0x00354C4E
		public string Label
		{
			get
			{
				return this.labelOverride ?? this.expectation.LabelCap;
			}
		}

		// Token: 0x06009242 RID: 37442 RVA: 0x00356A6A File Offset: 0x00354C6A
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "expectation", xmlRoot.Name, null, null, null);
			this.offset = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400589A RID: 22682
		public ExpectationDef expectation;

		// Token: 0x0400589B RID: 22683
		public float offset;

		// Token: 0x0400589C RID: 22684
		[MustTranslate]
		public string labelOverride;
	}
}
