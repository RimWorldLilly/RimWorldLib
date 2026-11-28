using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E2 RID: 738
	public abstract class PlaceWorker
	{
		// Token: 0x06001660 RID: 5728 RVA: 0x000868A8 File Offset: 0x00084AA8
		public virtual bool IsBuildDesignatorVisible(BuildableDef def)
		{
			return true;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x000868AB File Offset: 0x00084AAB
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x000868B2 File Offset: 0x00084AB2
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x000868B4 File Offset: 0x00084AB4
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x000868B6 File Offset: 0x00084AB6
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x000868B9 File Offset: 0x00084AB9
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			return Enumerable.Empty<TerrainAffordanceDef>();
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x000868C0 File Offset: 0x00084AC0
		public virtual void DrawMouseAttachments(BuildableDef def)
		{
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000868C2 File Offset: 0x00084AC2
		public virtual void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef def, IntVec3 center, Rot4 rot)
		{
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000868C4 File Offset: 0x00084AC4
		public virtual void DrawOnGUIExtra(BuildableDef def)
		{
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000868C8 File Offset: 0x00084AC8
		protected static void DrawTextLine(float curX, ref float y, string text)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}
	}
}
