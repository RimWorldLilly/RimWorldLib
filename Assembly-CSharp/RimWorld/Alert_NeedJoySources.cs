using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEA RID: 7146
	public class Alert_NeedJoySources : Alert
	{
		// Token: 0x0600B6D3 RID: 46803 RVA: 0x00406BEE File Offset: 0x00404DEE
		public Alert_NeedJoySources()
		{
			this.defaultLabel = "NeedJoySource".Translate();
		}

		// Token: 0x0600B6D4 RID: 46804 RVA: 0x00406C0C File Offset: 0x00404E0C
		public override TaggedString GetExplanation()
		{
			Map map = this.badMap;
			int value = JoyUtility.JoyKindsOnMapCount(map);
			string label = map.info.parent.Label;
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			int joyKindsNeeded = expectationDef.joyKindsNeeded;
			string value2 = "AvailableRecreationTypes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + JoyUtility.JoyKindsOnMapString(map);
			string value3 = "MissingRecreationTypes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + JoyUtility.JoyKindsNotOnMapString(map);
			return "NeedJoySourceDesc".Translate(value, label, expectationDef.label, joyKindsNeeded, value2, value3);
		}

		// Token: 0x0600B6D5 RID: 46805 RVA: 0x00406CC9 File Offset: 0x00404EC9
		public override AlertReport GetReport()
		{
			return this.BadMap() != null;
		}

		// Token: 0x0600B6D6 RID: 46806 RVA: 0x00406CDC File Offset: 0x00404EDC
		private Map BadMap()
		{
			this.badMap = null;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedJoySource(maps[i]))
				{
					this.badMap = maps[i];
					break;
				}
			}
			return this.badMap;
		}

		// Token: 0x0600B6D7 RID: 46807 RVA: 0x00406D2C File Offset: 0x00404F2C
		private bool NeedJoySource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			int num = JoyUtility.JoyKindsOnMapCount(map);
			int joyKindsNeeded = ExpectationsUtility.CurrentExpectationFor(map).joyKindsNeeded;
			return num < joyKindsNeeded;
		}

		// Token: 0x040068F1 RID: 26865
		private Map badMap;
	}
}
