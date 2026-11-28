using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C32 RID: 7218
	public class Designator_AreaAllowedClear : Designator_AreaAllowed
	{
		// Token: 0x0600B821 RID: 47137 RVA: 0x0040FCEC File Offset: 0x0040DEEC
		public Designator_AreaAllowedClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorClearAreaAllowed".Translate();
			this.defaultDesc = "DesignatorClearAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedClear", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "AreaAllowedClear";
		}

		// Token: 0x0600B822 RID: 47138 RVA: 0x0040FD6E File Offset: 0x0040DF6E
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x0600B823 RID: 47139 RVA: 0x0040FD98 File Offset: 0x0040DF98
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = false;
		}
	}
}
