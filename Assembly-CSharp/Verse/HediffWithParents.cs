using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200049E RID: 1182
	public class HediffWithParents : HediffWithComps
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x000C40A1 File Offset: 0x000C22A1
		public Pawn Father
		{
			get
			{
				return this.father;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x000C40A9 File Offset: 0x000C22A9
		public Pawn Mother
		{
			get
			{
				return this.mother;
			}
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000C40B1 File Offset: 0x000C22B1
		public void SetParents(Pawn mother, Pawn father, GeneSet geneSet)
		{
			this.mother = mother;
			this.father = father;
			this.geneSet = geneSet;
			Find.WorldPawns.AddPreservedPawnHediff(mother, this);
			Find.WorldPawns.AddPreservedPawnHediff(father, this);
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000C40E0 File Offset: 0x000C22E0
		public override void PreRemoved()
		{
			base.PreRemoved();
			Find.WorldPawns.RemovePreservedPawnHediff(this.mother, this);
			Find.WorldPawns.RemovePreservedPawnHediff(this.father, this);
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000C410A File Offset: 0x000C230A
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			HediffWithParents.<SpecialDisplayStats>d__9 <SpecialDisplayStats>d__ = new HediffWithParents.<SpecialDisplayStats>d__9(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000C4124 File Offset: 0x000C2324
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			HediffWithParents hediffWithParents = other as HediffWithParents;
			if (hediffWithParents != null)
			{
				Pawn pawn = hediffWithParents.Mother;
				if (hediffWithParents.Mother == hediffWithParents.pawn)
				{
					pawn = this.pawn;
				}
				Pawn pawn2 = pawn;
				Pawn pawn3 = hediffWithParents.Father;
				GeneSet geneSet = hediffWithParents.geneSet;
				this.SetParents(pawn2, pawn3, (geneSet != null) ? geneSet.Copy() : null);
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000C417D File Offset: 0x000C237D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			HediffWithParents.<GetGizmos>d__11 <GetGizmos>d__ = new HediffWithParents.<GetGizmos>d__11(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000C4190 File Offset: 0x000C2390
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", true);
			Scribe_References.Look<Pawn>(ref this.mother, "mother", true);
			Scribe_Deep.Look<GeneSet>(ref this.geneSet, "geneSet", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Find.WorldPawns.AddPreservedPawnHediff(this.mother, this);
				Find.WorldPawns.AddPreservedPawnHediff(this.father, this);
			}
		}

		// Token: 0x0400192D RID: 6445
		private Pawn father;

		// Token: 0x0400192E RID: 6446
		private Pawn mother;

		// Token: 0x0400192F RID: 6447
		public GeneSet geneSet;
	}
}
