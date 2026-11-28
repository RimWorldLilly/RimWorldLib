using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001850 RID: 6224
	public class CompChimera : ThingComp
	{
		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x06009BD3 RID: 39891 RVA: 0x003827C1 File Offset: 0x003809C1
		public CompProperties_Chimera Props
		{
			get
			{
				return (CompProperties_Chimera)this.props;
			}
		}

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x06009BD4 RID: 39892 RVA: 0x003827CE File Offset: 0x003809CE
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x06009BD5 RID: 39893 RVA: 0x003827DC File Offset: 0x003809DC
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Pawn.Dead)
			{
				return;
			}
			this.totalDamageTaken += totalDamageDealt;
			if (!this.Pawn.Dead && this.totalDamageTaken > 0f && !this.Pawn.health.hediffSet.HasHediff(HediffDefOf.RageSpeed, false) && !this.Pawn.health.Downed)
			{
				this.Pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.RageSpeed, this.Pawn, null), null, null, null);
				if (this.Pawn.Spawned)
				{
					EffecterDefOf.ChimeraRage.Spawn(this.Pawn.Position, this.Pawn.Map, 1f).Cleanup();
				}
			}
		}

		// Token: 0x06009BD6 RID: 39894 RVA: 0x003828B8 File Offset: 0x00380AB8
		public override void Notify_Downed()
		{
			Hediff firstHediffOfDef = this.Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.RageSpeed, false);
			if (firstHediffOfDef != null)
			{
				this.Pawn.health.RemoveHediff(firstHediffOfDef);
			}
		}

		// Token: 0x06009BD7 RID: 39895 RVA: 0x003828F8 File Offset: 0x00380AF8
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.Pawn.Dead)
			{
				return;
			}
			if (this.Pawn.health.summaryHealth.SummaryHealthPercent >= this.Props.rageEndHealthPercentThreshold)
			{
				Hediff firstHediffOfDef = this.Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.RageSpeed, false);
				if (firstHediffOfDef != null)
				{
					this.Pawn.health.RemoveHediff(firstHediffOfDef);
					this.totalDamageTaken = 0f;
				}
			}
		}

		// Token: 0x06009BD8 RID: 39896 RVA: 0x00382976 File Offset: 0x00380B76
		public Thought_MemoryObservation GiveObservedThought(Pawn observer)
		{
			Thought_MemoryObservation_Chimera thought_MemoryObservation_Chimera = (Thought_MemoryObservation_Chimera)ThoughtMaker.MakeThought(ThoughtDefOf.Chimera);
			thought_MemoryObservation_Chimera.Target = this.Pawn;
			return thought_MemoryObservation_Chimera;
		}

		// Token: 0x06009BD9 RID: 39897 RVA: 0x00382993 File Offset: 0x00380B93
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompChimera.<CompGetGizmosExtra>d__9 <CompGetGizmosExtra>d__ = new CompChimera.<CompGetGizmosExtra>d__9(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009BDA RID: 39898 RVA: 0x003829A3 File Offset: 0x00380BA3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.totalDamageTaken, "totalDamageTaken", 0f, false);
		}

		// Token: 0x04005CAA RID: 23722
		private float totalDamageTaken;
	}
}
