using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200068A RID: 1674
	public class BackCompatibilityConverter_1_5 : BackCompatibilityConverter
	{
		// Token: 0x060034EA RID: 13546 RVA: 0x001579B7 File Offset: 0x00155BB7
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 5);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x001579CC File Offset: 0x00155BCC
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef) && defName == "DropPodIncomingMechanoid")
			{
				return "DropPodIncomingMechanoidRapid";
			}
			if (defType == typeof(ThingDef) && defName == "AncientContainer")
			{
				return "AncientLargeContainer";
			}
			if (defType == typeof(IssueDef) && defName == "SmallSpaces")
			{
				return "Indoors";
			}
			if (defType == typeof(PreceptDef) && defName == "SmallSpaces_Acceptable")
			{
				return "Indoors_Acceptable";
			}
			return null;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x00157A6E File Offset: 0x00155C6E
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (baseType == typeof(Area) && providedClassName == "Area_SnowClear")
			{
				return typeof(Area_SnowOrSandClear);
			}
			return null;
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x00157A9C File Offset: 0x00155C9C
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				WorldGenData worldGenData = obj as WorldGenData;
				if (worldGenData != null)
				{
					List<int> list = new List<int>();
					Scribe_Collections.Look<int>(ref list, "roadNodes", LookMode.Undefined, Array.Empty<object>());
					List<PlanetTile> list2 = worldGenData.roadNodes[Find.WorldGrid.Surface] = new List<PlanetTile>();
					foreach (int tileId in list)
					{
						list2.Add(new PlanetTile(tileId, Find.WorldGrid.Surface));
					}
				}
				Quest quest = obj as Quest;
				if (quest != null)
				{
					int num = -1;
					Scribe_Values.Look<int>(ref num, "ticksUntilAcceptanceExpiry", -1, false);
					if (num >= 0)
					{
						quest.acceptanceExpireTick = GenTicks.TicksGame + num;
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && obj is WorldInfo)
			{
				WorldGenStep_Mutators.AddMutatorsFromTile(Find.WorldGrid.Surface);
			}
		}
	}
}
