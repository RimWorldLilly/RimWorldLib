using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB2 RID: 8114
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x0600CDF5 RID: 52725 RVA: 0x004BA670 File Offset: 0x004B8870
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.8f;
			}
		}

		// Token: 0x0600CDF6 RID: 52726 RVA: 0x004BA694 File Offset: 0x004B8894
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				return "Outdoors".Translate().CapitalizeFirst() + ": x" + 0.8f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CDF7 RID: 52727 RVA: 0x004BA6EA File Offset: 0x004B88EA
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x0600CDF8 RID: 52728 RVA: 0x004BA704 File Offset: 0x004B8904
		public static bool Applies(ThingDef def, Map map, IntVec3 c)
		{
			if (def.building == null)
			{
				return false;
			}
			if (map == null)
			{
				return false;
			}
			Room room = c.GetRoom(map);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x04008343 RID: 33603
		public const float WorkRateFactor = 0.8f;
	}
}
