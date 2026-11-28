using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C41 RID: 7233
	public class Designator_Claim : Designator
	{
		// Token: 0x17002148 RID: 8520
		// (get) Token: 0x0600B85C RID: 47196 RVA: 0x00410DF4 File Offset: 0x0040EFF4
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B85D RID: 47197 RVA: 0x00410DFC File Offset: 0x0040EFFC
		public Designator_Claim()
		{
			this.defaultLabel = "DesignatorClaim".Translate();
			this.defaultDesc = "DesignatorClaimDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Claim", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc4;
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x0600B85E RID: 47198 RVA: 0x00410E84 File Offset: 0x0040F084
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateClaimable".Translate();
			}
			return true;
		}

		// Token: 0x0600B85F RID: 47199 RVA: 0x00410EF0 File Offset: 0x0040F0F0
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x0600B860 RID: 47200 RVA: 0x00410F3F File Offset: 0x0040F13F
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.Faction != Faction.OfPlayer)
			{
				return t.ClaimableBy(Faction.OfPlayer);
			}
			return false;
		}

		// Token: 0x0600B861 RID: 47201 RVA: 0x00410F60 File Offset: 0x0040F160
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			foreach (IntVec3 cell in t.OccupiedRect())
			{
				FleckMaker.ThrowMetaPuffs(new TargetInfo(cell, base.Map, false));
			}
		}
	}
}
