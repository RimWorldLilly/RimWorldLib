using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6E RID: 7534
	public class Dialog_ViewGenes : Window
	{
		// Token: 0x17002430 RID: 9264
		// (get) Token: 0x0600C33E RID: 49982 RVA: 0x00473ADD File Offset: 0x00471CDD
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(736f, 700f);
			}
		}

		// Token: 0x0600C33F RID: 49983 RVA: 0x00473AEE File Offset: 0x00471CEE
		public Dialog_ViewGenes(Pawn target) : base(null)
		{
			this.target = target;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x0600C340 RID: 49984 RVA: 0x00473B05 File Offset: 0x00471D05
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("genes viewing"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x0600C341 RID: 49985 RVA: 0x00473B24 File Offset: 0x00471D24
		public override void DoWindowContents(Rect inRect)
		{
			inRect.yMax -= Window.CloseButSize.y;
			Rect rect = inRect;
			rect.xMin += 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, "ViewGenes".Translate() + ": " + this.target.genes.XenotypeLabelCap);
			Text.Font = GameFont.Small;
			GUI.color = XenotypeDef.IconColor;
			GUI.DrawTexture(new Rect(inRect.x, inRect.y, 30f, 30f), this.target.genes.XenotypeIcon);
			GUI.color = Color.white;
			inRect.yMin += 34f;
			Vector2 zero = Vector2.zero;
			GeneUIUtility.DrawGenesInfo(inRect, this.target, this.InitialSize.y, ref zero, ref this.scrollPosition, null);
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x, inRect.yMax, Window.CloseButSize.x, Window.CloseButSize.y), "Close".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x04006F8E RID: 28558
		private Pawn target;

		// Token: 0x04006F8F RID: 28559
		private Vector2 scrollPosition;

		// Token: 0x04006F90 RID: 28560
		private const float HeaderHeight = 30f;
	}
}
