using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3B RID: 7227
	public class Designator_AreaPollutionClearExpand : Designator_AreaPollutionClear
	{
		// Token: 0x0600B848 RID: 47176 RVA: 0x00410588 File Offset: 0x0040E788
		public Designator_AreaPollutionClearExpand() : base(DesignateMode.Add)
		{
			this.defaultLabel = "DesignatorAreaPollutionClearExpand".Translate();
			this.defaultDesc = "DesignatorAreaPollutionClearExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Icons/PollutionClearArea", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_RemovePollution;
			this.tutorTag = "AreaPollutionClearExpand";
		}
	}
}
