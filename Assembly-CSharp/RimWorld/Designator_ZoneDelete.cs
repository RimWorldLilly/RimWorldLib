using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C86 RID: 7302
	public class Designator_ZoneDelete : Designator_Zone
	{
		// Token: 0x170021D5 RID: 8661
		// (get) Token: 0x0600BA69 RID: 47721 RVA: 0x0041B4ED File Offset: 0x004196ED
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.RemoveZones;
			}
		}

		// Token: 0x0600BA6A RID: 47722 RVA: 0x0041B4F4 File Offset: 0x004196F4
		public Designator_ZoneDelete()
		{
			this.defaultLabel = "DesignatorZoneDelete".Translate();
			this.defaultDesc = "DesignatorZoneDeleteDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.useMouseIcon = true;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneDelete", true);
			this.hotKey = KeyBindingDefOf.Misc3;
		}

		// Token: 0x0600BA6B RID: 47723 RVA: 0x0041B57C File Offset: 0x0041977C
		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			if (!sq.InBounds(base.Map))
			{
				return false;
			}
			if (sq.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.zoneManager.ZoneAt(sq) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600BA6C RID: 47724 RVA: 0x0041B5D4 File Offset: 0x004197D4
		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		// Token: 0x0600BA6D RID: 47725 RVA: 0x0041B614 File Offset: 0x00419814
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x040069F5 RID: 27125
		private readonly List<Zone> justDesignated = new List<Zone>();
	}
}
