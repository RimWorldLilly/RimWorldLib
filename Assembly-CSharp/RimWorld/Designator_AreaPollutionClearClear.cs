using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3C RID: 7228
	public class Designator_AreaPollutionClearClear : Designator_AreaPollutionClear
	{
		// Token: 0x0600B849 RID: 47177 RVA: 0x00410604 File Offset: 0x0040E804
		public Designator_AreaPollutionClearClear() : base(DesignateMode.Remove)
		{
			this.defaultLabel = "DesignatorAreaPollutionClearClear".Translate();
			this.defaultDesc = "DesignatorAreaPollutionClearClearDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Icons/PollutionClearAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
		}
	}
}
