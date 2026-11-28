using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2F RID: 3887
	public class SkillGain
	{
		// Token: 0x06006475 RID: 25717 RVA: 0x0023E2B3 File Offset: 0x0023C4B3
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.amount = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
			}
		}

		// Token: 0x040039AD RID: 14765
		public SkillDef skill;

		// Token: 0x040039AE RID: 14766
		public int amount;
	}
}
