using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200014E RID: 334
	public class MechWorkTypePriority
	{
		// Token: 0x06000A2E RID: 2606 RVA: 0x00038C2C File Offset: 0x00036E2C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.Name, null, null, null);
			this.priority = (xmlRoot.HasChildNodes ? ParseHelper.FromString<int>(xmlRoot.FirstChild.Value) : 3);
		}

		// Token: 0x04000682 RID: 1666
		public WorkTypeDef def;

		// Token: 0x04000683 RID: 1667
		public int priority;
	}
}
