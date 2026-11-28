using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001E1B RID: 7707
	public class Lesson_Note : Lesson
	{
		// Token: 0x170024FF RID: 9471
		// (get) Token: 0x0600C842 RID: 51266 RVA: 0x00496B4D File Offset: 0x00494D4D
		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		// Token: 0x17002500 RID: 9472
		// (get) Token: 0x0600C843 RID: 51267 RVA: 0x00496B5C File Offset: 0x00494D5C
		public Rect MainRect
		{
			get
			{
				float height = Text.CalcHeight(this.def.HelpTextAdjusted, 432f) + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		// Token: 0x17002501 RID: 9473
		// (get) Token: 0x0600C844 RID: 51268 RVA: 0x00496BA0 File Offset: 0x00494DA0
		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		// Token: 0x0600C845 RID: 51269 RVA: 0x00496BBB File Offset: 0x00494DBB
		public Lesson_Note()
		{
		}

		// Token: 0x0600C846 RID: 51270 RVA: 0x00496BD5 File Offset: 0x00494DD5
		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		// Token: 0x0600C847 RID: 51271 RVA: 0x00496BF6 File Offset: 0x00494DF6
		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		// Token: 0x0600C848 RID: 51272 RVA: 0x00496C08 File Offset: 0x00494E08
		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600C849 RID: 51273 RVA: 0x00496C1C File Offset: 0x00494E1C
		public override void LessonOnGUI()
		{
			Rect mainRect = this.MainRect;
			float alpha = 1f;
			if (this.doFadeIn)
			{
				alpha = Mathf.Clamp01(base.AgeSeconds / 0.4f);
			}
			if (this.Expiring)
			{
				float num = this.expiryTime - Time.timeSinceLevelLoad;
				if (num < 1.1f)
				{
					alpha = num / 1.1f;
				}
			}
			Find.WindowStack.ImmediateWindow(134706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.Expiring)
				{
					this.def.HighlightAllTags();
				}
				if (this.doFadeIn || this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				rect2.width = 432f;
				Widgets.Label(rect2, this.def.HelpTextAdjusted);
				Rect butRect = new Rect(rect.xMax - 32f - 8f, rect.y + 8f, 32f, 32f);
				Texture2D tex;
				if (this.Expiring)
				{
					tex = Widgets.CheckboxOnTex;
				}
				else
				{
					tex = TexButton.CloseXBig;
				}
				if (Widgets.ButtonImage(butRect, tex, new Color(0.95f, 0.95f, 0.95f), new Color(0.8352941f, 0.6666667f, 0.27450982f), true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.CloseButtonClicked();
				}
				if (Time.timeSinceLevelLoad > this.expiryTime)
				{
					this.CloseButtonClicked();
				}
				GUI.color = Color.white;
			}, false, false, alpha, null, false);
		}

		// Token: 0x0600C84A RID: 51274 RVA: 0x00496CC4 File Offset: 0x00494EC4
		private void CloseButtonClicked()
		{
			KnowledgeAmount know = this.def.noteTeaches ? KnowledgeAmount.NoteTaught : KnowledgeAmount.NoteClosed;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		// Token: 0x0600C84B RID: 51275 RVA: 0x00496CF9 File Offset: 0x00494EF9
		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.2f && !this.Expiring)
			{
				this.expiryTime = Time.timeSinceLevelLoad + 2.1f;
			}
		}

		// Token: 0x040071EC RID: 29164
		public ConceptDef def;

		// Token: 0x040071ED RID: 29165
		public bool doFadeIn = true;

		// Token: 0x040071EE RID: 29166
		private float expiryTime = float.MaxValue;

		// Token: 0x040071EF RID: 29167
		private const float RectWidth = 500f;

		// Token: 0x040071F0 RID: 29168
		private const float TextWidth = 432f;

		// Token: 0x040071F1 RID: 29169
		private const float FadeInDuration = 0.4f;

		// Token: 0x040071F2 RID: 29170
		private const float DoneButPad = 8f;

		// Token: 0x040071F3 RID: 29171
		private const float DoneButSize = 32f;

		// Token: 0x040071F4 RID: 29172
		private const float ExpiryDuration = 2.1f;

		// Token: 0x040071F5 RID: 29173
		private const float ExpiryFadeTime = 1.1f;
	}
}
