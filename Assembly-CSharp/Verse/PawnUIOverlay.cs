using System;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200037F RID: 895
	public class PawnUIOverlay
	{
		// Token: 0x06001B42 RID: 6978 RVA: 0x000A83E9 File Offset: 0x000A65E9
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x000A83F8 File Offset: 0x000A65F8
		public void DrawPawnGUIOverlay()
		{
			if (!this.pawn.Spawned || this.pawn.Map.fogGrid.IsFogged(this.pawn.Position) || WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				if (this.pawn.RaceProps.Animal)
				{
					if (!Prefs.AnimalNameMode.ShouldDisplayAnimalName(this.pawn))
					{
						return;
					}
				}
				else
				{
					if (!this.pawn.IsColonyMech)
					{
						return;
					}
					if (this.pawn.IsSelfShutdown())
					{
						this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.SelfShutdown);
					}
					if (!Prefs.MechNameMode.ShouldDisplayMechName(this.pawn))
					{
						return;
					}
				}
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.hideLabel)
			{
				return;
			}
			Vector2 pos = GenMapUI.LabelDrawPosFor(this.pawn, -0.6f);
			GenMapUI.DrawPawnLabel(this.pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
			if (this.pawn.ShouldShowQuestionMark())
			{
				this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.QuestionMark);
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null && lord.CurLordToil != null)
			{
				lord.CurLordToil.DrawPawnGUIOverlay(this.pawn);
			}
		}

		// Token: 0x04001667 RID: 5735
		private Pawn pawn;

		// Token: 0x04001668 RID: 5736
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x04001669 RID: 5737
		private const int PawnStatBarWidth = 32;

		// Token: 0x0400166A RID: 5738
		private const float ActivityIconSize = 13f;

		// Token: 0x0400166B RID: 5739
		private const float ActivityIconOffsetY = 12f;
	}
}
