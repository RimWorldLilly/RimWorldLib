using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200208E RID: 8334
	public class Thought_PsychicHarmonizer : Thought_Memory
	{
		// Token: 0x170025BA RID: 9658
		// (get) Token: 0x0600D21C RID: 53788 RVA: 0x004D308C File Offset: 0x004D128C
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.harmonizer.pawn.Named("HARMONIZER")).CapitalizeFirst();
			}
		}

		// Token: 0x0600D21D RID: 53789 RVA: 0x004D30CB File Offset: 0x004D12CB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Hediff>(ref this.harmonizer, "harmonizer", false);
		}

		// Token: 0x0600D21E RID: 53790 RVA: 0x004D30E4 File Offset: 0x004D12E4
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			float num = base.MoodOffset();
			float num2 = Mathf.Lerp(-1f, 1f, this.harmonizer.pawn.needs.mood.CurLevel);
			float statValue = this.harmonizer.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, 1);
			return num * num2 * statValue;
		}

		// Token: 0x0600D21F RID: 53791 RVA: 0x004D3164 File Offset: 0x004D1364
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x0600D220 RID: 53792 RVA: 0x004D316C File Offset: 0x004D136C
		public override bool GroupsWith(Thought other)
		{
			Thought_PsychicHarmonizer thought_PsychicHarmonizer = other as Thought_PsychicHarmonizer;
			return thought_PsychicHarmonizer != null && thought_PsychicHarmonizer.harmonizer == this.harmonizer && base.GroupsWith(other);
		}

		// Token: 0x170025BB RID: 9659
		// (get) Token: 0x0600D221 RID: 53793 RVA: 0x004D319C File Offset: 0x004D139C
		public override bool ShouldDiscard
		{
			get
			{
				Hediff hediff = this.harmonizer;
				Pawn pawn = (hediff != null) ? hediff.pawn : null;
				if (pawn == null)
				{
					return true;
				}
				if (!pawn.health.Dead)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.mood : null) != null)
					{
						return this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false) || ((pawn.Spawned || this.pawn.Spawned || pawn.GetCaravan() != this.pawn.GetCaravan()) && (!pawn.Spawned || !this.pawn.Spawned || pawn.Map != this.pawn.Map || pawn.Position.DistanceTo(this.pawn.Position) > this.harmonizer.TryGetComp<HediffComp_PsychicHarmonizer>().Props.range));
					}
				}
				return true;
			}
		}

		// Token: 0x04008525 RID: 34085
		public Hediff harmonizer;
	}
}
