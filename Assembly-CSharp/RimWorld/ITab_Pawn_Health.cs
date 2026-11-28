using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D77 RID: 7543
	public class ITab_Pawn_Health : ITab
	{
		// Token: 0x1700243D RID: 9277
		// (get) Token: 0x0600C38B RID: 50059 RVA: 0x00478388 File Offset: 0x00476588
		private Pawn PawnForHealth
		{
			get
			{
				if (this.SelPawn != null)
				{
					return this.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return null;
			}
		}

		// Token: 0x0600C38C RID: 50060 RVA: 0x004783BB File Offset: 0x004765BB
		public ITab_Pawn_Health()
		{
			this.size = new Vector2(630f, 430f);
			this.labelKey = "TabHealth";
			this.tutorTag = "Health";
		}

		// Token: 0x0600C38D RID: 50061 RVA: 0x004783F0 File Offset: 0x004765F0
		protected override void FillTab()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth == null)
			{
				Log.Error("Health tab found no selected pawn to display.");
				return;
			}
			HealthCardUtility.DrawPawnHealthCard(new Rect(Vector2.zero, this.size), pawnForHealth, this.ShouldAllowOperations(), HealthCardUtility.ShowBloodLoss(base.SelThing), base.SelThing);
		}

		// Token: 0x0600C38E RID: 50062 RVA: 0x00478440 File Offset: 0x00476640
		private bool ShouldAllowOperations()
		{
			Pawn pawn = this.PawnForHealth;
			return !pawn.Dead && base.SelThing.def.AllRecipes.Any((RecipeDef x) => x.AvailableNow && x.AvailableOnNow(pawn, null)) && (!pawn.IsMutant || pawn.mutant.Def.entitledToMedicalCare) && (pawn.Faction == Faction.OfPlayer || (pawn.IsPrisonerOfColony || (pawn.HostFaction == Faction.OfPlayer && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))) || ((!pawn.RaceProps.IsFlesh || pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer)) && (!pawn.RaceProps.Humanlike && pawn.Downed)));
		}

		// Token: 0x04006FB8 RID: 28600
		public const float Width = 630f;
	}
}
