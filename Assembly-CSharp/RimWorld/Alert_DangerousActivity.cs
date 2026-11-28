using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA8 RID: 7080
	public class Alert_DangerousActivity : Alert_Critical
	{
		// Token: 0x170020DD RID: 8413
		// (get) Token: 0x0600B5BA RID: 46522 RVA: 0x0040013E File Offset: 0x003FE33E
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020DE RID: 8414
		// (get) Token: 0x0600B5BB RID: 46523 RVA: 0x00400144 File Offset: 0x003FE344
		private List<Thing> HighActivity
		{
			get
			{
				this.highActivity.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Suppressable))
					{
						ThingWithComps thingWithComps = (ThingWithComps)thing;
						if (Alert_DangerousActivity.DangerousActivity(thingWithComps))
						{
							this.highActivity.Add(thingWithComps);
						}
					}
					foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
					{
						Building_HoldingPlatform building_HoldingPlatform = thing2 as Building_HoldingPlatform;
						if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied && Alert_DangerousActivity.DangerousActivity(building_HoldingPlatform.HeldPawn))
						{
							this.highActivity.Add(building_HoldingPlatform.HeldPawn);
						}
					}
				}
				return this.highActivity;
			}
		}

		// Token: 0x0600B5BC RID: 46524 RVA: 0x00400278 File Offset: 0x003FE478
		public Alert_DangerousActivity()
		{
			this.defaultLabel = "ActivityMultipleDangerous".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B5BD RID: 46525 RVA: 0x004002A8 File Offset: 0x003FE4A8
		private static bool DangerousActivity(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			return comp != null && comp.ActivityLevel > comp.Props.warning && comp.State == ActivityState.Passive;
		}

		// Token: 0x0600B5BE RID: 46526 RVA: 0x004002E0 File Offset: 0x003FE4E0
		public override string GetLabel()
		{
			if (this.highActivity.Count == 1)
			{
				return this.highActivity[0].LabelNoParenthesisCap + ": " + this.highActivity[0].TryGetComp<CompActivity>().ActivityLevel.ToStringPercent("0");
			}
			return this.defaultLabel;
		}

		// Token: 0x0600B5BF RID: 46527 RVA: 0x00400340 File Offset: 0x003FE540
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Thing thing in this.highActivity)
			{
				CompActivity compActivity = thing.TryGetComp<CompActivity>();
				stringBuilder.AppendLine("  - " + thing.LabelNoParenthesisCap + ": " + compActivity.ActivityLevel.ToStringPercent("0"));
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ActivityDangerousDesc".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(":\n");
			defaultInterpolatedStringHandler.AppendFormatted<StringBuilder>(stringBuilder);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ActivityDangerousDescAppended".Translate());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600B5C0 RID: 46528 RVA: 0x0040041C File Offset: 0x003FE61C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HighActivity);
		}

		// Token: 0x04006887 RID: 26759
		private readonly List<Thing> highActivity = new List<Thing>();
	}
}
