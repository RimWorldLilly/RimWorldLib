using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C36 RID: 7222
	public class Designator_AreaHomeExpand : Designator_AreaHome
	{
		// Token: 0x0600B832 RID: 47154 RVA: 0x0041008C File Offset: 0x0040E28C
		public Designator_AreaHomeExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaHomeExpand".Translate();
			this.defaultDesc = "DesignatorAreaHomeExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.tutorTag = "AreaHomeExpand";
			this.hotKey = KeyBindingDefOf.Misc4;
		}
	}
}
