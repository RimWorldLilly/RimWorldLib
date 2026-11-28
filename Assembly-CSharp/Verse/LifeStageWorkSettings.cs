using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200014D RID: 333
	public class LifeStageWorkSettings
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x00038BE3 File Offset: 0x00036DE3
		public bool IsDisabled(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears < this.minAge;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00038BF8 File Offset: 0x00036DF8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "workType", xmlRoot.Name, null, null, null);
			this.minAge = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04000680 RID: 1664
		public WorkTypeDef workType;

		// Token: 0x04000681 RID: 1665
		public int minAge;
	}
}
