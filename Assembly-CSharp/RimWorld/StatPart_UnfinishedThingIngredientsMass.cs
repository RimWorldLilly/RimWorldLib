using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FAE RID: 8110
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x0600CDE7 RID: 52711 RVA: 0x004BA374 File Offset: 0x004B8574
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600CDE8 RID: 52712 RVA: 0x004BA394 File Offset: 0x004B8594
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_IngredientsMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x0600CDE9 RID: 52713 RVA: 0x004BA3D4 File Offset: 0x004B85D4
		private bool TryGetValue(StatRequest req, out float value)
		{
			UnfinishedThing unfinishedThing = req.Thing as UnfinishedThing;
			if (unfinishedThing == null)
			{
				value = 0f;
				return false;
			}
			float num = 0f;
			for (int i = 0; i < unfinishedThing.ingredients.Count; i++)
			{
				num += unfinishedThing.ingredients[i].GetStatValue(StatDefOf.Mass, true, -1) * (float)unfinishedThing.ingredients[i].stackCount;
			}
			value = num;
			return true;
		}
	}
}
