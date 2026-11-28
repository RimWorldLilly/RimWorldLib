using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001E1A RID: 7706
	public abstract class Lesson_Instruction : Lesson
	{
		// Token: 0x170024FA RID: 9466
		// (get) Token: 0x0600C836 RID: 51254 RVA: 0x00496694 File Offset: 0x00494894
		protected Map Map
		{
			get
			{
				return Find.AnyPlayerHomeMap;
			}
		}

		// Token: 0x170024FB RID: 9467
		// (get) Token: 0x0600C837 RID: 51255 RVA: 0x0049669B File Offset: 0x0049489B
		protected virtual float ProgressPercent
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170024FC RID: 9468
		// (get) Token: 0x0600C838 RID: 51256 RVA: 0x004966A2 File Offset: 0x004948A2
		protected virtual bool ShowProgressBar
		{
			get
			{
				return this.ProgressPercent >= 0f;
			}
		}

		// Token: 0x170024FD RID: 9469
		// (get) Token: 0x0600C839 RID: 51257 RVA: 0x004966B4 File Offset: 0x004948B4
		public override string DefaultRejectInputMessage
		{
			get
			{
				return this.def.rejectInputMessage;
			}
		}

		// Token: 0x170024FE RID: 9470
		// (get) Token: 0x0600C83A RID: 51258 RVA: 0x004966C1 File Offset: 0x004948C1
		public override InstructionDef Instruction
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x0600C83B RID: 51259 RVA: 0x004966C9 File Offset: 0x004948C9
		public override void ExposeData()
		{
			Scribe_Defs.Look<InstructionDef>(ref this.def, "def");
			base.ExposeData();
		}

		// Token: 0x0600C83C RID: 51260 RVA: 0x004966E4 File Offset: 0x004948E4
		public override void OnActivated()
		{
			base.OnActivated();
			if (this.def.giveOnActivateCount > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.def.giveOnActivateDef, null);
				thing.stackCount = this.def.giveOnActivateCount;
				GenSpawn.Spawn(thing, TutorUtility.FindUsableRect(2, 2, this.Map, 0f, false).CenterCell, this.Map, WipeMode.Vanish);
			}
			if (this.def.resetBuildDesignatorStuffs)
			{
				foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
				{
					foreach (Designator designator in designationCategoryDef.ResolvedAllowedDesignators)
					{
						Designator_Build designator_Build = designator as Designator_Build;
						if (designator_Build != null)
						{
							designator_Build.ResetStuffToDefault();
						}
					}
				}
			}
		}

		// Token: 0x0600C83D RID: 51261 RVA: 0x004967D8 File Offset: 0x004949D8
		public override void LessonOnGUI()
		{
			Text.Font = GameFont.Small;
			string textAdj = this.def.Text.AdjustedForKeys(null, true);
			float num = Text.CalcHeight(textAdj, 290f) + 20f;
			if (this.ShowProgressBar)
			{
				num += 47f;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth - 17f - 155f, 17f + num / 2f);
			if (!Find.TutorialState.introDone)
			{
				float screenOverlayAlpha = 0f;
				if (this.def.startCentered)
				{
					Vector2 vector2 = new Vector2((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
					if (base.AgeSeconds < 4f)
					{
						vector = vector2;
						screenOverlayAlpha = 0.9f;
					}
					else if (base.AgeSeconds < 5f)
					{
						float t = (base.AgeSeconds - 4f) / 1f;
						vector = Vector2.Lerp(vector2, vector, t);
						screenOverlayAlpha = Mathf.Lerp(0.9f, 0f, t);
					}
				}
				if (screenOverlayAlpha > 0f)
				{
					Rect fullScreenRect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
					Find.WindowStack.ImmediateWindow(972651, fullScreenRect, WindowLayer.SubSuper, delegate
					{
						GUI.color = new Color(1f, 1f, 1f, screenOverlayAlpha);
						GUI.DrawTexture(fullScreenRect, BaseContent.BlackTex);
						GUI.color = Color.white;
					}, false, true, 0f, null, false);
				}
				else
				{
					Find.TutorialState.introDone = true;
				}
			}
			Rect mainRect = new Rect(vector.x - 155f, vector.y - num / 2f - 10f, 310f, num);
			if (Find.TutorialState.introDone && Find.WindowStack.IsOpen<Page_ConfigureStartingPawns>())
			{
				Rect mainRect2 = mainRect;
				mainRect2.x = 17f;
				if ((mainRect.Contains(Event.current.mousePosition) || (this.def == InstructionDefOf.RandomizeCharacter && UI.screenHeight <= 768)) && !mainRect2.Contains(Event.current.mousePosition))
				{
					mainRect.x = 17f;
				}
			}
			Find.WindowStack.ImmediateWindow(177706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				Text.Font = GameFont.Small;
				Rect rect3 = rect2;
				if (this.ShowProgressBar)
				{
					rect3.height -= 47f;
				}
				Widgets.Label(rect3, textAdj);
				if (this.ShowProgressBar)
				{
					Widgets.FillableBar(new Rect(rect2.x, rect2.yMax - 30f, rect2.width, 30f), this.ProgressPercent, LearningReadout.ProgressBarFillTex);
				}
				if (this.AgeSeconds < 0.5f)
				{
					GUI.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, this.AgeSeconds / 0.5f));
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = Color.white;
				}
			}, false, false, 1f, null, false);
			if (this.def.highlightTags != null)
			{
				for (int i = 0; i < this.def.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.def.highlightTags[i]);
				}
			}
		}

		// Token: 0x0600C83E RID: 51262 RVA: 0x00496A85 File Offset: 0x00494C85
		public override void Notify_Event(EventPack ep)
		{
			if (this.def.eventTagsEnd != null && this.def.eventTagsEnd.Contains(ep.Tag))
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0600C83F RID: 51263 RVA: 0x00496AB7 File Offset: 0x00494CB7
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			return this.def.actionTagsAllowed != null && this.def.actionTagsAllowed.Contains(ep.Tag);
		}

		// Token: 0x0600C840 RID: 51264 RVA: 0x00496AE8 File Offset: 0x00494CE8
		public override void PostDeactivated()
		{
			SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			TutorSystem.Notify_Event("InstructionDeactivated-" + this.def.defName);
			if (this.def.endTutorial)
			{
				Find.ActiveLesson.Deactivate();
				Find.TutorialState.Notify_TutorialEnding();
				LessonAutoActivator.Notify_TutorialEnding();
			}
		}

		// Token: 0x040071E9 RID: 29161
		public InstructionDef def;

		// Token: 0x040071EA RID: 29162
		private const float RectWidth = 310f;

		// Token: 0x040071EB RID: 29163
		private const float BarHeight = 30f;
	}
}
