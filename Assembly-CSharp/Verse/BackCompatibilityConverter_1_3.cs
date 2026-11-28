using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000688 RID: 1672
	public class BackCompatibilityConverter_1_3 : BackCompatibilityConverter
	{
		// Token: 0x060034E0 RID: 13536 RVA: 0x001576BE File Offset: 0x001558BE
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 3);
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x001576D4 File Offset: 0x001558D4
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(JobDef) && defName == "TriggerFirefoamPopper")
			{
				return "TriggerObject";
			}
			if (defType == typeof(TerrainDef) && defName == "CarpetDark")
			{
				return "CarpetGreyDark";
			}
			return null;
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x0015772C File Offset: 0x0015592C
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x00157730 File Offset: 0x00155930
		public override void PostExposeData(object obj)
		{
			Pawn pawn = obj as Pawn;
			if (pawn != null && pawn.RaceProps.Humanlike && pawn.genes == null)
			{
				pawn.genes = new Pawn_GeneTracker(pawn);
			}
		}
	}
}
