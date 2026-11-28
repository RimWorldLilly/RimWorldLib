using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB3 RID: 7603
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Bond : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5A6 RID: 50598 RVA: 0x004891EC File Offset: 0x004873EC
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (!allColonistBondsFor.Any<Pawn>())
			{
				return null;
			}
			if (allColonistBondsFor.Any((Pawn bond) => bond == pawn.playerSettings.Master))
			{
				return PawnColumnWorker_Bond.BondIcon;
			}
			return PawnColumnWorker_Bond.BondBrokenIcon;
		}

		// Token: 0x0600C5A7 RID: 50599 RVA: 0x0048923B File Offset: 0x0048743B
		protected override string GetIconTip(Pawn pawn)
		{
			return TrainableUtility.GetIconTooltipText(pawn);
		}

		// Token: 0x0600C5A8 RID: 50600 RVA: 0x00489244 File Offset: 0x00487444
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetCompareValueFor(a).CompareTo(this.GetCompareValueFor(b));
		}

		// Token: 0x0600C5A9 RID: 50601 RVA: 0x00489268 File Offset: 0x00487468
		public int GetCompareValueFor(Pawn a)
		{
			Texture2D iconFor = this.GetIconFor(a);
			if (iconFor == null)
			{
				return 0;
			}
			if (iconFor == PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return 1;
			}
			if (iconFor == PawnColumnWorker_Bond.BondIcon)
			{
				return 2;
			}
			Log.ErrorOnce("Unknown bond type when trying to sort", 20536378);
			return 0;
		}

		// Token: 0x0600C5AA RID: 50602 RVA: 0x004892B8 File Offset: 0x004874B8
		protected override void PaintedIcon(Pawn pawn)
		{
			if (this.GetIconFor(pawn) != PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return;
			}
			if (!pawn.training.HasLearned(TrainableDefOf.Obedience))
			{
				return;
			}
			pawn.playerSettings.Master = (from master in TrainableUtility.GetAllColonistBondsFor(pawn)
			where TrainableUtility.CanBeMaster(master, pawn, true)
			select master).FirstOrDefault<Pawn>();
		}

		// Token: 0x040070E0 RID: 28896
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x040070E1 RID: 28897
		private static readonly Texture2D BondBrokenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/BondBroken", true);
	}
}
