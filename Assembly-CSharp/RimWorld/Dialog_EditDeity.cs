using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA6 RID: 7334
	public class Dialog_EditDeity : Window
	{
		// Token: 0x17002225 RID: 8741
		// (get) Token: 0x0600BBD1 RID: 48081 RVA: 0x0042CE08 File Offset: 0x0042B008
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 250f);
			}
		}

		// Token: 0x0600BBD2 RID: 48082 RVA: 0x0042CE1C File Offset: 0x0042B01C
		public Dialog_EditDeity(IdeoFoundation_Deity.Deity deity, Ideo ideo) : base(null)
		{
			this.deity = deity;
			this.ideo = ideo;
			this.absorbInputAroundWindow = true;
			this.newDeityName = deity.name;
			this.newDeityTitle = deity.type;
			this.newDeityGender = deity.gender;
		}

		// Token: 0x0600BBD3 RID: 48083 RVA: 0x0042CE69 File Offset: 0x0042B069
		public override void OnAcceptKeyPressed()
		{
			this.ApplyChanges();
			Event.current.Use();
		}

		// Token: 0x0600BBD4 RID: 48084 RVA: 0x0042CE7C File Offset: 0x0042B07C
		public override void DoWindowContents(Rect rect)
		{
			float num = rect.x + rect.width / 3f;
			float width = rect.xMax - num;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 35f), "EditDeity".Translate());
			Text.Font = GameFont.Small;
			float num2 = rect.y + 35f + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityName".Translate());
			this.newDeityName = Widgets.TextField(new Rect(num, num2, width, Dialog_EditDeity.EditFieldHeight), this.newDeityName);
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityTitle".Translate());
			this.newDeityTitle = Widgets.TextField(new Rect(num, num2, width, Dialog_EditDeity.EditFieldHeight), this.newDeityTitle);
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			Widgets.Label(new Rect(rect.x, num2, width, Dialog_EditDeity.EditFieldHeight), "DeityGender".Translate());
			Rect rect2 = new Rect(num, num2, Dialog_EditDeity.EditFieldHeight + 8f + Text.CalcSize(this.newDeityGender.GetLabel(false).CapitalizeFirst()).x, Dialog_EditDeity.EditFieldHeight);
			Rect rect3 = new Rect(rect2.x, num2, Dialog_EditDeity.EditFieldHeight, Dialog_EditDeity.EditFieldHeight);
			GUI.DrawTexture(rect3.ContractedBy(2f), this.newDeityGender.GetIcon());
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect3.xMax + 4f, num2, rect2.width - rect3.width, Dialog_EditDeity.EditFieldHeight), this.newDeityGender.GetLabel(false).CapitalizeFirst());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.DrawHighlightIfMouseover(rect2);
			if (Widgets.ButtonInvisible(rect2, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				Gender[] array = (Gender[])Enum.GetValues(typeof(Gender));
				for (int i = 0; i < array.Length; i++)
				{
					Gender g = array[i];
					list.Add(new FloatMenuOption(g.GetLabel(false).CapitalizeFirst(), delegate()
					{
						this.newDeityGender = g;
					}, g.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num2 += Dialog_EditDeity.EditFieldHeight + 10f;
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_EditDeity.ButSize.y, Dialog_EditDeity.ButSize.x, Dialog_EditDeity.ButSize.y), "Back".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditDeity.ButSize.x, rect.height - Dialog_EditDeity.ButSize.y, Dialog_EditDeity.ButSize.x, Dialog_EditDeity.ButSize.y), "DoneButton".Translate(), true, true, true, null))
			{
				this.ApplyChanges();
			}
		}

		// Token: 0x0600BBD5 RID: 48085 RVA: 0x0042D1F0 File Offset: 0x0042B3F0
		private void ApplyChanges()
		{
			this.deity.name = this.newDeityName;
			this.deity.type = this.newDeityTitle;
			this.deity.gender = this.newDeityGender;
			this.ideo.RegenerateAllPreceptNames();
			this.ideo.RegenerateDescription(false);
			this.Close(true);
		}

		// Token: 0x04006B12 RID: 27410
		private IdeoFoundation_Deity.Deity deity;

		// Token: 0x04006B13 RID: 27411
		private Ideo ideo;

		// Token: 0x04006B14 RID: 27412
		private string newDeityName;

		// Token: 0x04006B15 RID: 27413
		private string newDeityTitle;

		// Token: 0x04006B16 RID: 27414
		private Gender newDeityGender;

		// Token: 0x04006B17 RID: 27415
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006B18 RID: 27416
		private static readonly float EditFieldHeight = 30f;
	}
}
