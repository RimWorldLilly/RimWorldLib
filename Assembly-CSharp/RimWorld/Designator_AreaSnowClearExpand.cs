using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3E RID: 7230
	public class Designator_AreaSnowClearExpand : Designator_AreaSnowClear
	{
		// Token: 0x0600B850 RID: 47184 RVA: 0x00410774 File Offset: 0x0040E974
		public Designator_AreaSnowClearExpand() : base(DesignateMode.Add)
		{
			string key = "DesignatorAreaSnowClearExpand";
			string key2 = "DesignatorAreaSnowClearExpandDesc";
			if (ModsConfig.OdysseyActive)
			{
				key = "DesignatorAreaSnowOrSandClearExpand";
				key2 = "DesignatorAreaSnowOrSandClearExpandDesc";
			}
			this.defaultLabel = key.Translate();
			this.defaultDesc = key2.Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_RemoveSnow;
		}
	}
}
