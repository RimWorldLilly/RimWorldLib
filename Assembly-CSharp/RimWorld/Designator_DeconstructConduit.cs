using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C43 RID: 7235
	public class Designator_DeconstructConduit : Designator_Deconstruct
	{
		// Token: 0x0600B86C RID: 47212 RVA: 0x00411318 File Offset: 0x0040F518
		public Designator_DeconstructConduit()
		{
			this.defaultLabel = "DesignatorDeconstructConduit".Translate();
			this.defaultDesc = "DesignatorDeconstructConduitDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/DeconstructConduit", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Designator_Deconstruct;
		}

		// Token: 0x0600B86D RID: 47213 RVA: 0x00411399 File Offset: 0x0040F599
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.building == null || !t.def.building.isPowerConduit)
			{
				return false;
			}
			return base.CanDesignateThing(t);
		}

		// Token: 0x0600B86E RID: 47214 RVA: 0x004113C8 File Offset: 0x0040F5C8
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
		}
	}
}
