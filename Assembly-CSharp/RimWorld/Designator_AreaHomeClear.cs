using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C37 RID: 7223
	public class Designator_AreaHomeClear : Designator_AreaHome
	{
		// Token: 0x0600B833 RID: 47155 RVA: 0x00410114 File Offset: 0x0040E314
		public Designator_AreaHomeClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaHomeClear".Translate();
			this.defaultDesc = "DesignatorAreaHomeClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HomeAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.hotKey = KeyBindingDefOf.Misc7;
		}
	}
}
