using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C51 RID: 7249
	public class Designator_MineVein : Designator_Mine
	{
		// Token: 0x17002161 RID: 8545
		// (get) Token: 0x0600B8DE RID: 47326 RVA: 0x0041332C File Offset: 0x0041152C
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002162 RID: 8546
		// (get) Token: 0x0600B8DF RID: 47327 RVA: 0x0041332F File Offset: 0x0041152F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.MineVein;
			}
		}

		// Token: 0x17002163 RID: 8547
		// (get) Token: 0x0600B8E0 RID: 47328 RVA: 0x00413336 File Offset: 0x00411536
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B8E1 RID: 47329 RVA: 0x00413340 File Offset: 0x00411540
		public Designator_MineVein()
		{
			this.defaultLabel = "DesignatorMineVein".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/MineVein", true);
			this.defaultDesc = "DesignatorMineVeinDesc".Translate();
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc11;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Mine;
		}

		// Token: 0x0600B8E2 RID: 47330 RVA: 0x004133C4 File Offset: 0x004115C4
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (c.Fogged(base.Map))
			{
				return true;
			}
			Mineable firstMineable = c.GetFirstMineable(base.Map);
			if (firstMineable == null)
			{
				return "MessageMustDesignateMineable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstMineable);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B8E3 RID: 47331 RVA: 0x00413454 File Offset: 0x00411654
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.mineable || !t.def.building.veinMineable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			return true;
		}

		// Token: 0x0600B8E4 RID: 47332 RVA: 0x004134B1 File Offset: 0x004116B1
		public override void DesignateSingleCell(IntVec3 loc)
		{
			Designator_MineVein.FloodFillDesignations(loc, base.Map, loc.GetEdifice(base.Map).def);
			Designator_Mine.PossiblyWarnPlayerOnDesignatingMining();
		}

		// Token: 0x0600B8E5 RID: 47333 RVA: 0x004134D8 File Offset: 0x004116D8
		public static void FloodFillDesignations(IntVec3 loc, Map map, ThingDef def)
		{
			Designator_MineVein.<>c__DisplayClass10_0 CS$<>8__locals1 = new Designator_MineVein.<>c__DisplayClass10_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.def = def;
			CS$<>8__locals1.map.floodFiller.FloodFill(loc, new Predicate<IntVec3>(CS$<>8__locals1.<FloodFillDesignations>g__Validator|0), delegate(IntVec3 c)
			{
				CS$<>8__locals1.map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.MineVein, null));
				CS$<>8__locals1.map.designationManager.TryRemoveDesignation(c, DesignationDefOf.Mine);
			}, int.MaxValue, false, null);
		}

		// Token: 0x0600B8E6 RID: 47334 RVA: 0x0041352C File Offset: 0x0041172C
		public static void RemoveContiguousDesignations(IntVec3 loc, Map map, ThingDef def)
		{
			map.floodFiller.FloodFill(loc, (IntVec3 c) => map.designationManager.DesignationAt(c, DesignationDefOf.MineVein) != null && c.GetEdifice(map).def == def, delegate(IntVec3 c)
			{
				map.designationManager.RemoveDesignation(map.designationManager.DesignationAt(c, DesignationDefOf.MineVein));
			}, int.MaxValue, false, null);
		}
	}
}
