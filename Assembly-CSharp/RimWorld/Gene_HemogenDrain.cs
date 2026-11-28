using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002081 RID: 8321
	public class Gene_HemogenDrain : Gene, IGeneResourceDrain
	{
		// Token: 0x170025A7 RID: 9639
		// (get) Token: 0x0600D1D7 RID: 53719 RVA: 0x004D1798 File Offset: 0x004CF998
		public Gene_Resource Resource
		{
			get
			{
				if (this.cachedHemogenGene == null || !this.cachedHemogenGene.Active)
				{
					this.cachedHemogenGene = this.pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
				}
				return this.cachedHemogenGene;
			}
		}

		// Token: 0x170025A8 RID: 9640
		// (get) Token: 0x0600D1D8 RID: 53720 RVA: 0x004D17CB File Offset: 0x004CF9CB
		public bool CanOffset
		{
			get
			{
				return this.Active && !this.pawn.Deathresting;
			}
		}

		// Token: 0x170025A9 RID: 9641
		// (get) Token: 0x0600D1D9 RID: 53721 RVA: 0x004D17E5 File Offset: 0x004CF9E5
		public float ResourceLossPerDay
		{
			get
			{
				return this.def.resourceLossPerDay;
			}
		}

		// Token: 0x170025AA RID: 9642
		// (get) Token: 0x0600D1DA RID: 53722 RVA: 0x004D17F2 File Offset: 0x004CF9F2
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170025AB RID: 9643
		// (get) Token: 0x0600D1DB RID: 53723 RVA: 0x004D17FA File Offset: 0x004CF9FA
		public string DisplayLabel
		{
			get
			{
				return this.Label + " (" + "Gene".Translate() + ")";
			}
		}

		// Token: 0x0600D1DC RID: 53724 RVA: 0x004D182A File Offset: 0x004CFA2A
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			GeneResourceDrainUtility.TickResourceDrainInterval(this, delta);
		}

		// Token: 0x0600D1DD RID: 53725 RVA: 0x004D183A File Offset: 0x004CFA3A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Gene_HemogenDrain.<GetGizmos>d__13 <GetGizmos>d__ = new Gene_HemogenDrain.<GetGizmos>d__13(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x04008508 RID: 34056
		[Unsaved(false)]
		private Gene_Hemogen cachedHemogenGene;

		// Token: 0x04008509 RID: 34057
		private const float MinAgeForDrain = 3f;
	}
}
