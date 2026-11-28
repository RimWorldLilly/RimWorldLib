using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DD0 RID: 7632
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
		// Token: 0x0600C652 RID: 50770 RVA: 0x0048AF03 File Offset: 0x00489103
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 100);
		}

		// Token: 0x0600C653 RID: 50771 RVA: 0x0048AF13 File Offset: 0x00489113
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(170, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C654 RID: 50772 RVA: 0x0048AF2D File Offset: 0x0048912D
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x0600C655 RID: 50773 RVA: 0x0048AF3D File Offset: 0x0048913D
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.CanAssignMaster(pawn))
			{
				return;
			}
			TrainableUtility.MasterSelectButton(rect.ContractedBy(2f), pawn, true);
		}

		// Token: 0x0600C656 RID: 50774 RVA: 0x0048AF5C File Offset: 0x0048915C
		public override int Compare(Pawn a, Pawn b)
		{
			int valueToCompare = this.GetValueToCompare1(a);
			int valueToCompare2 = this.GetValueToCompare1(b);
			if (valueToCompare != valueToCompare2)
			{
				return valueToCompare.CompareTo(valueToCompare2);
			}
			return this.GetValueToCompare2(a).CompareTo(this.GetValueToCompare2(b));
		}

		// Token: 0x0600C657 RID: 50775 RVA: 0x0048AF99 File Offset: 0x00489199
		private bool CanAssignMaster(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600C658 RID: 50776 RVA: 0x0048AFCC File Offset: 0x004891CC
		private int GetValueToCompare1(Pawn pawn)
		{
			if (!this.CanAssignMaster(pawn))
			{
				return 0;
			}
			if (pawn.playerSettings.Master == null)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x0600C659 RID: 50777 RVA: 0x0048AFE9 File Offset: 0x004891E9
		private string GetValueToCompare2(Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
			{
				return pawn.playerSettings.Master.Label;
			}
			return "";
		}
	}
}
