using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190C RID: 6412
	public class CompMetalhorrorInfectible : ThingComp
	{
		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x0600A0B8 RID: 41144 RVA: 0x00397B4C File Offset: 0x00395D4C
		public int Infections
		{
			get
			{
				return this.infectedPawns.Count;
			}
		}

		// Token: 0x0600A0B9 RID: 41145 RVA: 0x00397B59 File Offset: 0x00395D59
		public override void Notify_RecipeProduced(Pawn pawn)
		{
			if (Rand.Chance(0.04f) && MetalhorrorUtility.IsInfected(pawn))
			{
				this.infectedPawns.Add(pawn);
			}
		}

		// Token: 0x0600A0BA RID: 41146 RVA: 0x00397B7C File Offset: 0x00395D7C
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompMetalhorrorInfectible compMetalhorrorInfectible = piece.TryGetComp<CompMetalhorrorInfectible>();
			Pawn item;
			if (this.TryTakeInfector(out item))
			{
				compMetalhorrorInfectible.infectedPawns.Add(item);
			}
		}

		// Token: 0x0600A0BB RID: 41147 RVA: 0x00397BB0 File Offset: 0x00395DB0
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompMetalhorrorInfectible compMetalhorrorInfectible = otherStack.TryGetComp<CompMetalhorrorInfectible>();
			this.infectedPawns.AddRange(compMetalhorrorInfectible.infectedPawns);
		}

		// Token: 0x0600A0BC RID: 41148 RVA: 0x00397BE0 File Offset: 0x00395DE0
		public override void PostIngested(Pawn ingester)
		{
			Pawn source;
			if (this.TryTakeInfector(out source))
			{
				MetalhorrorUtility.Infect(ingester, source, "FoodImplant", null);
			}
		}

		// Token: 0x0600A0BD RID: 41149 RVA: 0x00397C04 File Offset: 0x00395E04
		public override string CompInspectStringExtra()
		{
			if (!DebugSettings.godMode || this.infectedPawns.Empty<Pawn>())
			{
				return null;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Infections: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.infectedPawns.Count);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600A0BE RID: 41150 RVA: 0x00397C58 File Offset: 0x00395E58
		private bool TryTakeInfector(out Pawn infector)
		{
			if (this.Infections <= 0)
			{
				infector = null;
				return false;
			}
			int index = Rand.Range(0, this.infectedPawns.Count);
			Pawn pawn = this.infectedPawns[index];
			this.infectedPawns.RemoveAt(index);
			infector = pawn;
			return true;
		}

		// Token: 0x0600A0BF RID: 41151 RVA: 0x00397CA2 File Offset: 0x00395EA2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.infectedPawns, "infectedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04005EE3 RID: 24291
		private List<Pawn> infectedPawns = new List<Pawn>();

		// Token: 0x04005EE4 RID: 24292
		private const float InfectionChance = 0.04f;
	}
}
