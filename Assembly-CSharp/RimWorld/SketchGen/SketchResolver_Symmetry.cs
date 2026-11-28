using System;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020D7 RID: 8407
	public class SketchResolver_Symmetry : SketchResolver
	{
		// Token: 0x0600D391 RID: 54161 RVA: 0x004E08F4 File Offset: 0x004DEAF4
		protected override void ResolveInt(SketchResolveParams parms)
		{
			bool valueOrDefault = parms.symmetryClear.GetValueOrDefault(true);
			int valueOrDefault2 = parms.symmetryOrigin.GetValueOrDefault();
			bool valueOrDefault3 = parms.symmetryVertical.GetValueOrDefault();
			bool valueOrDefault4 = parms.requireFloor.GetValueOrDefault();
			bool valueOrDefault5 = parms.symmetryOriginIncluded.GetValueOrDefault();
			if (valueOrDefault)
			{
				this.Clear(parms.sketch, valueOrDefault2, valueOrDefault3, valueOrDefault5);
			}
			foreach (SketchBuildable sketchBuildable in parms.sketch.Buildables.ToList<SketchBuildable>())
			{
				if (!this.ShouldKeepAlreadySymmetricalInTheMiddle(sketchBuildable, valueOrDefault2, valueOrDefault3, valueOrDefault5))
				{
					SketchBuildable sketchBuildable2 = (SketchBuildable)sketchBuildable.DeepCopy();
					SketchThing sketchThing = sketchBuildable2 as SketchThing;
					if (sketchThing != null && sketchThing.def.rotatable)
					{
						if (valueOrDefault3)
						{
							if (!sketchThing.rot.IsHorizontal)
							{
								sketchThing.rot = sketchThing.rot.Opposite;
							}
						}
						else if (sketchThing.rot.IsHorizontal)
						{
							sketchThing.rot = sketchThing.rot.Opposite;
						}
					}
					this.MoveUntilSymmetrical(sketchBuildable2, sketchBuildable.OccupiedRect, valueOrDefault2, valueOrDefault3, valueOrDefault5);
					if (valueOrDefault4 && sketchBuildable2.Buildable != ThingDefOf.Wall && sketchBuildable2.Buildable != ThingDefOf.Door)
					{
						bool flag = true;
						foreach (IntVec3 pos in sketchBuildable2.OccupiedRect)
						{
							if (!parms.sketch.AnyTerrainAt(pos))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							parms.sketch.Add(sketchBuildable2, true);
						}
					}
					else
					{
						parms.sketch.Add(sketchBuildable2, true);
					}
				}
			}
		}

		// Token: 0x0600D392 RID: 54162 RVA: 0x004E0AEC File Offset: 0x004DECEC
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600D393 RID: 54163 RVA: 0x004E0AF0 File Offset: 0x004DECF0
		private void Clear(Sketch sketch, int origin, bool vertical, bool originIncluded)
		{
			foreach (SketchBuildable sketchBuildable in sketch.Buildables.ToList<SketchBuildable>())
			{
				CellRect occupiedRect = sketchBuildable.OccupiedRect;
				if (((occupiedRect.maxX >= origin && !vertical) || (occupiedRect.maxZ >= origin && vertical)) && !this.ShouldKeepAlreadySymmetricalInTheMiddle(sketchBuildable, origin, vertical, originIncluded))
				{
					sketch.Remove(sketchBuildable);
				}
			}
		}

		// Token: 0x0600D394 RID: 54164 RVA: 0x004E0B7C File Offset: 0x004DED7C
		private bool ShouldKeepAlreadySymmetricalInTheMiddle(SketchBuildable buildable, int origin, bool vertical, bool originIncluded)
		{
			CellRect occupiedRect = buildable.OccupiedRect;
			if (vertical)
			{
				if (originIncluded)
				{
					return occupiedRect.maxZ - origin == origin - occupiedRect.minZ;
				}
				return occupiedRect.maxZ - origin + 1 == origin - occupiedRect.minZ;
			}
			else
			{
				if (originIncluded)
				{
					return occupiedRect.maxX - origin == origin - occupiedRect.minX;
				}
				return occupiedRect.maxX - origin + 1 == origin - occupiedRect.minX;
			}
		}

		// Token: 0x0600D395 RID: 54165 RVA: 0x004E0BEC File Offset: 0x004DEDEC
		private void MoveUntilSymmetrical(SketchBuildable buildable, CellRect initial, int origin, bool vertical, bool originIncluded)
		{
			if (vertical)
			{
				buildable.pos.x = buildable.pos.x + (initial.minX - buildable.OccupiedRect.minX);
				int num;
				if (originIncluded)
				{
					num = origin - initial.maxZ + origin;
				}
				else
				{
					num = origin - initial.maxZ - 1 + origin;
				}
				buildable.pos.z = buildable.pos.z + (num - buildable.OccupiedRect.minZ);
				return;
			}
			buildable.pos.z = buildable.pos.z + (initial.minZ - buildable.OccupiedRect.minZ);
			int num2;
			if (originIncluded)
			{
				num2 = origin - initial.maxX + origin;
			}
			else
			{
				num2 = origin - initial.maxX - 1 + origin;
			}
			buildable.pos.x = buildable.pos.x + (num2 - buildable.OccupiedRect.minX);
		}
	}
}
