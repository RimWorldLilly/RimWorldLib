using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAF RID: 7087
	public class Alert_InsufficientContainmentStrength : Alert_Critical
	{
		// Token: 0x0600B5E2 RID: 46562 RVA: 0x00400F31 File Offset: 0x003FF131
		public Alert_InsufficientContainmentStrength()
		{
			this.defaultLabel = "Alert_InsufficientContainment".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B5E3 RID: 46563 RVA: 0x00400F60 File Offset: 0x003FF160
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B5E4 RID: 46564 RVA: 0x00400F84 File Offset: 0x003FF184
		public override TaggedString GetExplanation()
		{
			string value = (from x in this.targets
			select x.NameShortColored.Resolve()).ToLineList("  - ", true);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Alert_InsufficientContainmentDesc".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(":\n");
			defaultInterpolatedStringHandler.AppendFormatted(value);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Alert_InsufficientContainmentDescAppended".Translate());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600B5E5 RID: 46565 RVA: 0x00401020 File Offset: 0x003FF220
		private void GetTargets()
		{
			this.targets.Clear();
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder);
			for (int i = 0; i < list.Count; i++)
			{
				Building_HoldingPlatform building_HoldingPlatform = list[i] as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.HeldPawn != null && !building_HoldingPlatform.SafelyContains(building_HoldingPlatform.HeldPawn) && ContainmentUtility.InitiateEscapeMtbDays(building_HoldingPlatform.HeldPawn, null) > 0f)
				{
					this.targets.Add(building_HoldingPlatform.HeldPawn);
				}
			}
		}

		// Token: 0x04006892 RID: 26770
		private List<Pawn> targets = new List<Pawn>();
	}
}
