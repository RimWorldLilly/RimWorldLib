using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000128 RID: 296
	public class CreepJoinerBenefitDef : CreepJoinerBaseDef
	{
		// Token: 0x04000519 RID: 1305
		[MustTranslate]
		public string letterExtra;

		// Token: 0x0400051A RID: 1306
		public List<BackstoryTrait> traits = new List<BackstoryTrait>();

		// Token: 0x0400051B RID: 1307
		public List<CreepJoinerBenefitDef.SkillValue> skills = new List<CreepJoinerBenefitDef.SkillValue>();

		// Token: 0x0400051C RID: 1308
		public List<HediffDef> hediffs = new List<HediffDef>();

		// Token: 0x0400051D RID: 1309
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x02002524 RID: 9508
		public class SkillValue
		{
			// Token: 0x0600ED30 RID: 60720 RVA: 0x00576BC7 File Offset: 0x00574DC7
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
				this.range = (xmlRoot.HasChildNodes ? ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value) : IntRange.Zero);
			}

			// Token: 0x040097EC RID: 38892
			public SkillDef skill;

			// Token: 0x040097ED RID: 38893
			public IntRange range;
		}
	}
}
