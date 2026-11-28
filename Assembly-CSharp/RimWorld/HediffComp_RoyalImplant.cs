using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002090 RID: 8336
	public class HediffComp_RoyalImplant : HediffComp
	{
		// Token: 0x170025BC RID: 9660
		// (get) Token: 0x0600D224 RID: 53796 RVA: 0x004D32A4 File Offset: 0x004D14A4
		public HediffCompProperties_RoyalImplant Props
		{
			get
			{
				return (HediffCompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x0600D225 RID: 53797 RVA: 0x004D32B4 File Offset: 0x004D14B4
		public static int GetImplantLevel(Hediff implant)
		{
			Hediff_Level hediff_Level = implant as Hediff_Level;
			if (hediff_Level != null)
			{
				return hediff_Level.level;
			}
			return 0;
		}

		// Token: 0x0600D226 RID: 53798 RVA: 0x004D32D3 File Offset: 0x004D14D3
		public bool IsViolatingRulesOf(Faction faction, int violationSourceLevel = -1)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(base.Def, this.parent.pawn, faction, (violationSourceLevel == -1) ? HediffComp_RoyalImplant.GetImplantLevel(this.parent) : violationSourceLevel);
		}

		// Token: 0x0600D227 RID: 53799 RVA: 0x004D3300 File Offset: 0x004D1500
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, -1);
			if (this.parent.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (!Rand.Chance(detectionChance))
			{
				return;
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (this.IsViolatingRulesOf(faction, violationSourceLevel))
				{
					faction.Notify_RoyalThingUseViolation(this.parent.def, base.Pawn, violationSourceName, detectionChance, violationSourceLevel);
				}
			}
		}
	}
}
