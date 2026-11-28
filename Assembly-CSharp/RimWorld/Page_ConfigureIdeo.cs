using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D00 RID: 7424
	public class Page_ConfigureIdeo : Page
	{
		// Token: 0x170022DA RID: 8922
		// (get) Token: 0x0600BFA8 RID: 49064 RVA: 0x00459CE0 File Offset: 0x00457EE0
		public override string PageTitle
		{
			get
			{
				return "CustomizeIdeoligion".Translate();
			}
		}

		// Token: 0x0600BFA9 RID: 49065 RVA: 0x00459CF1 File Offset: 0x00457EF1
		public Page_ConfigureIdeo()
		{
			this.grayOutIfOtherDialogOpen = true;
		}

		// Token: 0x0600BFAA RID: 49066 RVA: 0x00459D00 File Offset: 0x00457F00
		public override void PostOpen()
		{
			base.PostOpen();
			Find.IdeoManager.RemoveUnusedStartingIdeos();
			if (IdeoUIUtility.selected != null && Find.IdeoManager.IdeosListForReading.Contains(IdeoUIUtility.selected))
			{
				this.SelectOrMakeNewIdeo(IdeoUIUtility.selected);
			}
			else
			{
				this.ideo = null;
				IdeoUIUtility.UnselectCurrent();
			}
			TutorSystem.Notify_Event("PageStart-ConfigureIdeo");
		}

		// Token: 0x0600BFAB RID: 49067 RVA: 0x00459D64 File Offset: 0x00457F64
		public void SelectOrMakeNewIdeo(Ideo newIdeo = null)
		{
			this.ideo = (newIdeo ?? IdeoUtility.MakeEmptyIdeo());
			if (!Find.IdeoManager.IdeosListForReading.Contains(this.ideo))
			{
				Find.IdeoManager.Add(this.ideo);
				Faction.OfPlayer.ideos.SetPrimary(this.ideo);
			}
			if (!this.ideo.memes.Any<MemeDef>())
			{
				Find.WindowStack.Add(new Dialog_ChooseMemes(this.ideo, MemeCategory.Structure, true, null, null, false));
			}
			IdeoUIUtility.SetSelected(this.ideo);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.EditingMemes, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.EditingPrecepts, OpportunityType.Critical);
		}

		// Token: 0x0600BFAC RID: 49068 RVA: 0x00459E0C File Offset: 0x0045800C
		public virtual void DoIdeos(Rect rect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(base.GetMainRect(rect, 0f, false), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, true, !IdeoUIUtility.PlayerPrimaryIdeoNotShared, null, null, null, false, null, null, true, true, null);
		}

		// Token: 0x0600BFAD RID: 49069 RVA: 0x00459E58 File Offset: 0x00458058
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			this.DoIdeos(rect);
			if (this.ideo != null)
			{
				string text = null;
				Pair<Precept, Precept> lhs = this.ideo.FirstIncompatiblePreceptPair();
				if (lhs != default(Pair<Precept, Precept>))
				{
					string text2 = lhs.First.TipLabel;
					string text3 = lhs.Second.TipLabel;
					if (text2 == text3)
					{
						text2 = lhs.First.UIInfoSecondLine;
						text3 = lhs.Second.UIInfoSecondLine;
					}
					text = "MessageIdeoIncompatiblePrecepts".Translate(text2.Named("PRECEPT1"), text3.Named("PRECEPT2")).CapitalizeFirst();
				}
				else
				{
					Tuple<Precept_Ritual, List<string>> tuple = this.ideo.FirstRitualMissingTarget();
					Precept_Building precept_Building = this.ideo.FirstConsumableBuildingMissingRitual();
					if (tuple != null)
					{
						text = "MessageRitualMissingTarget".Translate(tuple.Item1.LabelCap.Named("PRECEPT")) + ": " + tuple.Item2.ToCommaList(false, false).CapitalizeFirst() + ".";
					}
					else if (precept_Building != null)
					{
						text = "MessageBuildingMissingRitual".Translate(precept_Building.LabelCap.Named("PRECEPT"));
					}
				}
				Rect rect2 = rect;
				rect2.xMin = rect2.xMax - Page.BottomButSize.x * 2.75f;
				rect2.width = Page.BottomButSize.x * 1.7f;
				rect2.yMin = rect2.yMax - Page.BottomButSize.y;
				Precept precept = this.ideo.FirstPreceptWithWarning();
				if (text != null)
				{
					GUI.color = Color.red;
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect2, text);
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
				}
				else if (precept != null)
				{
					GUI.color = ColorLibrary.Yellow;
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperRight;
					string text4;
					string description;
					precept.GetPlayerWarning(out text4, out description);
					text4 = "Warning".Translate() + ": " + text4.CapitalizeFirst();
					Widgets.Label(rect2, text4);
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					Widgets.DrawHighlightIfMouseover(rect2);
					TooltipHandler.TipRegion(rect2, () => description, 37584575);
				}
				else
				{
					IdeoUIUtility.DrawImpactInfo(rect2, this.ideo.memes);
				}
			}
			if (this.ideo != null)
			{
				base.DoBottomButtons(rect, null, "RandomizeAll".Translate(), new Action(this.Randomize), true, true);
				return;
			}
			base.DoBottomButtons(rect, null, null, null, true, true);
		}

		// Token: 0x0600BFAE RID: 49070 RVA: 0x0045A12C File Offset: 0x0045832C
		public virtual void Notify_ClosedChooseMemesDialog()
		{
			if (this.ideo != null)
			{
				if (!this.ideo.memes.Any((MemeDef x) => x.category == MemeCategory.Normal))
				{
					Faction.OfPlayer.ideos.SetPrimary(null);
					Find.IdeoManager.Remove(this.ideo);
				}
			}
		}

		// Token: 0x0600BFAF RID: 49071 RVA: 0x0045A194 File Offset: 0x00458394
		private void Randomize()
		{
			if (this.ideo != null && TutorSystem.AllowAction("ConfiguringIdeo"))
			{
				this.ideo.foundation.Init(new IdeoGenerationParms(IdeoUIUtility.FactionForRandomization(this.ideo), false, null, null, null, false, false, this.ideo.Fluid, false, "", null, null, false, "", false));
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600BFB0 RID: 49072 RVA: 0x0045A208 File Offset: 0x00458408
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.ideo == null)
			{
				Messages.Message("MessageMustChooseIdeo".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.ideo.name.NullOrEmpty())
			{
				Messages.Message("MessageIdeoNameCantBeEmpty".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Pair<Precept, Precept> lhs = this.ideo.FirstIncompatiblePreceptPair();
			if (lhs != default(Pair<Precept, Precept>))
			{
				Messages.Message("MessageIdeoIncompatiblePrecepts".Translate(lhs.First.Label.Named("PRECEPT1"), lhs.Second.Label.Named("PRECEPT2")).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Tuple<Precept_Ritual, List<string>> tuple = this.ideo.FirstRitualMissingTarget();
			if (tuple != null)
			{
				Messages.Message("MessageRitualMissingTarget".Translate(tuple.Item1.LabelCap.Named("PRECEPT")) + ": " + tuple.Item2.ToCommaList(false, false).CapitalizeFirst() + ".", MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Precept_Building precept_Building = this.ideo.FirstConsumableBuildingMissingRitual();
			if (precept_Building != null)
			{
				Messages.Message("MessageBuildingMissingRitual".Translate(precept_Building.LabelCap.Named("PRECEPT")), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Faction.OfPlayer.ideos.SetPrimary(this.ideo);
			foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
			{
				ideo.initialPlayerIdeo = false;
			}
			this.ideo.initialPlayerIdeo = true;
			Find.Scenario.PostIdeoChosen();
			return true;
		}

		// Token: 0x04006E34 RID: 28212
		public Ideo ideo;

		// Token: 0x04006E35 RID: 28213
		protected Vector2 scrollPosition_ideoList;

		// Token: 0x04006E36 RID: 28214
		protected float scrollViewHeight_ideoList;

		// Token: 0x04006E37 RID: 28215
		protected Vector2 scrollPosition_ideoDetails;

		// Token: 0x04006E38 RID: 28216
		protected float scrollViewHeight_ideoDetails;
	}
}
