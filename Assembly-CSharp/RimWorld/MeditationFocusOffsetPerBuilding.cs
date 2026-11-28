using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001902 RID: 6402
	public class MeditationFocusOffsetPerBuilding
	{
		// Token: 0x0600A07C RID: 41084 RVA: 0x00396AE4 File Offset: 0x00394CE4
		public MeditationFocusOffsetPerBuilding()
		{
		}

		// Token: 0x0600A07D RID: 41085 RVA: 0x00396AEC File Offset: 0x00394CEC
		public MeditationFocusOffsetPerBuilding(ThingDef building, float offset)
		{
			this.building = building;
			this.offset = offset;
		}

		// Token: 0x0600A07E RID: 41086 RVA: 0x00396B04 File Offset: 0x00394D04
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.Name != "li")
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "building", xmlRoot, null, null, null);
				this.offset = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "building", xmlRoot.InnerText, null, null, null);
			this.offset = float.MinValue;
		}

		// Token: 0x04005EC9 RID: 24265
		public ThingDef building;

		// Token: 0x04005ECA RID: 24266
		public float offset;
	}
}
