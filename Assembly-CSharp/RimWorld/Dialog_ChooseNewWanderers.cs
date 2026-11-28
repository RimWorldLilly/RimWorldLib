using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C9D RID: 7325
	public class Dialog_ChooseNewWanderers : Window
	{
		// Token: 0x17002209 RID: 8713
		// (get) Token: 0x0600BB59 RID: 47961 RVA: 0x004261A8 File Offset: 0x004243A8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1020f, 764f);
			}
		}

		// Token: 0x1700220A RID: 8714
		// (get) Token: 0x0600BB5A RID: 47962 RVA: 0x004261B9 File Offset: 0x004243B9
		private static List<Pawn> StartingAndOptionalPawns
		{
			get
			{
				return Find.GameInitData.startingAndOptionalPawns;
			}
		}

		// Token: 0x1700220B RID: 8715
		// (get) Token: 0x0600BB5B RID: 47963 RVA: 0x004261C8 File Offset: 0x004243C8
		private static PawnGenerationRequest DefaultStartingPawnRequest
		{
			get
			{
				PawnKindDef basicMemberKind = Faction.OfPlayer.def.basicMemberKind;
				Faction ofPlayer = Faction.OfPlayer;
				PawnGenerationContext context = PawnGenerationContext.PlayerStarter;
				FloatRange? excludeBiologicalAgeRange = ModsConfig.BiotechActive ? new FloatRange?(Dialog_ChooseNewWanderers.ExcludeBiologicalAgeRange) : null;
				return new PawnGenerationRequest(basicMemberKind, ofPlayer, context, null, true, false, false, true, false, 50f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, excludeBiologicalAgeRange, null, false, false, false, -1, 0, false);
			}
		}

		// Token: 0x0600BB5C RID: 47964 RVA: 0x00426285 File Offset: 0x00424485
		public Dialog_ChooseNewWanderers() : base(null)
		{
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BB5D RID: 47965 RVA: 0x004262A4 File Offset: 0x004244A4
		public override void PreOpen()
		{
			base.PreOpen();
			Current.Game.InitData = new GameInitData();
			Find.GameInitData.startingPawnCount = 3;
			StartingPawnUtility.ClearAllStartingPawns();
			this.generationIndex = 0;
			while (Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count < 3)
			{
				StartingPawnUtility.SetGenerationRequest(this.generationIndex, Dialog_ChooseNewWanderers.DefaultStartingPawnRequest);
				StartingPawnUtility.AddNewPawn(this.generationIndex);
				this.generationIndex++;
			}
			this.curPawnIndex = 0;
		}

		// Token: 0x0600BB5E RID: 47966 RVA: 0x0042631C File Offset: 0x0042451C
		public override void DoWindowContents(Rect rect)
		{
			if (!base.IsOpen)
			{
				return;
			}
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect2.width, 45f), "ChooseNewWanderers".Translate());
			Text.Font = GameFont.Small;
			rect2.yMin += 45f;
			Rect rect3 = new Rect(rect.width - Dialog_ChooseNewWanderers.ConfirmButtonSize.x, rect.height - Dialog_ChooseNewWanderers.ConfirmButtonSize.y, Dialog_ChooseNewWanderers.ConfirmButtonSize.x, Dialog_ChooseNewWanderers.ConfirmButtonSize.y);
			if (Widgets.ButtonText(rect3, "Confirm".Translate(), true, true, true, null))
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.AnyPlayerHomeMap;
				incidentParms.forced = true;
				Find.Storyteller.TryFire(new FiringIncident(IncidentDefOf.GameEndedWanderersJoin, null, incidentParms), false);
				Find.LetterStack.RemoveLetter(Find.LetterStack.LettersListForReading.Find((Letter letter) => letter.def == LetterDefOf.GameEnded));
				Find.GameEnder.gameEnding = false;
				Find.GameEnder.newWanderersCreatedTick = Find.TickManager.TicksGame;
				this.Close(true);
				Current.Game.InitData = null;
				return;
			}
			rect2.yMax -= rect3.height + 10f;
			Rect rect4 = rect2;
			rect4.width = 140f;
			this.DrawPawnList(rect4);
			Rect rect5 = rect2;
			rect5.xMin += 150f;
			Rect rect6 = rect5.BottomPartPixels(127f);
			rect5.yMax = rect6.yMin - 10f;
			StartingPawnUtility.DrawPortraitArea(rect5, this.curPawnIndex, true, true);
			StartingPawnUtility.DrawSkillSummaries(rect6);
		}

		// Token: 0x0600BB5F RID: 47967 RVA: 0x004264FC File Offset: 0x004246FC
		private void DrawPawnList(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = 60f;
			rect2 = rect2.ContractedBy(4f);
			for (int i = 0; i < Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count; i++)
			{
				this.DoPawnRow(rect2, i);
				rect2.y += 60f;
			}
			rect2.y += 4f;
			if (Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count < 6 && Widgets.ButtonText(new Rect(rect2.x, rect2.y, rect2.width, 25f), "+", true, true, true, null))
			{
				Find.GameInitData.startingPawnCount++;
				StartingPawnUtility.AddNewPawn(this.generationIndex);
				this.generationIndex++;
			}
		}

		// Token: 0x0600BB60 RID: 47968 RVA: 0x004265D8 File Offset: 0x004247D8
		private void DoPawnRow(Rect rect, int index)
		{
			Pawn pawn = Dialog_ChooseNewWanderers.StartingAndOptionalPawns[index];
			Widgets.DrawOptionBackground(rect, this.curPawnIndex == index);
			MouseoverSounds.DoRegion(rect);
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero().ContractedBy(4f);
			GUI.color = new Color(1f, 1f, 1f, 0.2f);
			GUI.DrawTexture(new Rect(110f - Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.x / 2f, 40f - Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.y / 2f, Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.x, Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.y), PortraitsCache.Get(pawn, Dialog_ChooseNewWanderers.PawnSelectorPortraitSize, Rot4.South, default(Vector3), 1f, true, true, true, true, null, null, false, null));
			GUI.color = Color.white;
			NameTriple nameTriple = pawn.Name as NameTriple;
			string label;
			if (nameTriple != null)
			{
				label = (string.IsNullOrEmpty(nameTriple.Nick) ? nameTriple.First : nameTriple.Nick);
			}
			else
			{
				label = pawn.LabelShort;
			}
			Widgets.Label(rect2.TopPart(0.5f).Rounded(), label);
			if (Text.CalcSize(pawn.story.TitleCap).x > rect2.width)
			{
				Widgets.Label(rect2.BottomPart(0.5f).Rounded(), pawn.story.TitleShortCap);
			}
			else
			{
				Widgets.Label(rect2.BottomPart(0.5f).Rounded(), pawn.story.TitleCap);
			}
			if (Mouse.IsOver(rect2) && Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count > 1 && Widgets.ButtonImage(new Rect(rect2.xMax - 15f, rect2.y, 15f, 15f), TexButton.Delete, true, null))
			{
				Find.GameInitData.startingPawnCount--;
				Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Remove(pawn);
				this.curPawnIndex = Math.Min(Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count - 1, this.curPawnIndex);
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.curPawnIndex = index;
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BB61 RID: 47969 RVA: 0x00426813 File Offset: 0x00424A13
		public override void PostClose()
		{
			base.PostClose();
			if (Find.GameEnder.gameEnding)
			{
				StartingPawnUtility.ClearAllStartingPawns();
			}
		}

		// Token: 0x04006A9B RID: 27291
		private int curPawnIndex;

		// Token: 0x04006A9C RID: 27292
		private int generationIndex;

		// Token: 0x04006A9D RID: 27293
		private const float TitleAreaHeight = 45f;

		// Token: 0x04006A9E RID: 27294
		private const float PawnEntryHeight = 60f;

		// Token: 0x04006A9F RID: 27295
		private const float SkillSummaryHeight = 127f;

		// Token: 0x04006AA0 RID: 27296
		private const float CrossIconSize = 15f;

		// Token: 0x04006AA1 RID: 27297
		private const float TabAreaWidth = 140f;

		// Token: 0x04006AA2 RID: 27298
		private static readonly Vector2 PawnSelectorPortraitSize = new Vector2(70f, 110f);

		// Token: 0x04006AA3 RID: 27299
		private static readonly Vector2 ConfirmButtonSize = new Vector2(150f, 38f);

		// Token: 0x04006AA4 RID: 27300
		private const int DefaultPawnCount = 3;

		// Token: 0x04006AA5 RID: 27301
		private const int MinPawnCount = 1;

		// Token: 0x04006AA6 RID: 27302
		private const int MaxPawnCount = 6;

		// Token: 0x04006AA7 RID: 27303
		private static readonly FloatRange ExcludeBiologicalAgeRange = new FloatRange(12.1f, 13f);
	}
}
