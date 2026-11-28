using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A13 RID: 6675
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		// Token: 0x17001D6C RID: 7532
		// (get) Token: 0x0600A7E2 RID: 42978 RVA: 0x003BBB73 File Offset: 0x003B9D73
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		// Token: 0x0600A7E3 RID: 42979 RVA: 0x003BBB80 File Offset: 0x003B9D80
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = target as Pawn;
			Faction faction = (pawn != null) ? pawn.HomeFaction : target.Faction;
			if (user.Faction == Faction.OfPlayer && faction != null && !faction.HostileTo(user.Faction) && (pawn == null || !pawn.IsSlaveOfColony))
			{
				Faction.OfPlayer.TryAffectGoodwillWith(faction, this.PropsGoodwillImpact.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulItem, null);
			}
		}
	}
}
