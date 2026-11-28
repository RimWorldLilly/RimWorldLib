using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C31 RID: 7217
	public class Designator_AreaAllowedExpand : Designator_AreaAllowed
	{
		// Token: 0x0600B81E RID: 47134 RVA: 0x0040FC28 File Offset: 0x0040DE28
		public Designator_AreaAllowedExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorExpandAreaAllowed".Translate();
			this.defaultDesc = "DesignatorExpandAreaAllowedDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/AreaAllowedExpand", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_AllowedArea;
			this.hotKey = KeyBindingDefOf.Misc8;
			this.tutorTag = "AreaAllowedExpand";
		}

		// Token: 0x0600B81F RID: 47135 RVA: 0x0040FCAE File Offset: 0x0040DEAE
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return c.InBounds(base.Map) && Designator_AreaAllowed.SelectedArea != null && !Designator_AreaAllowed.SelectedArea[c];
		}

		// Token: 0x0600B820 RID: 47136 RVA: 0x0040FCDB File Offset: 0x0040DEDB
		public override void DesignateSingleCell(IntVec3 c)
		{
			Designator_AreaAllowed.SelectedArea[c] = true;
		}
	}
}
