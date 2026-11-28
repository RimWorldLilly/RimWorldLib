using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000669 RID: 1641
	public class DiaOption
	{
		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060033F4 RID: 13300 RVA: 0x0014E7A9 File Offset: 0x0014C9A9
		public static DiaOption DefaultOK
		{
			get
			{
				return new DiaOption("OK".Translate())
				{
					resolveTree = true
				};
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x0014E7C6 File Offset: 0x0014C9C6
		protected Dialog_NodeTree OwningDialog
		{
			get
			{
				return (Dialog_NodeTree)this.dialog;
			}
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x0014E7D4 File Offset: 0x0014C9D4
		public DiaOption()
		{
			this.text = "OK".Translate();
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x0014E821 File Offset: 0x0014CA21
		public DiaOption(string text)
		{
			this.text = text;
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x0014E858 File Offset: 0x0014CA58
		public DiaOption(Dialog_InfoCard.Hyperlink hyperlink)
		{
			this.hyperlink = hyperlink;
			this.text = "ViewHyperlink".Translate(hyperlink.Label);
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0014E8B8 File Offset: 0x0014CAB8
		public DiaOption(DiaOptionMold def)
		{
			this.text = def.Text;
			DiaNodeMold diaNodeMold = def.RandomLinkNode();
			if (diaNodeMold != null)
			{
				this.link = new DiaNode(diaNodeMold);
			}
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x0014E912 File Offset: 0x0014CB12
		public void Disable(string newDisabledReason)
		{
			this.disabled = true;
			this.disabledReason = newDisabledReason;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0014E922 File Offset: 0x0014CB22
		public void SetText(string newText)
		{
			this.text = newText;
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x0014E92C File Offset: 0x0014CB2C
		public float OptOnGUI(Rect rect, bool active = true)
		{
			Color textColor = Widgets.NormalOptionColor;
			string text = this.text;
			if (this.disabled)
			{
				textColor = this.DisabledOptionColor;
				if (this.disabledReason != null)
				{
					text = text + " (" + this.disabledReason + ")";
				}
			}
			rect.height = Text.CalcHeight(text, rect.width);
			if (this.hyperlink.def != null)
			{
				Widgets.HyperlinkWithIcon(rect, this.hyperlink, text, 2f, 6f, null, false, null);
			}
			else if (Widgets.ButtonText(rect, text, false, !this.disabled, textColor, active && !this.disabled, null))
			{
				this.Activate();
			}
			return rect.height;
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x0014E9F4 File Offset: 0x0014CBF4
		protected void Activate()
		{
			if (this.clickSound != null && !this.resolveTree)
			{
				this.clickSound.PlayOneShotOnCamera(null);
			}
			if (this.resolveTree)
			{
				this.OwningDialog.Close(true);
			}
			if (this.action != null)
			{
				this.action();
			}
			if (this.linkLateBind != null)
			{
				this.OwningDialog.GotoNode(this.linkLateBind());
				return;
			}
			if (this.link != null)
			{
				this.OwningDialog.GotoNode(this.link);
			}
		}

		// Token: 0x040021D5 RID: 8661
		public Window dialog;

		// Token: 0x040021D6 RID: 8662
		protected string text;

		// Token: 0x040021D7 RID: 8663
		public DiaNode link;

		// Token: 0x040021D8 RID: 8664
		public Func<DiaNode> linkLateBind;

		// Token: 0x040021D9 RID: 8665
		public bool resolveTree;

		// Token: 0x040021DA RID: 8666
		public Action action;

		// Token: 0x040021DB RID: 8667
		public bool disabled;

		// Token: 0x040021DC RID: 8668
		public string disabledReason;

		// Token: 0x040021DD RID: 8669
		public SoundDef clickSound = SoundDefOf.PageChange;

		// Token: 0x040021DE RID: 8670
		public Dialog_InfoCard.Hyperlink hyperlink;

		// Token: 0x040021DF RID: 8671
		protected readonly Color DisabledOptionColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
