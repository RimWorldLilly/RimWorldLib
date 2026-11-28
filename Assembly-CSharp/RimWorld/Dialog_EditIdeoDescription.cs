using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CA7 RID: 7335
	public class Dialog_EditIdeoDescription : Window
	{
		// Token: 0x17002226 RID: 8742
		// (get) Token: 0x0600BBD7 RID: 48087 RVA: 0x0042D26E File Offset: 0x0042B46E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 400f);
			}
		}

		// Token: 0x0600BBD8 RID: 48088 RVA: 0x0042D27F File Offset: 0x0042B47F
		public Dialog_EditIdeoDescription(Ideo ideo) : base(null)
		{
			this.ideo = ideo;
			this.newDescription = ideo.description;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BBD9 RID: 48089 RVA: 0x0042D2A2 File Offset: 0x0042B4A2
		public override void OnAcceptKeyPressed()
		{
			Event.current.Use();
		}

		// Token: 0x0600BBDA RID: 48090 RVA: 0x0042D2B0 File Offset: 0x0042B4B0
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditNarrative".Translate());
			Text.Font = GameFont.Small;
			float num = rect.y + 35f + 10f;
			string a = Widgets.TextArea(new Rect(rect.x, num, rect.width, rect.height - Dialog_EditIdeoDescription.ButSize.y - 17f - num), this.newDescription, false);
			if (a != this.newDescription)
			{
				this.newDescription = a;
				this.newDescriptionTemplate = null;
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "Cancel".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - Dialog_EditIdeoDescription.ButSize.x / 2f, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "Randomize".Translate(), true, true, true, null))
			{
				IdeoDescriptionResult ideoDescriptionResult = this.ideo.GetNewDescription(true);
				this.newDescription = ideoDescriptionResult.text;
				this.newDescriptionTemplate = ideoDescriptionResult.template;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditIdeoDescription.ButSize.x, rect.height - Dialog_EditIdeoDescription.ButSize.y, Dialog_EditIdeoDescription.ButSize.x, Dialog_EditIdeoDescription.ButSize.y), "DoneButton".Translate(), true, true, true, null))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x0600BBDB RID: 48091 RVA: 0x0042D4BC File Offset: 0x0042B6BC
		private void ApplyChanges()
		{
			if (this.ideo.description != this.newDescription)
			{
				this.ideo.description = this.newDescription;
				this.ideo.descriptionTemplate = this.newDescriptionTemplate;
				this.ideo.descriptionLocked = true;
			}
			this.Close(true);
		}

		// Token: 0x04006B19 RID: 27417
		private Ideo ideo;

		// Token: 0x04006B1A RID: 27418
		private string newDescription;

		// Token: 0x04006B1B RID: 27419
		private string newDescriptionTemplate;

		// Token: 0x04006B1C RID: 27420
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006B1D RID: 27421
		private const float HeaderHeight = 35f;
	}
}
