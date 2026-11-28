using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001222 RID: 4642
	public class LayoutStructureSketch : IExposable, ILoadReferenceable
	{
		// Token: 0x06007332 RID: 29490 RVA: 0x00284C93 File Offset: 0x00282E93
		public string GetUniqueLoadID()
		{
			return this.uniqueId;
		}

		// Token: 0x06007333 RID: 29491 RVA: 0x00284C9C File Offset: 0x00282E9C
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.thingDiscoveredMessage, "thingDiscoveredMessage", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			Scribe_Values.Look<IntVec3>(ref this.center, "center", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.uniqueId, "uniqueId", null, false);
			Scribe_Defs.Look<LayoutDef>(ref this.layoutDef, "layoutDef");
			Scribe_Deep.Look<LayoutSketch>(ref this.layoutSketch, "layoutSketch", Array.Empty<object>());
			Scribe_Deep.Look<StructureLayout>(ref this.structureLayout, "structureLayout", new object[]
			{
				this
			});
			Scribe_Collections.Look<Thing>(ref this.thingsToSpawn, "thingsToSpawn", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && string.IsNullOrEmpty(this.uniqueId))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
				defaultInterpolatedStringHandler.AppendLiteral("LayoutStructureSketch_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Find.UniqueIDsManager.GetNextStructureSketchID());
				this.uniqueId = defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06007334 RID: 29492 RVA: 0x00284DAC File Offset: 0x00282FAC
		public bool AnyRoomContains(IntVec3 c, LayoutRoom exclude = null)
		{
			foreach (LayoutRoom layoutRoom in this.structureLayout.Rooms)
			{
				if (layoutRoom != exclude && layoutRoom.Contains(c, 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007335 RID: 29493 RVA: 0x00284E14 File Offset: 0x00283014
		public float ClosestDistTo(IntVec3 c)
		{
			float num = float.MaxValue;
			foreach (LayoutRoom layoutRoom in this.structureLayout.Rooms)
			{
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					num = Mathf.Min(cellRect.ClosestDistanceTo(c), num);
				}
			}
			return num;
		}

		// Token: 0x040048D9 RID: 18649
		public LayoutSketch layoutSketch;

		// Token: 0x040048DA RID: 18650
		public StructureLayout structureLayout;

		// Token: 0x040048DB RID: 18651
		public List<Thing> thingsToSpawn = new List<Thing>();

		// Token: 0x040048DC RID: 18652
		public string thingDiscoveredMessage;

		// Token: 0x040048DD RID: 18653
		public LayoutDef layoutDef;

		// Token: 0x040048DE RID: 18654
		public bool spawned;

		// Token: 0x040048DF RID: 18655
		public IntVec3 center;

		// Token: 0x040048E0 RID: 18656
		public int id;

		// Token: 0x040048E1 RID: 18657
		public string uniqueId;
	}
}
