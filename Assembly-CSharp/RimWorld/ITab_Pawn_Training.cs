using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D7C RID: 7548
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x17002442 RID: 9282
		// (get) Token: 0x0600C3A1 RID: 50081 RVA: 0x00478FC0 File Offset: 0x004771C0
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.training != null && this.SelPawn.Faction == Faction.OfPlayer && !this.SelPawn.RaceProps.hideTrainingTab && (!this.SelPawn.IsMutant || this.SelPawn.mutant.Def.tameable);
			}
		}

		// Token: 0x0600C3A2 RID: 50082 RVA: 0x00479024 File Offset: 0x00477224
		public ITab_Pawn_Training()
		{
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x0600C3A3 RID: 50083 RVA: 0x00479044 File Offset: 0x00477244
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, this.SelPawn);
		}

		// Token: 0x0600C3A4 RID: 50084 RVA: 0x004790A0 File Offset: 0x004772A0
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = new Vector2(300f, TrainingCardUtility.TotalHeightForPawn(this.SelPawn));
		}
	}
}
