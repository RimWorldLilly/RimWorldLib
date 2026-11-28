using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200086B RID: 2155
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x0600438B RID: 17291 RVA: 0x001A9956 File Offset: 0x001A7B56
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x001A996C File Offset: 0x001A7B6C
		public override bool ForceHostileTo(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.Roamer)
			{
				return false;
			}
			if (t.Faction == null)
			{
				Pawn pawn2 = t as Pawn;
				return pawn2 != null && pawn2.RaceProps.Humanlike;
			}
			return this.ForceHostileTo(t.Faction);
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x001A99B9 File Offset: 0x001A7BB9
		public override bool ForceHostileTo(Faction f)
		{
			return (ModsConfig.AnomalyActive && f == Faction.OfEntities) || f.def.humanlikeFaction || f == Faction.OfMechanoids;
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x001A99E3 File Offset: 0x001A7BE3
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
