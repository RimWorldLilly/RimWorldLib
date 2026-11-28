using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043C RID: 1084
	public class HediffComp_GiveHediffsInRange : HediffComp
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x000BE86E File Offset: 0x000BCA6E
		public HediffCompProperties_GiveHediffsInRange Props
		{
			get
			{
				return (HediffCompProperties_GiveHediffsInRange)this.props;
			}
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000BE87C File Offset: 0x000BCA7C
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.parent.pawn.Awake() || this.parent.pawn.health == null || this.parent.pawn.health.InPainShock || !this.parent.pawn.Spawned)
			{
				return;
			}
			if (!this.Props.hideMoteWhenNotDrafted || this.parent.pawn.Drafted)
			{
				if (this.Props.mote != null && (this.mote == null || this.mote.Destroyed))
				{
					this.mote = MoteMaker.MakeAttachedOverlay(this.parent.pawn, this.Props.mote, Vector3.zero, 1f, -1f);
				}
				if (this.mote != null)
				{
					this.mote.Maintain();
				}
			}
			IReadOnlyList<Pawn> readOnlyList;
			if (this.Props.onlyPawnsInSameFaction && this.parent.pawn.Faction != null)
			{
				readOnlyList = this.parent.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.parent.pawn.Faction);
			}
			else
			{
				readOnlyList = this.parent.pawn.Map.mapPawns.AllPawnsSpawned;
			}
			foreach (Pawn pawn in readOnlyList)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Dead && pawn.health != null && pawn != this.parent.pawn && pawn.Position.DistanceTo(this.parent.pawn.Position) <= this.Props.range && this.Props.targetingParameters.CanTarget(pawn, null))
				{
					Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
					if (hediff == null)
					{
						hediff = pawn.health.AddHediff(this.Props.hediff, pawn.health.hediffSet.GetBrain(), null, null);
						hediff.Severity = this.Props.initialSeverity;
						HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
						if (hediffComp_Link != null)
						{
							hediffComp_Link.drawConnection = true;
							hediffComp_Link.other = this.parent.pawn;
						}
					}
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears == null)
					{
						Log.Error("HediffComp_GiveHediffsInRange has a hediff in props which does not have a HediffComp_Disappears");
					}
					else
					{
						hediffComp_Disappears.ticksToDisappear = 5;
					}
				}
			}
		}

		// Token: 0x04001876 RID: 6262
		private Mote mote;
	}
}
